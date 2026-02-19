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
        // Retorna una lista de clientes o null si no hay datos (mock)
        Task<List<ClientDto>> GetById(int id);
    }
}
