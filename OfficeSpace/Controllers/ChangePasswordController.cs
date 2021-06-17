using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeSpace.Models;

namespace OfficeSpace.Controllers
{
    public class ChangePasswordController : Controller
    {
        // GET: ChangePassword
        public ActionResult Index()
        {
            if (Session["CurrentUserName"] == null)
            {
                return RedirectToAction("Login", "Account");
            }
            ChangePasswordModel model = new ChangePasswordModel();
            model.Username = Session["CurrentUserName"].ToString();
            return View(model);
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Validate(ChangePasswordModel model)
        {
            try
            {
               
                if (ModelState.IsValid)
                {
                    if (!string.IsNullOrEmpty(model.Password) && !string.IsNullOrEmpty(model.NewPassword))
                {
                    string MessageString = model.ChangePassword(Session["CurrentUserName"].ToString());
                    if (MessageString =="1")
                    {
                        //return RedirectToAction("Index", "Dashboard");
                            ModelState.AddModelError("", "Password updated Succesfully !!");
                        }
                    else
                    {
                        //return RedirectToAction("Index", "ChangePassword");
                        ModelState.AddModelError("", "Invalid Current Password !!");
                    }
                }
                }
              return View("Index", model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}