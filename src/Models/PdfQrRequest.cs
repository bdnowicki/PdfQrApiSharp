using System;

namespace PdfQrApiSharp.Models;

public class PdfQrRequest
{
    public byte[]? PdfContent { get; set; }
    public string? QrContent { get; set; }
} 