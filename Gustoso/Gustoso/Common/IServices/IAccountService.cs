using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gustoso.Common.DTO.Communication;
using Gustoso.Common.DTO.Response.Registration;
using Gustoso.Common.DTO.Response.Login;
using Gustoso.Common.Interfaces;

namespace Gustoso.Common.IServices
{
    public interface IAccountService
    {
        Task<Response<IRegResponseDTO>> Registration(IRegistrationDTO registration);

        Task<Response<ILoginResponseDTO>> Token(ILoginDTO loginObj);
    }
}
