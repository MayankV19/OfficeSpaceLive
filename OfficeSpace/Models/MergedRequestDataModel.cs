using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OfficeSpace.Models
{
    public class MergedRequestDataModel
    {
        public int ID { get; set; }
        public string Company { get; set; }
        public string SelectedMenu { get; set; }
        public string BuisnessType { get; set; }
        public string City { get; set; }
        public int NoOfPersons { get; set; }
        public string Signage { get; set; }
        public string Remarks { get; set; }
        public string SecurityDeposit { get; set; }
        public string Fitouts { get; set; }
        public string CostPerSquareFeet { get; set; }
        public string SuperArea { get; set; }
        public string CarPark { get; set; }       
        public bool IsClosed { get; set; }
        public string Name { get; set; }
        public bool IsRecommended { get; set; }
        public string Status { get; set; }
        public string Location { get; set; }
    }
}