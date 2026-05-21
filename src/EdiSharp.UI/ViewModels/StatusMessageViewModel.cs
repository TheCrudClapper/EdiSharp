using System;

namespace EdiSharp.UI.ViewModels;

public class StatusMessageViewModel
{
    public string Message { get; set; } = null!;
    public bool IsError { get; set; }
    public TimeSpan TimeStamp { get; set; }
}
