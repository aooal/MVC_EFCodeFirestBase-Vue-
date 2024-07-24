namespace MVC_EFCodeFirstWithVueBase.Services
{
    public interface IFileService
    {
        Task<string> HandleFileAsync(IFormFile selectedfile, string fileTypeFolderName, string tableName, string filePath);
        bool IsImageFile(IFormFile file);
        bool IsImgFileSizeValid(IFormFile file);
    }
}
