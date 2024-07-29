namespace MVC_EFCodeFirstWithVueBase.Services
{
    public class FileService : IFileService
    {
        public readonly IWebHostEnvironment Environment;
        private readonly string[] _acceptedImgTypes = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly long _maxImgFileSize = 100 * 1024 * 1024; // 100MB

        public FileService(IWebHostEnvironment environment)
        {
            Environment = environment;
        }
        public async Task<string> HandleFileAsync(IFormFile selectedfile, string fileTypeFolderName, string tableName,
            string filePath, CancellationToken token)
        {
            var fullFilePath = Path.Combine(Environment.WebRootPath, "img", "users", filePath);
            var directory = Path.GetDirectoryName(fullFilePath);
            if (!Directory.Exists(directory) && !string.IsNullOrEmpty(directory))
            {
                Directory.CreateDirectory(directory);
            }
            using (var stream = new FileStream(fullFilePath, FileMode.Create))
            {
                await selectedfile.CopyToAsync(stream, token);
            }
            return filePath;
        }

        public bool IsImageFile(IFormFile file)
        {
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            return _acceptedImgTypes.Contains(extension);
        }

        public bool IsImgFileSizeValid(IFormFile file)
        {
            return file.Length <= _maxImgFileSize;
        }
    }
}
