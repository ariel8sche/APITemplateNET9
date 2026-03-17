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
    public interface IClientSecretService
    {
        Task<List<ClientSecretDto>> GetAll();

        Task<List<ClientSecretDto>> GetById(int id);

        Task<int> Create(CreateClientSecretRequest dto);

        Task<bool> Update(int id, UpdateClientSecretRequest dto);

        Task<bool> Delete(int id);

    }
}
