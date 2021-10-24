using ProgrammersBlog.Entities.Concrete;
using ProgrammersBlog.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Areas.Admin.Models
{
    public class DashboardViewModel
    {
        public int CategoriesSize { get; set; }
        public int ArticlesSize { get; set; }
        public int CommentsSize { get; set; }
        public int UsersSize { get; set; }
        public ArticleListDto Articles { get; set; }
    }
}
