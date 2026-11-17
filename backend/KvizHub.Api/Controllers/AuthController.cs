using KvizHub.Api.Data.Entities;
using KvizHub.Shared.Auth;
using KvizHub.Shared.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace KvizHub.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager) : ControllerBase
    {
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<AuthUserDto>> Register(RegisterRequest request)
        {
            // Administrators are created only on startup, never through registration
            if (request.Role is not (UserRole.Quizmaster or UserRole.TeamLeader))
            {
                ModelState.AddModelError(nameof(request.Role), "Izberite vlogo kvizmaster ali vodja ekipe.");
                return ValidationProblem(ModelState);
            }

            var user = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                DisplayName = request.DisplayName,
                Role = request.Role
            };

            var result = await userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                // The user name equals the email, so duplicate email and user name errors share one message
                foreach (var error in result.Errors.DistinctBy(e => e.Description))
                {
                    var field = error.Code.StartsWith("Password") ? nameof(request.Password) : nameof(request.Email);
                    ModelState.AddModelError(field, error.Description);
                }
                return ValidationProblem(ModelState);
            }

            await signInManager.SignInAsync(user, isPersistent: false);
            return Ok(new AuthUserDto(user.Email!, user.DisplayName, user.Role));
        }
    }
}