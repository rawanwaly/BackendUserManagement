using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Domain.Core.Models;

namespace UserManagement.Application.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
         Task<User?> GetByMobileAsync(string mobile, int? excludeId = null);
         Task<User?> GetByEmailAsync(string email, int? excludeId = null);
    }
}
