using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace NewApi_app.Controllers {

    [ApiController]
    [Route("api/admin/[controller]/[action]")]
    public class AdminController : Controller{

        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
        }

        [HttpGet]
        [Authorize(Roles =UserRoles.Admin)]
        public async Task<IActionResult> GetUsersByRole (string role) {
            if (role == UserRoles.Admin || role == UserRoles.Manager) {
                var list = await this._userManager.GetUsersInRoleAsync(role);
                return Ok(list);
            }
            return StatusCode(StatusCodes.Status400BadRequest);
        }

       

        [HttpPost]
        [Route("removeAdmin")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> RemoveRole(string username, string role) {

            var user = await _userManager.FindByNameAsync(username);
            if (user != null) {

                if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

                if (!await _roleManager.RoleExistsAsync(UserRoles.Manager))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));

                if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                    await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));


                if (await this._roleManager.RoleExistsAsync(role)) {
                    if(await this._userManager.IsInRoleAsync(user, role)){
                        await this._userManager.RemoveFromRoleAsync(user, role);
                    }
                }

                return Ok("Role removed!");
            }
            return StatusCode(404);
        }
    }
}
