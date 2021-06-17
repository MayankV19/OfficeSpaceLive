using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace OfficeSpace.Models
{
    public class DataModel
    {
        [DisplayName("S.No.")]
        public string SN { get; set; }
        [DisplayName("Creation Date")]
        public string CreationDate { get; set; }
        public string City { get; set; }
        [DisplayName("Company")]
        public string Company { get; set; }

        [DisplayName("Type")]
        public string Type { get; set; }
        [DisplayName("Status")]
        public string Status { get; set; }

        [DisplayName("Request Type")]
        public string Request_Type { get; set; }
        [DisplayName("Branch/R.o.")]
        public string Branch { get; set; }
        [DisplayName("Rent/Buy")]
        public string Rent_Buy { get; set; }

        [DisplayName("Lease Renew Date")]
        public string Lease_Renew_Date { get; set; }
        [DisplayName("Requirement Date")]
        public string Requirement_Date { get; set; }
        public string Fitouts { get; set; }
        [DisplayName("Created By Name")]
        public string Created_By_Name { get; set; }
        [DisplayName("Created By Email")]
        public string Created_By_Email { get; set; }
        [DisplayName("Created By Phone")]
        public string Created_By_Phone { get; set; }
        public string Location { get; set; }
        public string Signage { get; set; }
        [DisplayName("Employee Count")]
        public string Employee_Count { get; set; }
        [DisplayName("SuperBuilt UpArea (Sq.Ft.)")]
        public string SuperBuilt_UpArea { get; set; }
        [DisplayName("BuiltUp area (Sq.Ft.)")]
        public string BuiltUp_area { get; set; }
        [DisplayName("Carpet Area (Sq.Ft.)")]
        public string Carpet_Area { get; set; }
        [DisplayName("Rental Area (Sq.Ft.)")]
        public string Rental_Area { get; set; }
        [DisplayName("Rental Cost (INR/Sq.Ft)")]
        public string Rental_Cost { get; set; }
        [DisplayName("Total Monthly Rental Cost(INR)")]
        public string Total_Monthly_Rental_Cost { get; set; }
        [DisplayName("Security Deposit (INR)")]
        public string Security_Deposit { get; set; }
        [DisplayName("Car Park")]
        public string Car_Park { get; set; }
        public string Remarks { get; set; }



    }
}