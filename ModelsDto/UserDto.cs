using Microsoft.EntityFrameworkCore;
using MVC_EFCodeFirstWithVueBase.Models;
using MVC_EFCodeFirstWithVueBase.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace MVC_EFCodeFirstWithVueBase.ModelsDto
{
    public class UserDto
    {
        [MaxLength(50)]
        public string? Id { get; set; }
        [Required, MaxLength(50)]
        public string? Name { get; set; }
        [Required, MaxLength(50)]
        public string? Email { get; set; }
        public IFormFile? ImageFile { get; set; }
        [Required, MaxLength(50)]
        public string? Password { get; set; }
        public string? ImagePath { get; set; }
        public string? CreatedTimeFormat { get; set; }
        public async Task<bool> SaveAsync(AppDbContext dbContext)
        {
            if (string.IsNullOrEmpty(Password))
            {
                return false;
            }

            byte[] passwordHash, passwordSalt;
            CreatePasswordHash(Password, out passwordHash, out passwordSalt);

            User model;
            if (!string.IsNullOrEmpty(Id))
            {
                model = await dbContext.Users
                            .Where(u => u.Id == Id)
                            .FirstAsync();
                if (model == null) return false;

                model.Name = Name;
                model.Email = Email;
                model.PasswordHash = passwordHash;
                model.PasswordSalt = passwordSalt;
            }
            else
            {
                model = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Name = Name,
                    Email = Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedTime = DateTime.Now,
                    Active = true
                };
                dbContext.Users.Add(model);
            }

            // Handle file saving (if any)
            if (ImageFile != null)
            {
                // Example: Save file logic
                var filePath = Path.Combine("Uploads", ImageFile.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }
            }

            await dbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(AppDbContext dbContext, string uid)
        {
            var user = await GetUserAsyncById(dbContext, uid);
            if (user == null) return false;
            user.Active = false;
            await dbContext.SaveChangesAsync();
            return true;
        }

        public static async Task<User?> GetUserAsyncById(AppDbContext dbContext, string? uid)
        {
            var result = await dbContext.Users.FindAsync(uid);
            if(result == null) return null;
            if(!result.Active) return null;
            return result ;                       
        }
        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            // HMACSHA512初始化
            using (var hmac = new HMACSHA512())
            {
                // 取得雜湊演算法的金鑰
                passwordSalt = hmac.Key;
                // 將明碼搭配 passwordSalt 產出 Hash
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            // 使用儲存的 passwordSalt 初始化 HMACSHA512 
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                // 使用相同的 Salt 重新計算密碼的Hash
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));

                // 將重新計算的Hash值與儲存的Hash進行比較
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
