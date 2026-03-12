using APITemplate.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Interfaces
{
    public interface IClientService
    {
        Task<List<ClientDto>> GetAll();

        Task<List<ClientDto>> GetById(int id);

        Task<int> Create(CreateClientRequest dto);

        Task<Boolean> Update(int id, UpdateClientRequest dto);

        Task<Boolean> Delete(int id);

    }
}
