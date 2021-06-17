using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeSpace.Models;
using System.IO;
using System.ComponentModel;
using System.Data;
using Newtonsoft.Json;
using System.Threading;
using System.Reflection;
using OfficeSpace.Extension;

namespace OfficeSpace.Controllers
{
    public class FurnishedRentalsController : Controller
    {
        // GET: FurnishedRentals


        public ActionResult Index()
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                string roleName = Session["CurrentUserRole"].ToString();

                ViewBag.pageSize = 15;
                FurnishedRentalModel model = new FurnishedRentalModel();
               
                BAL bal = new BAL();
                var companies = new List<Company>();
                if (roleName == "Admin")
                {
                    companies = bal.GetCompanyList(roleName);
                }
                else
                {
                    companies = bal.GetCompanyList(roleName, Session["CurrentUserName"].ToString());
                }
                model.Companies = (from company in companies
                                   select new SelectListItem { Text = company.CompayName, Value = company.CompanyId.ToString() }).ToList();
                model.SelectedCompanyIds = companies.Select(comp => comp.CompayName).ToArray();

                model.GetFurnishedRentalOfficesDetails(Session["CurrentUserName"].ToString(), string.Join(",", model.SelectedCompanyIds.Select(com => com)));
                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult Index(FurnishedRentalModel model)
        {
            try
            {

                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                string roleName = Session["CurrentUserRole"].ToString();

                ViewBag.pageSize = 15;
                BAL bal = new BAL();
                var companies = new List<Company>();
                if (roleName == "Admin")
                {
                    companies = bal.GetCompanyList(roleName);
                }
                else
                {
                    companies = bal.GetCompanyList(roleName, Session["CurrentUserName"].ToString());
                }
                model.Companies = (from company in companies
                                   select new SelectListItem { Text = company.CompayName, Value = company.CompanyId.ToString() }).ToList();
                model.BranchDataList = new List<FurnishedOffice>();
                if (model.SelectedCompanyIds != null && model.SelectedCompanyIds.Count() > 0)
                {
                    model.GetFurnishedRentalOfficesDetails(Session["CurrentUserName"].ToString(), string.Join(",", model.SelectedCompanyIds.Select(com => com)));
                }
                return View(model);



            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult Downloadfile(string companyName, string Flag)
        {
            FurnishedRentalModel model = new FurnishedRentalModel();
            //model.Company = companyName;
            //model.Flag = Flag;
            string roleName = Session["CurrentUserRole"].ToString();
            BAL bal = new BAL();
            var companies = new List<Company>();
            if (roleName == "Admin")
            {
                companies = bal.GetCompanyList(roleName);
            }
            else
            {
                companies = bal.GetCompanyList(roleName, Session["CurrentUserName"].ToString());
            }
            model.Companies = (from company in companies
                               select new SelectListItem { Text = company.CompayName, Value = company.CompanyId.ToString() }).ToList();
            model.GetFurnishedRentalOfficesDetails(Session["CurrentUserName"].ToString(), string.IsNullOrEmpty(model.SelectedCompanyId) ? string.Join(",", companies.Select(com => com.CompayName)) : model.SelectedCompanyId);
            string userDateFormat = "dd/MM/yyyy";

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(ExistingRentalExcelModel));
            Dictionary<string, string> columnDisplayName = new Dictionary<string, string>();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                columnDisplayName.Add(property.Name, property.DisplayName);
            }

            List<ExistingRentalExcelModel> ExistingRentalDataModel = new List<ExistingRentalExcelModel>();
            foreach (FurnishedOffice obj in model.BranchDataList)
            {
                ExistingRentalExcelModel DMObjExisting = new ExistingRentalExcelModel();
                DMObjExisting.SNI = obj.ID;
                DMObjExisting.Date = obj.Date;
                DMObjExisting.City = obj.City;
                DMObjExisting.Company = obj.Company;
                DMObjExisting.BORO = obj.OfficeType;
                DMObjExisting.RegionalOffice = obj.RegionalOffice;
                DMObjExisting.OfficeName = obj.OfficeName;
                DMObjExisting.OfficeAddress = obj.Address;
                DMObjExisting.PropertyType = obj.PropertyType;
                DMObjExisting.LeaseStartDate = obj.LeaseStartDate;
                DMObjExisting.LeaseStartRentalAmount = obj.LeaseStartingAmount;
                DMObjExisting.RentalEscallation = obj.RentalEscallation;
                DMObjExisting.EscallationPeriod = obj.EscallationPeriod;
                DMObjExisting.LeasePeriod = obj.LeasePeriod;
                DMObjExisting.LeaseClouserDate = obj.LeaseClouserDate;
                DMObjExisting.Security_Deposit = obj.SecurityDeposit;
                DMObjExisting.AdvanceRental = obj.AdvanceRental;
                DMObjExisting.TotalAmountHoldWithOwner = obj.TotalAmountHoldWithOwner;
                DMObjExisting.FitoutsNew = obj.FitOuts;
                DMObjExisting.Car_Park = obj.NoOfCarParking;
                DMObjExisting.NoticePeriod = obj.NoticePeriod;
                DMObjExisting.SignageRoads = obj.Signage;
                DMObjExisting.NoOfEmployee = obj.NoOfEmployee;
                DMObjExisting.SuperBuilt_UpArea = obj.SuperBuiltUpArea;
                DMObjExisting.BuiltUp_area = obj.BuiltUpArea;
                DMObjExisting.Carpet_Area = obj.CarpetArea;
                DMObjExisting.Rental_Area = obj.RentalArea;
                DMObjExisting.PresentRentalCost = obj.PresentRentalCost;
                DMObjExisting.PresentMonthlyRentalCost = obj.PresentMonthlyRentalCost;
                DMObjExisting.PresentMonthlyBilling = obj.PresentMonthlyBilling;
                DMObjExisting.RenatlCostPercentage = obj.RenatlCostPercentage;
                DMObjExisting.MonthlyMaintenanceCost = obj.MonthlyMaintenanceCost;
                DMObjExisting.AvgMonthltMaintenanceCost = obj.AvgMonthltMaintenanceCost;
                DMObjExisting.MonthlyElectricityCost = obj.MonthlyElectricityCost;
                DMObjExisting.MonthlyAllOtherCosts = obj.MonthlyAllOtherCosts;
                DMObjExisting.TotalMonthlyRentalCost = obj.TotalMonthlyRentalCost;
                DMObjExisting.Created_By_Name = obj.Name;
                DMObjExisting.Created_By_Email = obj.Email;
                DMObjExisting.Created_By_Phone = obj.Mobile;
                DMObjExisting.Remarks = obj.Remarks;

                ExistingRentalDataModel.Add(DMObjExisting);

        }


        byte[] filecontent = ExcelExportExtension.ExportToExcel(ExistingRentalDataModel, "FurnishedRentals", null, false, userDateFormat, columnDisplayName);
            return File(filecontent, ExcelExportExtension.ExcelContentType, $"ExistingRentalDetails_{DateTime.Now.ToString("yyyyMMddHHmm")}.xlsx");
    }

        public ActionResult RentalDetssils()
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                string roleName = Session["CurrentUserRole"].ToString();

                ViewBag.pageSize = 15;
                FurnishedRentalModel model = new FurnishedRentalModel();
                BAL bal = new BAL();
                var companies = new List<Company>();
                if (roleName == "Admin")
                {
                    companies = bal.GetCompanyList(roleName);
                }
                else
                {
                    companies = bal.GetCompanyList(roleName, Session["CurrentUserName"].ToString());
                }
                model.Companies = (from company in companies
                                   select new SelectListItem { Text = company.CompayName, Value = company.CompanyId.ToString() }).ToList();
                model.SelectedCompanyIds = companies.Select(comp => comp.CompayName).ToArray();

                model.GetFurnishedRentalOfficesDetails(Session["CurrentUserName"].ToString(), string.Join(",", model.SelectedCompanyIds.Select(com => com)));
                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }
        [HttpPost]
        public ActionResult UpdateFurnitureDetails(string updatedData)
        {
            try
            {
                Thread.Sleep(3000);
                var collectionObject = JsonConvert.DeserializeObject<List<FurnitureUpdateDetails>>(updatedData);
                var dt = collectionObject.ToDataTable();
                BAL bal = new BAL();
                var result = bal.UpdateFurnitureDetails(dt);
                return Json(new { Result = result });
            }
            catch (Exception ex)
            {
                return Json(new { Result = "Failed" });
            }
        }

        public ActionResult UploadLeaseDocument(int RequestId)
        {
            Session["UploadRequestID"] = RequestId;
            //  Session["Flag"] = "0";
            if (Session["UploadFileName"] != null)
            {
                if (Session["Flag"].ToString() != "3")
                {
                    FurnishedRentalModel Model = new FurnishedRentalModel();
                       Model.UploadLeaseDocument(Convert.ToInt32(Session["UploadRequestID"].ToString()), Session["UploadFileName"].ToString());
                   // _RequestBussinessService.UploadLeaseDocument(Convert.ToInt32(Session["UploadRequestID"].ToString()), Session["UploadFileName"].ToString());
                    Session["UploadFileName"] = null;
                }
            }
            if (Session["Flag"] != null)
            {
                if (Session["Flag"].ToString() == "1")
                {
                    ViewBag.Message = "Lease Document uploaded successfully !!";
                }
                else if (Session["Flag"].ToString() == "2")
                {
                    ViewBag.Message = "No Document selected.Please select valid PDF Document !!";

                }
                else
                {
                    ViewBag.Message = "Please Upload Lease Document in Pdf format only !!";
                }

                Session["Flag"] = null;
            }
            return View();
        }

        [HttpPost]
        public ActionResult UploadDocument(HttpPostedFileBase postedFile, string Submit)
        {

            switch (Submit)
            {
                case "Upload":
                    if (postedFile != null)
                    {
                        string path = Server.MapPath("~/LeaseDocuments/");
                        string extension = Path.GetExtension(postedFile.FileName);
                        if (extension == ".pdf")
                        {
                            if (!Directory.Exists(path))
                            {
                                Directory.CreateDirectory(path);
                            }
                            string FileName = Path.GetFileName(postedFile.FileName);
                            Session["UploadFileName"] = FileName = Session["UploadRequestID"].ToString() + "-" + FileName;
                            postedFile.SaveAs(path + FileName);
                            Session["Flag"] = "1";
                            ViewBag.Message = "File uploaded successfully.";
                            //return RedirectToAction("UploadLeaseDocument");
                        }
                        else
                        {
                            Session["Flag"] = "3";
                            ViewBag.Message = "Please Upload Pdf File";
                        }
                    }
                    else
                    {
                        Session["Flag"] = "2";
                        ViewBag.Message = "Please select file";
                    }
                    //   return RedirectToAction("UploadLeaseDocument");
                    return RedirectToAction("UploadLeaseDocument", "FurnishedRentals", new { @RequestId = Convert.ToInt32(Session["UploadRequestID"].ToString()) });

                    break;
                case "Back":
                    Session["UploadFileName"] = null;
                    Session["Flag"] = null;
                    return RedirectToAction("Index");
                    break;


            }
            return View();

        }

        public FileResult DownloadLeaseFile(string fileName)
        {
            //Build the File Path.
            string path = Server.MapPath("~/LeaseDocuments/") + fileName;

            //Read the File data into Byte Array.
            byte[] bytes = System.IO.File.ReadAllBytes(path);

            //Send the File to Download.
            //return File(bytes, "application/octet-stream", fileName); //for downloading file of any format
            return File(bytes, "application/pdf"); //for viewing file of pdf format  
        }
    }
    public static class Extensions
    {
        public static DataTable ToDataTable<T>(this List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }
    }

}
