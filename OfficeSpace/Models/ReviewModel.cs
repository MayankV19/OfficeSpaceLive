using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
namespace OfficeSpace.Models
{
    public class ReviewModel
    {
        public string Company { get; set; }
        public string Flag { get; set; }
        public List<MergedRequestDataModel> RequestDataList { get; set; }
        public Dictionary<int, List<string>> IDandCompanyNameList { get; set; }
        public Dictionary<int, List<string>> IDandMenuSelection { get; set; }
        public Dictionary<int, List<string>> IDandBuisnessType { get; set; }

        public List<NavigationModel> BranchDataList { get; set; }

        public void GetBranchRequestsAll(string UserName)
        {
            BranchDataList = new List<NavigationModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = string.Format(@"
                    DECLARE @CompanyName nvarchar(500)
                    SET @CompanyName=(SELECT companyName FROM UserDetails WHERE UserName = '{0}')
                    SELECT * FROM (
                    SELECT 'Existing' as 'RequestType',NavigationDetailsNew.NavigationAutoID,convert(varchar(10), ROW_NUMBER() OVER (ORDER BY NavigationDetailsNew.NavigationAutoID)) +'-A' as RequestId ,case when NavigationDetailsNew.Company='VProtect' then 'VProtect'  when NavigationDetailsNew.company='SIS Cash services Pvt ltd' then 'SIS Cash'  
 when NavigationDetailsNew.company='Rare Hospitality and Services Pvt. Ltd' then 'Rare Hospitality'  
 when NavigationDetailsNew.company='SIS Prosegur Holdings' then 'SIS Prosegur' 
  when NavigationDetailsNew.company='SIS INDIA- MG' then 'SIS MG' 
   when NavigationDetailsNew.company='SLV Securities Services Pvt. Ltd.' then 'SLV' 
    when NavigationDetailsNew.company='Service Master Clean Limited' then 'SMCL'  
	when NavigationDetailsNew.company='Tech SIS Limited' then 'Tech SIS'  
	when NavigationDetailsNew.company='Terminix-SIS' then 'Terminix' 
	 when NavigationDetailsNew.company='Uniq Detective And Security Service Pvt. Ltd.' then 'Uniq' else 'DTSS' end as Company,NavigationDetailsNew.MenuSelection,
                        NavigationDetailsNew.BussinessType,NavigationDetailsNew.AllocationType,NavigationDetailsNew.City,convert(varchar(12),NavigationDetailsNew.LeaseRenewalDate,106) as LeaseRenewalDate,convert(varchar(12),NavigationDetailsNew.DateOfRequired,106) as DateOfRequired,
                        NavigationDetailsNew.FitOuts,NavigationDetailsNew.ExistingLocation as Location ,NavigationDetailsNew.ExistingSignage as Signage ,NavigationDetailsNew.ExistingEmployee as Employee,NavigationDetailsNew.ExistingSuperBuiltUpArea as SuperBuiltUpArea  ,NavigationDetailsNew.ExistingBuiltUpArea as BuiltUpArea ,NavigationDetailsNew.ExistingCarpetArea as CarpetArea,
                        NavigationDetailsNew.ExistingRentalArea as RentalArea,NavigationDetailsNew.ExistingRentalCost as RentalCost ,NavigationDetailsNew.ExistingSecurityDeposit as SecurityDeposit,NavigationDetailsNew.ExistingCarPark as CarPark,NavigationDetailsNew.ExistingRequest as Request  ,NavigationDetailsNew.Remark1 as Remark,Status,convert(varchar(12),NavigationDetailsNew.CreationDate,106) as CreationDate,NavigationDetailsNew.ExistingMonthlyCost as MonthlyCost,[UserDetails].FirstName+' '+[UserDetails].LastName as CreatedBy,[UserDetails].CompanyEmail,[UserDetails].PhoneNumber
                        FROM OfficeSpace.dbo.NavigationDetailsNew left outer join [UserDetails] on NavigationDetailsNew.CreatedBy=[UserDetails].UserName where @CompanyName LIKE '%'+ NavigationDetailsNew.Company +'%' and NavigationDetailsNew.Status IN ('Reviewed')
                    UNION ALL
                    SELECT 'Proposed' as 'RequestType', NavigationDetailsNew.NavigationAutoID,
                        convert(varchar(10), ROW_NUMBER() OVER (ORDER BY NavigationDetailsNew.NavigationAutoID)) +'-B' as RequestId ,case when NavigationDetailsNew.Company='VProtect' then 'VProtect'  when NavigationDetailsNew.company='SIS Cash services Pvt ltd' then 'SIS Cash'  
 when NavigationDetailsNew.company='Rare Hospitality and Services Pvt. Ltd' then 'Rare Hospitality'  
 when NavigationDetailsNew.company='SIS Prosegur Holdings' then 'SIS Prosegur' 
  when NavigationDetailsNew.company='SIS INDIA- MG' then 'SIS MG' 
   when NavigationDetailsNew.company='SLV Securities Services Pvt. Ltd.' then 'SLV' 
    when NavigationDetailsNew.company='Service Master Clean Limited' then 'SMCL'  
	when NavigationDetailsNew.company='Tech SIS Limited' then 'Tech SIS'  
	when NavigationDetailsNew.company='Terminix-SIS' then 'Terminix' 
	 when NavigationDetailsNew.company='Uniq Detective And Security Service Pvt. Ltd.' then 'Uniq' else 'DTSS' end as Company,NavigationDetailsNew.MenuSelection,NavigationDetailsNew.BussinessType,NavigationDetailsNew.AllocationType,NavigationDetailsNew.City,convert(varchar(12),
                        NavigationDetailsNew.LeaseRenewalDate,106) as LeaseRenewalDate,convert(varchar(12),NavigationDetailsNew.DateOfRequired,106) as DateOfRequired,NavigationDetailsNew.FitOuts  ,NavigationDetailsNew.ProposedLocation as Location ,
                        NavigationDetailsNew.ProposedSignage as Signage ,NavigationDetailsNew.ProposedEmployee as Employee ,NavigationDetailsNew.ProposedSuperBuiltUpArea as SuperBuiltUpArea ,NavigationDetailsNew.ProposedBuiltupArea  as BuiltUpArea  ,NavigationDetailsNew.ProposedCarpetArea as CarpetArea ,NavigationDetailsNew.ProposedRentalArea as RentalArea,
                        NavigationDetailsNew.ProposedRentalCost  as RentalCost ,NavigationDetailsNew.ProposedSecurityDeposit as SecurityDeposit,NavigationDetailsNew.ProposedCarPark as CarPark ,NavigationDetailsNew.ProposedRequest  as Request  ,NavigationDetailsNew.Remark2 as Remark,NavigationDetailsNew.Status,convert(varchar(12),NavigationDetailsNew.CreationDate,106) as CreationDate,NavigationDetailsNew.ProposedMonthlyCost as MonthlyCost,[UserDetails].FirstName+' '+[UserDetails].LastName as CreatedBy,[UserDetails].CompanyEmail,[UserDetails].PhoneNumber
                        FROM NavigationDetailsNew left outer join [UserDetails] on NavigationDetailsNew.CreatedBy=[UserDetails].UserName where @CompanyName LIKE '%'+ NavigationDetailsNew.Company +'%' and NavigationDetailsNew.Status  IN ('Reviewed')) as Temp ORDER BY NavigationAutoID,RequestId
                    ", UserName);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NavigationModel obj = new NavigationModel();
                        if (reader["RequestType"].ToString() == "Existing")
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ExistingLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.Signage = reader["Signage"].ToString();
                            obj.NoOfPersons = reader["Employee"].ToString();
                            obj.SuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.BuiltUp = reader["BuiltUpArea"].ToString();
                            obj.CarpetArea = reader["CarpetArea"].ToString();
                            obj.RentalArea = reader["RentalArea"].ToString();
                            obj.CostPerSquareFeet = reader["RentalCost"].ToString();
                            obj.SecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.CarPark = reader["CarPark"].ToString();
                            //obj.RequestedBy = reader["Request"].ToString();
                            obj.Remarks = reader["Remark"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.ExistingMonthlyCost = reader["MonthlyCost"].ToString();
                            obj.CreatedByName = reader["CreatedBy"].ToString();
                            obj.CreatedByEmail = reader["CompanyEmail"].ToString();
                            obj.CreatedByPhone = reader["PhoneNumber"].ToString();
                        }
                        else
                        {
                            obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                            obj.RequestID = reader["RequestId"].ToString();
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ProposedLocation = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                            obj.ProposedSignage = reader["Signage"].ToString();
                            obj.ProposedNoOfPersons = reader["Employee"].ToString();
                            obj.ProposedSuperBuiltUp = reader["SuperBuiltUpArea"].ToString();
                            obj.ProposedBuiltUp = reader["BuiltUpArea"].ToString();
                            obj.ProposedCarpetArea = reader["CarpetArea"].ToString();
                            obj.ProposedRentalArea = reader["RentalArea"].ToString();
                            obj.ProposedCostPerSquareFeet = reader["RentalCost"].ToString();
                            obj.ProposedSecurityDeposit = reader["SecurityDeposit"].ToString();
                            obj.ProposedCarPark = reader["CarPark"].ToString();
                            //obj.ProposedRequestedBy = reader["Request"].ToString();
                            obj.ProposedRemarks = reader["Remark"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.DataType = reader["RequestType"].ToString();
                            obj.CreationDate = reader["CreationDate"].ToString();
                            obj.ProposedMonthlyCost = reader["MonthlyCost"].ToString();
                            obj.CreatedByName = reader["CreatedBy"].ToString();
                            obj.CreatedByEmail = reader["CompanyEmail"].ToString();
                            obj.CreatedByPhone = reader["PhoneNumber"].ToString();
                        }

                        BranchDataList.Add(obj);
                    }
                }
                reader.Close();
            }
        }

        public void UpdateBranchRequest()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    string status = string.Empty;
                    foreach (NavigationModel obj in BranchDataList)
                    {
                        status = obj.Status;
                       if (obj.IsInitaited)
                        {
                            status = "Initiated";
                        }

                        if (obj.DataType == "Existing")
                        {
                            command.CommandText = string.Format(@"UPDATE NavigationDetailsNew SET ExistingLocation = @ExistingLocation,ExistingSignage = @ExistingSignage,
                                                    ExistingEmployee = @ExistingEmployee,ExistingSuperBuiltUpArea = @ExistingSuperBuiltUpArea,
                                                    ExistingBuiltUpArea = @ExistingBuiltUpArea ,ExistingCarpetArea = @ExistingCarpetArea,
                                                    ExistingRentalArea = @ExistingRentalArea ,ExistingRentalCost = @ExistingRentalCost,
                                                    ExistingSecurityDeposit = @ExistingSecurityDeposit ,ExistingCarPark = @ExistingCarPark,
                                                    Remark1 = @Remark1,LastUpdatedDate=Getdate(),ExistingMonthlyCost=@ExistingMonthlyCost,
                                                    Status = @Status Where NavigationAutoID = {0}", obj.ID);
                            command.Parameters.AddWithValue("@ExistingLocation", obj.ExistingLocation);
                            command.Parameters.AddWithValue("@ExistingSignage", obj.Signage);
                            command.Parameters.AddWithValue("@ExistingEmployee", obj.NoOfPersons);
                            command.Parameters.AddWithValue("@ExistingSuperBuiltUpArea", obj.SuperBuiltUp);
                            command.Parameters.AddWithValue("@ExistingBuiltUpArea", obj.BuiltUp);
                            command.Parameters.AddWithValue("@ExistingCarpetArea", obj.CarpetArea);
                            command.Parameters.AddWithValue("@ExistingRentalArea", obj.RentalArea);
                            command.Parameters.AddWithValue("@ExistingRentalCost", obj.CostPerSquareFeet);
                            command.Parameters.AddWithValue("@ExistingSecurityDeposit", obj.SecurityDeposit);
                            command.Parameters.AddWithValue("@ExistingCarPark", obj.CarPark);
                            command.Parameters.AddWithValue("@Remark1", obj.Remarks);
                            command.Parameters.AddWithValue("@ExistingMonthlyCost", obj.ExistingMonthlyCost);
                            command.Parameters.AddWithValue("@Status", status);

                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                        else
                        {
                            command.CommandText = string.Format(@"UPDATE NavigationDetailsNew SET ProposedLocation = @ProposedLocation, ProposedSignage = @ProposedSignage,
                            ProposedEmployee = @ProposedEmployee, ProposedSuperBuiltUpArea = @ProposedSuperBuiltUpArea,
                            ProposedBuiltupArea = @ProposedBuiltupArea, ProposedCarpetArea = @ProposedCarpetArea,
                            ProposedRentalArea = @ProposedRentalArea, ProposedRentalCost = @ProposedRentalCost,
                            ProposedSecurityDeposit = @ProposedSecurityDeposit, ProposedCarPark = @ProposedCarPark,
                            Remark2 = @Remark2,LastUpdatedDate=getdate(),ProposedMonthlyCost=@ProposedMonthlyCost,
                            Status = @Status Where NavigationAutoID = {0}", obj.ID);
                            command.Parameters.AddWithValue("@ProposedLocation", obj.ProposedLocation);
                            command.Parameters.AddWithValue("@ProposedSignage", obj.ProposedSignage);
                            command.Parameters.AddWithValue("@ProposedEmployee", obj.ProposedNoOfPersons);
                            command.Parameters.AddWithValue("@ProposedSuperBuiltUpArea", obj.ProposedSuperBuiltUp);
                            command.Parameters.AddWithValue("@ProposedBuiltupArea", obj.ProposedBuiltUp);
                            command.Parameters.AddWithValue("@ProposedCarpetArea", obj.ProposedCarpetArea);
                            command.Parameters.AddWithValue("@ProposedRentalArea", obj.ProposedRentalArea);
                            command.Parameters.AddWithValue("@ProposedRentalCost", obj.ProposedCostPerSquareFeet);
                            command.Parameters.AddWithValue("@ProposedSecurityDeposit", obj.ProposedSecurityDeposit);
                            command.Parameters.AddWithValue("@ProposedCarPark", obj.ProposedCarPark);
                            command.Parameters.AddWithValue("@Remark2", obj.ProposedRemarks);
                            command.Parameters.AddWithValue("@ProposedMonthlyCost", obj.ProposedMonthlyCost);
                            command.Parameters.AddWithValue("@Status", status);

                            command.ExecuteNonQuery();
                            command.Parameters.Clear();
                        }
                        EmailManager emailManager = new EmailManager();

                        if (obj.IsInitaited)
                        {
                            emailManager.NewEmailReviewed("NDI", obj.ID);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}