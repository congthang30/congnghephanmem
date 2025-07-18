using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class User
{
    public int UserId { get; set; }

    public string? FullName { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public string? AccountStatus { get; set; }

    public int? FailedLoginCount { get; set; }

    public string? Email { get; set; }

    public DateTime? LockedAt { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;

    public string? Role { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<Cart> Carts { get; set; } = new List<Cart>();

    public virtual ICollection<LogActivity> LogActivities { get; set; } = new List<LogActivity>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Rating> Ratings { get; set; } = new List<Rating>();

    public virtual ICollection<Wishlist> Wishlists { get; set; } = new List<Wishlist>();
}
