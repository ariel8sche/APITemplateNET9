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
    public interface IClientScopeGrantService
    {
        Task<List<ClientScopeGrantDto>> GetAll();

        Task<List<ClientScopeGrantDto>> GetById(int id);

        Task<int> Create(CreateClientScopeGrantRequest dto);

        Task<bool> Update(int id, UpdateClientScopeGrantRequest dto);

        Task<bool> Delete(int id);

    }
}
