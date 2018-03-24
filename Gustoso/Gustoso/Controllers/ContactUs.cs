using Gustoso.Common.DTO.Request;
using Gustoso.Common.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Gustoso.Controllers
{
    [Authorize("Bearer")]
    [Route("api/ContactUs")]
    public class ContactUs : Controller
    {
        private readonly IContactUsService _contactUsService;
        public ContactUs(IContactUsService service)
        {
            _contactUsService = service;
        }

        [HttpPost]
        public async Task<IActionResult> AddMessage([FromBody]ContactUsDTO obj)
        {
            if(!ModelState.IsValid)
            {
                return BadRequest("Model is not valid!");
            }
            var response = await _contactUsService.addMessage(obj);
            return Ok(response.Data);
        }
    }
}
