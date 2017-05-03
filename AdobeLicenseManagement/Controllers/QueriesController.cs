using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using UTEP_Printing_Management.Models;

namespace UTEP_Printing_Management.Controllers
{
    [Authorize]
    public class QueriesController : Controller
    {
        private UTEP_Printing_ManagementContext db = new UTEP_Printing_ManagementContext();

        // GET: Queries
        // Handles Get requests for manual queries, and queries created from the query builder
        // The Index has so many parameteres because those are which checkboxes were clicked in the query builder
        // Maybe this could be improved and the parameters passed a different way? Don't have time to work on that now
        public ActionResult Index(string manQuery,
            string ConsumableDeliveriesCheckBox, string ConsumableDeliveryIDCheckBox, string DateDeliveredCheckBox, string DateChangedCheckBox, string EmployeeNameCheckBox,
            string ConsumablesOrderedCheckBox, string ConsumableOrderedIDCheckBox, string QtyOrderedCheckBox, string DateOrderedCheckBox,
            string ConsumablesRecievedCheckBox, string ConsumableRecievedIDCheckBox, string QtyRecievedCheckBox, string DateRecievedCheckBox,
            string DevicesCheckBox, string DeviceIDCheckBox, string PrinterMakeCheckBox, string PrinterTypeCheckBox,
            string DeviceConsumablesCheckBox, string DeviceConsumableIDCheckBox, string PartDescriptionCheckBox, string PartTypeCheckBox, string PartNoCheckBox,
            string InventoryStockCheckBox, string InventoryStockItemIDCheckBox, string SerialNoCheckBox,
            string LabsCheckBox, string LabIDCheckBox, string LabNameCheckBox, string BuildingCheckBox, string RoomNoCheckBox,
            string PurchaseOrdersCheckBox, string UTEP_PurchaseOrderNumberCheckBox, string POAmountCheckBox, string PODateCheckBox,
            string StudentPrintersCheckBox, string StudentPrinterIDCheckBox, string PrinterNameCheckBox, string PrinterModelCheckBox)
        {

            /* Two tables query: 
                SELECT Labs.LabName, StudentPrinters.PrinterName
                FROM Labs
                INNER JOIN Student
                ON Labs.LabID = StudentPrinters.LabID
            */

            /* Three tables query:
                SELECT *, *, *
                FROM ConsumableDeliveries
                INNER JOIN ConsumableOrdereds
                ON ConsumableDeliveries.ConsumableDeliveryID = ConsumableOrdereds.ConsumableOrderedID
                INNER JOIN ConsumableRecieveds
                ON ConsumableOrdereds.ConsumableOrderedID = ConsumableRecieveds.ConsumableRecievedID
             */
            
            string qbQuery = null;

            Boolean anyConsumableDelivery, anyConsumableOrdered, anyConsumableRecieved, anyDevice, anyDeviceConsumable, anyInventoryStockItem, anyLab, anyPurchaseOrder, anyStudentPrinter;
            anyConsumableDelivery = anyConsumableOrdered = anyConsumableRecieved = anyDeviceConsumable = anyDevice = anyInventoryStockItem = anyLab = anyPurchaseOrder = anyStudentPrinter = false;

            int countOfTables = 0;  // Different ways to create the query string depending on how many tables are going to be queried

            // Check if any thing was checked
            if (toBool(ConsumableDeliveriesCheckBox) || toBool(ConsumableDeliveryIDCheckBox) || toBool(DateDeliveredCheckBox) || toBool(DateChangedCheckBox) || toBool(EmployeeNameCheckBox))
            {
                anyConsumableDelivery = true;
                countOfTables++;
            }
            if (toBool(ConsumablesOrderedCheckBox) || toBool(ConsumableOrderedIDCheckBox) || toBool(QtyOrderedCheckBox) || toBool(DateOrderedCheckBox))
            {
                anyConsumableOrdered = true;
                countOfTables++;
            }
            if (toBool(ConsumablesRecievedCheckBox) || toBool(ConsumableRecievedIDCheckBox) || toBool(QtyRecievedCheckBox) || toBool(DateRecievedCheckBox))
            {
                anyConsumableRecieved = true;
                countOfTables++;
            }
            if (toBool(DevicesCheckBox) || toBool(DeviceIDCheckBox) || toBool(PrinterMakeCheckBox) || toBool(PrinterTypeCheckBox))
            {
                anyDevice = true;
                countOfTables++;
            }
            if (toBool(DeviceConsumablesCheckBox) || toBool(DeviceConsumableIDCheckBox) || toBool(PartDescriptionCheckBox) || toBool(PartTypeCheckBox) || toBool(PartNoCheckBox))
            {
                anyDeviceConsumable = true;
                countOfTables++;
            }
            if (toBool(InventoryStockCheckBox) || toBool(InventoryStockItemIDCheckBox) || toBool(SerialNoCheckBox))
            {
                anyInventoryStockItem = true;
                countOfTables++;
            }
            if (toBool(LabsCheckBox) || toBool(LabIDCheckBox) || toBool(LabNameCheckBox) || toBool(BuildingCheckBox) || toBool(RoomNoCheckBox))
            {
                anyLab = true;
                countOfTables++;
            }
            if (toBool(PurchaseOrdersCheckBox) || toBool(UTEP_PurchaseOrderNumberCheckBox) || toBool(POAmountCheckBox) || toBool(PODateCheckBox))
            {
                anyPurchaseOrder = true;
                countOfTables++;
            }
            if (toBool(StudentPrintersCheckBox) || toBool(StudentPrinterIDCheckBox) || toBool(PrinterNameCheckBox) || toBool(PrinterModelCheckBox))
            {
                anyStudentPrinter = true;
                countOfTables++;
            }

            if (countOfTables > 0)
            {
                qbQuery = "SELECT";

                ////// Add Fields to query string

                // Consumable Delivery fields
                if (toBool(ConsumableDeliveriesCheckBox))
                {
                    qbQuery += " *,";
                }
                else if (toBool(ConsumableDeliveryIDCheckBox) || toBool(DateDeliveredCheckBox) || toBool(DateChangedCheckBox) || toBool(EmployeeNameCheckBox))
                {
                    if (toBool(ConsumableDeliveryIDCheckBox))
                        qbQuery += " ConsumableDeliveries.ConsumableDeliveryID,";
                    if (toBool(DateDeliveredCheckBox))
                        qbQuery += " ConsumableDeliveries.DateDelivered,";
                    if (toBool(DateChangedCheckBox))
                        qbQuery += " ConsumableDeliveries.DateChanged,";
                    if (toBool(EmployeeNameCheckBox))
                        qbQuery += " ConsumableDeliveries.EmployeeName,";
                }
                // Consumable Ordered fields
                if (toBool(ConsumablesOrderedCheckBox))
                {
                    qbQuery += " *,";
                }
                else if (toBool(ConsumableOrderedIDCheckBox) || toBool(QtyOrderedCheckBox) || toBool(DateOrderedCheckBox))
                {
                    // Unfortunately I can't figure out how to change the table name in the Server, so it automatically
                    // made it ConsumableOrdereds and not ConsumablesOrdered
                    if (toBool(ConsumableOrderedIDCheckBox))
                        qbQuery += " ConsumableOrdereds.ConsumableOrderedID,";
                    if (toBool(QtyOrderedCheckBox))
                        qbQuery += " ConsumableOrdereds.QtyOrdered,";
                    if (toBool(DateOrderedCheckBox))
                        qbQuery += " ConsumableOrdereds.DateOrdered,";
                }
                // Consumable Ordered fields
                if (toBool(ConsumablesRecievedCheckBox))
                {
                    qbQuery += " *,";
                }
                else if (toBool(ConsumableRecievedIDCheckBox) || toBool(QtyRecievedCheckBox) || toBool(DateRecievedCheckBox))
                {
                    // Unfortunately I can't figure out how to change the table name in the Server, so it automatically
                    // made it ConsumableRecieveds and not ConsumablesRecieved
                    if (toBool(ConsumableRecievedIDCheckBox))
                        qbQuery += " ConsumableRecieveds.ConsumableRecievedID,";
                    if (toBool(QtyRecievedCheckBox))
                        qbQuery += " ConsumableRecieveds.QtyRecieved,";
                    if (toBool(DateRecievedCheckBox))
                        qbQuery += " ConsumableRecieveds.DateRecieved,";
                }
                // Device fields
                if (toBool(DevicesCheckBox))
                {
                    qbQuery += " *,";
                }
                else if (toBool(DeviceIDCheckBox) || toBool(PrinterMakeCheckBox) || toBool(PrinterTypeCheckBox))
                {
                    if (toBool(DeviceIDCheckBox))
                        qbQuery += " Devices.DeviceID,";
                    if (toBool(PrinterMakeCheckBox))
                        qbQuery += " Devices.PrinterMake,";
                    if (toBool(PrinterTypeCheckBox))
                        qbQuery += " Devices.PrinterType,";
                }
                // Device Consumables fields
                if (toBool(DeviceConsumablesCheckBox))
                {
                    qbQuery += " *,";
                }
                else if (toBool(DeviceConsumableIDCheckBox) || toBool(PartDescriptionCheckBox) || toBool(PartTypeCheckBox) || toBool(PartNoCheckBox))
                {
                    if (toBool(DeviceConsumableIDCheckBox))
                        qbQuery += " DeviceConsumables.DeviceConsumableID,";
                    if (toBool(PartDescriptionCheckBox))
                        qbQuery += " DeviceConsumables.PartDescription,";
                    if (toBool(PartTypeCheckBox))
                        qbQuery += " DeviceConsumables.PartType,";
                    if (toBool(PartNoCheckBox))
                        qbQuery += " DeviceConsumables.PartNo,";
                }
                // Inventory Stock Item fields
                if (toBool(InventoryStockCheckBox))
                {
                    qbQuery += " *,";
                }
                else if (toBool(InventoryStockItemIDCheckBox) || toBool(SerialNoCheckBox))
                {
                    // Unfortunately I can't figure out how to change the table name in the Server, so it automatically
                    // made it InventoryStockItems and not InventoryStock
                    if (toBool(InventoryStockItemIDCheckBox))
                        qbQuery += " InventoryStockItems.InventoryStockItemID,";
                    if (toBool(SerialNoCheckBox))
                        qbQuery += " InventoryStockItems.SerialNo,";
                }
                // Lab fields
                if (toBool(LabsCheckBox))
                {
                    qbQuery += " *,";
                } else if (toBool(LabIDCheckBox) || toBool(LabNameCheckBox) || toBool(BuildingCheckBox) || toBool(RoomNoCheckBox))
                {
                    if(toBool(LabIDCheckBox))
                        qbQuery += " Labs.LabID,";
                    if (toBool(LabNameCheckBox))
                        qbQuery += " Labs.LabName,";
                    if(toBool(BuildingCheckBox))
                        qbQuery += " Labs.Building,";
                    if(toBool(RoomNoCheckBox))
                        qbQuery += " Labs.RoomNo,";
                }
                // Purchase Order fields
                if (toBool(PurchaseOrdersCheckBox))
                {
                    qbQuery += " *,";
                }
                else if (toBool(UTEP_PurchaseOrderNumberCheckBox) || toBool(POAmountCheckBox) || toBool(PODateCheckBox))
                {
                    if (toBool(UTEP_PurchaseOrderNumberCheckBox))
                        qbQuery += " PurchaseOrders.UTEP_PurchaseOrderNumber,";
                    if (toBool(POAmountCheckBox))
                        qbQuery += " PurchaseOrders.POAmount,";
                    if (toBool(PODateCheckBox))
                        qbQuery += " PurchaseOrders.PODate,";
                }
                // Student Printer Fields
                if (toBool(StudentPrintersCheckBox))
                {
                    qbQuery += " *,";
                }
                else if (toBool(StudentPrinterIDCheckBox) || toBool(PrinterNameCheckBox) || toBool(PrinterModelCheckBox))
                {
                    if (toBool(StudentPrinterIDCheckBox))
                        qbQuery += " StudentPrinters.StudentPrinterID,";
                    if (toBool(PrinterNameCheckBox))
                        qbQuery += " StudentPrinters.PrinterName,";
                    if (toBool(PrinterModelCheckBox))
                        qbQuery += " StudentPrinters.PrinterModel,";
                }

                qbQuery = qbQuery.TrimEnd(',');     // Trims the last ',' because that is invalid SQL

                ////// Add Inner Joins to query string

                string firstTableQueried = "";

                if (anyConsumableDelivery)
                {
                    if(firstTableQueried == "")
                    {
                        qbQuery += " FROM ConsumableDeliveries";
                        firstTableQueried = "ConsumableDeliveries.ConsumableDeliveryID";
                    } else
                    {
                        qbQuery += " INNER JOIN ConsumableDeliveries";
                        qbQuery += " ON " + firstTableQueried + " = ConsumableDeliveries.ConsumableDeliveryID";
                    }
                }
                if (anyConsumableOrdered)
                {
                    // SQL requires ConsumableOrdereds instead of ConsumablesOrdered
                    if (firstTableQueried == "")
                    {
                        qbQuery += " FROM ConsumableOrdereds";
                        firstTableQueried = "ConsumableOrdereds.ConsumableOrderedID";
                    }
                    else
                    {
                        qbQuery += " INNER JOIN ConsumableOrdereds";
                        qbQuery += " ON " + firstTableQueried + " = ConsumableOrdereds.ConsumableOrderedID";
                    }
                }
                if (anyConsumableRecieved)
                {
                    // SQL requires ConsumableRecieveds instead of ConsumablesRecieved
                    if (firstTableQueried == "")
                    {
                        qbQuery += " FROM ConsumableRecieveds";
                        firstTableQueried = "ConsumableRecieveds.ConsumableRecievedID";
                    }
                    else
                    {
                        qbQuery += " INNER JOIN ConsumableRecieveds";
                        qbQuery += " ON " + firstTableQueried + " = ConsumableRecieveds.ConsumableRecievedID";
                    }
                }
                if (anyDevice)
                {
                    if (firstTableQueried == "")
                    {
                        qbQuery += " FROM Devices";
                        firstTableQueried = "Devices.DeviceID";
                    }
                    else
                    {
                        qbQuery += " INNER JOIN Devices";
                        qbQuery += " ON " + firstTableQueried + " = Devices.DeviceID";
                    }
                }
                if (anyDeviceConsumable)
                {
                    if (firstTableQueried == "")
                    {
                        qbQuery += " FROM DeviceConsumables";
                        firstTableQueried = "DeviceConsumables.DeviceConsumableID";
                    }
                    else
                    {
                        qbQuery += " INNER JOIN DeviceConsumables";
                        qbQuery += " ON " + firstTableQueried + " = DeviceConsumables.DeviceConsumableID";
                    }
                }
                if (anyInventoryStockItem)
                {
                    // SQL requires InventoryStockItems instead of InventoryStock
                    if (firstTableQueried == "")
                    {
                        qbQuery += " FROM InventoryStockItems";
                        firstTableQueried = "InventoryStockItems.InventoryStockItemID";
                    }
                    else
                    {
                        qbQuery += " INNER JOIN InventoryStockItems";
                        qbQuery += " ON " + firstTableQueried + " = InventoryStockItems.InventoryStockItemID";
                    }
                }
                if (anyLab)
                {
                    if (firstTableQueried == "")
                    {
                        qbQuery += " FROM Labs";
                        firstTableQueried = "Labs.LabID";
                    }
                    else
                    {
                        qbQuery += " INNER JOIN Labs";
                        qbQuery += " ON " + firstTableQueried + " = Labs.LabID";
                    }
                }
                if (anyPurchaseOrder)
                {
                    if (firstTableQueried == "")
                    {
                        qbQuery += " FROM PurchaseOrders";
                        firstTableQueried = "PurchaseOrders.UTEP_PurchaseOrderNumber";
                    }
                    else
                    {
                        qbQuery += " INNER JOIN PurchaseOrders";
                        qbQuery += " ON " + firstTableQueried + " = PurchaseOrders.UTEP_PurchaseOrderNumber";
                    }
                }
                if (anyStudentPrinter)
                {
                    if (firstTableQueried == "")
                    {
                        qbQuery += " FROM StudentPrinters";
                        firstTableQueried = "StudentPrinters.StudentPrinterID";
                    }
                    else
                    {
                        qbQuery += " INNER JOIN StudentPrinters";
                        qbQuery += " ON " + firstTableQueried + " = StudentPrinters.StudentPrinterID";
                    }
                }
            }

            ////////

            // Encode query because you cannot use wildcards in URL route parameters
            if(qbQuery == null && manQuery == null)
            {
                return View(db.SavedQueries.ToList());
            }
            else if (manQuery != null && manQuery != "")
            {
                if (manQuery.Length >= 6 && manQuery.Substring(0, 6) == "SELECT")
                {
                    manQuery = manQuery.Replace("*", "xyy");
                    return RedirectToAction("DisplayQuery", new { query = manQuery });
                } else
                {
                    ViewBag.Message = "Invalid Query";
                    return View(db.SavedQueries.ToList());
                }
            }
            else
            {
                qbQuery = qbQuery.Replace("*", "xyy");
                return RedirectToAction("DisplayQuery", new { query = qbQuery });
            }
            
        }

        public ActionResult DisplayQuery(string query)
        {
            try
            {
                // Decode query because you cannot use wildcards in URL route parameters
                query = query.Replace("xyy", "*");

                IEnumerable<DTO> data = db.Database.SqlQuery<DTO>(query);
                ViewBag.QueryRan = query;
                return View(data.ToList());
            } catch(Exception e)
            {
                return View();
            }
        }

        private Boolean toBool(string val)
        {
            return Convert.ToBoolean(val);
        }

    }
}