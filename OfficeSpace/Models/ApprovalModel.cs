using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OfficeSpace.Models
{
    public class ApprovalModel
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
                        FROM OfficeSpace.dbo.NavigationDetailsNew left outer join [UserDetails] on NavigationDetailsNew.CreatedBy=[UserDetails].UserName where @CompanyName LIKE '%'+ NavigationDetailsNew.Company +'%' and NavigationDetailsNew.Status NOT IN ('Closed','Disapproved','Reviewed')
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
                        FROM NavigationDetailsNew left outer join [UserDetails] on NavigationDetailsNew.CreatedBy=[UserDetails].UserName where @CompanyName LIKE '%'+ NavigationDetailsNew.Company +'%' and NavigationDetailsNew.Status NOT IN ('Closed','Disapproved','Reviewed')) as Temp ORDER BY NavigationAutoID,RequestId
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

        public void GetBranchRequests(string Flag, string Company)
        {
            BranchDataList = new List<NavigationModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                string query = @"SELECT * FROM (
                       
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
                        FROM OfficeSpace.dbo.NavigationDetailsNew left outer join [UserDetails] on NavigationDetailsNew.CreatedBy=[UserDetails].UserName {0}
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
                        FROM NavigationDetailsNew left outer join [UserDetails] on NavigationDetailsNew.CreatedBy=[UserDetails].UserName {0}) as Temp ORDER BY NavigationAutoID,RequestId";

                if (Flag == "1")
                {
                    if (Company == "ALL")
                    {
                      command.CommandText = string.Format(query, "WHERE NavigationDetailsNew.Status != '' and NavigationDetailsNew.Status not in ('Reviewed')");
                      
                    }
                    else
                    {
                        command.CommandText = string.Format(query, "WHERE NavigationDetailsNew.Status not in ('Reviewed') and NavigationDetailsNew.Company = '" + Company + "'");
                    }
                }
                if (Flag == "2")
                {
                    if (Company == "ALL")
                    {
                        //command.CommandText = string.Format(query, "WHERE Status NOT IN ('Closed','Disapproved')");
                        command.CommandText = string.Format(query, "WHERE NavigationDetailsNew.Status  IN ('Approved')");
                    }
                    else
                    {
                        command.CommandText = string.Format(query, "WHERE  NavigationDetailsNew.Status  IN ('Approved') and NavigationDetailsNew.Company = '" + Company + "'");
                    }
                }
                if (Flag == "3")
                {
                    if (Company == "ALL")
                    {
                        command.CommandText = string.Format(query, "WHERE  NavigationDetailsNew.Status  IN ('Closed')");
                    }
                    else
                    {
                        command.CommandText = string.Format(query, "WHERE  NavigationDetailsNew.Status  IN ('Closed') and NavigationDetailsNew.Company = '" + Company + "'");
                    }
                }
                if (Flag == "4")
                {
                    if (Company == "ALL")
                    {
                        command.CommandText = string.Format(query, "WHERE  NavigationDetailsNew.Status  IN ('Disapproved')");
                    }
                    else
                    {
                        command.CommandText = string.Format(query, "WHERE  NavigationDetailsNew.Status  IN ('Disapproved') and NavigationDetailsNew.Company = '" + Company + "'");
                    }
                }
                if (Flag == "5")
                {
                    if (Company == "ALL")
                    {
                        command.CommandText = string.Format(query, "WHERE  NavigationDetailsNew.Status  IN ('Initiated')");
                    }
                    else
                    {
                        command.CommandText = string.Format(query, "WHERE  NavigationDetailsNew.Status  IN ('Initiated') and NavigationDetailsNew.Company = '" + Company + "'");
                    }
                }
                if (Flag == "6")
                {
                    if (Company == "ALL")
                    {
                        command.CommandText = string.Format(query, @"WHERE CONVERT(VARCHAR(10), NavigationDetailsNew.LeaseRenewalDate, 120)  between CONVERT(VARCHAR(10), GETDATE(), 120) 
                                                                    and CONVERT(VARCHAR(10), DATEADD(day, 90, GETDATE()), 120)");
                    }
                    else
                    {
                        command.CommandText = string.Format(query, @"WHERE CONVERT(VARCHAR(10), NavigationDetailsNew.LeaseRenewalDate, 120)  between CONVERT(VARCHAR(10), GETDATE(), 120) 
                                                                    and CONVERT(VARCHAR(10), DATEADD(day, 90, GETDATE()), 120) and NavigationDetailsNew.Company = '" + Company + "'");
                    }
                }

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
                            obj.RequestedBy = reader["Request"].ToString();
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
                            obj.ProposedRequestedBy = reader["Request"].ToString();
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

        public void GetUserRequests()
        {
            RequestDataList = new List<MergedRequestDataModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"SELECT MergeAutoID as ID,Company,MenuSelection,BussinessType,SecurityDeposit,City,Location,Fitouts,NumberOfPersons,FinalPrice,Signage,SuperArea,Legal,CarPark,Status,'MR' as Name
                                        FROM MergedRequests WHERE IsClosed = 0 and Status != 'Closed'
                                        UNION
                                        SELECT NavigationAutoID as ID,Company,MenuSelection,BussinessType,SecurityDeposit,City,Location,Fitouts,NumberOfPersons,FinalPrice,Signage,SuperArea,Legal,CarPark,Status,'ND' as Name
                                        FROM NavigationDetails WHERE IsMerged = 0 and Status IN ('Initiated','Pending') and BussinessType = 'Region'";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        MergedRequestDataModel obj = new MergedRequestDataModel();
                        obj.ID = int.Parse(reader["ID"].ToString());
                        obj.Company = reader["Company"].ToString();
                        obj.SelectedMenu = reader["MenuSelection"].ToString();
                        obj.BuisnessType = reader["BussinessType"].ToString();
                        obj.SecurityDeposit = reader["SecurityDeposit"].ToString();
                        obj.City = reader["City"].ToString();
                        obj.Location = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                        obj.Fitouts = reader["Fitouts"].ToString();
                        obj.NoOfPersons = int.Parse(reader["NumberOfPersons"].ToString());
                        obj.CostPerSquareFeet = reader["FinalPrice"].ToString();
                        obj.Signage = reader["Signage"].ToString();
                        obj.SuperArea = reader["SuperArea"].ToString();
                        obj.Remarks = reader["Legal"].ToString();
                        obj.CarPark = reader["CarPark"].ToString();
                        obj.Status = reader["Status"].ToString();
                        obj.Name = reader["Name"].ToString();
                        RequestDataList.Add(obj);
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
                        if (obj.IsClosed)
                        {
                            status = "Closed";
                        }
                        if (obj.IsApproved)
                        {
                            status = "Approved";
                        }
                        if (obj.IsDisapproved)
                        {
                            status = "Disapproved";
                        }
                        if (obj.IsReviewed)
                        {
                            status = "Reviewed";
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

                        if (obj.IsApproved)
                        {
                            emailManager.ApprovedEmail("ND", obj.ID);
                        }

                        if (obj.IsDisapproved)
                        {
                            emailManager.Disapproved("ND", obj.ID);
                        }
                        if (obj.IsReviewed)
                        {
                            emailManager.EmailReviewed("NDR", obj.ID);
                        }
                    }
                }
                }
            
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateRecommendedRequest()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    foreach (MergedRequestDataModel obj in RequestDataList)
                    {
                        if (obj.Name == "MR")
                        {
                            command.CommandText = @"UPDATE MergedRequests SET Company = @Company,MenuSelection = @MenuSelection,BussinessType = @BussinessType,SecurityDeposit =@SecurityDeposit,
                                                    Fitouts = @Fitouts,NumberOfPersons = @NumberOfPersons,FinalPrice = @FinalPrice,Signage = @Signage,SuperArea = @SuperArea,CarPark = @CarPark,
                                                    IsClosed = @IsClosed,Status = @Status WHERE MergeAutoID =" + obj.ID;
                            command.Parameters.AddWithValue("@Company", obj.Company);
                            command.Parameters.AddWithValue("@MenuSelection", obj.SelectedMenu);
                            command.Parameters.AddWithValue("@BussinessType", obj.BuisnessType);
                            command.Parameters.AddWithValue("@SecurityDeposit", obj.SecurityDeposit);
                            command.Parameters.AddWithValue("@Fitouts", obj.Fitouts);
                            command.Parameters.AddWithValue("@NumberOfPersons", obj.NoOfPersons);
                            command.Parameters.AddWithValue("@FinalPrice", obj.CostPerSquareFeet);
                            command.Parameters.AddWithValue("@Signage", obj.Signage);
                            command.Parameters.AddWithValue("@SuperArea", obj.SuperArea);
                            command.Parameters.AddWithValue("@CarPark", obj.CarPark);
                            command.Parameters.AddWithValue("@IsClosed", obj.IsClosed);
                            command.Parameters.AddWithValue("@Status", obj.IsClosed == true ? "Closed" : "Pending");
                        }
                        else
                        {
                            if (obj.IsClosed)
                            {
                                command.CommandText = @"UPDATE NavigationDetails SET Status = 'Closed' WHERE NavigationAutoID =" + obj.ID;
                            }
                        }

                        command.ExecuteNonQuery();
                        command.Parameters.Clear();

                        //Email to CEO
                        if (!obj.IsClosed && obj.IsRecommended)
                        {
                            NavigationModel model = new NavigationModel();
                            model.ID = obj.ID;
                            model.AllocationType = "BUY";
                            model.Company = obj.Company;
                            model.SelectedMenu = obj.SelectedMenu;
                            model.BuisnessType = obj.BuisnessType;
                            model.SecurityDeposit = obj.SecurityDeposit;
                            model.Fitouts = obj.Fitouts;
                            model.NoOfPersons = Convert.ToString(obj.NoOfPersons);
                            model.CostPerSquareFeet = obj.CostPerSquareFeet;
                            model.Signage = obj.Signage;
                            model.SuperArea = obj.SuperArea;
                            model.CarPark = obj.CarPark;
                            EmailManager emailManager = new EmailManager();
                            emailManager.EmailIntitate(model, obj.Name == "MR" ? true : false, obj.Name);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public void GetCompanyNameforID()
        {
            IDandCompanyNameList = new Dictionary<int, List<string>>();
            IDandMenuSelection = new Dictionary<int, List<string>>();
            IDandBuisnessType = new Dictionary<int, List<string>>();

            int ID = 0;
            string companyName = string.Empty;
            string requestType = string.Empty;
            string buisnessType = string.Empty;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = @"SELECT MergeAutoID,ND.Company,ND.MenuSelection,ND.BussinessType FROM NavigationDetails ND JOIN MergedRequests MR ON MR.MergeAutoID = ND.MergedRowID
                                        ORDER BY MR.MergeAutoID";

                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        ID = int.Parse(reader[0].ToString());
                        companyName = reader[1].ToString();
                        requestType = reader[2].ToString();
                        buisnessType = reader[3].ToString();

                        if (!IDandCompanyNameList.ContainsKey(ID))
                            IDandCompanyNameList.Add(ID, new List<string>());
                        if (!IDandCompanyNameList[ID].Contains(companyName))
                            IDandCompanyNameList[ID].Add(companyName);

                        if (!IDandMenuSelection.ContainsKey(ID))
                            IDandMenuSelection.Add(ID, new List<string>());
                        if (!IDandMenuSelection[ID].Contains(requestType))
                            IDandMenuSelection[ID].Add(requestType);

                        if (!IDandBuisnessType.ContainsKey(ID))
                            IDandBuisnessType.Add(ID, new List<string>());
                        if (!IDandBuisnessType[ID].Contains(buisnessType))
                            IDandBuisnessType[ID].Add(buisnessType);
                    }
                }
                reader.Close();

            }
        }
    }
}