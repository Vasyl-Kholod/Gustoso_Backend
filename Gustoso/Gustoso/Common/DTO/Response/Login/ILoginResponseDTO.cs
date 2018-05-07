using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.DTO.Response.Login
{
    public interface ILoginResponseDTO
    {
        string Token { get; set; }

        string ExpirationTime { get; set; }

        string Role { get; set; }
    }
}
