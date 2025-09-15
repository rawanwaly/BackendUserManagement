using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UserManagement.Application.Dtos.UsersDtos;
using UserManagement.Application.Interfaces;
using UserManagement.Application.Shared;
using UserManagement.Domain.Core.Models;

namespace UserManagement.Infrastructure.Repositories
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repo;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<IEnumerable<UserGetDto>> GetAllAsync()
        {
            var users = await _repo.GetAllAsync();
            return _mapper.Map<IEnumerable<UserGetDto>>(users);
        }

        public async Task<UserGetDto?> GetByIdAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            return _mapper.Map<UserGetDto?>(user);
        }

        public async Task<UserGetDto> AddAsync(UserPostDto dto)
        {
            var user = _mapper.Map<User>(dto);
            await _repo.AddAsync(user);
            await _repo.SaveChangesAsync();
            return _mapper.Map<UserGetDto>(user);
        }

        public async Task<UserGetDto?> UpdateAsync(int id, UserPutDto dto)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return null;

            _mapper.Map(dto, user);
            _repo.Update(user);
            await _repo.SaveChangesAsync();

            return _mapper.Map<UserGetDto>(user);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return false;

            _repo.Remove(user);
            await _repo.SaveChangesAsync();
            return true;
        }
        public async Task<bool> IsEmailExistsAsync(string email, int? excludeId = null)
        {
            var user = await _repo.GetByEmailAsync(email, excludeId);
            return user != null;
        }

        public async Task<bool> IsMobileExistsAsync(string mobile, int? excludeId = null)
        {
            var user = await _repo.GetByMobileAsync(mobile, excludeId);
            return user != null;
        }
        public async Task<PagedResult<UserGetDto>> GetPagedUsersAsync(GridParams gridParams, bool useServerSide)
        {
            var pagedUsers = await _repo.GetPagedUsersAsync(gridParams, useServerSide);

            return new PagedResult<UserGetDto>
            {
                Data = _mapper.Map<IEnumerable<UserGetDto>>(pagedUsers.Data),
                TotalRecords = pagedUsers.TotalRecords
            };
        }

    }

}
