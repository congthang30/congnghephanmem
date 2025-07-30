using System;
using System.Collections.Generic;

namespace EasyBuy.Models
{
    public partial class Invoice
    {
        public int InvoiceId { get; set; }         // Mã biên lai

        public int OrderId { get; set; }           // Liên kết đơn hàng

        public DateTime InvoiceDate { get; set; } = DateTime.Now;  // Ngày lập biên lai

        public int? CreatedBy { get; set; }        // Nhân viên lập biên lai

        public DateTime CreatedAt { get; set; } = DateTime.Now;    // Thời gian tạo
        public DateTime? UpdatedAt { get; set; }   // Thời gian cập nhật

        // 🔗 Navigation properties
        public virtual Order Order { get; set; } = null!;
        public virtual User? CreatedByUser { get; set; }           // Điều hướng đến User
    }
}
