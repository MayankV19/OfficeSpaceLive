
using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Net.Mail;
using System.Net;

namespace OfficeSpace.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Validate(LoginModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(model.Username) && !string.IsNullOrEmpty(model.Password))
                    {
                        string roleName = model.Validate();
                        if (!string.IsNullOrEmpty(roleName))
                        {
                            Session["CurrentUserName"] = model.Username;
                            Session["CurrentUserRole"] = roleName;
                            return RedirectToAction("Index", "Dashboard");
                        }
                        else
                        {
                            ModelState.AddModelError("", "Invalid Credentials");
                        }
                    }
                }
                return View("Login", model);
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult ForgotPassword()
        {
            try
            {
                return View();
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        public ActionResult LogOut()
        {
            try
            {   
                Session.Clear();                
                return RedirectToAction("Index", "Dashboard");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult SendMail(EmailModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(model.EmailID))
                    {
                        string[] data = model.GetPasswordString();
                        string Password = data[0];
                        string UserName = data[1];

                        if (!string.IsNullOrEmpty(Password))
                        {
                            var subject = "Office Space-Forgot Password";                           
                            var body = "Hello " + UserName + ", \n \n Your Current Password is - " + Password;

                            EmailManager manager = new EmailManager();
                            manager.SendEmail(subject, body, new List<string>() { model.EmailID }, null, true);
                        }
                        else
                        {
                            ModelState.AddModelError("", "Email Id Doesn't Exists !!");
                            return View("ForgotPassword", model);
                        }
                        return RedirectToAction("Login", "Account");
                    }
                }
                else
                {                    
                    return View("ForgotPassword",model);
                }

            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
            return RedirectToAction("Login", "Account");

        }
    }
}