using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Domain.Models;

namespace NewApi_app.Controllers {

    [ApiController]
    [Route("api/admin/[controller]/[action]")]
    public class AdminController : Controller {

        private readonly IConfiguration _configuration;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminController(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration) {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._configuration = configuration;
        }

        [HttpGet]
        [Route("GetUsersWithRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> GetUsersWithRole() {

            List<UserInfo> userInfos = new List<UserInfo>();
            foreach (var role in this._roleManager.Roles.ToList()) {
                if (role.Name != "User") {
                    var list = await this._userManager.GetUsersInRoleAsync(role.Name);
                    foreach (var user in list) {
                        if (!userInfos.Any(x => x.Name.Equals(user.UserName))) {
                            userInfos.Add(new UserInfo(user.UserName, user.Email, await this._userManager.GetRolesAsync(user)));
                        }
                    }
                }
            }


            return Ok(userInfos);
        }

        [Route("acl/getRoles")]
        [Authorize(Roles = UserRoles.Admin)]
        [HttpGet]
        public async Task<IActionResult> GetAllRoles() {
            return Ok(this._roleManager.Roles);
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
                    if (await this._userManager.IsInRoleAsync(user, role)) {
                        await this._userManager.RemoveFromRoleAsync(user, role);
                    }
                }

                return Ok("Role removed!");
            }
            return StatusCode(404);
        }
    }
}
