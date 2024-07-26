using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Hosting;
using MVC_EFCodeFirstWithVueBase.Models;
using MVC_EFCodeFirstWithVueBase.Repository;
using MVC_EFCodeFirstWithVueBase.Services;
using System;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography;
using System.Text;

namespace MVC_EFCodeFirstWithVueBase.ModelsDto
{
    public class UserDto : User
    {
        [MaxLength(50)]
        public override string? Id { get; set; }
        [Required, MaxLength(50)]
        public override string? Name { get; set; }
        [Required, MaxLength(50)]
        public override string? Email { get; set; }
        public IFormFile? ImageFile { get; set; }
        [Required, MaxLength(50)]
        public string? Password { get; set; }
        public string? ImagePath { get; set; }
        public string? CreatedTimeFormat { get; set; }
        public async Task<bool> SaveAsync(IDatabaseHelper dbHelper, IFileService fileService)
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
                model = await dbHelper.GetByIdAsync<User>(Id);
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
                    Id = Guid.NewGuid().ToString("N"),
                    Name = Name,
                    Email = Email,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
                    CreatedTime = DateTime.Now,
                    Active = true
                };
                await dbHelper.AddAsync(model);
            }
            if (ImageFile != null)
            {
                var filePath = $"{model.Id}.jpg";
                model.ImageFileName = await fileService.HandleFileAsync(ImageFile, "img", "users", filePath);
            }
            await dbHelper.UpdateAsync(model);
            return true;
        }

        public async Task<bool> DeleteAsync(IDatabaseHelper dbHelper, string uid)
        {
            var user = await dbHelper.GetByIdAsync<User>(uid);
            if (user == null) return false;
            user.Active = false;
            await dbHelper.UpdateAsync(user);
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
