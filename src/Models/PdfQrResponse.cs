using System;

namespace PdfQrApiSharp.Models;

public class PdfQrResponse
{
    public byte[]? ModifiedPdf { get; set; }
    public string? ErrorMessage { get; set; }
    public bool Success { get; set; }
} 