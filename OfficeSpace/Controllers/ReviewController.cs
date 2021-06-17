using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeSpace.Models;

namespace OfficeSpace.Controllers
{
    public class ReviewController : Controller
    {
        public ActionResult Index()
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                ViewBag.pageSize = 4;
                ReviewModel model = new ReviewModel();
                model.GetBranchRequestsAll(Session["CurrentUserName"].ToString());
                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult UpdateBranchRequest(ReviewModel model)
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }

                model.UpdateBranchRequest();

                return RedirectToAction("Index", "Review");
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
    }
}