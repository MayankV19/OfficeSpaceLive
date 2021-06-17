using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Data;


namespace OfficeSpace.Models
{
    public class DashboardModel
    {
        public List<string> CompanyList { get; set; }

        public string Total { get; set; }
        public string Closed { get; set; }
        public string Underprocess { get; set; }
        public string Disapprove { get; set; }

        public string NewRequests { get; set; }
        public string LeaseExpire { get; set; }

        public void Init()
        {
            CompanyList= GetCompanyList();
            GetDashboardValues("ALL");
            GetDashboardValuesOther("ALL");
            GetDashboardValuesLeaseExpire("ALL");
        }
       
        public List<string> GetCompanyList()
        {
          List<string> companyList = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT CompanyName FROM CompanyMaster ORDER BY CompanyName";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        companyList.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
            return companyList;
        }

        public void GetDashboardValuesOther(string CompanyName)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                if (CompanyName == "ALL")
                {
                    command.CommandText = @" select count(*) as NewRequirement from [NavigationDetailsNew] where Status='Initiated' and Status not in ('Reviewed')";
                }
                else
                {
                    command.CommandText = @" select count(*) as NewRequirement from [NavigationDetailsNew] where Status='Initiated' and Status not in ('Reviewed') and Company='" + CompanyName+"'";
                }
                SqlDataReader reader = command.ExecuteReader();
            //string NewRequests = string.Empty;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        NewRequests = reader["NewRequirement"].ToString();
                       
                    }
                    reader.Close();
                }

            }
        }

        public void GetDashboardValuesLeaseExpire(string CompanyName)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                if (CompanyName == "ALL")
                {
                    command.CommandText = @" select count(*) as LeaseExpired from NavigationDetailsNew where 
 CONVERT(VARCHAR(10),LeaseRenewalDate , 120)  between CONVERT(VARCHAR(10), GETDATE() , 120) and CONVERT(VARCHAR(10),DATEADD(day, 90, GETDATE()) , 120)";
                }
                else
                {
                    command.CommandText = @" select count(*) as LeaseExpired from NavigationDetailsNew where 
 CONVERT(VARCHAR(10),LeaseRenewalDate , 120)  between CONVERT(VARCHAR(10), GETDATE() , 120) and CONVERT(VARCHAR(10),DATEADD(day, 90, GETDATE()) , 120) and Company='" + CompanyName + "'";
                }
                SqlDataReader reader = command.ExecuteReader();
                //string NewRequests = string.Empty;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        LeaseExpire = reader["LeaseExpired"].ToString();

                    }
                    reader.Close();
                }

            }
        }

        public void GetDashboardValues(string CompanyName)
        {           
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
               connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;

                if (CompanyName == "ALL")
                {
                    command.CommandText = @"SELECT Status = ISNULL(Status,'Total'), Count(*) as Count FROM
	                                (
		                                SELECT Status FROM (SELECT CASE WHEN Status IN ('Pending','Approved') THEN 'UnderProcess' ELSE Status END as 
                                        Status FROM NavigationDetailsNew) AS NavigationDetailsNew where Status not in ('Reviewed')
		                                UNION ALL
		                                SELECT Status FROM (SELECT CASE WHEN Status IN ('Pending','Approved') THEN 'UnderProcess' ELSE Status END as 
                                        Status FROM MergedRequests) AS MergedRequests where Status not in ('Reviewed')
	                                ) AS CombinedData Group by ROLLUP(Status)";
                }
                else
                {
                    command.CommandText = string.Format(@"SELECT Status = ISNULL(Status,'Total'), Count(*) as Count FROM
	                                (
		                                SELECT Status FROM (SELECT CASE WHEN Status IN ('Pending','Approved') THEN 'UnderProcess' ELSE Status END as 
                                        Status,Company FROM NavigationDetailsNew) AS NavigationDetailsNew  where Status not in ('Reviewed') and Company = '{0}'
		                                UNION ALL
		                                SELECT Status FROM (SELECT CASE WHEN Status IN ('Pending','Approved') THEN 'UnderProcess' ELSE Status END as 
                                        Status,Company FROM MergedRequests) AS MergedRequests   Where Status not in ('Reviewed') and Company = '{0}'
	                                ) AS CombinedData Group by ROLLUP(Status)", CompanyName);
                }
                Closed = "0";
                Disapprove = "0";
                Underprocess = "0";
                Total = "0";
             
                SqlDataReader reader = command.ExecuteReader();
                string status = string.Empty;
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        status = reader["Status"].ToString();
                        if(status == "Closed")
                        {
                            Closed = reader["Count"].ToString();
                        }
                        else if(status == "Disapproved")
                        {
                            Disapprove = reader["Count"].ToString();
                        }
                        else if(status == "UnderProcess")
                        {
                            Underprocess = reader["Count"].ToString();
                        }
                        else if(status == "Total")
                        {
                            Total = reader["Count"].ToString();
                        }

                    }
                }
                reader.Close();


            }           
        }
       
    }


}
