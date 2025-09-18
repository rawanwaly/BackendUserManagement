using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Dtos.UsersDtos;
using UserManagement.Application.Shared;

namespace UserManagement.Application.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserGetDto>> GetAllAsync();
        Task<List<int>> GetAllIdsAsync(string? search = null);
        Task<UserGetDto?> GetByIdAsync(int id);
        Task<UserGetDto> AddAsync(UserPostDto dto);
        Task<UserGetDto?> UpdateAsync(int id, UserPutDto dto);
        Task<bool> DeleteAsync(int id);
        Task<bool> IsEmailExistsAsync(string email, int? excludeId = null);
        Task<bool> IsMobileExistsAsync(string mobile, int? excludeId = null);
        Task<PagedResult<UserGetDto>> GetPagedUsersAsync(GridParams gridParams);


    }
}
