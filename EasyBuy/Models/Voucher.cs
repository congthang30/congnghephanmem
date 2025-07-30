using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class Voucher
{
    public int VoucherId { get; set; }

    public string? Code { get; set; }

    public string? Description { get; set; }

    public string? DiscountType { get; set; }

    public decimal? DiscountValue { get; set; }

    public decimal? MaxDiscountAmount { get; set; }

    public decimal? MinOrderAmount { get; set; }

    public int? Quantity { get; set; }

    public DateOnly? StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsPublic { get; set; }

    public DateTime? CreatedAt { get; set; }

    public string? CreatedBy { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

}
