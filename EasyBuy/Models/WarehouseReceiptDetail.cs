namespace EasyBuy.Models
{
    public class WarehouseReceiptDetail
    {
        public int DetailID { get; set; }
        public int ReceiptID { get; set; }
        public string Barcode { get; set; } = string.Empty;
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice { get; set; }

        public virtual WarehouseReceipt Receipt { get; set; } = null!;
        public virtual Product Product { get; set; } = null!;
    }
}
