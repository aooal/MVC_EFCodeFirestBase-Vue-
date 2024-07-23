using System.ComponentModel.DataAnnotations;

namespace MVC_EFCodeFirstWithVueBase.Models
{
    public class User
    {
        [MaxLength(50)]
        public string? Id { get; set; }
        [MaxLength(50)]
        public string? Name { get; set; }
        [MaxLength(50)]
        public string? Email { get; set; }
        [MaxLength(200)]
        public string? ImageFileName { get; set; }
        public byte[]? PasswordHash { get; set; }
        public byte[]? PasswordSalt { get; set; }
        public DateTime CreatedTime { get; set; }
        public bool Active { get; set; }
    }
}
