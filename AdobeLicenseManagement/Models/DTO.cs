using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace UTEP_Printing_Management.Models
{
    public class DTO
    {
        public int ID { get; set; }

        // Lab
        [Display(Name = "Lab ID")]
        public int LabID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Lab Name")]
        public string LabName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Building")]
        public string Building { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Room No")]
        public string RoomNo { get; set; }

        //Student Printer
        [Display(Name = "Student Printer ID")]
        public int StudentPrinterID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Printer Name")]
        public string PrinterName { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Printer Model")]
        public string PrinterModel { get; set; }

        // Device
        [Display(Name = "Device ID")]
        public int DeviceID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Printer Make")]
        public string PrinterMake { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Printer Type")]
        public string PrinterType { get; set; }

        // Device Consumable
        [Display(Name = "Device Consumable ID")]
        public int DeviceConsumableID { get; set; }
        [StringLength(140)]
        [Display(Name = "Part Description")]
        public string PartDescription { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Part Type")]
        public string PartType { get; set; }
        [Required]
        [StringLength(50)]
        [Display(Name = "Part No")]
        public string PartNo { get; set; }

        // Consumable Ordered
        [Display(Name = "Consumable Ordered ID")]
        public int ConsumableOrderedID { get; set; }
        [Required]
        [Display(Name = "Qty Ordered")]
        public int QtyOrdered { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Ordered")]
        public DateTime DateOrdered { get; set; }

        // Consumable Recieved
        [Display(Name = "Consumable Recieved ID")]
        public int ConsumableRecievedID { get; set; }
        [Required]
        [Display(Name = "Qty Recieved")]
        public int QtyRecieved { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Recieved")]
        public DateTime DateRecieved { get; set; }

        // Inventory Stock Item
        [Display(Name = "Inventory Stock Item ID")]
        public int InventoryStockItemID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Serial No")]
        public string SerialNo { get; set; }

        // Consumable Delivery
        [Display(Name = "Consumable Delivery ID")]
        public int ConsumableDeliveryID { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Delivered")]
        public DateTime DateDelivered { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "Date Changed")]
        public DateTime DateChanged { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 3)]
        [Display(Name = "Employee Name")]
        public string EmployeeName { get; set; }

        // Purchase Order
        [Display(Name = "Purchase Order ID")]
        public int UTEP_PurchaseOrderNumber { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Column(TypeName = "money")]
        [Display(Name = "PO Amount ($)")]
        public decimal POAmount { get; set; }
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:MM-dd-yyyy}", ApplyFormatInEditMode = true)]
        [Display(Name = "PO Date")]
        public DateTime PODate { get; set; }
    }
}