using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ProgrammersBlog.Data.Abstract;
using ProgrammersBlog.Data.Concrete;
using ProgrammersBlog.Data.Concrete.EntityFramework.Contexts;
using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Services.Abstract;
using ProgrammersBlog.Services.Concrete;

namespace ProgrammersBlog.Services.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection LoadMyServices(this IServiceCollection serviceCollection,string conncectionString)
        {
            serviceCollection.AddDbContext<ProgrammersBlogContext>(options=>options.UseSqlServer
                (conncectionString).UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking));//Sorgu hizlandirma tüm baglantili verileri cekmiyor
            serviceCollection.AddIdentity<User,Role>(options=> {
                //Password
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequiredLength = 5;
                options.Password.RequiredUniqueChars = 0;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireLowercase = false;
                //User,Username and Email
                options.User.RequireUniqueEmail = true;
                options.User.AllowedUserNameCharacters = "asdfghjklyxcvbnmqwertzuiopQWERTZUIOPASDFGHJKLYXCVBNM0123456789.@+,_-!?";
            }).AddEntityFrameworkStores<ProgrammersBlogContext>();
            serviceCollection.Configure<SecurityStampValidatorOptions>(options =>
            {
                options.ValidationInterval = TimeSpan.FromMinutes(15);
            });
            serviceCollection.AddScoped<IUnitOfWork, UnitOfWork>();
            serviceCollection.AddScoped<ICategoryService, CategoryManager>();
            serviceCollection.AddScoped<IArticleService, ArticleManager>();
            serviceCollection.AddScoped<ICommentService, CommentManager>();
            serviceCollection.AddSingleton<IMailService, MailManager>();
            return serviceCollection;
        }
    }
}
