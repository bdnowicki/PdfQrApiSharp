# PdfQrApiSharp

A .NET library for communicating with the PDF QR Code API. This library provides a simple interface to add QR codes to PDF documents through an API service. The API project source code is available at: https://github.com/bdnowicki/pdf_qr_api

## Features

- Simple API for adding QR codes to PDF documents
- Configurable API endpoint and settings
- Async operations support
- Proper resource management with IDisposable
- Error handling and validation

## Installation

The library is available as a NuGet package. You can install it using the Package Manager Console:

```powershell
Install-Package PdfQrApiSharp
```

Or using the .NET CLI:

```bash
dotnet add package PdfQrApiSharp
```

## Usage

### Basic Usage

```csharp
using System.IO;
using PdfQrApiSharp.Models;
using PdfQrApiSharp.Services;

// Create a request
var request = new PdfQrRequest
{
    PdfContent = File.ReadAllBytes("input.pdf"),
    QrContent = "https://example.com"
};

// Use the service
using var service = new PdfQrService();
var response = await service.AddQrToPdfAsync(request);

if (response.Success)
{
    // Save the modified PDF
    File.WriteAllBytes("output.pdf", response.ModifiedPdf);
}
else
{
    Console.WriteLine($"Error: {response.ErrorMessage}");
}
```

### Custom Configuration

```csharp
// Create custom configuration
var config = new PdfQrApiConfig
{
    BaseUrl = "https://your-api-url.com",
    ApiVersion = "v1",
    Endpoint = "add-qr-to-pdf",
    Timeout = TimeSpan.FromMinutes(1)
};

// Use the service with custom configuration
using var service = new PdfQrService(config);
var response = await service.AddQrToPdfAsync(request);
```

## API Configuration

The `PdfQrApiConfig` class allows you to configure the following settings:

| Property | Default Value | Description |
|----------|---------------|-------------|
| BaseUrl | "http://127.0.0.1:8000" | The base URL of the API service |
| ApiVersion | "v1" | The API version to use |
| Endpoint | "add-qr-to-pdf" | The API endpoint for adding QR codes |
| Timeout | 30 seconds | The timeout for API requests |

## Error Handling

The library provides comprehensive error handling:

- Input validation (PDF content and QR content are required)
- API communication errors
- HTTP status code handling
- Exception handling

All errors are returned in the `PdfQrResponse` object with:
- `Success` flag indicating if the operation was successful
- `ErrorMessage` containing details about any errors that occurred
- `ModifiedPdf` containing the modified PDF when successful

## Resource Management

The `PdfQrService` class implements `IDisposable` to properly manage HTTP client resources. It's recommended to use the service within a `using` statement or explicitly call `Dispose()` when you're done with it:

```csharp
using var service = new PdfQrService();
// Use the service...
```

## Dependencies

- System.Net.Http
- Newtonsoft.Json

## License

This project is licensed under the MIT License - see the LICENSE file for details.

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request. 