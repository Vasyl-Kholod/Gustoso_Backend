using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Gustoso.Context;
using Gustoso.Common.DTO.Communication;
using Gustoso.Common.DTO.Request;
using Gustoso.Common.DTO.Response.PriceList;
using Gustoso.Common.IServices;

namespace Gustoso.Services
{
    public class PriceListService: IPriceListService
    {
        private readonly MSContext _db;

        public PriceListService(MSContext db)
        {
            _db = db;
        }

        public async Task<Response<IPriceListDTO>> GetAllPrices()
        {
            var response = new Response<IPriceListDTO>();

            var priceList = await _db.MenuItems.ToListAsync();

            if (priceList.ToArray().Length == 0)
            {
                response.Error = new Error(404, "Price list not found");
                return response;
            }

            var data = new PriceListDTO(priceList);

            response.Data = data;
            return response;
        }
    }
}
