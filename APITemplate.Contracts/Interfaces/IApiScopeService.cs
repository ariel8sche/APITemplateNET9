using APITemplate.Contracts.Dtos;
using APITemplate.Contracts.Requests;
using APITemplate.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Interfaces
{
    public interface IApiScopeService
    {
        Task<List<ApiScopeDto>> GetAll();

        Task<List<ApiScopeDto>> GetById(int id);

        Task<int> Create(CreateApiScopeRequest dto);

        Task<bool> Update(int id, UpdateApiScopeRequest dto);

        Task<bool> Delete(int id);

    }
}
