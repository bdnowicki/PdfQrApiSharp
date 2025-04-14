using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using PdfQrApiSharp.Models;

namespace PdfQrApiSharp.Services;

public class PdfQrService : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly PdfQrApiConfig _config;
    private bool _disposed;

    public PdfQrService(PdfQrApiConfig? config = null)
    {
        _config = config ?? new PdfQrApiConfig();
        _httpClient = new HttpClient
        {
            Timeout = _config.Timeout
        };
    }

    public async Task<PdfQrResponse> AddQrToPdfAsync(PdfQrRequest request)
    {
        try
        {
            if (request == null || request.PdfContent == null || request.PdfContent.Length == 0 || string.IsNullOrEmpty(request.QrContent))
            {
                return new()
                {
                    Success = false,
                    ErrorMessage = "PDF content and QR content are required"
                };
            }

            using var formData = new MultipartFormDataContent();
            
            // Add PDF file
            var pdfContent = new ByteArrayContent(request.PdfContent);
            pdfContent.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            formData.Add(pdfContent, "pdf_file", "document.pdf");

            // Add QR content as query parameter
            var endpoint = $"{_config.GetFullEndpoint()}?qr_content={Uri.EscapeDataString(request.QrContent)}";

            var response = await _httpClient.PostAsync(endpoint, formData);
            
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                return new()
                {
                    Success = false,
                    ErrorMessage = $"API request failed with status {response.StatusCode}: {errorContent}"
                };
            }

            var modifiedPdf = await response.Content.ReadAsByteArrayAsync();
            return new()
            {
                Success = true,
                ModifiedPdf = modifiedPdf
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                Success = false,
                ErrorMessage = $"Error processing request: {ex.Message}"
            };
        }
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _httpClient.Dispose();
            }
            _disposed = true;
        }
    }
} 