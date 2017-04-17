using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdobeLicenseManagement.Models
{
    public class Product
    {
        [Display(Name = "Product ID")]
        [Key]
        public int ProductID { get; set; }

        [Display(Name = "Product Description")]
        [Required(ErrorMessage = "The Product Description is required")]
        [StringLength(50, MinimumLength = 3)]
        public string ProductDesc { get; set; }

        public virtual ICollection<Request> Requests { get; set; }
    }
}