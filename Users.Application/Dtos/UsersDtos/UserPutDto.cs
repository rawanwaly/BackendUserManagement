using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UserManagement.Application.Dtos.UsersDtos
{
    public class UserPutDto : UserBaseDto
    {
        public int Id { get; set; }
        public int MaritalStatus { get; set; }

    }
}
