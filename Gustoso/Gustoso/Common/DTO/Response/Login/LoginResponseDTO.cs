using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gustoso.Common.DTO.Response.Login;

namespace Gustoso.Common.DTO.Response.Login
{
    public class LoginResponseDTO: ILoginResponseDTO
    {
        public string Token { get; set; }

        public string ExpirationTime { get; set; }

        public string Role { get; set; }

        public LoginResponseDTO() { }

        public LoginResponseDTO(string _token, string _expirationTime, string role)
        {
            Token = _token;
            ExpirationTime = _expirationTime;
            Role = role;
        }

    }
}
