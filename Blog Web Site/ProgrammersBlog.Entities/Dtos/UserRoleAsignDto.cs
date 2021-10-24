using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProgrammersBlog.Entities.Dtos
{
    public class UserRoleAsignDto
    {
        public UserRoleAsignDto()
        {
            RoleAsignDtos= new List<RoleAsignDto>();
        }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public IList<RoleAsignDto> RoleAsignDtos { get; set; }
    }
}
