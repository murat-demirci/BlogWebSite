using ProgrammersBlog.Entities.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProgrammersBlog.Mvc.Areas.Admin.Models
{
    public class UserRoleAsignAjaxViewModel
    {
        public UserRoleAsignDto UserRoleAsignDto { get; set; }
        public string RoleAsignPartial { get; set; }
        public UserDto UserDto { get; set; }
    }
}
