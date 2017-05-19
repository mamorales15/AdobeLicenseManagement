using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AdobeLicenseManagement.Models
{
    public class QueryViewModel
    {
        public IEnumerable<QueryDTOViewModel> Queries { get; set; }
        public IEnumerable<SavedQuery> SavedQueries { get; set; }
        public string Query { get; set; }
        public string Description { get; set; }
        [AllowHtml]
        public string Csv { get; set; }
    }

    public class QueryDTOViewModel
    {
        // Common
        [Display(Name = "Request ID")]
        public int Request_RequestID { get; set; }
        [Display(Name = "License Type ID")]
        public int LicenseType_LicenseTypeID { get; set; }
        [Display(Name = "Point of Contact Name")]
        public string PointOfContact_POCName { get; set; }
        [Display(Name = "Product ID")]
        public int Product_ProductID { get; set; }
        [Display(Name = "VIP ID")]
        public int VIP_VIPID { get; set; }

        // End User
        [Display(Name = "End User ID")]
        public int EndUserID { get; set; }
        [Required(ErrorMessage = "The UserName is required")]
        [StringLength(50, MinimumLength = 3)]
        public string UserName { get; set; }
        [Display(Name = "Email address")]
        [EmailAddress(ErrorMessage = "Invalid Email Address")]
        public string Email { get; set; }
        [StringLength(50, MinimumLength = 3)]
        public string Building { get; set; }
        [Display(Name = "Room Number")]
        [StringLength(50)]
        public string RmNo { get; set; }
        [Display(Name = "Tag Number")]
        [Required(ErrorMessage = "Tag number is required")]
        [RegularExpression("^[0-9]{6,6}$", ErrorMessage = "Tag number must be length 6")]
        public int Tag { get; set; }
        [Display(Name = "Computer Serial")]
        [StringLength(50, MinimumLength = 3)]
        public string ComputerSerial { get; set; }
        [Display(Name = "Computer Name")]
        [StringLength(50, MinimumLength = 3)]
        public string ComputerName { get; set; }
        [Display(Name = "Adobe ID")]
        [StringLength(50, MinimumLength = 3)]
        public string AdobeID { get; set; }

        // License Type
        [Display(Name = "License Type ID")]
        public int LicenseTypeID { get; set; }
        [Display(Name = "License Type Description")]
        [Required(ErrorMessage = "The License Type Description is required")]
        [StringLength(50, MinimumLength = 3)]
        public string LicenseTypeDesc { get; set; }

        // Point of Contact
        [Display(Name = "Point of Contact Name")]
        [Required(ErrorMessage = "The Point of Contact Name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string POCName { get; set; }

        // Product
        [Display(Name = "Product ID")]
        public int ProductID { get; set; }
        [Display(Name = "Product Description")]
        [Required(ErrorMessage = "The Product Description is required")]
        [StringLength(50, MinimumLength = 3)]
        public string ProductDesc { get; set; }

        // Purchase Order
        [Display(Name = "Purchase Order ID")]
        public int PurchaseOrderID { get; set; }
        [Display(Name = "Quantity")]
        [Required(ErrorMessage = "Quantity is required")]
        public int Qty { get; set; }
        [Display(Name = "Purchase Order Number")]
        public string PONo { get; set; }
        [Display(Name = "Purchase Order Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PODate { get; set; }

        // Request
        [Display(Name = "Request ID")]
        public int RequestID { get; set; }

        // ServiceDesk Request
        [Display(Name = "ServiceDesk Request ID")]
        public int ServiceDeskRequestID { get; set; }

        // VIP
        [Display(Name = "VIP ID")]
        public int VIPID { get; set; }
        [Display(Name = "VIP Name")]
        [Required(ErrorMessage = "The VIP Name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string VIPName { get; set; }
        [Display(Name = "VIP Number")]
        [Required(ErrorMessage = "The VIP Number is required")]
        [StringLength(50, MinimumLength = 3)]
        public string VIPNumber { get; set; }
        [Display(Name = "VIP Renewal Date")]
        [Required(ErrorMessage = "VIP Renewal Date is required")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime VIPRenewalDate { get; set; }
    }

    public class SubmitQuery
    {
        [Required]
        public string Query { get; set; }
    }
}