using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace AdobeLicenseManagement.Models
{
    public class EndUser
    {
        [Display(Name = "End User ID")]
        [Key]
        public int EndUserID { get; set; }

        [Required(ErrorMessage = "The UserName is required")]
        [StringLength(50, MinimumLength =3)]
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

        public virtual Request Request { get; set; }
    }
}