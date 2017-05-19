using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdobeLicenseManagement.Models
{
    public class EndUserViewModel
    {
        [Display(Name = "End User ID")]
        [Key]
        public int EndUserID { get; set; }

        [Required(ErrorMessage = "The UserName is required")]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }

        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }

        [Display(Name = "Room Number")]
        [StringLength(50)]
        public string RmNo { get; set; }

        [Display(Name = "Tag Number")]
        [Required(ErrorMessage = "Tag number is required")]
        [RegularExpression("^[0-9]{6,6}$", ErrorMessage = "Tag number must be length 6")]
        public int? Tag { get; set; }

        [Display(Name = "Computer Serial")]
        [StringLength(50, MinimumLength = 3)]
        public string ComputerSerial { get; set; }

        [Display(Name = "Computer Name")]
        [StringLength(50, MinimumLength = 3)]
        public string ComputerName { get; set; }

        [Display(Name = "Adobe ID")]
        [StringLength(50, MinimumLength = 3)]
        public string AdobeID { get; set; }

        [Display(Name = "Building ID")]
        public int BuildingID { get; set; }

        [Display(Name = "Request ID")]
        public int RequestID { get; set; }
    }

    public class RequestEditViewModel
    {
        [Display(Name = "Request ID")]
        [Key]
        public int RequestID { get; set; }

        [Display(Name = "Purchase Order ID")]
        public int PurchaseOrderID { get; set; }
        [Display(Name = "VIP ID")]
        public int VIPID { get; set; }
        [Display(Name = "License Type ID")]
        public int LicenseTypeID { get; set; }
        [Display(Name = "Product ID")]
        public int ProductID { get; set; }
        [Display(Name = "Point of Contact Name")]
        public string POCName { get; set; }
    }

    public class ServiceDeskRequestEditViewModel
    {
        [Display(Name = "ServiceDesk Request ID")]
        [Key]
        public int ServiceDeskRequestID { get; set; }

        [Display(Name = "Request ID")]
        public int RequestID { get; set; }
    }
}