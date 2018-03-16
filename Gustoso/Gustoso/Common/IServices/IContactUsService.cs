using Gustoso.Common.DTO.Communication;
using Gustoso.Common.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Common.IServices
{
    public interface IContactUsService
    {
        Task<Response<string>> addMessage(IContactUsDTO obj);

        Task sendToEmail(IContactUsDTO obj);
    }
}
