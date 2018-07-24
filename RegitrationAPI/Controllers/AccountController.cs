using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegitrationAPI.Model;
using RegitrationAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.Web;
using RegitrationAPI.Extention;
using System.Security.Cryptography;
using System.IO;

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
                    await _userManager.AddToRoleAsync(user, "Leader");
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
                if (result == IdentityResult.Success)
                {
                    try
                    {
                        string code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        code = HttpUtility.UrlEncode(code);
                        Email.SendEmailAfterRegistration(user.Email, user.UserName, model.Password, code, user.FirstName, user.Id);
                    }
                    catch (Exception)
                    {
                    }
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
        [Authorize(Roles = "user")]
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

        #region ConfirmEmail
        [HttpGet]
        [Route("ConfirmEmail")]
        public async Task<IdentityResult> ConfirmEmail([FromQuery] string userId, [FromHeader] string Token)
        {
            var result = IdentityResult.Failed(new IdentityError()
            {
                Code = "InvalidUserName",
                Description = "این لینک معتبر نمیباشد"
            });

            try
            {
                var user = await _userManager.FindByIdAsync(userId);
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

        #region ForgetPassword
        [HttpPost]
        [Route("ForgetPassword")]
        public async Task<IdentityResult> ForgetPassword([FromBody] ForgetPasswordModel forgetPassword, [FromHeader] string Token)
        {
            var result = IdentityResult.Failed(new IdentityError()
            {
                Code = "InvalidUser",
                Description = "این کاربری ثبت نشده است"
            });
            try
            {
                var user = await _userManager.FindByIdAsync(forgetPassword.UserId);
                if (user != null)
                {
                    result = await _userManager.ResetPasswordAsync(user, Token, forgetPassword.NewPassword);

                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        #endregion

        #region RequestForForgetPassword
        [HttpGet]
        [Route("RequestForForgetPassword")]
        public async Task<IdentityResult> RequestForForgetPassword([FromQuery] string email)
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
                    if (code != null)
                    {
                        code = HttpUtility.UrlEncode(code);
                        Email.SendEmailForgotPassword(user.Email, user.FirstName, user.UserName, code, user.Id);
                        return IdentityResult.Success;
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        #endregion

        #region ConfirmEmailAgain
        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("ConfirmEmailAgain")]
        public async Task<IdentityResult> ConfirmEmailAgain([FromHeader] string Authorization)
        {
            var result = IdentityResult.Failed(new IdentityError()
            {
                Code = "InvalidEmail",
                Description = "این ایمیل در سیستم ثبت نشده است"
            });

            try
            {
                var Ramovebearer = Authorization.Replace("bearer ", "");
                var Token = new JwtSecurityToken(Ramovebearer);
                var ClaimsLST = Token.Claims.ToArray();
                var userUserName = ClaimsLST[0];
                var user = await _userManager.FindByNameAsync(userUserName.Value);
                if (user != null)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = HttpUtility.UrlEncode(code);
                    Email.SendComfirmEmailAgain(user.Email, user.UserName, code, user.FirstName, user.Id);
                    return IdentityResult.Success;
                }
            }
            catch (Exception)
            {
            }

            return result;
        }
        #endregion

        #region ChangePassword
        [HttpPost]
        [Authorize(Roles = "User")]
        [Route("ChangePassword")]
        public async Task<IdentityResult> ChangePassword([FromHeader] string Authorization, [FromBody] ChangePasswordModel passwordModel)
        {
            var result = IdentityResult.Failed(new IdentityError()
            {
                Code = "InvalidLink",
                Description = "این لینک معتبر نمیباشد"
            });
            try
            {
                int a = 2;
                var Ramovebearer = Authorization.Replace("bearer ", "");
                var Token = new JwtSecurityToken(Ramovebearer);
                var ClaimsLST = Token.Claims.ToArray();
                var userUserName = ClaimsLST[0];
                var user = await _userManager.FindByNameAsync(userUserName.Value);
                if (user != null)
                {
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = HttpUtility.UrlEncode(code);
                    result = await _userManager.ChangePasswordAsync(user, passwordModel.CurrentPassword, passwordModel.NewPassword);
                    if (result == IdentityResult.Success)
                    {
                        Email.ChangePassword(user.Email, user.UserName, user.FirstName, passwordModel.NewPassword);
                    }
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        #endregion

        #region RequestChangeEmail
        [HttpGet]
        [Authorize(Roles = "User")]
        [Route("RequestChangeEmail")]
        public async Task<IdentityResult> RequestChangeEmail([FromHeader] string Authorization, [FromQuery] string NewEmail)
        {
            #region DefaultResult
            var result = IdentityResult.Failed(new IdentityError()
            {
                Code = "InvalidUserName",
                Description = "این نام کاربری ثبت نشده است"
            });
            try
            {
                System.Net.Mail.MailAddress mailAddress = new System.Net.Mail.MailAddress(NewEmail);

            }
            catch (FormatException)
            {
                return result = IdentityResult.Failed(new IdentityError()
                {
                    Code = "InvalidEmail",
                    Description = "لطفا یک ایمیل معتبر وارد کنید"
                });
            }
            #endregion
            try
            {
                #region GetUserFromAuthorization
                var Ramovebearer = Authorization.Replace("bearer ", "");
                var Token = new JwtSecurityToken(Ramovebearer);
                var ClaimsLST = Token.Claims.ToArray();
                var userUserName = ClaimsLST[0];
                var user = await _userManager.FindByNameAsync(userUserName.Value);
                #endregion

                #region SendChangeEmail'sEmail
                if (user != null)
                {
                    var code = await _userManager.GenerateChangeEmailTokenAsync(user, NewEmail);
                    code = HttpUtility.UrlEncode(code);
                    var UserID = HttpUtility.UrlEncode(user.Id);
                    Email.ChangeEmail(NewEmail, user.UserName, code, UserID);
                    result = IdentityResult.Success;
                }
                #endregion
            }
            catch (Exception)
            {
            }
            return result;
        }
        #endregion

        #region ChangeEmail
        [HttpPost]
        [Route("ChangeEmail")]
        public async Task<IdentityResult> ChangeEmail([FromBody] ChangeEmailModel ChangeEmail, [FromHeader] string Token)
        {
            var result = IdentityResult.Failed(new IdentityError()
            {
                Code = "InvalidLink",
                Description = "این لینک معتبر نمیباشد"
            });
            try
            {
                var user = await _userManager.FindByIdAsync(ChangeEmail.UserId);
                if (user != null)
                {
                    result = await _userManager.ChangeEmailAsync(user, ChangeEmail.NewEmail, Token);
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        #endregion

        #region SendNews
        [Authorize(Roles = "Admin, Leader")]
        [HttpPost]
        [Route("SendNews")]
        public async Task<IdentityResult> SendNews([FromBody] SendNewsModel SendNews, [FromHeader] string Authorization)
        {
            var result = IdentityResult.Failed(new IdentityError()
            {
                Code = "InvalidLink",
                Description = "Desciption"
            });
            try
            {
                var Ramovebearer = Authorization.Replace("bearer ", "");
                var Token = new JwtSecurityToken(Ramovebearer);
                var ClaimsLST = Token.Claims.ToArray();
                var userUserName = ClaimsLST[0];
                var user = await _userManager.FindByNameAsync(userUserName.Value);
                if (user != null)
                {
                    var emailLST = _context.Users.ToList();
                    foreach (var item in emailLST)
                    {
                        Email.SendNewsLetter(item.Email, SendNews.Category, item.FirstName, SendNews.NewsSubject, SendNews.Content, user.UserName);
                    }
                    return IdentityResult.Success;
                }
            }
            catch (Exception)
            {
            }
            return result;
        }
        #endregion

        #region encrypt
        public string Encrypt(string encryptString)
        {
            string EncryptionKey = "kjdsfhsikdy87f6yasd8fgyasdlkfgjasdhfg";
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }
            return encryptString;
        }
        #endregion

        #region Decrypt
        public string Decrypt(string cipherText)
        {
            string EncryptionKey = "kjdsfhsikdy87f6yasd8fgyasdlkfgjasdhfg";
            cipherText = cipherText.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(cipherText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] {
            0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76
        });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherText;
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

        #region IsUser
        [Authorize(Roles = "User")]
        [HttpGet]
        [Route("IsUser")]
        public bool IsUser()
        {
            return true;
        }
        #endregion

        #region IsAdmin
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("IsAdmin")]
        public bool IsAdmin()
        {
            return true;
        }
        #endregion

        #region IsLeader
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("IsLeader")]
        public bool IsLeader()
        {
            return true;
        }
        #endregion
    }
}