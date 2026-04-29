using APITemplate.Contracts.Dtos;
using APITemplate.Contracts.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Interfaces
{
    public interface IAccessTokenService
    {
        Task<List<AccessTokenDto>> GetAll();

        Task<List<AccessTokenDto>> GetById(int accessTokenId);

        Task<int> Create(CreateAccessTokenRequest request);

        Task<bool> Update(int userId, UpdateAccessTokenRequest request);

        Task<bool> Delete(int accessTokenId);
    }
}
