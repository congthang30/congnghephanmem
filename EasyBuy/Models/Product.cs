using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class Product
{
    public int ProductId { get; set; }

    public string? ProductName { get; set; }

    public string? Barcode { get; set; }

    public string? Description { get; set; }

    public int? Quantity { get; set; }

    public decimal? ImportPrice { get; set; }

    public decimal? SellingPrice { get; set; }

    public string? StatusProduct { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public decimal? Discount { get; set; }

    public bool? IsFeatured { get; set; }

    public string? ImagePr { get; set; }

    public int? ViewCount { get; set; }

    public int? BrandId { get; set; }

    public int? CateId { get; set; }

    public virtual Brand? Brand { get; set; }

    public virtual ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();

    public virtual Category? Cate { get; set; }

    public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
