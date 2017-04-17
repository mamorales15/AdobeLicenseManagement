using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdobeLicenseManagement.Models
{
    public class LicenseType
    {
        public LicenseType()
        {
            this.Requests = new List<Request>();    // Need to initialize Requests on creation otherwise get null reference exceptions
        }

        [Display(Name = "License Type ID")]
        [Key]
        public int LicenseTypeID { get; set; }

        [Display(Name = "License Type Description")]
        [Required(ErrorMessage = "The License Type Description is required")]
        [StringLength(50, MinimumLength = 3)]
        public string LicenseTypeDesc { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}