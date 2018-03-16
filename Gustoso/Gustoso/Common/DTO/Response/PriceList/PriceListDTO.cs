using System.Collections.Generic;
using Gustoso.Common.DTO.Response.PriceList;
using Gustoso.Common.Models;

namespace Gustoso.Common.DTO.Request
{
    public class PriceListDTO: IPriceListDTO
    {
        public IList<MenuItem> PriceList { get; set; }

        public PriceListDTO() { }

        public PriceListDTO(IList<MenuItem> list)
        {
            PriceList = list;
        }
    }
}
