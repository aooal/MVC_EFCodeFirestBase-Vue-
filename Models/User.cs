using System.ComponentModel.DataAnnotations;

namespace MVC_EFCodeFirstWithVueBase.Models
{
    public class User
    {
        [MaxLength(50)]
        public virtual string? Id { get; set; }
        [MaxLength(50)]
        public virtual string? Name { get; set; }
        [MaxLength(50)]
        public virtual string? Email { get; set; }
        [MaxLength(200)]
        public virtual string? ImageFileName { get; set; }
        public virtual byte[]? PasswordHash { get; set; }
        public virtual byte[]? PasswordSalt { get; set; }
        public virtual DateTime CreatedTime { get; set; }
        public virtual bool Active { get; set; }
    }
}
