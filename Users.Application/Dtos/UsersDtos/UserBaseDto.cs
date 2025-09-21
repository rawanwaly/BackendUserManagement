using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Core.Enums;

namespace UserManagement.Application.Dtos.UsersDtos
{
    public class UserBaseDto
    {
        public string FirstNameEN { get; set; }
        public string LastNameEN { get; set; }
        public string FirstNameAR { get; set; }
        public string LastNameAR { get; set; }
        public string Email { get; set; }
        public string? Address { get; set; }
        public string MobileNumber { get; set; }
        public bool isActive { get; set; } = true;
    }
}
