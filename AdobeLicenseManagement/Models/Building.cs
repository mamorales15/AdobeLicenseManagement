using System.ComponentModel.DataAnnotations;

namespace AdobeLicenseManagement.Models
{
    public class Building
    {
        [Display(Name = "Building ID")]
        [Key]
        public int BuildingID { get; set; }

        [Display(Name = "Building Name")]
        [Required(ErrorMessage = "The Building Name is required")]
        [StringLength(50, MinimumLength = 3)]
        public string BuildingName { get; set; }
    }
}