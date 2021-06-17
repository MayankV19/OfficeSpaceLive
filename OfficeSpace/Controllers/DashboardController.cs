using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeSpace.Models;

namespace OfficeSpace.Controllers
{
    public class DashboardController : Controller
    {
        // GET: Dashboard
        public ActionResult Index()
        {
            if (Session["CurrentUserName"] == null)
            {
                return RedirectToAction("Login", "Account");
            }

            DashboardModel model = new DashboardModel();
            model.Init();
            return View(model);
        }

        [HttpPost]
        public ActionResult Total()
        {
            Session["DashboardFlag"] = 1;
            return RedirectToAction("Index", "DashboardDetail");           
        }

        [HttpPost]
        public ActionResult UnderProcess()
        {
            Session["DashboardFlag"] = 2;
            return RedirectToAction("Index", "DashboardDetail");
            //ViewBag.Msg = "Details saved successfully.";
            //return View();
        }

        public ActionResult Closed()
        {
            Session["DashboardFlag"] = 3;
            return RedirectToAction("Index", "DashboardDetail");
            //ViewBag.Msg = "Details saved successfully.";
            //return View();
        }

        public ActionResult Disapproved()
        {
            Session["DashboardFlag"] = 4;
            return RedirectToAction("Index", "DashboardDetail");
            //ViewBag.Msg = "Details saved successfully.";
            //return View();
        }

        public ActionResult NewRequirement()
        {
            Session["DashboardFlag"] = 5;
            return RedirectToAction("Index", "DashboardDetail");
            //ViewBag.Msg = "Details saved successfully.";
            //return View();
        }

        public ActionResult LeaseExpire()
        {
            Session["DashboardFlag"] = 6;
            return RedirectToAction("Index", "DashboardDetail");
            //ViewBag.Msg = "Details saved successfully.";
            //return View();
        }

        public ActionResult Existing()
        {
           // Session["DashboardFlag"] = 6;
            return RedirectToAction("Index", "FurnishedRentals");
            //ViewBag.Msg = "Details saved successfully.";
            //return View();
        }



        public JsonResult FetchDashboardValues(string CompanyName)
        {
            DashboardModel model = new DashboardModel();
            model.GetDashboardValues(CompanyName);

            return Json(new { model = model }, JsonRequestBehavior.AllowGet);
           // return model;           
        }

        public JsonResult FetchDashboardValuesOther(string CompanyName)
        {
            DashboardModel model = new DashboardModel();
            model.GetDashboardValuesOther(CompanyName);

            return Json(new { model = model }, JsonRequestBehavior.AllowGet);
            // return model;           
        }

        public JsonResult FetchDashboardValuesLeaseExpire(string CompanyName)
        {
            DashboardModel model = new DashboardModel();
            model.GetDashboardValuesLeaseExpire(CompanyName);

            return Json(new { model = model }, JsonRequestBehavior.AllowGet);
            // return model;           
        }

    }
}