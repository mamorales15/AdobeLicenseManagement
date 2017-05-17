using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdobeLicenseManagement.Models
{
    public class ServiceDeskRequest
    {
        
        [Display(Name = "ServiceDesk Request ID")]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        [Key]
        public int ServiceDeskRequestID { get; set; }

        public virtual Request Request { get; set; }
    }
}