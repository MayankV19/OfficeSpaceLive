using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

namespace OfficeSpace.Models
{
    public class ChangePasswordModel
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        //[Required]
        //public string CurrentPassword { get; set; }

        [Required]
        public string NewPassword { get; set; }

        public string ChangePassword(string UserName)
        {
            string result = string.Empty;
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()))
            {
                connection.Open();
                SqlCommand command = new SqlCommand();
                command.Connection = connection;
                command.CommandText = string.Format(@"   
                    Declare @OldPassword nvarchar(50)
                    set @OldPassword=(select Password FROM UserDetails WHERE Username = '{0}')
                    if(@OldPassword='{1}')
					BEGIN    
					update  UserDetails set Password='{2}' WHERE Username = '{0}' and Password='{1}'
				    Select '1' as MessageString
                   	END
					ELSE
					BEGIn
					Select '-1' as MessageString
					END", UserName, EncryptPass(Password), EncryptPass(NewPassword));
                var obj = command.ExecuteScalar();
                result = obj == null ? string.Empty : obj.ToString();
            }

            return result;
        }

        private string EncryptPass(string Newpassword)
        {
            string msg = "";
            byte[] encode = new byte[Newpassword.Length];
            encode = Encoding.UTF8.GetBytes(Newpassword);
            msg = Convert.ToBase64String(encode);
            return msg;
        }

        private string DecodePass(string Oldpassword)
        {
            string returntext = "";
            byte[] mybyte = Encoding.ASCII.GetBytes(Oldpassword);
            returntext = System.Text.Encoding.UTF8.GetString(mybyte);
            return returntext;

        }

    }
}