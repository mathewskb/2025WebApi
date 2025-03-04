using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using NZWalks.API.Interfaces;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Services;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UserManager<IdentityUser> userManager, ITokenService tokenService) : ControllerBase
    {
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterRequestDto registerRequestDto)
        {
            var identityUser = new IdentityUser
            {
                UserName = registerRequestDto.Username,
                Email = registerRequestDto.Username
            };



            var identityResult = await userManager.CreateAsync(identityUser, registerRequestDto.Password);

            if (identityResult.Succeeded)
            {
                // Add roles to user
                if (registerRequestDto.Roles != null && registerRequestDto.Roles.Any())
                {
                    identityResult = await userManager.AddToRolesAsync(identityUser, registerRequestDto.Roles);

                    if (identityResult.Succeeded)
                    {
                        return Ok("user has been registered. please try login");
                    }


                }


            }

            return BadRequest("Something went wrong.");
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequestDto loginRequestDto)
        {
            var identityUser = await userManager.FindByEmailAsync(loginRequestDto.Username);

            if (identityUser == null) 
                return BadRequest("no user exist with this Username");

            var checkPasswordValid = await userManager.CheckPasswordAsync(identityUser, loginRequestDto.Password);

            if (checkPasswordValid) 
            {
                
                var roles = await userManager.GetRolesAsync(identityUser);
                
                if (roles == null) return BadRequest("No roles assigned to this user");

                var token = tokenService.CreateToken(identityUser, roles.ToList());
                // login for creating token

                var loginresponse = new LoginResponseDto() 
                { 
                    Token = token 
                };
                return Ok(loginresponse);


            }
            else
            {
                return BadRequest("Password not matching");
            }

        }
    }
}
