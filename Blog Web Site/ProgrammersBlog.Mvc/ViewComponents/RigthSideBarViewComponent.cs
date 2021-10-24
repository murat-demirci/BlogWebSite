using Microsoft.AspNetCore.Mvc;
using ProgrammersBlog.Mvc.Models;
using ProgrammersBlog.Services.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.ViewComponents
{
    public class RigthSideBarViewComponent : ViewComponent
    {
        private readonly ICategoryService _categoryService;
        private readonly IArticleService _articleService;
        public RigthSideBarViewComponent(ICategoryService categoryService, IArticleService articleService)
        {
            _categoryService = categoryService;
            _articleService = articleService;
        }

        
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categoryResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            var articleResult = await _articleService.GetAllByViewCountAsync(isAscending:false,takeSize:5);
            return View(new RightSideBarViewModel
            {
                Categories = categoryResult.Data.Categories,
                Articles = articleResult.Data.Articles
            });
        }
    }
}
