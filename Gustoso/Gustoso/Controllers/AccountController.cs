using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Gustoso.Common.DTO.Request;
using Gustoso.Common.IServices;


namespace Gustoso.Controllers
{
    [Route("api/Account")]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpPost("Registration")]
        public async Task<IActionResult> Registration([FromBody]RegistrationDTO registration)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await this._accountService.Registration(registration);

            if (response.Error != null)
            {
                return StatusCode(response.Error.ErrorCode, response.Error.ErrorDescription);
            }
            return Ok(response.Data.Message);
        }

        [HttpPost("Token")]
        public async Task<IActionResult> Token([FromBody]LoginDTO login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var response = await this._accountService.Token(login);

            if (response.Error != null)
            {
                return StatusCode(response.Error.ErrorCode, response.Error.ErrorDescription);
            }
            return Ok(response.Data);
        }

    }

}