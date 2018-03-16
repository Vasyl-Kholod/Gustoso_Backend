using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Gustoso.Services;
using Gustoso.Common.IServices;
using Microsoft.AspNetCore.Authorization;

namespace Gustoso.Controllers
{
    [Authorize("Bearer")]
    [Route("api/PriceList")]
    public class PriceListController : Controller
    {
        private readonly IPriceListService _pricelistService;
        public PriceListController(IPriceListService pricelistService)
        {
            _pricelistService = pricelistService;
        }

        [HttpGet("GetAllPrices")]
        public async Task<IActionResult> GetAllPrices()
        {
            var response = await _pricelistService.GetAllPrices();
            if (response.Error != null)
            {
                return StatusCode(response.Error.ErrorCode, response.Error.ErrorDescription);
            }
            return Ok(response.Data.PriceList);
        }
    }
}