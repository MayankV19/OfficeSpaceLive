using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace OfficeSpace.Models
{
    public class ExistingRentalExcelModel
    {
        [DisplayName("SL.No.")]
        public int SNI { get; set; }

        [DisplayName("Date")]
        public string Date { get; set; }
        public string City { get; set; }
        [DisplayName("Company")]
        public string Company { get; set; }

        [DisplayName("BO/ RO/ CO_A/ CO")]
        public string BORO { get; set; }

        [DisplayName("Regional Office")]
        public string RegionalOffice { get; set; }

        [DisplayName("Office Name")]
        public string OfficeName { get; set; }

        [DisplayName("Office Location / Address")]
        public string OfficeAddress { get; set; }

        [DisplayName("Rent / Owned")]
        public string PropertyType { get; set; }

        [DisplayName("Lease Start Date")]
        public string LeaseStartDate { get; set; }


        [DisplayName("Lease Starting Rental Amount (INR)")]
        public string LeaseStartRentalAmount { get; set; }

        [DisplayName("Rental Escalation %")]
        public string RentalEscallation { get; set; }

        [DisplayName("Escalation Period yrs ( Yly/ 3Yrs/ No Escalation)")]
        public string EscallationPeriod { get; set; }

        [DisplayName("Lease Period (In Years)")]
        public string LeasePeriod { get; set; }

        [DisplayName("Lease Closure Date")]
        public string LeaseClouserDate { get; set; }

        [DisplayName("Security Deposit (INR)")]
        public string Security_Deposit { get; set; }

        [DisplayName("Advance Rental Paid (INR)")]
        public string AdvanceRental { get; set; }

        [DisplayName("Total Amount onhold with owner")]
        public string TotalAmountHoldWithOwner { get; set; }

        [DisplayName("Fitouts Provided by [ Self / Owner]")]
        public string FitoutsNew { get; set; }

        [DisplayName("Number of_Car Parking Allotted")]
        public string Car_Park { get; set; }

        [DisplayName("Notice Period (In Months)")]
        public string NoticePeriod { get; set; }


        [DisplayName("Signage covering Roads")]
        public string SignageRoads { get; set; }


        [DisplayName("Number of Employee")]
        public string NoOfEmployee { get; set; }

        [DisplayName("SuperBuilt UpArea (Sq.Ft.)")]
        public string SuperBuilt_UpArea { get; set; }
        [DisplayName("BuiltUp area (Sq.Ft.)")]
        public string BuiltUp_area { get; set; }
        [DisplayName("Carpet Area (Sq.Ft.)")]
        public string Carpet_Area { get; set; }
        [DisplayName("Rental Area (Sq.Ft.)")]
        public string Rental_Area { get; set; }
       


        [DisplayName("Present Rental Cost (INR/ Sq.Ft)")]
        public string PresentRentalCost { get; set; }

        [DisplayName("Present Monthly Rental Cost (INR)")]
        public string PresentMonthlyRentalCost { get; set; }

        [DisplayName("Present Monthly Billing (INR)")]
        public string PresentMonthlyBilling { get; set; }

        [DisplayName("Rental Cost % to Billing")]
        public string RenatlCostPercentage { get; set; }


        [DisplayName("Present Monthly Maintenance Cost (INR)")]
        public string MonthlyMaintenanceCost { get; set; }

        [DisplayName("Present Avg Monthly Maintenance Cost (INR)")]
        public string AvgMonthltMaintenanceCost { get; set; }

        [DisplayName("Present Avg Monthly Electricity Cost(INR)")]
        public string MonthlyElectricityCost { get; set; }

        [DisplayName("Present Avg_Monthly All_other Costs(INR)")]
        public string MonthlyAllOtherCosts { get; set; }

        [DisplayName("Total_Monthly Avg_Costs on_Rental &_Related (INR)")]
        public string TotalMonthlyRentalCost { get; set; }

        [DisplayName("Name")]
        public string Created_By_Name { get; set; }
        [DisplayName("Email")]
        public string Created_By_Email { get; set; }
        [DisplayName("Phone")]
        public string Created_By_Phone { get; set; }

        public string Remarks { get; set; }


    }
}