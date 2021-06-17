using OfficeSpace.Extension;
using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeSpace.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                string roleName = Session["CurrentUserRole"].ToString();

                NavigationModel model = new NavigationModel();
                if (roleName == "Admin")
                {
                    model.Init(roleName);
                }
                else
                {
                    model.Init(roleName, Session["CurrentUserName"].ToString());
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult AddBranchOffice(string companyName,string selectedMenu,string error)
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                NavigationModel model = new NavigationModel();
                model.Company = companyName;
                model.SelectedMenu = selectedMenu;
                model.GetCityList();
                if (!string.IsNullOrEmpty(error))
                {
                    ModelState.AddModelError("", error);
                }
                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult Merge()
        {
            try
            {
                NavigationModel model = new NavigationModel();
                model.GetUserRequests();
                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        [PreventDuplicateRequest]
        public ActionResult CreateNewRequest(NavigationModel model)
        {
            try
            {
                model.GetCityList();
                if (ModelState.IsValid)
                {
                    if (Session["CurrentUserName"] == null)
                    {
                        return RedirectToAction("Login", "Account");
                    }

                    int id = model.CreateNewRequest(Session["CurrentUserName"].ToString());
                    model.ID = id;
                    EmailManager emailManager = new EmailManager();
                    emailManager.EmailIntitate(model, false, "ND");
                    return RedirectToAction("Index", "Home");
                }
                else if (!ModelState.IsValid && ModelState.Values.Count == 33)
                {
                    //model = new NavigationModel();
                    //model.GetCityList();
                    //ModelState.AddModelError("", );
                    return RedirectToAction("AddBranchOffice", new object[] { model.Company, model.SelectedMenu, "Looks like you accidentally tried to double post." });
                }
                return View("AddBranchOffice", model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult UpdateMerge(NavigationModel model)
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                model.UpdateMergeData();
                return RedirectToAction("Merge", "Home");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

    }
}