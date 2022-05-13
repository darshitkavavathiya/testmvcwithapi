using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using testmvc.Models;
using System;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;

namespace testmvc.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUser _user = null;
        private readonly IEmailServices _emailServices;
        private readonly Jwt _jwt;
        private readonly int useruid;


        public AuthenticationController(IUser user, IEmailServices emailServices, IOptions<Jwt> jwt)
        {
            this._jwt = jwt.Value;
                    _user = user;
            _emailServices = emailServices;
        }

       


        public IActionResult Login()
        {
            if (Request.Cookies["JWT"] != null)
            {
               
                HttpContext.Session.SetString("JWToken", Request.Cookies["JWT"].ToString());
                HttpContext.Session.SetInt32("userId", Convert.ToInt32(Request.Cookies["userid"]));
                    return RedirectToAction("index", "Customerapi");

            }

            return View();
        }
        [HttpPost]
       
        public IActionResult Login(Authentication auth)
        {


            if (ModelState.IsValid)
            {
                List<User> u = _user.GetUserByUserName(auth.UserName);

                if (!u.Any())
                {
                    TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", "User not found", "error");

                    return View(auth);
                }
                else if (_user.VerifyUser(auth.Password, u.First().Password))
                {



                    auth.Id = u.First().UserId;

                    var token = this.GenerateJwtToken(auth);
                    HttpContext.Session.SetString("JWToken", token);
                    HttpContext.Session.SetInt32("userId", u.First().UserId);
                    if (1 == 1)
                    {
                        CookieOptions cookieRemember = new CookieOptions();
                        cookieRemember.Expires = DateTime.Now.AddSeconds(86400);
                        Response.Cookies.Append("JWT", token, cookieRemember);
                        Response.Cookies.Append("userid", Convert.ToString(u.First().UserId), cookieRemember);
                    }

                    ViewBag.FirstName = u.First().FirstName;

                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                 
                    TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", "Password is incorrect ! please try again", "error");

                    return View(auth);
                }

            }
            @ViewBag.validation = "alert alert-danger d-flex justify-content-center mx-4 mt-3";

            return View(auth);
        }

        public IActionResult Changepassword()
        {
            int? id = HttpContext.Session.GetInt32("userId");

            if (id == null || id == 0)
            {
               

                TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", "To change password you must login", "warning");

                return RedirectToAction("Login");
            }
            ViewBag.FirstName = _user.GetUserById(id).FirstName;
            return View();
        }
        [HttpPost]
        public IActionResult Changepassword(string Password, string NewPassword, string ConfirmPassword)
        {

            ChangePWD password = new ChangePWD();

            password.Password = Password;
            password.NewPassword = NewPassword;
            password.ConfirmPassword = ConfirmPassword;


            if (!ModelState.IsValid)
            {
                @ViewBag.validation = "alert alert-danger d-flex justify-content-center mx-4 mt-3";


                return View(password);
            }

            int Id = (int)HttpContext.Session.GetInt32("userId");

            User user = _user.GetUserById(Id);


            if (_user.VerifyUser(password.Password, user.Password))
            {

                bool result = _user.ChangePassWord(password.NewPassword, Id);
                if (result)
                {
                    TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", "Password Change Successful", "success");

                    return View();
                }
                
                TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", "Unable to change password, please try again", "warning");

                return View();
            }
            TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", "Old password is Incorrect", "error");

            return View();


        }



        public IActionResult LogOut()
        {
            HttpContext.Session.Clear();

            Response.Cookies.Delete("JWT");
            Response.Cookies.Delete("userid");
            TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", " User LogOut successful", "success");

            return RedirectToAction("Login");
        }






        [HttpGet]
        public IActionResult ResetModule()
        {

            return View();
        }



        [HttpPost]
        public IActionResult ResetModule(string UserName)
        {


            if (UserName != null)
            {

                var U = _user.GetUserByUserName(UserName);
              
                if (!U.Any())
                {
                    TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", "User not found", "error");

                    return View();

                }
                else
                {
                    User user = U.First();
                    string mailbody = "<h1>Reset your password by click below link</h1>" +
                        "<a href='" + Url.Action("ResetPassword", "Authentication", new { userId = user.UserId }, "https") + "'>Reset Password</a>";

                    UserEmailOptions options = new UserEmailOptions {
                        ToEmails = new List<string>() { user.Email },
                        Body = mailbody,


                    };

                    _emailServices.SendResetPassword(options);


                 
                    TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", "Please check your Email", "success");

                    return View();
                }
            }
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]

        public IActionResult ResetPassword(int userId)
        {

            TempData["id"] = userId;
            return View();
        }

        [HttpPost]
        public IActionResult ResetPassword(Resetpassword rp)
        {
            bool result = _user.ChangePassWord(rp.NewPassword, rp.UserId);
            if (result)
            {

                TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "", "Password is updated", "success");

                return RedirectToAction("Login");
            }
            else
            {
           

                TempData["toast"] = string.Format("firecustomtoast('{0}','{1}','{2}')", "opps!", "please try again later", "success");

                return View(rp);
            }
          



        }






        private string GenerateJwtToken(Authentication authentication)
        {

            var securityKey = Encoding.UTF8.GetBytes(_jwt.Secret);

            var claims = new Claim[] {            
                     
                        new Claim("id",authentication.Id.ToString() ),
                    new Claim("username",authentication.UserName),
                };

            var credentials = new SigningCredentials(new SymmetricSecurityKey(securityKey), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_jwt.Issuer,
              _jwt.Issuer,
              claims,
              expires: DateTime.Now.AddDays(1),
              signingCredentials: credentials);




            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
