using Domain;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NewApi_app.Helpers {
    public class AuthHelper {
       
        internal static JwtSecurityToken GetToken(List<Claim> claimsList, IConfiguration _configuration) {
            var signKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(6),
                    claims: claimsList,
                    signingCredentials: new SigningCredentials(signKey, SecurityAlgorithms.HmacSha256)
                );

            return token;
        }

        internal async static void CheckIfRolesExist(RoleManager<IdentityRole> _roleManager) {
            if (!await _roleManager.RoleExistsAsync(UserRoles.Admin))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));

            if (!await _roleManager.RoleExistsAsync(UserRoles.Manager))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.Manager));

            if (!await _roleManager.RoleExistsAsync(UserRoles.User))
                await _roleManager.CreateAsync(new IdentityRole(UserRoles.User));
        }
    }
}
