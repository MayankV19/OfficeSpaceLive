using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Mvc;

namespace OfficeSpace.Models
{
    public class FurnishedRentalModel
    {
        public string Company { get; set; }
        public string Flag { get; set; }
        public string SelectedCompanyId { get; set; }
        public string[] SelectedCompanyIds { get; set; }
        public List<SelectListItem> Companies { get; set; }

        public List<FurnishedOffice> BranchDataList { get; set; }

        public void GetFurnishedRentalOfficesDetails(string UserName, string companyNames = null)
        {
            BranchDataList = new List<FurnishedOffice>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SP_GetFurnishedRentalOfficesDetails";
                command.Parameters.Add(new SqlParameter() { ParameterName = "@CompanyNames", Value = companyNames, DbType = System.Data.DbType.String });
                command.CommandType = System.Data.CommandType.StoredProcedure;
                //              command.CommandText = string.Format(@"SELECT  convert(varchar(10), ROW_NUMBER() OVER (ORDER BY [AutoID])) as ID , [AutoID]
                //    ,convert(varchar(12),Date,106) as Date
                //    ,[City]
                //    ,[Company]
                //    ,[OfficeType]
                //    ,[Address]
                //    ,[PropertyType]
                //    , convert(varchar(12),LeaseStartDate,106) as LeaseStartDate
                //    ,[LeaseStartingAmount]
                //    ,[RentalEscallation]
                //    ,[EscallationPeriod]
                //    ,[LeasePeriod]
                //    , convert(varchar(12),LeaseClouserDate,106) as LeaseClouserDate
                //    ,[SecurityDeposit]
                //    ,[AdvanceRental]
                //    ,[TotalAmountHoldWithOwner]
                //    ,[FitOuts]
                //    ,[NoOfCarParking]
                //    ,[NoticePeriod]
                //    ,[Signage]
                //    ,[NoOfEmployee]
                //    ,[SuperBuiltUpArea]
                //    ,[BuiltUpArea]
                //    ,[CarpetArea]
                //    ,[RentalArea]
                //    ,[PresentRentalCost]
                //    ,[PresentMonthlyRentalCost]
                //    ,[PresentMonthlyBilling]
                //    ,[RenatlCostPercentage]
                //    ,[MonthlyMaintenanceCost]
                //    ,[AvgMonthltMaintenanceCost]
                //    ,[MonthlyElectricityCost]
                //    ,[MonthlyAllOtherCosts]
                //    ,[TotalMonthlyRentalCost]
                //    ,[Name]
                //    ,[Email]
                //    ,[Mobile]
                //    ,[Remarks],OfficeName
                //FROM [OfficeSpace].[dbo].[FurnishedRentalDetails]");

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        FurnishedOffice obj = new FurnishedOffice();
                        obj.ID = int.Parse(reader["ID"].ToString());
                        obj.AutoID = int.Parse(reader["AutoID"].ToString());
                        obj.Date = reader["Date"].ToString();
                        obj.City = reader["City"].ToString();
                        obj.Company = reader["Company"].ToString();
                        obj.OfficeType = reader["OfficeType"].ToString();
                        obj.Address = reader["Address"].ToString();
                        obj.PropertyType = reader["PropertyType"].ToString();
                        obj.LeaseStartDate = reader["LeaseStartDate"].ToString();
                        obj.LeaseStartingAmount = reader["LeaseStartingAmount"].ToString();
                        obj.RentalEscallation = reader["RentalEscallation"].ToString();
                        obj.EscallationPeriod = reader["EscallationPeriod"].ToString();
                        obj.LeasePeriod = reader["LeasePeriod"].ToString();
                        obj.LeaseClouserDate = reader["LeaseClouserDate"].ToString();
                        obj.SecurityDeposit = reader["SecurityDeposit"].ToString();
                        obj.AdvanceRental = reader["AdvanceRental"].ToString();
                        obj.TotalAmountHoldWithOwner = reader["TotalAmountHoldWithOwner"].ToString();
                        obj.FitOuts = reader["FitOuts"].ToString();
                        obj.NoOfCarParking = reader["NoOfCarParking"].ToString();
                        obj.NoticePeriod = reader["NoticePeriod"].ToString();
                        obj.Signage = reader["Signage"].ToString();
                        obj.NoOfEmployee = reader["NoOfEmployee"].ToString();
                        obj.SuperBuiltUpArea = reader["SuperBuiltUpArea"].ToString();
                        obj.BuiltUpArea = reader["BuiltUpArea"].ToString();
                        obj.CarpetArea = reader["CarpetArea"].ToString();
                        obj.RentalArea = reader["RentalArea"].ToString();
                        obj.PresentRentalCost = reader["PresentRentalCost"].ToString();
                        obj.PresentMonthlyRentalCost = reader["PresentMonthlyRentalCost"].ToString();
                        obj.PresentMonthlyBilling = reader["PresentMonthlyBilling"].ToString();

                        obj.RenatlCostPercentage = reader["RenatlCostPercentage"].ToString();
                        obj.MonthlyMaintenanceCost = reader["MonthlyMaintenanceCost"].ToString();
                        obj.AvgMonthltMaintenanceCost = reader["AvgMonthltMaintenanceCost"].ToString();
                        obj.MonthlyElectricityCost = reader["MonthlyElectricityCost"].ToString();
                        obj.MonthlyAllOtherCosts = reader["MonthlyAllOtherCosts"].ToString();
                        obj.TotalMonthlyRentalCost = reader["TotalMonthlyRentalCost"].ToString();
                        obj.Name = reader["Name"].ToString();
                        obj.Email = reader["Email"].ToString();
                        obj.Mobile = reader["Mobile"].ToString();
                        obj.Remarks = reader["Remarks"].ToString();

                        obj.OfficeName = reader["OfficeName"].ToString();
                        obj.RegionalOffice = reader["RegionalOffice"].ToString();
                        obj.IsDocumentUploded = reader["IsDocumentUploaded"].ToString();
                        obj.DocumentName = reader["LeaseDocumentName"].ToString();

                        BranchDataList.Add(obj);
                    }
                }
                reader.Close();
            }

        }

        public void UploadLeaseDocument(int ID, string FileName)
        {
            try
            {

                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = "udp_Store_UpdateLeaseDocument";
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@ID", Convert.ToInt32(ID));
                    command.Parameters.AddWithValue("@FileName", FileName);
                    command.ExecuteNonQuery();
                    command.Parameters.Clear();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}