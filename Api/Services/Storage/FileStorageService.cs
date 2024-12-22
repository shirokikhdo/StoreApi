using Microsoft.Extensions.Options;
using System.Globalization;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text.RegularExpressions;
using System.Text;

namespace Api.Services.Storage;

/// <summary>
/// Сервис для работы с файловым хранилищем, который реализует интерфейс <see cref="IFileStorageService"/>.
/// Предоставляет методы для загрузки и удаления файлов в облачном хранилище.
/// </summary>
public class FileStorageService : IFileStorageService
{
    private readonly string _serviceURL;
    private readonly string _region;
    private readonly string _accessKey;
    private readonly string _secretKey;
    private readonly string _bucketName;
    private readonly HttpClient _httpClient;

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="FileStorageService"/>.
    /// </summary>
    /// <param name="options">Настройки для подключения к файловому хранилищу.</param>
    public FileStorageService(IOptions<TimeWebSettings> options)
    {
        _serviceURL = options.Value.ServiceURL;
        _region = options.Value.Region;
        _accessKey = options.Value.AccessKey;
        _secretKey = options.Value.SecretKey;
        _bucketName = options.Value.BucketName;
        _httpClient = new HttpClient();
    }

    /// <summary>
    /// Асинхронно загружает файл в файловое хранилище.
    /// </summary>
    /// <param name="file">Файл для загрузки.</param>
    /// <returns>Асинхронная задача, представляющая URL загруженного файла.</returns>
    /// <exception cref="Exception">Выбрасывается, если загрузка файла завершилась неудачно.</exception>
    public async Task<string> UploadFileAsync(IFormFile file)
    {
        var fileName = GenerateUniqueFileName(file.FileName);
        using (var memoryStream = new MemoryStream())
        {
            await file.CopyToAsync(memoryStream);
            memoryStream.Position = 0;
            var fileContent = memoryStream.ToArray();

            var requestUri = $"{_serviceURL}/{_bucketName}/{fileName}";
            var host = new Uri(_serviceURL).Host;
            var date = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ");
            var dateShort = DateTime.UtcNow.ToString("yyyyMMdd");

            var canonicalRequest = $"PUT\n/{_bucketName}/{fileName}\n\nhost:{host}\nx-amz-content-sha256:{ToHexString(HashSHA256(fileContent))}\nx-amz-date:{date}\n\nhost;x-amz-content-sha256;x-amz-date\n{ToHexString(HashSHA256(fileContent))}";

            var stringToSign = $"AWS4-HMAC-SHA256\n{date}\n{dateShort}/{_region}/s3/aws4_request\n{ToHexString(HashSHA256(Encoding.UTF8.GetBytes(canonicalRequest)))}";

            var signingKey = GetSignatureKey(_secretKey, dateShort, _region, "s3");

            var signature = ToHexString(HMACSHA256(signingKey, stringToSign));

            using (var request = new HttpRequestMessage(HttpMethod.Put, requestUri))
            {
                request.Headers.Host = host;
                request.Headers.Add("x-amz-date", date);
                request.Headers.Add("x-amz-content-sha256", ToHexString(HashSHA256(fileContent)));
                request.Headers.Authorization = new AuthenticationHeaderValue("AWS4-HMAC-SHA256", $"Credential={_accessKey}/{dateShort}/{_region}/s3/aws4_request, SignedHeaders=host;x-amz-content-sha256;x-amz-date, Signature={signature}");
                request.Content = new ByteArrayContent(fileContent);
                request.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");

                var response = await _httpClient.SendAsync(request);

                if (response.IsSuccessStatusCode)
                    return $"{_serviceURL}/{_bucketName}/{fileName}";
                
                var responseBody = await response.Content.ReadAsStringAsync();
                throw new Exception($"Error uploading file: {response.StatusCode}, Response: {responseBody}");
            }
        }
    }

    /// <summary>
    /// Асинхронно удаляет файл из файлового хранилища.
    /// </summary>
    /// <param name="fileName">Имя файла, который необходимо удалить.</param>
    /// <returns>Асинхронная задача, представляющая результат операции удаления в виде булевого значения.</returns>
    /// <exception cref="Exception">Выбрасывается, если удаление файла завершилось неудачно.</exception>
    public async Task<bool> RemoveFileAsync(string fileName)
    {
        var requestUri = $"{_serviceURL}/{_bucketName}/{fileName}";
        var host = new Uri(_serviceURL).Host;
        var date = DateTime.UtcNow.ToString("yyyyMMddTHHmmssZ");
        var dateShort = DateTime.UtcNow.ToString("yyyyMMdd");

        var canonicalRequest = $"DELETE\n/{_bucketName}/{fileName}\n\nhost:{host}\nx-amz-date:{date}\n\nhost;x-amz-date\nUNSIGNED-PAYLOAD";

        var stringToSign = $"AWS4-HMAC-SHA256\n{date}\n{dateShort}/{_region}/s3/aws4_request\n{ToHexString(HashSHA256(Encoding.UTF8.GetBytes(canonicalRequest)))}";

        var signingKey = GetSignatureKey(_secretKey, dateShort, _region, "s3");

        var signature = ToHexString(HMACSHA256(signingKey, stringToSign));

        using (var request = new HttpRequestMessage(HttpMethod.Delete, requestUri))
        {
            request.Headers.Host = host;
            request.Headers.Add("x-amz-date", date);
            request.Headers.Authorization = new AuthenticationHeaderValue("AWS4-HMAC-SHA256", $"Credential={_accessKey}/{dateShort}/{_region}/s3/aws4_request, SignedHeaders=host;x-amz-date, Signature={signature}");

            var response = await _httpClient.SendAsync(request);

            if (response.IsSuccessStatusCode)
                return true;
            
            var responseBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error deleting file: {response.StatusCode}, Response: {responseBody}");
        }
    }

    private string GenerateUniqueFileName(string originalFileName)
    {
        var fileNameWithoutExtension = Path.GetFileNameWithoutExtension(originalFileName);
        var extension = Path.GetExtension(originalFileName);

        var uniqueId = GetTransliteration(fileNameWithoutExtension);

        var guid = Guid.NewGuid().ToString().Replace("-", "");
        uniqueId = Regex.Replace(uniqueId, @"[^a-zA-Z0-9]", "");

        uniqueId = uniqueId.Substring(0, Math.Min(uniqueId.Length, 10));

        return $"{uniqueId}_{guid}{extension}";
    }

    private string GetTransliteration(string input)
    {
        var normalizedString = input.Normalize(NormalizationForm.FormD);
        var stringBuilder = new StringBuilder();
        foreach (var c in normalizedString)
        {
            var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
            if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                stringBuilder.Append(c);
        }

        return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
    }

    private static byte[] HashSHA256(byte[] data)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            return sha256.ComputeHash(data);
        }
    }

    private static byte[] HMACSHA256(byte[] key, string data)
    {
        using (var hmac = new HMACSHA256(key))
        {
            return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
        }
    }

    private static byte[] GetSignatureKey(string key, string dateStamp, string regionName, string serviceName)
    {
        var kDate = HMACSHA256(Encoding.UTF8.GetBytes("AWS4" + key), dateStamp);
        var kRegion = HMACSHA256(kDate, regionName);
        var kService = HMACSHA256(kRegion, serviceName);
        return HMACSHA256(kService, "aws4_request");
    }

    private static string ToHexString(byte[] bytes)
    {
        var builder = new StringBuilder();
        foreach (var b in bytes)
            builder.Append(b.ToString("x2"));
        return builder.ToString();
    }
}