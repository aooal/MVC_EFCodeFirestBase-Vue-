namespace MVC_EFCodeFirstWithVueBase.Services
{
    public class FileService
    {
        public readonly IWebHostEnvironment Environment;
        private readonly string[] _acceptedImgTypes = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly long _maxImgFileSize = 100 * 1024 * 1024; // 100MB

        public FileService(IWebHostEnvironment environment)
        {
            Environment = environment;
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
