using System;

namespace PdfQrApiSharp.Models;

public class PdfQrApiConfig
{
    public string BaseUrl { get; set; } = "http://127.0.0.1:8000";
    public string ApiVersion { get; set; } = "v1";
    public string Endpoint { get; set; } = "add-qr-to-pdf";
    public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(30);

    public string GetFullEndpoint() => $"{BaseUrl}/api/{ApiVersion}/{Endpoint}";
} 