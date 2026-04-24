using APITemplate.Contracts.Dtos;
using System.Collections.Generic;

namespace APITemplate.Contracts.Responses
{
    public class UserListResponse
    {
        public List<UserDto> Users { get; set; } = new();
    }
}