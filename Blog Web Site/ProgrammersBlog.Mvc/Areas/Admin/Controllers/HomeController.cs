using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Results.ComplexTypes;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class HomeController : Controller
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        private readonly ICommentService _commentService;
        private readonly UserManager<User> _userManager;

        public HomeController(UserManager<User> userManager, ICommentService commentService, IArticleService articleService, ICategoryService categoryService)
        {
            _userManager = userManager;
            _commentService = commentService;
            _articleService = articleService;
            _categoryService = categoryService;
        }
        [Authorize(Roles = "CoAdmin,SuperAdmin")]
        public async Task<IActionResult> Index()
        {
            var categoriesCountResult = await _categoryService.CountByNonDeletedAsync();
            var articlesCountResult = await _articleService.CountByNonDeletedAsync();
            var commentsCountResult = await _commentService.CountByNonDeletedAsync();
            var userCount = await _userManager.Users.CountAsync();
            var articlesResult = await _articleService.GetAllAsync();
            if (categoriesCountResult.ResultStatus==ResultStatus.Success&& articlesCountResult.ResultStatus == ResultStatus.Success && 
                commentsCountResult.ResultStatus == ResultStatus.Success &&userCount>-1&&articlesResult.ResultStatus==ResultStatus.Success)
            {
                return View(new DashboardViewModel { 
                    CategoriesSize=categoriesCountResult.Data,
                    ArticlesSize=articlesCountResult.Data,
                    CommentsSize=commentsCountResult.Data,
                    UsersSize=userCount,
                    Articles=articlesResult.Data
                });
            }
            return NotFound();
            
        }
    }
}
