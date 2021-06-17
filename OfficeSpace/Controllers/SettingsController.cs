using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace OfficeSpace.Controllers
{
    public class SettingsController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                SettingsModel model = new SettingsModel();
                model.GetCompanyList();
                model.Init();
               
                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult CreateUser(SettingsModel model)
        {
            try
            {
                model.GetCompanyList();
                model.Init();
                if (ModelState.IsValid)
                {
                    try
                    {
                        bool isDuplicate = model.CheckUsernameExists();
                        if (!isDuplicate)
                        {
                            model.CreateUser();
                        }
                        else
                        {
                            TempData["error"] = "UserName already exist";
                        }
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("", ex.Message);
                    }
                    return RedirectToAction("Index", "Settings", new { tabIndex = 0 });
                }
                return View("Index", model);
                
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult Update(SettingsModel model)
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                model.UpdateUsers();
                return RedirectToAction("Index", "Settings", new { tabIndex = 1 });
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }        
    }
}