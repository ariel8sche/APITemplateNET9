using APITemplate.Contracts.Dtos;
using APITemplate.Contracts.Requests;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace APITemplate.Contracts.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAll();

        Task<List<UserDto>> GetById(int userId);

        Task<int> Create(CreateUserRequest request);

        Task<bool> Update(int userId, UpdateUserRequest request);

        Task<bool> Delete(int userId);
    }
}