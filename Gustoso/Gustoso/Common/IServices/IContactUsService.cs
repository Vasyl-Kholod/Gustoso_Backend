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
        Task<Response<Dictionary<string, string>>> addMessage(IContactUsDTO obj);
    }
}
