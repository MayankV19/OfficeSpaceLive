using OfficeSpace.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OfficeSpace.Controllers
{
    public class ApprovalController : Controller
    {
        // GET: Approval
        public ActionResult Index()
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                ViewBag.pageSize = 6;
                ApprovalModel model = new ApprovalModel();
                model.GetBranchRequestsAll(Session["CurrentUserName"].ToString());
                return View(model);
            }
            catch(Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult UpdateBranchRequest(ApprovalModel model)
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                model.UpdateBranchRequest();

                return RedirectToAction("Index", "Approval");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult Recommend()
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                ApprovalModel model = new ApprovalModel();
                model.GetCompanyNameforID();
                model.GetUserRequests();
                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }



        [HttpPost]
        public ActionResult UpdateRecommendedRequest(ApprovalModel model)
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                model.UpdateRecommendedRequest();

                return RedirectToAction("Recommend", "Approval");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

    }
}