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
    public interface IApiResourceService
    {
        Task<List<ApiResourceDto>> GetAll();

        Task<List<ApiResourceDto>> GetById(int id);

        Task<int> Create(CreateApiResourceRequest dto);

        Task<bool> Update(int id, UpdateApiResourceRequest dto);

        Task<bool> Delete(int id);

    }
}
