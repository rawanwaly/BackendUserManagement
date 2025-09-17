using Microsoft.EntityFrameworkCore;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Shared;
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

        public async Task<PagedResult<User>> GetPagedUsersAsync(GridParams gridParams)
        {
            var query = _context.Users.AsQueryable();

            // Apply Search
            if (!string.IsNullOrWhiteSpace(gridParams.Search))
            {
                string search = gridParams.Search.ToLower();
                query = query.Where(u =>
                    u.FirstNameEN.ToLower().Contains(search) ||
                    u.LastNameEN.ToLower().Contains(search) ||
                    u.FirstNameAR.ToLower().Contains(search) ||
                    u.LastNameAR.ToLower().Contains(search) ||
                    u.Email.ToLower().Contains(search) ||
                    u.MobileNumber.Contains(search)
                );
            }

            // Server-side sorting
            if (!string.IsNullOrEmpty(gridParams.SortColumn))
            {
                var property = char.ToUpper(gridParams.SortColumn[0]) + gridParams.SortColumn.Substring(1);

                bool ascending = gridParams.SortDirection.ToLower() == "asc";
                query = ascending
                    ? query.OrderBy(e => EF.Property<object>(e, property))
                    : query.OrderByDescending(e => EF.Property<object>(e, property));
            }

            int total = await query.CountAsync();

            var data = await query
                .Skip((gridParams.Page - 1) * gridParams.PageSize)
                .Take(gridParams.PageSize)
                .ToListAsync();

            return new PagedResult<User>
            {
                Data = data,
                TotalRecords = total
            };
        }
    }
}
