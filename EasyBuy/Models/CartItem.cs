using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class CartItem
{
    public int CartId { get; set; }

    public int ProductId { get; set; }

    public int? Quantity { get; set; }

    public decimal? UnitPrice { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public virtual Cart Cart { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
