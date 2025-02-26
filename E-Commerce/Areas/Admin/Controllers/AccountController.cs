using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using E_Commerce.DTO;
using E_Commerce.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;


namespace E_Commerce.Areas.Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IConfiguration _config;


        public AccountController(UserManager<IdentityUser> userManager, IConfiguration config)
        {
            _userManager = userManager;
            _config = config;
        }

        //[HttpPost("register/admin")]
        ////[Authorize(Roles = "Admin")]
        //public async Task<IActionResult> RegisterAdmin([FromBody] RegisterAdminDto userDto)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    var user = new IdentityUser
        //    {
        //        UserName = userDto.UserName,
        //        Email = userDto.Email
        //    };

        //    var result = await _userManager.CreateAsync(user, userDto.Password);
        //    if (result.Succeeded)
        //    {

        //        var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
        //        if (!await roleManager.RoleExistsAsync("Admin"))
        //        {
        //            await roleManager.CreateAsync(new IdentityRole("Admin"));
        //        }


        //        await _userManager.AddToRoleAsync(user, "Admin");

        //        return Ok(new { Message = "Admin registered successfully" });
        //    }

        //    return BadRequest(result.Errors);
        //}
        [HttpPost("Customer register")]
        public async Task<IActionResult> CustomerRegisteration(RegisterUserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = new IdentityUser
            {
                UserName = userDto.UserName,
                Email = userDto.Email
            };
            

            var result = await _userManager.CreateAsync(user, userDto.Password);
            if (result.Succeeded)
            {

                var roleManager = HttpContext.RequestServices.GetRequiredService<RoleManager<IdentityRole>>();
                if (!await roleManager.RoleExistsAsync("Customer"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Customer"));
                }

                // Assign the Custmerr role
                await _userManager.AddToRoleAsync(user, "Customer");

                return Ok(new { Message = "Customer registered successfully" });
            }

            return BadRequest(result.Errors);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginUserDto userDto)
        {
            if (ModelState.IsValid)
            {
                // Check and create token
                var user = await _userManager.FindByNameAsync(userDto.UserName);
                if (user != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(user, userDto.Password);
                    if (found)
                    {
                        // Claims token
                        var myclaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                        // Get Roles
                        var roles = await _userManager.GetRolesAsync(user);
                        foreach (var role in roles)
                        {
                            myclaims.Add(new Claim(ClaimTypes.Role, role));
                        }

                        // Signature
                        SecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
                        SigningCredentials signincreds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

                        // Create Token
                        JwtSecurityToken mytoken = new JwtSecurityToken(
                            issuer: _config["JWT:ValidIssuer"],
                            audience: _config["JWT:ValidAudience"],
                            claims: myclaims,
                            expires: DateTime.Now.AddMinutes(60),
                            signingCredentials: signincreds
                        );

                        return Ok(new
                        {
                            token = new JwtSecurityTokenHandler().WriteToken(mytoken),
                            expiration = mytoken.ValidTo,
                        });
                    }
                }
                return Unauthorized();
            }
            return BadRequest(ModelState);
        }

    }
    }

        



