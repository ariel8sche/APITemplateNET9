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

        Task<ClientDto> Create(CreateClientDto dto);

    }
}
