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
    [Route("api/Reservation")]
    public class ReservationController : Controller
    {
        private readonly IReservationService _service;

        public ReservationController(IReservationService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetUnConfirmedReservation()
        {
            var response = await _service.GetReservationListAsync();
            if (response.Error != null)
            {
                return StatusCode(response.Error.ErrorCode, response.Error.ErrorDescription);
            }
            return Ok(response.Data);
        }

        [HttpGet("GetActiveReservation")]
        public async Task<IActionResult> GetActiveReservation()
        {
            var response = await _service.GetActiveReservationListAsync();
            if (response.Error != null)
            {
                return StatusCode(response.Error.ErrorCode, response.Error.ErrorDescription);
            }
            return Ok(response.Data);
        }

        [HttpPost]
        public async Task<IActionResult> CreateReservation([FromBody] ReservationDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid!");
            }
            var response = await _service.CreateReservationAsync(dto);
            if (response.Error != null)
            {
                return StatusCode(response.Error.ErrorCode, response.Error.ErrorDescription);
            }
            return Ok(response.Data);
        }

        [HttpPut("{reservationId}")]
        public async Task<IActionResult> ChangeStatus([FromRoute] int reservationId, [FromBody] Boolean isCanceled)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Model is not valid!");
            }
            var response = await _service.ChangeStatusAsync(reservationId, isCanceled);
            return Ok(response.Data);
        }
    }
}