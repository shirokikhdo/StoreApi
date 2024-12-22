using System.Net;

namespace Api.Models;

/// <summary>
/// Представляет ответ сервера, содержащий информацию о статусе выполнения запроса.
/// </summary>
public class ResponseServer
{
    /// <summary>
    /// Получает или задает значение, указывающее, был ли запрос успешным.
    /// </summary>
    public bool IsSuccess { get; set; }

    /// <summary>
    /// Получает или задает код статуса HTTP, возвращаемый сервером.
    /// </summary>
    public HttpStatusCode StatusCode { get; set; }

    /// <summary>
    /// Получает или задает список сообщений об ошибках, если запрос не был успешным.
    /// </summary>
    public List<string> ErrorMessages { get; set; }

    /// <summary>
    /// Получает или задает результат запроса. Может содержать любые данные, возвращаемые сервером.
    /// </summary>
    public object Result { get; set; }

    /// <summary>
    /// Инициализирует новый экземпляр класса <see cref="ResponseServer"/> и устанавливает 
    /// значение <see cref="IsSuccess"/> в <c>true</c> и инициализирует список сообщений об ошибках.
    /// </summary>
    public ResponseServer()
    {
        IsSuccess = true;
        ErrorMessages = new List<string>();
    }
}