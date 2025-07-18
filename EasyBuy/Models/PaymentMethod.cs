using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class PaymentMethod
{
    public int PaymentMethodId { get; set; }

    public string? MethodName { get; set; }

    public string? Description { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
