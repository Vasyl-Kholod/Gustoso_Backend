using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gustoso.Common.DTO.Response.Registration;

namespace Gustoso.Common.DTO.Response.Registration
{
    public class RegResponseDTO : IRegResponseDTO
    {
        public string Message { get; set; }

        public RegResponseDTO() { }

        public RegResponseDTO(string _message)
        {
            Message = _message;
        }
    }
}
