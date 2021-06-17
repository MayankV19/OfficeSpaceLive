using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace OfficeSpace.Models
{
    public class SettingsModel
    {
        [Required]
        public string NewUserName { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }       
        public string CompanyName { get; set; }
        [Required]
        public string CompanyEmail { get; set; }
        [Required]
        public string Position { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string UserPassword { get; set; }
        [Required]
        public string UserRole { get; set; }
        public List<UserProfile> UsersList { get; set; }
        public List<RoleNames> UserRoles { get; set; } 
        public List<string> CompanyList { get; set; }
        [Required]
        public string SelectedCompanyList { get; set; }

        public void Init()
        {
            UsersList = new List<UserProfile>();
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = "SELECT * FROM UserDetails";
                SqlDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        UserProfile obj = new UserProfile();
                        obj.UserId = int.Parse(reader["UserID"].ToString());
                        obj.FirstName = reader["FirstName"].ToString();
                        obj.LastName = reader["LastName"].ToString();
                        CompanyName = reader["CompanyName"].ToString();
                        obj.CompanyEmail = reader["CompanyEmail"].ToString();
                        obj.Position = reader["Position"].ToString();
                        obj.UserName = reader["UserName"].ToString();
                        obj.Password = reader["Password"].ToString();
                        obj.IsActive = bool.Parse(reader["IsActive"].ToString());
                        obj.Country = reader["Country"].ToString();
                        obj.PhoneNumber = reader["PhoneNumber"].ToString();
                        obj.UserRole = reader["RoleName"].ToString();

                        if(!string.IsNullOrEmpty(CompanyName))
                        {
                            obj.CompanyName = new List<string>();
                            obj.CompanyName.AddRange(CompanyName.Split(',').ToList());
                        }

                        UsersList.Add(obj);
                    }
                }
                reader.Close();

                UserRoles = new List<RoleNames>();
                command.CommandText = "SELECT * FROM UserRoles";
                reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        RoleNames obj = new RoleNames();
                        obj.ID = int.Parse(reader["ID"].ToString());
                        obj.RoleName = reader["RoleName"].ToString();
                       
                        UserRoles.Add(obj);
                    }
                }
                reader.Close();
            }
        }

        public void GetCompanyList()
        {
            CompanyList = new List<string>();

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
                        CompanyList.Add(reader[0].ToString());
                    }
                }
                reader.Close();
            }
        }

        public bool CheckUsernameExists()
        {
            try
            {
                int result = 0;
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = string.Format("SELECT COUNT(*) FROM UserDetails WHERE Username = '{0}'", NewUserName);
                    result = int.Parse(command.ExecuteScalar().ToString());
                }

                return result == 0 ? false : true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void CreateUser()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;
                    command.CommandText = @"INSERT INTO UserDetails(FirstName,LastName,CompanyName,CompanyEmail,Position,Username,Password,IsActive,RoleName) 
                                            VALUES(@FirstName,@LastName,@CompanyName,@CompanyEmail,@Position,@Username,@Password,@IsActive,@RoleName)";
                    command.Parameters.AddWithValue("@FirstName", FirstName);
                    command.Parameters.AddWithValue("@LastName", LastName);
                    command.Parameters.AddWithValue("@CompanyName", SelectedCompanyList);
                    command.Parameters.AddWithValue("@CompanyEmail", CompanyEmail);
                    command.Parameters.AddWithValue("@Position", Position);
                    command.Parameters.AddWithValue("@Username", NewUserName);
                    command.Parameters.AddWithValue("@Password", EncryptPass(UserPassword));
                    command.Parameters.AddWithValue("@IsActive", true);
                    command.Parameters.AddWithValue("@RoleName", UserRole);
                    command.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private string EncryptPass(string password)
        {
            string msg = "";
            byte[] encode = new byte[password.Length];
            encode = Encoding.UTF8.GetBytes(password);
            msg = Convert.ToBase64String(encode);
            return msg;
        }

        public void UpdateUsers()
        {
            try
            {
                using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command.Connection = connection;

                    foreach (UserProfile userInfo in UsersList)
                    {                        
                        command.CommandText = string.Format(@"UPDATE UserDetails SET Country = @Country ,PhoneNumber = @PhoneNumber,RoleName = @RoleName, IsActive = @IsActive Where UserID = {0}",userInfo.UserId);
                        command.Parameters.AddWithValue("@Country", string.IsNullOrEmpty(userInfo.Country) ? string.Empty : userInfo.Country );
                        command.Parameters.AddWithValue("@PhoneNumber", string.IsNullOrEmpty(userInfo.PhoneNumber) ? string.Empty : userInfo.PhoneNumber);
                        command.Parameters.AddWithValue("@RoleName", userInfo.UserRole);
                        command.Parameters.AddWithValue("@IsActive", userInfo.IsActive);                        
                        command.ExecuteNonQuery();
                        command.Parameters.Clear();
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
