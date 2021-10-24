using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Mvc.Helpers.Abstract;
using ProgrammersBlog.Shared.Utilities.Extensions;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class RoleController : BaseController
    {
        private readonly RoleManager<Role> _roleManager;

        public RoleController(RoleManager<Role> roleManager,UserManager<User> userManager,IMapper mapper,IImageHelper imageHelper):base(userManager,mapper,imageHelper)
        {
            _roleManager = roleManager;
        }

        [Authorize(Roles ="SuperAdmin,Role.Read")]
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleManager.Roles.ToListAsync(); 
            return View(new RoleListDto { 
            Roles=roles
            });
        }
        [Authorize(Roles ="SuperAdmin,Role.Read")]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var roles = await _roleManager.Roles.ToListAsync();
            var roleListDto = JsonSerializer.Serialize(new RoleListDto {
                Roles=roles
            });
            return Json(roleListDto);
        }
        [Authorize(Roles="SuperAdmin,User.Update")]
        [HttpGet]
        public async Task<IActionResult> Asign(int userId)
        {
            var user = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == userId);
            var roles = await _roleManager.Roles.ToListAsync();
            var userRoles = await UserManager.GetRolesAsync(user);
            UserRoleAsignDto userRoleAsignDto = new UserRoleAsignDto
            {
                UserId = user.Id,
                UserName = user.UserName

            };
            foreach (var role in roles)
            {
                RoleAsignDto rolesAsignDto = new RoleAsignDto
                {
                    RoleId = role.Id,
                    RoleName = role.Name,
                    HasRole = userRoles.Contains(role.Name)
                };
                userRoleAsignDto.RoleAsignDtos.Add(rolesAsignDto);
            }
            return PartialView("_RoleAsignPartial", userRoleAsignDto);
        }
        [Authorize(Roles = "SuperAdmin,User.Update")]
        [HttpPost]
        public async Task<IActionResult> Asign(UserRoleAsignDto userRoleAsignDto)
        {
            if (ModelState.IsValid)
            {
                var user = await UserManager.Users.SingleOrDefaultAsync(u => u.Id == userRoleAsignDto.UserId);
                foreach (var roleAsignDto in userRoleAsignDto.RoleAsignDtos)
                {
                    if (roleAsignDto.HasRole)
                        await UserManager.AddToRoleAsync(user, roleAsignDto.RoleName);
                    else
                    {
                        await UserManager.RemoveFromRoleAsync(user, roleAsignDto.RoleName);
                    }
                }
                await UserManager.UpdateSecurityStampAsync(user);
                var userRoleAsignAjaxViewModel = JsonSerializer.Serialize(new UserRoleAsignAjaxViewModel
                {
                    UserDto=new UserDto
                    {
                        User=user,
                        Message=$"{user.UserName} kullanicisina ait rol atama islemi basariyla tamamlanmistir",
                        ResultStatus=ResultStatus.Success
                    },
                    RoleAsignPartial=await this.RenderViewToStringAsync("_RoleAsignPartial",userRoleAsignDto)
                });
                return Json(userRoleAsignAjaxViewModel);
            }
            else
            {
                var userRoleAsignAjaxErrorModel= JsonSerializer.Serialize(new UserRoleAsignAjaxViewModel
                {
                    RoleAsignPartial = await this.RenderViewToStringAsync("_RoleAsignPartial", userRoleAsignDto),
                    UserRoleAsignDto=userRoleAsignDto //modelstate hatalari bunun üzerinde
                });
                return Json(userRoleAsignAjaxErrorModel);
            }
        }
    }
}
