using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace OfficeSpace.Models
{
    public class NavigationModel
    { 
        public string SuperArea { get; set; }
        public string ExistingArea { get; set; }           
        public bool IsClosed { get; set; }
        public bool IsReviewed { get; set; }

        public bool IsInitaited { get; set; }

        public bool IsApproved { get; set; }
        public bool IsDisapproved { get; set; }
        public string Dateoflapse { get; set; }
        public string ProposedBudget { get; set; }
        //----------------------
        public int ID { get; set; }
        public string Company { get; set; }
        public string SelectedMenu { get; set; }
        public string Status { get; set; }
        public string BuisnessType { get; set; }
        [Required(ErrorMessage ="Please select the City from the list")]
        public string City        { get; set; }       
        public string AllocationType { get; set; }
        [Required]
        public string ProposedLocation { get; set; }
        [Required]
        public string ExistingLocation { get; set; }
        [Display(Name = "Location")]
        public string Location { get; set; }
        [Required]
        public string ProposedSignage { get; set; }
        [Required]
        public string Signage { get; set; }
        [Required]
      
        public string ProposedNoOfPersons { get; set; }
        [Required]
       
     
        public string NoOfPersons { get; set; }
        [Required]
      
        public string ProposedSuperBuiltUp { get; set; }
        [Required]
      
        public string SuperBuiltUp { get; set; }
        [Required]
        public string ProposedBuiltUp { get; set; }
        [Required]
        public string BuiltUp { get; set; }
        [Required]
        public string ProposedCarpetArea { get; set; }
        [Required]
        public string CarpetArea { get; set; }
        [Required]
        public string ProposedRentalArea { get; set; }
        [Required]
        public string RentalArea { get; set; }
        [Required]
        public string ProposedCostPerSquareFeet { get; set; }
        [Required]
        public string CostPerSquareFeet { get; set; }
        [Required]
        public string ProposedSecurityDeposit { get; set; }
        [Required]
        public string SecurityDeposit { get; set; }
       
        public string ProposedCarPark { get; set; }

        public string RequestID { get; set; }

        public string CarPark { get; set; }
        [Required]
        public string ProposedDateofRenewal{ get; set; }
             
        [Required]
        public string DateFromWhich { get; set; }
        [Required]
        public string ProposedRemarks { get; set; }
        [Required]
        public string Remarks { get; set; }
        [Required]
        public string Fitouts { get; set; }
   
        public string RequestedBy { get; set; }
 
        public string ProposedRequestedBy { get; set; }

        public string DataType { get; set; }
        
             public string CreationDate { get; set; }

        public string CreatedByName { get; set; }
        public string CreatedByEmail { get; set; }
        public string CreatedByPhone { get; set; }

     
        public string ExistingMonthlyCost { get; set; }
       
        public string ProposedMonthlyCost { get; set; }

        public List<string> CompanyList { get; set; }
        public List<string> CityList { get; set; }
        public List<UserDataModel> UserDataList { get; set; }       

        public void Init(string roleName,string userName = null)
        {
            GetCompanyList(roleName,userName);  
        }

        public void GetCompanyList(string roleName,string userName)
        {
            CompanyList = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                if (roleName == "User")
                {
                    //command.CommandText = @"SELECT CompanyMaster.CompanyName FROM UserDetails JOIN CompanyMaster 
                    //                        ON CompanyMaster.CompanyName = UserDetails.CompanyName
                    //                        WHERE  RoleName = 'User' and UserName = '" + userName + "'";
                    command.CommandText = @"declare @Company nvarchar(1000) set @Company=(select CompanyName from UserDetails where UserName='"+userName+"'  and  RoleName = 'User') select Item as CompanyName FROM dbo.SplitString(@Company, ',')";
                }
                else
                {
                    command.CommandText = @"declare @Company nvarchar(1000) set @Company=(select CompanyName from UserDetails where UserName='" + userName + "') select Item as CompanyName FROM dbo.SplitString(@Company, ',')";
                }
                SqlDataReader reader =  command.ExecuteReader();
                if(reader.HasRows)
                {
                    while(reader.Read())
                    {
                       CompanyList.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
        }

        public void GetCityList()
        {
            CityList = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT City FROM CityMaster ORDER BY City";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        CityList.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
        }

        public void GetUserRequests()
        {
            UserDataList = new List<UserDataModel>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM NavigationDetails WHERE IsMerged = 0 and Status != 'Closed'";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserDataModel obj = new UserDataModel();
                        obj.ID = int.Parse(reader["NavigationAutoID"].ToString());
                        obj.Company = reader["Company"].ToString();
                        obj.SelectedMenu = reader["MenuSelection"].ToString();
                        obj.BuisnessType = reader["BussinessType"].ToString();
                        obj.SecurityDeposit = reader["SecurityDeposit"].ToString();
                        obj.City = reader["City"].ToString();
                        obj.Fitouts = reader["Fitouts"].ToString();
                        obj.NoOfPersons = int.Parse(reader["NumberOfPersons"].ToString());
                        obj.CostPerSquareFeet = reader["FinalPrice"].ToString();
                        obj.Signage = reader["Signage"].ToString();
                        obj.SuperArea = reader["SuperArea"].ToString();
                        obj.Remarks = reader["Legal"].ToString();
                        obj.CarPark = reader["CarPark"].ToString();
                        obj.AllocationType = reader["AllocationType"].ToString();
                        obj.IsMerged = bool.Parse(reader["IsMerged"].ToString());
                        obj.Status = reader["Status"].ToString();
                        obj.Location = reader["Location"] == DBNull.Value ? string.Empty : reader["Location"].ToString();
                        UserDataList.Add(obj);
                    }
                }
                reader.Close();
            }
        }

        public int CreateNewRequest(string CreatedBy)
        {
            int ID = 0;
          try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO NavigationDetailsNew(Company,MenuSelection,BussinessType,AllocationType,City,LeaseRenewalDate,DateOfRequired,FitOuts
,ExistingLocation,ProposedLocation,ExistingSignage,ProposedSignage,ExistingEmployee,ProposedEmployee,ExistingSuperBuiltUpArea,ProposedSuperBuiltUpArea,ExistingBuiltUpArea,
ProposedBuiltupArea,ExistingCarpetArea,ProposedCarpetArea,ExistingRentalArea,ProposedRentalArea,ExistingRentalCost,ProposedRentalCost,ExistingSecurityDeposit,ProposedSecurityDeposit,
ExistingCarPark,ProposedCarPark,ExistingRequest,ProposedRequest,Remark1,Remark2,Status,CreationDate,LastUpdatedDate,ExistingMonthlyCost,ProposedMonthlyCost,CreatedBy) values (@Company,@MenuSelection,@BussinessType,@AllocationType,@City,@LeaseRenewalDate,@DateOfRequired,@FitOuts
,@ExistingLocation,@ProposedLocation,@ExistingSignage,@ProposedSignage,@ExistingEmployee,@ProposedEmployee,@ExistingSuperBuiltUpArea,@ProposedSuperBuiltUpArea,@ExistingBuiltUpArea,
@ProposedBuiltupArea,@ExistingCarpetArea,@ProposedCarpetArea,@ExistingRentalArea,@ProposedRentalArea,@ExistingRentalCost,@ProposedRentalCost,@ExistingSecurityDeposit,@ProposedSecurityDeposit,
@ExistingCarPark,@ProposedCarPark,@ExistingRequest,@ProposedRequest,@Remark1,@Remark2,@Status,getdate(),getdate(),@ExistingMonthlyCost,@ProposedMonthlyCost,@CreatedBy)
  SELECT SCOPE_IDENTITY()";
                                       
                    command.Parameters.AddWithValue("@Company", Company );
                    command.Parameters.AddWithValue("@MenuSelection", SelectedMenu );
                    command.Parameters.AddWithValue("@BussinessType", BuisnessType) ;
                    command.Parameters.AddWithValue("@AllocationType", AllocationType );
                    command.Parameters.AddWithValue("@City", City );
                    command.Parameters.AddWithValue("@LeaseRenewalDate", ProposedDateofRenewal == null ? string.Empty : ProposedDateofRenewal);
                    command.Parameters.AddWithValue("@DateOfRequired", DateFromWhich == null ? string.Empty : DateFromWhich);
                    command.Parameters.AddWithValue("@Fitouts", Fitouts);
                    command.Parameters.AddWithValue("@ExistingLocation", ExistingLocation == null ? string.Empty : ExistingLocation);
                    command.Parameters.AddWithValue("@ProposedLocation", ProposedLocation == null ? string.Empty : ProposedLocation);
                    command.Parameters.AddWithValue("@ExistingSignage", Signage == null ? string.Empty : Signage);
                    command.Parameters.AddWithValue("@ProposedSignage", ProposedSignage == null ? string.Empty : ProposedSignage);
                    command.Parameters.AddWithValue("@ExistingEmployee", NoOfPersons == null ? string.Empty : NoOfPersons);
                    command.Parameters.AddWithValue("@ProposedEmployee", ProposedNoOfPersons == null ? string.Empty : ProposedNoOfPersons);
                    command.Parameters.AddWithValue("@ExistingSuperBuiltUpArea", SuperBuiltUp == null ? string.Empty : SuperBuiltUp);
                    command.Parameters.AddWithValue("@ProposedSuperBuiltUpArea", ProposedSuperBuiltUp == null ? string.Empty : ProposedSuperBuiltUp);
                    command.Parameters.AddWithValue("@ExistingBuiltUpArea", BuiltUp == null ? string.Empty : BuiltUp);
                    command.Parameters.AddWithValue("@ProposedBuiltupArea", ProposedBuiltUp == null ? string.Empty : ProposedBuiltUp);
                    command.Parameters.AddWithValue("@ExistingCarpetArea", CarpetArea == null ? string.Empty : CarpetArea);
                    command.Parameters.AddWithValue("@ProposedCarpetArea", ProposedCarpetArea == null ? string.Empty : ProposedCarpetArea);
                    command.Parameters.AddWithValue("@ExistingRentalArea", RentalArea == null ? string.Empty : RentalArea);
                    command.Parameters.AddWithValue("@ProposedRentalArea", ProposedRentalArea == null ? string.Empty : ProposedRentalArea);
                    command.Parameters.AddWithValue("@ExistingRentalCost", CostPerSquareFeet == null ? string.Empty : CostPerSquareFeet);
                    command.Parameters.AddWithValue("@ProposedRentalCost", ProposedCostPerSquareFeet == null ? string.Empty : ProposedCostPerSquareFeet);
                    command.Parameters.AddWithValue("@ExistingSecurityDeposit", SecurityDeposit == null ? string.Empty : SecurityDeposit);
                    command.Parameters.AddWithValue("@ProposedSecurityDeposit", ProposedSecurityDeposit == null ? string.Empty : ProposedSecurityDeposit);
                    command.Parameters.AddWithValue("@ExistingCarPark", CarPark );
                    command.Parameters.AddWithValue("@ProposedCarPark", ProposedCarPark );
                    command.Parameters.AddWithValue("@ExistingRequest", RequestedBy == null ? string.Empty : RequestedBy);
                    command.Parameters.AddWithValue("@ProposedRequest", ProposedRequestedBy == null ? string.Empty : ProposedRequestedBy);
                    command.Parameters.AddWithValue("@Remark1", Remarks == null ? string.Empty : Remarks);
                    command.Parameters.AddWithValue("@Remark2", ProposedRemarks == null ? string.Empty : ProposedRemarks);
                    command.Parameters.AddWithValue("@Status", "Initiated");
                    command.Parameters.AddWithValue("@ExistingMonthlyCost", (Convert.ToInt32(RentalArea) * Convert.ToInt32(CostPerSquareFeet)));
                    command.Parameters.AddWithValue("@ProposedMonthlyCost", (Convert.ToInt32(ProposedRentalArea) * Convert.ToInt32(ProposedCostPerSquareFeet)));
                    //command.Parameters.AddWithValue("@ExistingMonthlyCost", ExistingMonthlyCost == null ? string.Empty : ExistingMonthlyCost);
                    //command.Parameters.AddWithValue("@ProposedMonthlyCost", ProposedMonthlyCost == null ? string.Empty : ProposedMonthlyCost);
                    command.Parameters.AddWithValue("@CreatedBy", CreatedBy == null ? string.Empty : CreatedBy);
                    ID = int.Parse(command.ExecuteScalar().ToString());
                  
                }
                return ID;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateMergeData()
        {
            try
            {
                List<UserDataModel> userData = UserDataList.Where(r => r.IsMerged == true).ToList();
                if (userData.Count >= 2)
                {
                    using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand();
                        command.Connection = connection;

                        //Merge the rows, sum of the fields and insert into merged request table
                        command.CommandText = string.Format(@"INSERT INTO MergedRequests(SecurityDeposit,City,NumberOfPersons,FinalPrice,SuperArea,CarPark,Status,IsClosed)
                                                            SELECT SUM(Convert(float,SecurityDeposit)) as SecurityDeposit,City,SUM(Convert(int,NumberOfPersons)) as NumberOfPersons,
                                                            SUM(Convert(float,FinalPrice)) as FinalPrice,SUM(Convert(float,SuperArea)) as SuperArea,
                                                            SUM(Convert(int,CarPark)) as CarPark ,'Initiated' as Status,0 as IsClosed FROM NavigationDetails WHERE NavigationAutoID IN ({0})
                                                            GROUP BY  City SELECT SCOPE_IDENTITY()",
                                                           string.Join(",", userData.Select(r => r.ID)));

                        int rowID = int.Parse(command.ExecuteScalar().ToString());

                        command.CommandText = string.Format(@"UPDATE NavigationDetails SET Status = 'Merged',IsMerged = 1 , MergedRowID = {0} WHERE NavigationAutoID IN ({1})", rowID, string.Join(",", UserDataList.Where(r => r.IsMerged == true).Select(r => r.ID)));
                        command.ExecuteNonQuery();

                    }
                }
                else
                {
                    throw new Exception("Please select more than one row");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}