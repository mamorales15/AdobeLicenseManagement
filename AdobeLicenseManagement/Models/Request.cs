using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace AdobeLicenseManagement.Models
{
    public class Request
    {
        [Display(Name = "Request ID")]
        [Key]
        public int RequestID { get; set; }
        
        public virtual PurchaseOrder PurchaseOrder { get; set; }
        public virtual VIP VIP { get; set; }
        public virtual LicenseType LicenseType { get; set; }
        public virtual Product Product { get; set; }
        public virtual PointOfContact PointOfContact { get; set; }

        public virtual ICollection<ServiceDeskRequest> ServiceDeskRequests { get; set; }
        public virtual ICollection<EndUser> EndUsers { get; set; }
    }
}