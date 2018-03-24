using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gustoso.Common.DTO.Request;
using Gustoso.Common.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Gustoso.Controllers
{
    [Authorize("Bearer")]
    [Route("api/Rating")]
    public class RatingController : Controller
    {
        private readonly IRatingService _service;

        public RatingController(IRatingService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRatingsByUser()
        {
            var userName = User.Identity.Name;
            var response = await _service.GetAllRatingAsync(userName);
            if (response.Error != null)
            {
                return StatusCode(response.Error.ErrorCode, response.Error.ErrorDescription);
            }
            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> SetRatingByUser([FromBody]RatingDTO ratingDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid!");
            }
            var userName = User.Identity.Name;
            var response = await _service.SetRatingAsync(userName, ratingDTO);
            if (response.Error != null)
            {
                return StatusCode(response.Error.ErrorCode, response.Error.ErrorDescription);
            }
            return Ok(response.Data);
        }

    }
}