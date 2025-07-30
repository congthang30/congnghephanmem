using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace EasyBuy.Models;

public partial class Address
{
    public int AddressId { get; set; }

    public int? UserId { get; set; }

    public string? Street { get; set; }

    public string? City { get; set; }

    public string? Ward { get; set; }

    public string? District { get; set; }

    public string? Phone { get; set; }

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual User? User { get; set; }

    [NotMapped]
    public string FullAddress
    {
        get
        {
            return $"{Street}, {Ward}, {District}, {City}";
        }
    }
}
