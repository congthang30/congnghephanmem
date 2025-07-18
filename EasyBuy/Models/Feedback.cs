using System;
using System.Collections.Generic;

namespace EasyBuy.Models;

public partial class Feedback
{
    public int FeedbackId { get; set; }

    public string? FullName { get; set; }

    public string? Email { get; set; }

    public string? Subject { get; set; }

    public string? Message { get; set; }

    public bool? IsReplied { get; set; }

    public DateTime? CreatedAt { get; set; } = DateTime.Now;
}
