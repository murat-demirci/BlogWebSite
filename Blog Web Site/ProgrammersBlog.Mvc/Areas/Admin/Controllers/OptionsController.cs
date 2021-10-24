using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using NToastNotify;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Mvc.Areas.Admin.Models;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Shared.Utilities.Helpers.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class OptionsController : Controller
    {
        private readonly AboutUsPageInfo _aboutUsPageInfo;
        private readonly IWritableOptions<AboutUsPageInfo> _aboutUsPageInfoWriter;
        private readonly IToastNotification _toastNotification;
        private readonly WebsiteInfo _webSiteInfo;
        private readonly IWritableOptions<WebsiteInfo> _webSiteInfoWriter;
        private readonly SmtpSettings _sMTPSettings;
        private readonly IWritableOptions<SmtpSettings> _sMTPSettingsWriter;
        private readonly Entities.Concrete.ArticleRightSideBarWidgetOptions _articleRightSideBarWidgetOptions;
        private readonly IWritableOptions<Entities.Concrete.ArticleRightSideBarWidgetOptions> _articleRightSideBarWidgetOptionsWriter;
        private readonly ICategoryService _categoryService;
        private readonly IMapper _mapper;

        public OptionsController(IOptionsSnapshot<AboutUsPageInfo> aboutUsPageInfo, IWritableOptions<AboutUsPageInfo> aboutUsPageInfoWriter, IToastNotification toastNotification,
            IOptionsSnapshot<WebsiteInfo> webSiteInfo, IWritableOptions<WebsiteInfo> webSiteInfoWriter, IOptionsSnapshot<SmtpSettings> sMTPSettings,
            IWritableOptions<SmtpSettings> sMTPSettingsWriter, IOptionsSnapshot<Entities.Concrete.ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptions,
            IWritableOptions<ArticleRightSideBarWidgetOptions> articleRightSideBarWidgetOptionsWriter, ICategoryService categoryService, IMapper mapper)
        {
            _aboutUsPageInfo = aboutUsPageInfo.Value;
            _aboutUsPageInfoWriter = aboutUsPageInfoWriter;
            _toastNotification = toastNotification;
            _webSiteInfo = webSiteInfo.Value;
            _webSiteInfoWriter = webSiteInfoWriter;
            _sMTPSettings = sMTPSettings.Value;
            _sMTPSettingsWriter = sMTPSettingsWriter;
            _articleRightSideBarWidgetOptions = articleRightSideBarWidgetOptions.Value;
            _articleRightSideBarWidgetOptionsWriter = articleRightSideBarWidgetOptionsWriter;
            _categoryService = categoryService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult About()
        {
            return View(_aboutUsPageInfo);
        }

        [HttpPost]
        public IActionResult About(AboutUsPageInfo aboutUsPageInfo)
        {
            if (ModelState.IsValid)
            {
                _aboutUsPageInfoWriter.Update(x =>
                {
                    x.Header = aboutUsPageInfo.Header;
                    x.Content = aboutUsPageInfo.Content;
                    x.SeoAuthor = aboutUsPageInfo.SeoAuthor;
                    x.SeoDescription = aboutUsPageInfo.SeoDescription;
                    x.SeoTags = aboutUsPageInfo.SeoTags;
                });
                _toastNotification.AddSuccessToastMessage("Hakkımızda sayfa içerikleri başarıyla güncellenmiştir",new ToastrOptions { 
                    Title="Başarılı işlem!"
                });
                return View(aboutUsPageInfo);
            }
            return View(aboutUsPageInfo);
        }

        [HttpGet]
        public IActionResult GeneralSettings()
        {
            return View(_webSiteInfo);
        }

        [HttpPost]
        public IActionResult GeneralSettings(WebsiteInfo webSiteInfo)
        {
            if (ModelState.IsValid)
            {
                _webSiteInfoWriter.Update(x =>
                {
                    x.Title = webSiteInfo.Title;
                    x.MenuTitle = webSiteInfo.MenuTitle;
                    x.SeoAuthor = webSiteInfo.SeoAuthor;
                    x.SeoDescription = webSiteInfo.SeoDescription;
                    x.SeoTags = webSiteInfo.SeoTags;
                });
                _toastNotification.AddSuccessToastMessage("Genel Ayarlar başarıyla güncellenmiştir", new ToastrOptions
                {
                    Title = "Başarılı işlem!"
                });
                return View(webSiteInfo);
            }
            return View(webSiteInfo);
        }

        [HttpGet]
        public IActionResult EmailSettings()
        {
            return View(_sMTPSettings);
        }

        [HttpPost]
        public IActionResult EmailSettings(SmtpSettings sMTPSettings)
        {
            if (ModelState.IsValid)
            {
                _sMTPSettingsWriter.Update(x =>
                {
                    x.Server = sMTPSettings.Server;
                    x.Port = sMTPSettings.Port;
                    x.SenderName = sMTPSettings.SenderName;
                    x.SenderEmail = sMTPSettings.SenderEmail;
                    x.Username = sMTPSettings.Username;
                    x.Password = sMTPSettings.Password;
                });
                _toastNotification.AddSuccessToastMessage("E-Posta Ayarları başarıyla güncellenmiştir", new ToastrOptions
                {
                    Title = "Başarılı işlem!"
                });
                return View(sMTPSettings);
            }
            return View(sMTPSettings);
        }


        [HttpGet]
        public async Task<IActionResult> ArticleRightSideBarWidgetSettings()
        {
            var categoryResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            var articleRightSideBarWidgetOptionsViewModel = _mapper.Map<ArticleRightSideBarWidgetOptionsViewModel>(_articleRightSideBarWidgetOptions);
            articleRightSideBarWidgetOptionsViewModel.Categories = categoryResult.Data.Categories;
            return View(articleRightSideBarWidgetOptionsViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> ArticleRightSideBarWidgetSettings(ArticleRightSideBarWidgetOptionsViewModel articleRightSideBarWidgetOptionsViewModel)
        {
            var categoryResult = await _categoryService.GetAllByNonDeletedAndActiveAsync();
            articleRightSideBarWidgetOptionsViewModel.Categories = categoryResult.Data.Categories;
            if (ModelState.IsValid)
            {
                _articleRightSideBarWidgetOptionsWriter.Update(x =>
                {
                    x.Header = articleRightSideBarWidgetOptionsViewModel.Header;
                    x.TakeSize = articleRightSideBarWidgetOptionsViewModel.TakeSize;
                    x.FilterBy = articleRightSideBarWidgetOptionsViewModel.FilterBy;
                    x.OrderBy = articleRightSideBarWidgetOptionsViewModel.OrderBy;
                    x.CategoryId = articleRightSideBarWidgetOptionsViewModel.CategoryId;
                    x.IsAscending = articleRightSideBarWidgetOptionsViewModel.IsAscending;
                    x.EndAt = articleRightSideBarWidgetOptionsViewModel.EndAt;
                    x.StartAt = articleRightSideBarWidgetOptionsViewModel.StartAt;
                    x.MaxCommentCount = articleRightSideBarWidgetOptionsViewModel.MaxCommentCount;
                    x.MinCommentCount = articleRightSideBarWidgetOptionsViewModel.MinCommentCount;
                    x.MaxViewCount = articleRightSideBarWidgetOptionsViewModel.MaxViewCount;
                    x.MinViewCount = articleRightSideBarWidgetOptionsViewModel.MinViewCount;
                });
                _toastNotification.AddSuccessToastMessage("Makale Sayfalarinin Widget Ayarları başarıyla güncellenmiştir", new ToastrOptions
                {
                    Title = "Başarılı işlem!"
                });
                return View(articleRightSideBarWidgetOptionsViewModel);
            }
            return View(articleRightSideBarWidgetOptionsViewModel);
        }
    }
}
