using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class LogActivity
{
    public int LogId { get; set; }

    public int? UserId { get; set; }

    public string? Action { get; set; }

    public DateTime? Timestamp { get; set; }

    public string? Entity { get; set; }

    public int? EntityId { get; set; }

    public virtual User? User { get; set; }
}
