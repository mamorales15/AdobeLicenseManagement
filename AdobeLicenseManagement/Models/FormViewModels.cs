using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdobeLicenseManagement.Models
{
    public class HelpDeskFormViewModel
    {
        [Required]
        [Display(Name = "Service Desk Request ID")]
        public int SDReqID { get; set; }

        [Required]
        [Display(Name = "VIP Type")]
        public int VIPTypeID { get; set; }

        [Required]
        [Display(Name = "License Type")]
        public int LicenseTypeID { get; set; }

        [Required]
        [Display(Name = "Product")]
        public int ProductID { get; set; }

        [Required]
        [Display(Name = "Point of Contact")]
        public string POC { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public int Qty { get; set; }
    }

    public class ServiceDeskFormViewModel
    {

    }

    public class EndUserFormViewModel
    {

    }

    public class POFormViewModel
    {

    }

    public class AdobeIDFormViewModel
    {

    }
}