using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class Rating
{
    public int RatingId { get; set; }

    public int? ProductId { get; set; }

    public int? UserId { get; set; }

    public int? Star { get; set; }

    public string? Comment { get; set; }

    public bool? IsApproved { get; set; }

    public string? ImagePath { get; set; } 


    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Product? Product { get; set; }

    public virtual User? User { get; set; }
}
