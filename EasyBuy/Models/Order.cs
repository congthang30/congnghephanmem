using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class Order
{
    public int OrderId { get; set; }

    public int? UserId { get; set; }

    public int? AddressId { get; set; }

    public int? PaymentMethodId { get; set; }

    public decimal? TotalAmount { get; set; }
    public decimal? FinalTotal { get; set; }
    public int? VoucherId { get; set; }

    public string? Status { get; set; }

    public string? StatusPayment { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual Address? Address { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual PaymentMethod? PaymentMethod { get; set; }

    public virtual User? User { get; set; }
    public virtual Voucher? Voucher { get; set; }
}
