using System;
using System.Collections.Generic;

namespace EasyBuy.Models
{
    public partial class WarehouseReceipt
    {
        public int ReceiptID { get; set; }
        public string ReceiptNumber { get; set; } = string.Empty;
        public DateTime ReceiptDate { get; set; }
        public int? CreatedByStaff { get; set; }
        public string SupplierName { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
        public int TotalQuantity { get; set; }
        public decimal TotalAmount { get; set; }

        // Navigation properties
        public virtual User Staff { get; set; } = null!;
        public virtual ICollection<WarehouseReceiptDetail> ReceiptDetails { get; set; } = new List<WarehouseReceiptDetail>();
    }
}
