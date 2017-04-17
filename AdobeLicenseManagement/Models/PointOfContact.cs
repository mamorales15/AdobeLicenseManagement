using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdobeLicenseManagement.Models
{
    public class PointOfContact
    {
        [Display(Name = "Point of Contact Name")]
        [Required(ErrorMessage = "The Point of Contact Name is required")]
        [StringLength(50, MinimumLength = 3)]
        [Key]
        public string POCName { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}