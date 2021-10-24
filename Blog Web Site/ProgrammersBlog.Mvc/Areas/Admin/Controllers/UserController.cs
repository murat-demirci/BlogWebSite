 using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using ProgrammersBlog.Entities.ComplexTypes;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class UserController : BaseController
    {
        private readonly IToastNotification _toastNotification;
        private readonly SignInManager<User> _signInManager;

        public UserController(UserManager<User> userManager, IMapper mapper, SignInManager<User> signInManager, IImageHelper imageHelper, IToastNotification toastNotification):base(userManager,mapper,imageHelper)
        {
            _signInManager = signInManager;
            _toastNotification = toastNotification;
        }
        [Authorize(Roles ="SuperAdmin,User.Read")]
        public async Task<IActionResult> Index()
        {
            var users = await UserManager.Users.ToListAsync();
            return View(new UserListDto
            {
                Users = users,
                ResultStatus=ResultStatus.Success
            });
        }
       
        [Authorize(Roles = "SuperAdmin,User.Read")]
        [HttpGet]
        public async Task<JsonResult> GetAllUsers()
        {
            var users = await UserManager.Users.ToListAsync();
            var userListDto = JsonSerializer.Serialize(new UserListDto
            {
                Users = users,
                ResultStatus = ResultStatus.Success
            }, new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve
            });
            return Json(userListDto);
        }
        [Authorize(Roles = "SuperAdmin,User.Create")]
        [HttpGet]
        public IActionResult Add()
        {
                return PartialView("_UserAddPartial");   
        }
        [Authorize(Roles = "SuperAdmin,User.Create")]
        [HttpPost]
        public async Task<IActionResult> Add(UserAddDto userAddDto)
        {

            if (ModelState.IsValid)
            {
                //var uploadedImageDtoResult = ImageHelper.Upload(userAddDto.UserName, userAddDto.PictureFile, PictureType.User);
                //userAddDto.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success
                //    ? uploadedImageDtoResult.Data.FullName
                //    : "userImages/defaultUser.png";
                var user = Mapper.Map<User>(userAddDto);
                var result = await UserManager.CreateAsync(user, userAddDto.Password);
                if (result.Succeeded)
                {
                    var userAddAjaxModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserDto = new UserDto
                        {
                            ResultStatus = ResultStatus.Success,
                            Message = $"{user.UserName} kullanıcı başarıyla eklenmiştir",
                            User = user
                        },
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    }) ;
                    return Json(userAddAjaxModel); 
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                    var userAddAjaxErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
                    {
                        UserAddDto = userAddDto,
                        UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
                    });
                    return Json(userAddAjaxErrorModel);
                }
            }
            var userAddAjaxModelStateErrorModel = JsonSerializer.Serialize(new UserAddAjaxViewModel
            {
                UserAddDto = userAddDto,
                UserAddPartial = await this.RenderViewToStringAsync("_UserAddPartial", userAddDto)
            });
            return Json(userAddAjaxModelStateErrorModel);
        }
       

        [Authorize(Roles ="SuperAdmin,User.Delete")]
        [HttpPost]
        public async Task<JsonResult> Delete(int userId) 
        {
            var user = await UserManager.FindByIdAsync(userId.ToString());
            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                if (user.Picture!="userImages/Default.png")
                {
                    ImageHelper.Delete(user.Picture);
                }
                var deletedUser = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus=ResultStatus.Success,
                    Message=$"{user.UserName} kullanıcı başarıyla silinmiştir",
                    User=user
                });
                return Json(deletedUser);
            }
            else
            {
                string errorMessage = String.Empty;
                foreach (var error in result.Errors)
                {
                    errorMessage=$"*{error.Description}\n";
                }
                var deletedUserErrorModel = JsonSerializer.Serialize(new UserDto
                {
                    ResultStatus = ResultStatus.Error,
                    Message = $"{user.UserName} kullanıcı silinirken hata oluştu\n {errorMessage}",
                    User = user

                });
                return Json(deletedUserErrorModel);
            }
        }
        [Authorize(Roles = "SuperAdmin,User.Update")]
        [HttpGet]
        public async Task<PartialViewResult> Update(int userId)
        {
            var user = await UserManager.Users.FirstOrDefaultAsync(u=>u.Id==userId);   
            var userUpdateDto = Mapper.Map<UserUpdateDto>(user);
            return PartialView("_UserUpdatePartial", userUpdateDto);
        }
        [Authorize(Roles = "SuperAdmin,User.Update")]
        [HttpPost]
        public async Task<IActionResult> Update(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false;
                var oldUser = await UserManager.FindByIdAsync(userUpdateDto.Id.ToString());
                var oldUserPicture = oldUser.Picture;
                if (userUpdateDto.PictureFile!=null)
                {
                    var uploadedImageDtoResult = await ImageHelper.Uploaded(userUpdateDto.UserName, userUpdateDto.PictureFile, PictureType.User);
                    userUpdateDto.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success ? uploadedImageDtoResult.Data.FullName :
                        "userImages/Default.png";
                    if (oldUserPicture != "userImages/Default.png")
                    {
                        isNewPictureUploaded = true;
                    }

                }
                var updatedUser = Mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await UserManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isNewPictureUploaded)
                    {
                        ImageHelper.Delete(oldUserPicture);
                    }
                    var userUpdateViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserDto=new UserDto 
                        { 
                            ResultStatus=ResultStatus.Success,
                            Message=$"{updatedUser.UserName} kullanıcı başarıyla güncellenmiştir",
                            User=updatedUser
                        },
                        UserUpdatePartial=await this.RenderViewToStringAsync("_UserUpdatePartial",userUpdateDto)
                    });
                    return Json(userUpdateViewModel);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    var userUpdateErrorViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                    {
                        UserUpdateDto = userUpdateDto,
                        UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                    }); 
                    return Json(userUpdateErrorViewModel);
                }

            }
            else
            {
                var userUpdateErrorModelStateViewModel = JsonSerializer.Serialize(new UserUpdateAjaxViewModel
                {
                    UserUpdateDto = userUpdateDto,
                    UserUpdatePartial = await this.RenderViewToStringAsync("_UserUpdatePartial", userUpdateDto)
                });
                return Json(userUpdateErrorModelStateViewModel);
            }
        }
        [Authorize]
        [HttpGet]
        public async Task<ViewResult> ChangeDetail()
        {
            var user = await UserManager.GetUserAsync(HttpContext.User);
            var updateDto = Mapper.Map<UserUpdateDto>(user);
            return View(updateDto);
        }
        [Authorize]
        [HttpPost]
        public async Task<ViewResult> ChangeDetail(UserUpdateDto userUpdateDto)
        {
            if (ModelState.IsValid)
            {
                bool isNewPictureUploaded = false;
                var oldUser = await UserManager.GetUserAsync(HttpContext.User);
                var oldUserPicture = oldUser.Picture;
                if (userUpdateDto.PictureFile != null)
                {
                    //var uploadedImageDtoResult = await ImageHelper.Upload(userUpdateDto.UserName, userUpdateDto.PictureFile,PictureType.User);
                    //userUpdateDto.Picture = uploadedImageDtoResult.ResultStatus == ResultStatus.Success ? uploadedImageDtoResult.Data.FullName :
                    //    "userImages/Default.png";
                    if (oldUserPicture!= "userImages/Default.png")
                    {
                        isNewPictureUploaded = true;
                    }
                    

                }
                var updatedUser = Mapper.Map<UserUpdateDto, User>(userUpdateDto, oldUser);
                var result = await UserManager.UpdateAsync(updatedUser);
                if (result.Succeeded)
                {
                    if (isNewPictureUploaded)
                    {
                        ImageHelper.Delete(oldUserPicture);
                    }
                    _toastNotification.AddSuccessToastMessage($"{updatedUser.UserName} bilgiler basariyla güncellenmistir");
                    return View(userUpdateDto);
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(userUpdateDto);
                }

            }
            else
            {
                return View(userUpdateDto);
            }
        }
        [Authorize(Roles ="SuperAdmin,User.Read")]
        [HttpGet]
        public async Task<PartialViewResult> GetDetail(int userId)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            return PartialView("_GetDetailPartial", new UserDto { User = user });
        }

        [Authorize]
        [HttpGet]
        public ViewResult PasswordChange()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> PasswordChange(UserPasswordChangeDto userPasswordChangeDto)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);
                var isVerified = await UserManager.CheckPasswordAsync(user,userPasswordChangeDto.CurrentPassword);
                if (isVerified)
                {
                    var result = await UserManager.ChangePasswordAsync(user,userPasswordChangeDto.CurrentPassword,userPasswordChangeDto.NewPassword);
                    if (result.Succeeded)
                    {
                        await UserManager.UpdateSecurityStampAsync(user);
                        await _signInManager.SignOutAsync();
                        await _signInManager.PasswordSignInAsync(user,userPasswordChangeDto.NewPassword,true,false);
                        _toastNotification.AddSuccessToastMessage($"Sifreniz basariyla degistirilmistir");
                        return View();
                    }
                    else
                    {
                        foreach (var error in result.Errors)
                        {
                            ModelState.AddModelError("", error.Description);
                        }
                        return View(userPasswordChangeDto);
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Lütfen girmiş olduğunuz şifreyi kontrol ediniz");
                    return View(userPasswordChangeDto);
                }
            }
            else
            {
                return View(userPasswordChangeDto);
            }
        }
    }
}
