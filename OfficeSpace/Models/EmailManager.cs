using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Web;

namespace OfficeSpace.Models
{
    public class EmailManager
    {
        public EmailManager()
        {
        }        

        public void SendEmail(MailMessage mail)
        {
            try
            {
                SmtpClient SmtpServer = new SmtpClient(ConfigurationManager.AppSettings["SMTPHost"]);
                mail.From = new MailAddress(ConfigurationManager.AppSettings["EmailAddress"]);
                mail.IsBodyHtml = true;
                SmtpServer.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                SmtpServer.Credentials = new System.Net.NetworkCredential(ConfigurationManager.AppSettings["EnquiryEmailAddress"], ConfigurationManager.AppSettings["EnquiryEmailPassword"]);
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {

            }
        }

        public void SendEmail(String subject, string body, List<string> recipients, List<string> ccEmails, bool isBodyHtml)
        {
            try
            {
                using (SmtpClient client = new SmtpClient())
                {
                    client.Host = ConfigurationManager.AppSettings["SMTPHost"];
                    client.Port = Convert.ToInt32(ConfigurationManager.AppSettings["SMTPPort"]);
                    client.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["EnquiryEmailAddress"], ConfigurationManager.AppSettings["EnquiryEmailPassword"]);
                    client.EnableSsl = true;
                    MailMessage mail = new MailMessage();
                    mail.From = new MailAddress(ConfigurationManager.AppSettings["EnquiryEmailAddress"]);
                    if (recipients.Count != 0)
                    {
                        mail.Subject = subject;
                      
                        AlternateView alternativeView = AlternateView.CreateAlternateViewFromString(body, null, MediaTypeNames.Text.Html);
                        mail.AlternateViews.Add(alternativeView);
                        mail.IsBodyHtml = isBodyHtml;
                     

                        foreach (string email in recipients)
                        {
                            mail.To.Add(email);
                            if (mail.To.Count > 0)
                            {
                                client.Send(mail);
                            }
                            mail.To.Clear();
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void SendEmailWithButton(String subject, string body, List<string> recipients, bool isBodyHtml, string approveButtonUrl, string disapproveButtonUrl)
        {
            StringBuilder addButtons = new StringBuilder();
            addButtons.AppendFormat("<br><br>");
            addButtons.AppendFormat("<button>&nbsp;<a href=\""+ approveButtonUrl + "\" style=\"color:#4CAF50;border-radius:5px;padding:15px 30px;display:inline-block;font-size:17px;text-decoration:none;font-family:sans-serif;\">Approve</a></button>&nbsp;&nbsp;");
            addButtons.AppendFormat("<button>&nbsp;<a href=\"" + disapproveButtonUrl + "\" style=\"color:#f44336;border-radius:5px;padding:15px 30px;display:inline-block;font-size:17px;text-decoration:none;font-family:sans-serif;\">Disapprove</a></button>");
            addButtons.AppendFormat("</body></html>");
            body = body.Replace("</body></html>", addButtons.ToString());

            SendEmail(subject, body, recipients, null, isBodyHtml);
        }

        public void EmailIntitate(NavigationModel model, bool IsReqMerged,string name)
        {
            string httpPort = ConfigurationManager.AppSettings["HttpPort"];
            string approveButtonURL = httpPort + "api/email/approvedetails?ID=" + model.ID + "&Name=" + name;
            string disapproveButtonURL = httpPort + "api/email/disapprovedetails?ID=" + model.ID + "&Name=" + name;
             GetOtherData(model);
            string emailBody = GetMailBody(model);

            if (!IsReqMerged)
            {
                if (model.BuisnessType == "Branch" && model.AllocationType == "Rent")
                {
                    //Email is sent to President for approval/disapproval
                    string emailID = GetEmailId("President", model.Company);
                    emailBody = emailBody + "<br/><b>For any changes please visit the application with your username and password !!<b>";
                    SendEmailWithButton("New Branch Office Requirement for Rent", emailBody, new List<string>() { emailID }, true,approveButtonURL,disapproveButtonURL);

                    //Information Email is sent to all other presidents
                    List<string> EmailIds = GetEmailIdList("President", model.Company);
                    emailBody = emailBody + "<br/><b>This email is just for your information.No Action required on this !!<b>";
                    SendEmail("New Branch Office Requirement for Rent", emailBody, EmailIds, null, true);
                }
                else if (model.BuisnessType.ToUpper() == "Region" && model.AllocationType == "Rent")
                {
                    string emailID = GetEmailId("President", model.Company);
                    SendEmail("New Region Office Requirement for Rent", emailBody, new List<string>() { emailID }, null, true);

                    List<string> EmailIds = GetEmailIdList("CEO");
                    SendEmailWithButton("New Region Office Requirement for Rent", emailBody, EmailIds, true, approveButtonURL, disapproveButtonURL);
                }
                else if ((model.BuisnessType.ToUpper() == "Branch" && model.AllocationType == "Buy") || (model.BuisnessType.ToUpper() == "Region" && model.AllocationType == "Buy"))
                {
                    //Email sent to the CEO of the company for approval/disapproval 
                    List<string> EmailIds = GetEmailIdList("CEO");
                    SendEmailWithButton("New Office Space Requirement", emailBody, EmailIds, true, approveButtonURL, disapproveButtonURL);
                }
            }
            else
            {
                List<string> EmailIds = GetEmailIdList("CEO");
                SendEmailWithButton("New Office Space Requirement", emailBody, EmailIds, true, approveButtonURL, disapproveButtonURL);
            }

        }

        public void NewEmailReviewed(string name, int ID)
        {
            NavigationModel model = GetData(ID, name);
            string httpPort = ConfigurationManager.AppSettings["HttpPort"];
            string approveButtonURL = httpPort + "api/email/approvedetails?ID=" + ID + "&Name=" + name;
            string disapproveButtonURL = httpPort + "api/email/disapprovedetails?ID=" + ID + "&Name=" + name;
           // model = GetOtherDataWithoutModel(ID);
            string emailBody = GetMailBody(model);
            //if approved email sent to karan + gaosain + GMD + procurement  
            string emailID = GetEmailId("President", model.Company);
            emailBody = emailBody + "<br/><b>For any changes please visit the application with your username and password !!<b>";
            SendEmailWithButton("New Branch Office Requirement for Rent- Reviewed", emailBody, new List<string>() { emailID }, true, approveButtonURL, disapproveButtonURL);
        }

        public void ApprovedEmail(string name, int ID)
        {
            NavigationModel model = GetData(ID, name);           
            string emailBody = GetMailBody(model);
            //if approved email sent to karan + gaosain + GMD + procurement    
            List<string> EmailIds = GetEmailIdList("Procurement");
            SendEmail("New office space requirement is approved", emailBody, EmailIds, null, true);
        }

        public void Disapproved(string name, int ID)
        {
            NavigationModel model = GetData(ID, name);
        
            string emailBody = GetMailBody(model);
            //if disapproved then information email sent to all other presidents 
            List<string> EmailIds = GetEmailIdList("President");
            SendEmail("New office space requirement is disapproved", emailBody, EmailIds, null, true);

            //Request will be closed
            CloseRequest(ID, name);
        }

        public void EmailReviewed(string name, int ID)
        {
            NavigationModel model = GetData(ID, name);

            string emailBody = GetMailBody(model);
            //if disapproved then information email sent to all other presidents 
            List<string> EmailIds = new List<String>() { model.CreatedByEmail };
            SendEmail("Please Review your new office space requirement", emailBody, EmailIds, null, true);

            //Request will be closed
            //  CloseRequest(ID, name);
        }

        public List<string> GetEmailIdList(string designation, string companyName = null)
        {
            List<string> companyEmailIds = new List<string>();

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                if (!string.IsNullOrEmpty(companyName))
                {
                    command.CommandText = string.Format("SELECT EmailID FROM DesignationNEmailID WHERE Designation = '{0}' and CompanyName != '{1}'", designation, companyName);
                }
                else
                {
                    command.CommandText = string.Format("SELECT EmailID FROM DesignationNEmailID WHERE Designation = '{0}'", designation);
                }
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        companyEmailIds.Add(reader[0].ToString());
                    }
                }
                reader.Close();

            }

            return companyEmailIds;
        }

        public string GetEmailId(string designation, string companyName)
        {
            string emailID = string.Empty;

            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = string.Format("SELECT EmailID FROM DesignationNEmailID WHERE Designation = '{0}' and CompanyName = '{1}'", designation, companyName);
                emailID = command.ExecuteScalar().ToString();
            }

            return emailID;
        }

        private void CloseRequest(int ID, string name)
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    if (name == "MR")
                    {
                        command.CommandText = @"UPDATE MergeRequests SET Status = 'Disapproved',IsClosed = 1 WHERE MergeAutoID =" + ID;
                    }
                    else
                    {
                        command.CommandText = @"UPDATE NavigationDetailsNew SET Status = 'Disapproved',LastUpdatedDate=getdate() WHERE NavigationAutoID =" + ID;
                    }

                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void GetOtherData(NavigationModel model)
        {
          
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"SELECT 
     [UserDetails].FirstName+' '+[UserDetails].LastName as CreatedBy,[UserDetails].CompanyEmail,[UserDetails].PhoneNumber
  FROM [dbo].[NavigationDetailsNew] left outer join [UserDetails] on NavigationDetailsNew.CreatedBy=[UserDetails].UserName  WHERE [NavigationAutoID] =" + model.ID;

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            model.CreatedByName = reader["CreatedBy"].ToString();
                            model.CreatedByEmail = reader["CompanyEmail"].ToString();
                            model.CreatedByPhone = reader["PhoneNumber"].ToString();
                        }
                    }
                    reader.Close();
                    command.ExecuteNonQuery();
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private NavigationModel GetOtherDataWithoutModel(int ID)
        {
            NavigationModel obj = new NavigationModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"SELECT 
     [UserDetails].FirstName+' '+[UserDetails].LastName as CreatedBy,[UserDetails].CompanyEmail,[UserDetails].PhoneNumber
  FROM [dbo].[NavigationDetailsNew] left outer join [UserDetails] on NavigationDetailsNew.CreatedBy=[UserDetails].UserName  WHERE [NavigationAutoID] =" + ID;

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            obj.CreatedByName = reader["CreatedBy"].ToString();
                            obj.CreatedByEmail = reader["CompanyEmail"].ToString();
                            obj.CreatedByPhone = reader["PhoneNumber"].ToString();
                        }
                    }
                    reader.Close();
                    command.ExecuteNonQuery();
                }
                return obj;
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }

        private NavigationModel GetData(int ID, string name)
        {
            NavigationModel obj = new NavigationModel();
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    string updateQuery = string.Empty;

                    if (name == "MR")
                    {
                        command.CommandText = @"SELECT Company,MenuSelection,BussinessType,SecurityDeposit,City,Location,Fitouts,NumberOfPersons,FinalPrice,Signage,SuperArea,Legal,CarPark,'BUY' as AllocationType
                                                FROM MergedRequests WHERE MergeAutoID =" + ID;
                        updateQuery = @"UPDATE MergedRequests SET Status = 'Approved' WHERE MergeAutoID =" + ID;
                    }
                    else
                    {
                        command.CommandText = @"select [Company]
      ,[MenuSelection]
      ,[BussinessType]
      ,[AllocationType]
      ,[City]
      ,convert(varchar(12),[LeaseRenewalDate],106) as LeaseRenewalDate
      ,convert(varchar(12),DateOfRequired,106) as DateOfRequired
      ,[FitOuts]
      ,[ExistingLocation]
      ,[ProposedLocation]
      ,[ExistingSignage]
      ,[ProposedSignage]
      ,[ExistingEmployee]
      ,[ProposedEmployee]
      ,[ExistingSuperBuiltUpArea]
      ,[ProposedSuperBuiltUpArea]
      ,[ExistingBuiltUpArea]
      ,[ProposedBuiltupArea]
      ,[ExistingCarpetArea]
      ,[ProposedCarpetArea]
      ,[ExistingRentalArea]
      ,[ProposedRentalArea]
      ,[ExistingRentalCost]
      ,[ProposedRentalCost]
      ,[ExistingSecurityDeposit]
      ,[ProposedSecurityDeposit]
      ,[ExistingCarPark]
      ,[ProposedCarPark]
      ,[ExistingRequest]
      ,[ProposedRequest]
      ,[Remark1]
      ,[Remark2]
      ,[Status],ExistingMonthlyCost,ProposedMonthlyCost, [UserDetails].FirstName+' '+[UserDetails].LastName as CreatedBy,[UserDetails].CompanyEmail,[UserDetails].PhoneNumber
       FROM NavigationDetailsNew 
left outer join [UserDetails] on NavigationDetailsNew.CreatedBy=[UserDetails].UserName 
WHERE [NavigationAutoID] =" + ID;
                        if (name == "NDI")
                        {
                            updateQuery = @"UPDATE NavigationDetailsNew SET Status = 'Initiated',LastUpdatedDate=getdate() WHERE NavigationAutoID =" + ID;
                       }
                        if (name == "NDR")
                        {
                            updateQuery = @"UPDATE NavigationDetailsNew SET Status = 'Reviewed',LastUpdatedDate=getdate() WHERE NavigationAutoID =" + ID;
                        }
                        else
                        {
                            updateQuery = @"UPDATE NavigationDetailsNew SET Status = 'Approved',LastUpdatedDate=getdate() WHERE NavigationAutoID =" + ID;
                        }
                    }

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            obj.Company = reader["Company"].ToString();
                            obj.SelectedMenu = reader["MenuSelection"].ToString();
                            obj.BuisnessType = reader["BussinessType"].ToString();
                            obj.AllocationType = reader["AllocationType"].ToString();
                            obj.City = reader["City"].ToString();
                            obj.ProposedDateofRenewal = reader["LeaseRenewalDate"].ToString();
                            obj.DateFromWhich = reader["DateOfRequired"].ToString();
                            obj.Fitouts = reader["FitOuts"].ToString();
                            obj.ExistingLocation = reader["ExistingLocation"].ToString();
                            obj.ProposedLocation = reader["ProposedLocation"].ToString();
                            obj.Signage = reader["ExistingSignage"].ToString();
                            obj.ProposedSignage = reader["ProposedSignage"].ToString();
                            obj.NoOfPersons = reader["ExistingEmployee"].ToString();
                            obj.ProposedNoOfPersons = reader["ProposedEmployee"].ToString();
                            obj.SuperBuiltUp = reader["ExistingSuperBuiltUpArea"].ToString();
                            obj.ProposedSuperBuiltUp = reader["ProposedSuperBuiltUpArea"].ToString();
                            obj.BuiltUp = reader["ExistingBuiltUpArea"].ToString();
                            obj.ProposedBuiltUp = reader["ProposedBuiltupArea"].ToString();
                            obj.CarpetArea = reader["ExistingCarpetArea"].ToString();
                            obj.ProposedCarpetArea = reader["ProposedCarpetArea"].ToString();
                            obj.RentalArea = reader["ExistingRentalArea"].ToString();
                            obj.ProposedRentalArea = reader["ProposedRentalArea"].ToString();
                            obj.CostPerSquareFeet = reader["ExistingRentalCost"].ToString();
                            obj.ProposedCostPerSquareFeet = reader["ProposedRentalCost"].ToString();
                            obj.SecurityDeposit = reader["ExistingSecurityDeposit"].ToString();
                            obj.ProposedSecurityDeposit = reader["ProposedSecurityDeposit"].ToString();
                            obj.CarPark = reader["ExistingCarPark"].ToString();
                            obj.ProposedCarPark = reader["ProposedCarPark"].ToString();
                            obj.RequestedBy = reader["ExistingRequest"].ToString();
                            obj.ProposedRequestedBy = reader["ProposedRequest"].ToString();
                            obj.Remarks = reader["Remark1"].ToString();
                            obj.ProposedRemarks = reader["Remark2"].ToString();
                            obj.Status = reader["Status"].ToString();
                            obj.ExistingMonthlyCost = reader["ExistingMonthlyCost"].ToString();
                            obj.ProposedMonthlyCost = reader["ProposedMonthlyCost"].ToString();
                            obj.CreatedByName = reader["CreatedBy"].ToString();
                            obj.CreatedByEmail = reader["CompanyEmail"].ToString();
                            obj.CreatedByPhone = reader["PhoneNumber"].ToString();

                        }
                    }
                    reader.Close();

                    command.CommandText = updateQuery;
                    command.ExecuteNonQuery();
                }

                return obj;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string GetMailBody(NavigationModel model)
        {
           
            StringBuilder bodyText = new StringBuilder(string.Empty);
            bodyText.AppendFormat("<html><body>");
            bodyText.AppendFormat("<p>Dear Sir/Mam,</p>");
            bodyText.AppendFormat("<p>New request for office space has been registered. Please find the details below:</p>");
            bodyText.AppendFormat("<table border = " + 1 + " cellpadding = " + 0 + " cellspacing = " + 0 + " width = " + 1000 + " >");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td width = " + 300 + ">Company Name : " + model.Company + " </td><td width = " + 300 + ">Request Type : " + model.SelectedMenu + "</td><td width = " + 300 + ">Bussinss Type : " + model.BuisnessType + " </td><td width = " + 300 + ">Allocation Type : " + model.AllocationType + "</td><td width = " + 300 + ">City : " + model.City + "</td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td width = " + 300 + ">Existing Lease Renewal Date : " + model.ProposedDateofRenewal + " </td><td width = " + 300 + ">Date from which property required : " + model.DateFromWhich + "</td><td width = " + 300 + ">Property Type : " + model.Fitouts + "</td><td width = " + 300 + ">Creation Date  : " + System.DateTime.Today.ToShortDateString() + "</td><td width = " + 300 + ">Created By Name/Email/Phone : "+model.CreatedByName+"/"+model.CreatedByEmail+"/"+model.CreatedByPhone+"</td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#08F9F5'><td>OTHER DETAILS </td><td>EXISTING </td><td>PROPOSED </td><td>OBSERVATIONS OF SIS STORE  </td><td> </td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Location : </td><td> " + model.ExistingLocation + " </td><td> " + model.ProposedLocation + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Road Name for Signage : </td><td> " + model.Signage + " </td><td> " + model.ProposedSignage + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Number of Employee : </td><td> " + model.NoOfPersons + " </td><td> " + model.ProposedNoOfPersons + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Super Built Up Area (In Sq. Ft.) : </td><td> " + model.SuperBuiltUp + " </td><td> " + model.ProposedSuperBuiltUp + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Built Up Area (In Sq. Ft.) : </td><td> " + model.BuiltUp + " </td><td> " + model.ProposedBuiltUp + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Carpet Area (In Sq. Ft.) : </td><td> " + model.CarpetArea + " </td><td> " + model.ProposedCarpetArea + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental Area (In Sq. Ft.) : </td><td> " + model.RentalArea + " </td><td> " + model.ProposedRentalArea + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Rental cost/square feet (In INR) : </td><td> " + model.CostPerSquareFeet + " </td><td> " + model.ProposedCostPerSquareFeet + " </td><td></td><td></td></tr>");
            //bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Monthly Rental Cost (In INR) : </td><td> " + model.ExistingMonthlyCost + " </td><td> " + model.ProposedMonthlyCost + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Total Monthly Rental Cost (In INR) : </td><td> " + (Convert.ToInt32(model.RentalArea) * Convert.ToInt32(model.CostPerSquareFeet)) + " </td><td> " + (Convert.ToInt32(model.ProposedRentalArea) * Convert.ToInt32(model.ProposedCostPerSquareFeet)) + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Security Deposit (In INR) : </td><td> " + model.SecurityDeposit + " </td><td> " + model.ProposedSecurityDeposit + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Number of Car Park : </td><td> " + model.CarPark + " </td><td> " + model.ProposedCarPark + " </td><td></td><td></td></tr>");
            //bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Requested By : </td><td> " + model.RequestedBy + " </td><td> " + model.ProposedRequestedBy + " </td><td></td><td></td></tr>");
            bodyText.AppendFormat("<tr bgcolor = '#DFF9F9'><td>Remarks :  </td><td> " + model.Remarks + " </td><td> " + model.ProposedRemarks + " </td><td></td><td></td></tr></table>");
            //bodyText.AppendFormat("<p>New request for office space has been registered. Please find the details below:</p>");
            bodyText.AppendFormat("</body></html>");

            return bodyText.ToString();
        }

    }
}