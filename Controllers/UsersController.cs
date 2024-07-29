using Microsoft.AspNetCore.Mvc;
using MVC_EFCodeFirstWithVueBase.Models;
using MVC_EFCodeFirstWithVueBase.ModelsDto;
using MVC_EFCodeFirstWithVueBase.Repository;
using MVC_EFCodeFirstWithVueBase.Services;
 
namespace MVC_EFCodeFirstWithVueBase.Controllers
{
    public class UsersController : Controller
    {
        private readonly IFileService _fileService;
        private readonly IDatabaseHelper _databaseHelper;
        public UsersController(AppDbContext context, IFileService fileService, IDatabaseHelper databaseHelper) 
        {
            _fileService = fileService;
            _databaseHelper = databaseHelper;
        }
        public async Task<IActionResult> Index(string searchTerm, CancellationToken token)
        {
            var users = await _databaseHelper.GetAllAsync<User>(token);
            if (!string.IsNullOrEmpty(searchTerm))
             {
                users = users
                .Where
                (
                    u => (u.Name ?? string.Empty).Contains(searchTerm)
                    || (u.Email ?? string.Empty).Contains(searchTerm)
                )
                .ToList();
            }
            users = users.OrderByDescending(x => x.CreatedTime).Where(x => x.Active == true).ToList();
            return View(users);
        }
        #region Create
        public IActionResult _Create()
        {
            return PartialView("_Create");
        }

        [HttpPost]
        public async Task<IActionResult>Create(UserDto userDto, CancellationToken token)
        {
            
            if (userDto.ImageFile != null && !_fileService.IsImgFileSizeValid(userDto.ImageFile))
            {
                ModelState.AddModelError("ImageFile", "The file size should not exceed 100MB.");
            }
            else if (userDto.ImageFile != null && !_fileService.IsImageFile(userDto.ImageFile))
            {
                ModelState.AddModelError("ImageFile", "The file is not a valid image.");
            }
            if (!ModelState.IsValid)
            {
                return PartialView("_Create", userDto);
            }
            await userDto.SaveAsync(_databaseHelper, _fileService, token);

            return Json(new { success = true });
        }
        #endregion

        #region Edit
        public async Task<IActionResult>_Edit(string uid, CancellationToken token)
        {
            var user = await _databaseHelper.GetByIdAsync<User>(uid, token);
            if (user == null)
            {
                return NotFound(); 
            }
            var userDto = new UserDto
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                ImagePath = user.ImageFileName ,
                CreatedTimeFormat = user.CreatedTime.ToString("yyyy/MM/dd HH:mm"),
                Password = ""                             
            };

            return PartialView("_Edit", userDto);
        }
        [HttpPost]
        public async Task<IActionResult>Edit(string uid, UserDto userDto, CancellationToken token)
        {
            userDto.Id = uid;         
            if (!ModelState.IsValid)
            {
                return PartialView("_Edit",userDto);
            }
            var result = await userDto.SaveAsync(_databaseHelper, _fileService, token);
            if (!result) return NotFound();
            return Json(new { success = true });
        }
        #endregion

        #region Delete
        public async Task<IActionResult> Delete(string uid, CancellationToken token)
        {
            var user = new UserDto();        
            await user.DeleteAsync(_databaseHelper, uid, token);
            return RedirectToAction("Index", "Users");
        }
        #endregion
    }
}
