using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdobeLicenseManagement.Models
{
    public class PurchaseOrder
    {
        [Key, ForeignKey("Request")]
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

        public int RequestID { get; set; }
        public virtual Request Request { get; set; }
    }
}