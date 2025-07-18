using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class OrderDetail
{
    public int OrderId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public int? ExistFirst { get; set; }   // Tồn Trước
    public int? SurviveAfter { get; set; } // Tồn Sau

    public decimal? UnitPrice { get; set; }

    public decimal? Discount { get; set; }

    public virtual Order Order { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
