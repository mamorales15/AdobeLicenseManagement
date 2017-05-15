using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace AdobeLicenseManagement.Models
{
    public class SavedQuery
    {
        [Display(Name="Saved Query ID")]
        public int SavedQueryID { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Query { get; set; }

        [Display(Name = "Creation Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        public DateTime CreationDate { get; set; }
    }
}