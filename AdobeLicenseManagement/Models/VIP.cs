using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdobeLicenseManagement.Models
{
    public class VIP
    {
        public VIP()
        {
            this.Requests = new List<Request>();    // Need to initialize Requests on creation otherwise get null reference exceptions
        }

        [Display(Name = "VIP ID")]
        [Key]
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

        public virtual ICollection<Request> Requests { get; set; }
    }
}