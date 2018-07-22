using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegitrationAPI.Model;
using RegitrationAPI.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Web;
using RegitrationAPI.Extention;
using System.Net.Mail;
using Telegram.Bot;
using System.Net.Http;
using System.Net.Http.Headers;

namespace RegitrationAPI.Controllers
{
    //[EnableCors("SiteCorsPolicy")]
    [Produces("application/json")]
    [Route("api/[controller]")]
    public class AccountController : Controller
    {
        #region Other
        private readonly IHttpContextAccessor _accessor;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        #endregion

        #region Ctor
        public AccountController(UserManager<ApplicationUser> userManager, ApplicationDbContext context, RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor accessor)
        {
            try
            {
                _userManager = userManager;
                _context = context;
                _accessor = accessor;
                _roleManager = roleManager;
            }
            catch (Exception)
            {
            }
        }
        #endregion

        #region Register
        [HttpPost]
        [Route("Register")]
        public async Task<IdentityResult> Register([FromBody]AccountModel model)
        {
            var result = IdentityResult.Failed(new IdentityError()
            {
                Code = "UnknownError",
                Description = "شما ثبت نام نشدید. لطفا بعدا امتحان کنید"
            });

            try
            {
                #region IF Model Is Null
                if (model == null)
                {
                    return IdentityResult.Failed(new IdentityError()
                    {
                        Code = "NullValue",
                        Description = "لطفا تمام فیلد هارا پر کنید"
                    });
                }

                if (model.Email == null || model.UserName == null || model.FirstName == null || model.LastName == null || model.Email == "" ||
                    model.UserName == "" || model.FirstName == "" || model.LastName == "")
                {
                    return IdentityResult.Failed(new IdentityError()
                    {
                        Code = "NullValue",
                        Description = "لطفا تمام فیلد هارا پر کنید"
                    });
                }
                #endregion

                #region custom validation
                if (model.UserName.Length < 5)
                {
                    return IdentityResult.Failed(new IdentityError()
                    {
                        Code = "UserLength",
                        Description = "نام کاربری باید بالای 5 حرف باشد"
                    });
                }
                #endregion

                #region Create user and save
                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.UserName,
                    Email = model.Email,
                    LastName = model.LastName,
                    FirstName = model.FirstName,
                    SecurityStamp = Guid.NewGuid().ToString()
                };
                result = await _userManager.CreateAsync(user, model.Password);
                #endregion

                #region set roles
                if (user.UserName.ToUpper() == "ADMIN")
                {
                    await _userManager.AddToRoleAsync(user, "Admin");
                    await _userManager.AddToRoleAsync(user, "Manager");
                    await _userManager.AddToRoleAsync(user, "User");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
                #endregion

                #region sendEmailConfirm
                try
                {
                    string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = HttpUtility.UrlEncode(code);
                    Email.SendEmailAfterRegistration(user.Email, user.UserName, model.Password, code, user.FirstName);
                }
                catch (Exception)
                {
                }
                #endregion
            }
            catch (Exception)
            {
            }

            return result;
        }
        #endregion

        #region Login
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody]LoginModel model)
        {
            try
            {
                #region Create JWT Token
                var user = await _userManager.FindByNameAsync(model.UserName);
                var CheckPassword = await _userManager.CheckPasswordAsync(user, model.Password);

                if (user != null && CheckPassword)
                {
                    var userRole = await _userManager.GetRolesAsync(user);

                    var claims = new List<Claim>
                {
                new Claim("UserName", user.UserName),
                new Claim("Email", user.Email),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("LoggedOn", DateTime.Now.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };
                    foreach (var item in userRole)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, item));
                    }
                    var signinkey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("KLHIUYH*&6876ty87toi7uyt87**/f+9ffdefg"));

                    var Token = new JwtSecurityToken(
                        expires: DateTime.Now.AddHours(12),
                        claims: claims,
                        audience: "mahdihajian.ir",
                        issuer: "mahdihajian.ir",
                        signingCredentials: new SigningCredentials(signinkey, SecurityAlgorithms.HmacSha256)
                        );
                    #endregion

                    #region add token to dataBase and remove old token
                    try
                    {
                        UserTokenValidation userToken = new UserTokenValidation(DateTime.Now.AddHours(12), model.Ip)
                        {
                            User = user
                        };
                        _context.UserTokenValidations.Add(userToken);
                        await _context.SaveChangesAsync();
                        //
                        var ExpireTokens = await _context.UserTokenValidations.Where(c => c.ExpireDateTime < DateTime.Now).ToListAsync();
                        _context.UserTokenValidations.RemoveRange(ExpireTokens);
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception)
                    {

                    }
                    #endregion

                    #region Return token
                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(Token),
                        expieaion = Token.ValidTo.ToLocalTime(),
                        Message = "شما با موفقیت وارد شدید"
                    });
                    #endregion

                }
            }
            catch (Exception)
            {
            }

            return Unauthorized();
        }
        #endregion

        #region GetDetails
        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("GetDetails")]
        public List<string> GetDetails([FromHeader] string Authorization)
        {
            List<string> lstDetails = null;
            try
            {
                var Ramovebearer = Authorization.Replace("bearer ", "");
                var Token = new JwtSecurityToken(Ramovebearer);
                var ClaimsLST = Token.Claims;
                lstDetails = new List<string>();
                foreach (var item in ClaimsLST)
                {
                    lstDetails.Add(item.Type + " : " + item.Value);
                }

                for (int i = 0; i < ClaimsLST.Count(); i++)
                {
                    if (i > 4)
                    {
                        lstDetails.RemoveAt(5);
                    }
                }
            }
            catch (Exception)
            {
            }

            return lstDetails;
        }
        #endregion

        #region IsUserLogin
        [Authorize]
        [HttpGet]
        [Route("IsUserLogin")]
        public bool IsUserLogin()
        {
            return true;
        }
        #endregion

        #region ConfirmEmail
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IdentityResult> ConfirmEmail([FromHeader] string UserName, [FromHeader] string Token)
        {
            var result = IdentityResult.Failed(new IdentityError()
            {
                Code = "InvalidUserName",
                Description = "این نام کاربری ثبت نشده است"
            });

            try
            {
                var user = await _userManager.FindByNameAsync(UserName);
                if (user != null)
                {
                    result = await _userManager.ConfirmEmailAsync(user, Token);
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        #endregion

        #region ResetPassword
        [HttpGet]
        [Route("ResetPassword")]
        public async Task<IdentityResult> ResetPassword([FromHeader] string UserName, [FromHeader] string Token, [FromHeader] string NewPassword)
        {
            var result = IdentityResult.Failed(new IdentityError()
            {
                Code = "InvalidUserName",
                Description = "این نام کاربری ثبت نشده است"
            });
            try
            {
                var user = await _userManager.FindByNameAsync(UserName);
                if (user != null)
                {
                    result = await _userManager.ResetPasswordAsync(user, Token, NewPassword);

                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        #endregion

        #region RequestForResetPassword
        [HttpGet]
        [Route("RequestForResetPassword")]
        public async Task<IdentityResult> RequestForResetPassword([FromHeader] string email)
        {
            var result = IdentityResult.Failed(new IdentityError()
            {
                Code = "InvalidEmail",
                Description = "این ایمیل در سیستم ثبت نشده است"
            });
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    string code = await _userManager.GeneratePasswordResetTokenAsync(user);
                    code = HttpUtility.UrlEncode(code);
                    Email.SendEmailForgotPassword(user.Email, user.FirstName, user.UserName, code);
                    return IdentityResult.Success;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        #endregion
    }
}