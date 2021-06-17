using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using OfficeSpace.Models;

namespace OfficeSpace.Controllers
{
    public class DashboardDetailController : Controller
    {
        // GET: DashboardDetail
        public ActionResult Index(string companyName, string Flag)
        {
            try
            {
                if (Session["CurrentUserName"] == null)
                {
                    return RedirectToAction("Login", "Account");
                }
                ViewBag.pageSize = 6;
                ApprovalModel model = new ApprovalModel();
                model.Company = companyName;
                model.Flag = Flag;
                model.GetBranchRequests(Flag,companyName);
                return View(model);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        [HttpPost]
        public ActionResult UpdateRequest(ApprovalModel model)
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

        public ActionResult Downloadfile(string companyName,string Flag)
        {
            ApprovalModel model = new ApprovalModel();
            model.Company = companyName;
            model.Flag = Flag;
            model.GetBranchRequests(Flag, companyName);
            string userDateFormat = "dd/MM/yyyy";

            PropertyDescriptorCollection properties = TypeDescriptor.GetProperties(typeof(DataModel));
            Dictionary<string, string> columnDisplayName = new Dictionary<string, string>();
            for (int i = 0; i < properties.Count; i++)
            {
                PropertyDescriptor property = properties[i];
                columnDisplayName.Add(property.Name, property.DisplayName);
            }

            List<DataModel> dataModel = new List<DataModel>();
            foreach(NavigationModel obj in model.BranchDataList)
            {
                DataModel DMObj = new DataModel();
                DMObj.SN = obj.RequestID;
                DMObj.CreationDate = obj.CreationDate;
                DMObj.City = obj.City;
                DMObj.Company = obj.Company;
                DMObj.Type = obj.DataType;
                DMObj.Status = obj.Status;
           
                DMObj.Request_Type = obj.SelectedMenu;
                DMObj.Branch = obj.BuisnessType;
                DMObj.Rent_Buy = obj.AllocationType;              
                DMObj.Lease_Renew_Date = obj.ProposedDateofRenewal;
                DMObj.Requirement_Date = obj.DateFromWhich;
                DMObj.Fitouts = obj.Fitouts;
                DMObj.Created_By_Name = obj.CreatedByName;
                DMObj.Created_By_Email = obj.CreatedByEmail;
                DMObj.Created_By_Phone = obj.CreatedByPhone;

                if (obj.DataType == "Existing")
                {
                    DMObj.Location = obj.ExistingLocation;
                    DMObj.Signage = obj.Signage;
                    DMObj.Employee_Count = obj.NoOfPersons;
                    DMObj.SuperBuilt_UpArea = obj.SuperBuiltUp;
                    DMObj.BuiltUp_area = obj.BuiltUp;
                    DMObj.Carpet_Area = obj.CarpetArea;
                    DMObj.Rental_Area = obj.RentalArea;
                    DMObj.Rental_Cost = obj.CostPerSquareFeet;
                    DMObj.Total_Monthly_Rental_Cost = obj.ExistingMonthlyCost;
                    DMObj.Security_Deposit = obj.SecurityDeposit;
                    DMObj.Car_Park = obj.CarPark;
                    DMObj.Remarks = obj.Remarks;
                }
                else
                {
                    DMObj.Location = obj.ProposedLocation;
                    DMObj.Signage = obj.ProposedSignage;
                    DMObj.Employee_Count = obj.ProposedNoOfPersons;
                    DMObj.SuperBuilt_UpArea = obj.ProposedSuperBuiltUp;
                    DMObj.BuiltUp_area = obj.ProposedBuiltUp;
                    DMObj.Carpet_Area = obj.ProposedCarpetArea;
                    DMObj.Rental_Area = obj.ProposedRentalArea;
                    DMObj.Rental_Cost = obj.ProposedCostPerSquareFeet;
                    DMObj.Total_Monthly_Rental_Cost = obj.ProposedMonthlyCost;
                    DMObj.Security_Deposit = obj.ProposedSecurityDeposit;
                    DMObj.Car_Park = obj.ProposedCarPark;
                    DMObj.Remarks = obj.ProposedRemarks;
                }

                dataModel.Add(DMObj);

            }
            

            byte[] filecontent =  ExcelExportExtension.ExportToExcel(dataModel, "DashboardDetail", null, false, userDateFormat, columnDisplayName);
            return File(filecontent, ExcelExportExtension.ExcelContentType, $"DashboardDetail_{DateTime.Now.ToString("yyyyMMddHHmm")}.xlsx");
        }

    }
}