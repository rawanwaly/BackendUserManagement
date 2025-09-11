using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Interfaces;
using UserManagement.Domain.Core.Models;

namespace UserManagement.Infrastructure.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        private readonly AppDbContext _context;

        public UserRepository(AppDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<User?> GetByEmailAsync(string email, int? excludeId = null)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && (!excludeId.HasValue || u.Id != excludeId.Value));
        }

        public async Task<User?> GetByMobileAsync(string mobile, int? excludeId = null)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.MobileNumber == mobile && (!excludeId.HasValue || u.Id != excludeId.Value));
        }

    }
}
