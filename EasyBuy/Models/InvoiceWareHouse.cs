using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public class InvoiceWareHouse
{
    public int InvoiceWareHouseID { get; set; }//Mã hóa đơn
    public DateTime ExportDate { get; set; }

    public int UserID { get; set; }       // Khách hàng
    public int StaffID { get; set; }      // Nhân viên xuất kho
    public int OrderID { get; set; }      // Liên kết đơn hàng

    public int TotalQuantity { get; set; }
    // === NAVIGATION PROPERTIES ===
    public virtual User User { get; set; }
    public virtual User Staff { get; set; }
    public virtual Order Order { get; set; }

}
