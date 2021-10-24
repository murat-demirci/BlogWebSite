using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using ProgrammersBlog.Shared.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Filters
{
    public class MvcExceptionFilter : IExceptionFilter
    {
        private readonly IHostEnvironment _environment;
        private readonly IModelMetadataProvider _metadataProvider;
        private readonly ILogger _logger;

        public MvcExceptionFilter(IModelMetadataProvider metadataProvider, IHostEnvironment environment, ILogger<MvcExceptionFilter> logger)
        {
            _metadataProvider = metadataProvider;
            _environment = environment;
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            if (_environment.IsDevelopment())
            {
                context.ExceptionHandled = true;
                var mvcErrorModel = new MvcErrorModel();
                ViewResult result;
                switch (context.Exception)
                {
                    case SqlNullValueException:
                        mvcErrorModel.Message= $"Beklenmedik bir veritabani hatasi olustu :(";
                        mvcErrorModel.Detail = context.Exception.Message;
                        result = new ViewResult
                        {
                            ViewName = "error"
                        };
                        _logger.LogError(context.Exception,context.Exception.Message);
                        break;
                    case NullReferenceException:
                        mvcErrorModel.Message = $"Beklenmedik bir null veriye rastlandi :(";
                        mvcErrorModel.Detail = context.Exception.Message;
                        result = new ViewResult
                        {
                            ViewName = "error"
                        };
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;
                    default:
                        mvcErrorModel.Message = $"Beklenmedik bir hata olustu :(";
                        mvcErrorModel.Detail = context.Exception.Message;
                        result = new ViewResult
                        {
                            ViewName = "error"
                        };
                        _logger.LogError(context.Exception, context.Exception.Message);
                        break;
                }
                result.StatusCode = 500;
                result.ViewData = new ViewDataDictionary(_metadataProvider, context.ModelState);
                result.ViewData.Add("MvcErrorModel", mvcErrorModel);
                context.Result = result;
            }
        }
    }
}
