using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class Cart
{
    public int CartId { get; set; }

    public int? UserId { get; set; }

    public bool? IsCheckedOut { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual User? User { get; set; }
}
