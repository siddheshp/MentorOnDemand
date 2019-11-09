using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MOD.AuthLibrary.Models;
using MOD.AuthService.Dtos;

namespace MOD.AuthService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<MODUser> signInManager;
        private readonly UserManager<MODUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration configuration;

        public AccountController(UserManager<MODUser> userManager,
            SignInManager<MODUser> signInManager,
            RoleManager<IdentityRole> roleManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
            this.configuration = configuration;
        }

        [Route("login")]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await signInManager.PasswordSignInAsync(
                model.Email, model.Password, false, false);

            if (result.Succeeded)
            {
                var appUser = userManager.Users.SingleOrDefault(
                    r => r.Email == model.Email);
                var response = await GenerateJwtToken(model.Email, appUser);
                return Ok(response);
            }
            return BadRequest(result);
        }

        [Route("logout")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LoginDto model)
        {
            try
            {
                await signInManager.SignOutAsync();
            }
            catch (Exception)
            {
                //InternalServerError
                return StatusCode(StatusCodes.Status500InternalServerError,
                    "Logout failed");
            }

            return Ok();
        }

        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] RegisterDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new MODUser
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                UserName = model.Email,
                Email = model.Email
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                //role
                var roleName = roleManager.Roles.FirstOrDefault(
                    r => r.Id == model.Role.ToString()).NormalizedName;
                var reslt1 = await userManager.AddToRoleAsync(user, roleName);
                if (reslt1.Succeeded)
                {
                    return Created("Register", new { Message = "Registered Successfully"});
                }
                return BadRequest(reslt1.Errors);
            }

            return BadRequest(result.Errors);
        }

        private async Task<TokenDto> GenerateJwtToken(string email,
            IdentityUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration["JwtKey"]));
            var creds = new SigningCredentials(key,
                SecurityAlgorithms.HmacSha256);
            // recommednded is 5 mins
            var expires = DateTime.Now.AddDays(
                Convert.ToDouble(configuration["JwtExpireDays"]));

            var token = new JwtSecurityToken(
                configuration["JwtIssuer"],
                configuration["JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: creds
            );

            var response = new TokenDto
            {
                Email = email,
                Token = new JwtSecurityTokenHandler().WriteToken(token)
            };

            return response;
        }
    }
}