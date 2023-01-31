using Domain;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NewApi_app.Helpers;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewApi_app.Controllers {

    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : Controller {

        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AuthController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] Login model) {
            var user = await this._userManager.FindByNameAsync(model.UserName);
            var d = this._userManager.CheckPasswordAsync(user, model.Password);
            if (user != null && await this._userManager.CheckPasswordAsync(user, model.Password)) {
                var userRole = await this._userManager.GetRolesAsync(user);
                var authClaims = new List<Claim> {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                foreach (var role in userRole) {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }

                var token = AuthHelper.GetToken(authClaims,this._configuration);

                return Ok(new {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    expiration = token.ValidTo
                });
            }
            return Unauthorized();
        }




        [HttpPost]
        [Route("regUser")]
        public async Task<IActionResult> RegUser([FromBody] Register model) {
            var userEx = await this._userManager.FindByNameAsync(model.UserName);
            if (userEx != null)
                return StatusCode(StatusCodes.Status500InternalServerError, "User in db already");

            IdentityUser user = new() {
                UserName = model.UserName,
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var res = await this._userManager.CreateAsync(user, model.Password);
            if (!res.Succeeded) { return StatusCode(StatusCodes.Status500InternalServerError, "Creation failed!"); }

            return Ok("User added!");
        }



        [HttpPost]
        [Route("setAdmin")]
        [Authorize(Roles = UserRoles.Admin)] 
        public async Task<IActionResult> SetRole([FromBody] string username, string role) {
           
            var user = await _userManager.FindByNameAsync(username);
            if (user != null) {

                AuthHelper.CheckIfRolesExist(this._roleManager);

                if (await this._roleManager.RoleExistsAsync(role))
                    await this._userManager.AddToRoleAsync(user, role);

                return Ok("Admin added!");
            }
            return StatusCode(404);
        }
    }
}
