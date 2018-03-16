using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Gustoso.Context;
using Microsoft.AspNetCore.Identity;
using Gustoso.Common.Models;
using Gustoso.Common.DTO.Communication;
using Gustoso.Common.Interfaces;
using Gustoso.Common.DTO.Response.Registration;
using Gustoso.Common.DTO.Response.Login;
using Gustoso.Common.IServices;
using Gustoso.Common.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Gustoso.Services
{
    public class AccountService: IAccountService
    {
        private readonly UserManager<User> _userManager;
        private readonly MSContext _db;
        public AccountService(UserManager<User> userManager, MSContext context)
        {
            _userManager = userManager;
            _db = context;
        }

        public async Task<Response<IRegResponseDTO>> Registration(IRegistrationDTO registration)
        {
            var response = new Response<IRegResponseDTO>();

            var isEmailAlreadyUsed = await _userManager.Users.AnyAsync(p => p.Email == registration.Email);
            if (isEmailAlreadyUsed)
            {
                response.Error = new Error(418, "Даний email вже використовується.");
                return response;
            }

            DateTime dateNow = DateTime.UtcNow;

            var user = new User {
                Email = registration.Email,
                UserName = registration.Email,
                FirstName = registration.FirstName,
                LastName = registration.LastName,
                Phone = registration.Phone,
                DateOfRegistration = dateNow.ToString()
            };

            // добавляем пользователя
            var resultCreateUser = await _userManager.CreateAsync(user, registration.Password);
            var resultCreateRole = await _userManager.AddToRoleAsync(user, Roles.User.ToString());

            if (resultCreateUser.Succeeded && resultCreateRole.Succeeded)
            {
                var message = new RegResponseDTO("Вітаємо в системі!");
                response.Data = message;
            }
            else
            {
                response.Error = new Error(400, "Помилка реєстрації!");
                return response;
            }

            return response;
        }

        public async Task<Response<ILoginResponseDTO>> Token(ILoginDTO loginObj)
        {
            var response = new Response<ILoginResponseDTO>();

            var user = await GetIdentity(loginObj.Login, loginObj.Password);

            if(user == null)
            {
                response.Error = new Error(404, "Користувач не знайдений, або ви ввели некорекні дані");
                return response;
            }

            var now = DateTime.UtcNow;

            var expires = now.Add(TimeSpan.FromMinutes(AuthOptions.LIFETIME));

            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: user.Claims,
                    expires: expires,
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var loginResponse = new LoginResponseDTO(encodedJwt, expires.ToString());

            response.Data = loginResponse;

            return response;
        }

        private async Task<ClaimsIdentity> GetIdentity(string username, string password)
        {
            User person = await _db.Users.AsNoTracking().FirstOrDefaultAsync(p => p.Email == username);

            if (!await _userManager.CheckPasswordAsync(person, password))
            {
                return null;
            }

            var role = await _userManager.GetRolesAsync(person);

            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Email),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // якщо не знайдено користувача
            return null;
        }
    }
}
