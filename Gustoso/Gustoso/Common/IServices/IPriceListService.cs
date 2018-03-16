using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gustoso.Common.DTO.Communication;
using Gustoso.Common.DTO.Response.PriceList;

namespace Gustoso.Common.IServices
{
    public interface IPriceListService
    {
        Task<Response<IPriceListDTO>> GetAllPrices();
    }
}
