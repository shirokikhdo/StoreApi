namespace Api.Services.Storage;

/// <summary>
/// Интерфейс для сервиса хранения файлов.
/// </summary>
public interface IFileStorageService
{
    /// <summary>
    /// Удаляет файл из хранилища по заданному имени файла.
    /// </summary>
    /// <param name="fileName">Имя файла, который нужно удалить.</param>
    /// <returns>Задача, представляющая асинхронную операцию. 
    /// Возвращает <c>true</c>, если файл был успешно удален; в противном случае <c>false</c>.</returns>
    Task<bool> RemoveFileAsync(string fileName);

    /// <summary>
    /// Загружает файл в хранилище.
    /// </summary>
    /// <param name="file">Файл, который нужно загрузить.</param>
    /// <returns>Задача, представляющая асинхронную операцию. 
    /// Возвращает строку, представляющую URL или путь к загруженному файлу.</returns>
    Task<string> UploadFileAsync(IFormFile file);
}