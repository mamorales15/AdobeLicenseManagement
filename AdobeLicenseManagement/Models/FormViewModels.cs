﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Display(Name = "VIP")]
        public int VIPID { get; set; }

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
        // Same as ServiceDeskRequest model but I included it to
        // so you can just add needed fields later.
        [Display(Name = "Service Desk Request ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ServiceDeskRequestID { get; set; }

        [Display(Name = "Request ID")]
        public int RequestID { get; set; }
    }

    public class EndUserFormViewModel
    {
        [Display(Name = "Request ID")]
        public int RequestID { get; set; }

        [Required(ErrorMessage = "The username is required")]
        [StringLength(50, MinimumLength = 3)]
        public string Username { get; set; }

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
        public int Tag { get; set; }

        [Display(Name = "Computer Serial")]
        [StringLength(50, MinimumLength = 3)]
        public string ComputerSerial { get; set; }

        [Display(Name = "Computer Name")]
        [StringLength(50, MinimumLength = 3)]
        public string ComputerName { get; set; }

        [Display(Name = "Counter 'TESTING'")]
        public int? Counter { get; set; }

        [Display(Name = "Adobe ID")]
        [StringLength(50, MinimumLength = 3)]
        public string AdobeID { get; set; }
    }

    public class POFormViewModel
    {
        [Display(Name = "Request ID")]
        public int RequestID { get; set; }

        [Display(Name = "Purchase Order Number")]
        public int PONo { get; set; }

        [Display(Name = "Purchase Order Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PODate { get; set; }
    }

    public class AdobeIDFormViewModel
    {
        [Display(Name = "End User ID")]
        public int EndUserID { get; set; }

        [Display(Name = "Adobe ID")]
        [StringLength(50, MinimumLength = 3)]
        public string AdobeID { get; set; }
    }
}