using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gustoso.Common.Models;

namespace Gustoso.Common.DTO.Response.PriceList
{
    public interface IPriceListDTO
    {
        IList<MenuItem> PriceList { get; set; }
    }
}
