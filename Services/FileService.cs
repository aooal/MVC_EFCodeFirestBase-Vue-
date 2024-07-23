namespace MVC_EFCodeFirstWithVueBase.Services
{
    public class FileService
    {
        private readonly string[] _aceptImgType = { ".jpg", ".jpeg", ".png", ".gif" };
        private readonly long _maxImgFileSize = 100 * 1024 * 1024; // 100MB

        public bool IsImageFile(IFormFile file)
        {
            var extension = System.IO.Path.GetExtension(file.FileName).ToLowerInvariant();
            return _aceptImgType.Contains(extension);
        }

        public bool IsImgFileSizeValid(IFormFile file)
        {
            return file.Length <= _maxImgFileSize;
        }
    }
}
