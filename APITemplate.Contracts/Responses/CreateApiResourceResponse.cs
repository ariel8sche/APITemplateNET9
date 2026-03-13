using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APITemplate.Contracts.Responses
{
    public class CreateApiResourceResponse
    {
        public int ApiResourcePk { get; set; }
        public bool Success { get; set; }

    }

}
