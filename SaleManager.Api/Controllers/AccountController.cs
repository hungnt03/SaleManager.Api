using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SaleManager.Api.Entities;
using SaleManager.Api.Models;
using SaleManager.Api.Models.Account;
using System.Net.Mail;
using System.Net.Mime;

namespace SaleManager.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IConfiguration _configuration;
        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [Authorize(Roles ="admin")]
        [Authorize(Roles = "subAdmin")]
        [HttpGet("users")]
        public IActionResult GetUsers()
        {
            var results = new List<UserViewModel>();
            var users = _userManager.Users.ToList();
            if (users == null || users.Count == 0)
                return NotFound();

            foreach(var user in users)
            {
                results.Add(new UserViewModel() 
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    Level = user.Level,
                    JoinDate = user.JoinDate
                });
            }
            return Ok(results);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]RegisterAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userRegis = new ApplicationUser()
                {
                    UserName = model.Username,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Level = model.Level,
                    JoinDate = DateTime.Now.AddYears(-2)
                };
                var result = await _userManager.CreateAsync(userRegis, model.Password);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    var roles = _userManager.GetRolesAsync(user).Result.ToList();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var token = await GenerateJwtTokenAsync(user, roles);
                    return Ok(token);
                }
                AddErrors(result);
            }

            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("update")]
        public async Task<IActionResult> Update([FromBody]UpdateAccountViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.Level = model.Level;
                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                AddErrors(result);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]UpdatePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                var result = await _userManager.ChangePasswordAsync(user, model.OldPassword, model.Password);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                AddErrors(result);
            }
            return BadRequest(ModelState);
        }

        //TODO
        [HttpPost("resetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody]SingleIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByNameAsync(model.Id);

            if (user == null)
            {
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                // Send email
                    SmtpClient mySmtpClient = new SmtpClient("my.smtp.exampleserver.net");
                    // set smtp-client with basicAuthentication
                    mySmtpClient.UseDefaultCredentials = false;
                    System.Net.NetworkCredential basicAuthenticationInfo = new
                       System.Net.NetworkCredential("username", "password");
                    mySmtpClient.Credentials = basicAuthenticationInfo;

                    // add from,to mailaddresses
                    MailAddress from = new MailAddress("test@example.com", "TestFromName");
                    MailAddress to = new MailAddress(user.Email, "TestToName");
                    MailMessage myMail = new System.Net.Mail.MailMessage(from, to);

                    // add ReplyTo
                    //MailAddress replyto = new MailAddress("reply@example.com");
                    //myMail.ReplyToList.Add(replyTo);

                    // set subject and encoding
                    myMail.Subject = "Cài đặt lại mật khẩu";
                    myMail.SubjectEncoding = System.Text.Encoding.UTF8;

                    // set body-message and encoding
                    myMail.Body = "<b>Test Mail</b><br>using <b><br>"+ token +"HTML</b>.";
                    myMail.BodyEncoding = System.Text.Encoding.UTF8;
                    // text or html
                    myMail.IsBodyHtml = true;

                try
                {
                    mySmtpClient.Send(myMail);
                }
                catch (Exception)
                {
                    return BadRequest(ModelState);
                }
            }
            return Ok();
        }

        //TODO
        [HttpPost("confirmResetPassword")]
        public async Task<IActionResult> ConfirmResetPassword([FromBody]UpdatePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(model.Id);

            if (user != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                AddErrors(result);
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("delete")]
        public async Task<IActionResult> Delete([FromBody]SingleIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                var result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                AddErrors(result);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("getRoles")]
        public async Task<IActionResult> GetRoles([FromBody]SingleIdViewModel model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var roles = new List<string>();
            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                var results = await _userManager.GetRolesAsync(user);
                foreach(var role in results)
                {
                    roles.Add(role);
                }
                if (roles.Count > 0)
                    return Ok(roles);
                else
                    return NotFound();
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("addRole")]
        public async Task<IActionResult> AddToRole([FromBody]RoleModelView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                var result = await _userManager.AddToRoleAsync(user, model.Role);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                AddErrors(result);
            }
            return BadRequest(ModelState);
        }

        [Authorize(Roles = "admin")]
        [HttpPost("deleteRole")]
        public async Task<IActionResult> DeleteToRole([FromBody]RoleModelView model)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var user = await _userManager.FindByIdAsync(model.Id);
            if (user != null)
            {
                var result = await _userManager.RemoveFromRoleAsync(user, model.Role);
                if (result.Succeeded)
                {
                    return Ok(result);
                }
                AddErrors(result);
            }
            return BadRequest(ModelState);
        }

        [Authorize]
        [HttpPost("current")]
        public async Task<IActionResult> Current()
        {
            var user = await _userManager.FindByIdAsync(_userManager.Users.FirstOrDefault().Id);
            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new { User = user, Roles = roles });
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, false, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);
                    var roles = _userManager.GetRolesAsync(user).Result.ToList();
                    var token = await GenerateJwtTokenAsync(user, roles);
                    return Ok(token);
                }
            }

            return BadRequest(ModelState);
        }

        ////[Authorize]
        //[HttpPost("logout")]
        //public async void Signout()
        //{
        //    await _signInManager.SignOutAsync();
        //    await HttpContext.SignOutAsync();
        //    //await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
        //    //await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);
        //    //await HttpContext.SignOutAsync(IdentityConstants.TwoFactorUserIdScheme);
        //    //this.HttpContext.Response.Cookies.Delete(".AspNetCore.Identity.Application");
        //}

        async Task<TokenViewModel> GenerateJwtTokenAsync(ApplicationUser user, List<string> roles)
        {
            var principal = await _signInManager.CreateUserPrincipalAsync(user);

            var claims = new List<Claim>(principal.Claims);
            claims.Add(new Claim(JwtRegisteredClaimNames.Sub, user.UserName));
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Auth0:JwtKey"]));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expires = DateTime.Now.AddDays(1);

            var token = new JwtSecurityToken(
                _configuration["Auth0:JwtIssuer"],
                _configuration["Auth0:JwtIssuer"],
                claims,
                expires: expires,
                signingCredentials: credentials
            );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return new TokenViewModel()
            {
                Token = jwt
            };
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }
    }
}