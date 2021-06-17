using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSpace.Models
{
    public class FurnitureUpdateDetails
    {
        public int Id { get; set; }
        public Nullable<System.DateTime> Date { get; set; }
        public string City { get; set; }
        public string Company { get; set; }
        public string OfficeType { get; set; }
        public string Address { get; set; }
        public string PropertyType { get; set; }
        public Nullable<System.DateTime> LeaseStartDate { get; set; }
        public string LeaseStartingAmount { get; set; }
        public string RentalEscallation { get; set; }
        public string EscallationPeriod { get; set; }
        public string LeasePeriod { get; set; }
        public Nullable<System.DateTime> LeaseClouserDate { get; set; }
        public string SecurityDeposit { get; set; }
        public string AdvanceRental { get; set; }
        public string TotalAmountHoldWithOwner { get; set; }
        public string FitOuts { get; set; }
        public string NoOfCarParking { get; set; }
        public string NoticePeriod { get; set; }
        public string Signage { get; set; }
        public string NoOfEmployee { get; set; }
        public string SuperBuiltUpArea { get; set; }
        public string BuiltUpArea { get; set; }
        public string CarpetArea { get; set; }
        public string RentalArea { get; set; }
        public string PresentRentalCost { get; set; }
        public string PresentMonthlyRentalCost { get; set; }
        public string PresentMonthlyBilling { get; set; }
        public string RenatlCostPercentage { get; set; }
        public string MonthlyMaintenanceCost { get; set; }
        public string AvgMonthltMaintenanceCost { get; set; }
        public string MonthlyElectricityCost { get; set; }
        public string MonthlyAllOtherCosts { get; set; }
        public string TotalMonthlyRentalCost { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public string Remarks { get; set; }
        public string OfficeName { get; set; }
        public string RegionalOffice { get; set; }
    }
}