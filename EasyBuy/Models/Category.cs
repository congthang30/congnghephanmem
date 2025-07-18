using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class Category
{
    public int CateId { get; set; }

    public string? CategoryName { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
