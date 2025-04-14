using System;
using System.Net;
using System.Net.Http;
using Moq;
using Moq.Protected;
using PdfQrApiSharp.Models;
using PdfQrApiSharp.Services;
using PdfQrApiSharp.Tests.TestData;

namespace PdfQrApiSharp.Tests.Services;

public abstract class BasePdfQrServiceTests : IDisposable
{
    protected readonly Mock<HttpMessageHandler> MockHttpMessageHandler;
    protected readonly HttpClient HttpClient;
    protected readonly PdfQrService Service;
    protected readonly PdfQrApiConfig Config;

    protected BasePdfQrServiceTests()
    {
        MockHttpMessageHandler = new Mock<HttpMessageHandler>();
        HttpClient = new HttpClient(MockHttpMessageHandler.Object);
        Config = PdfQrTestDataFactory.CreateTestConfig();
        
        // Create a field-level service instance with the mocked HttpClient
        var serviceField = typeof(PdfQrService).GetField("_httpClient", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
        Service = new PdfQrService(Config);
        serviceField?.SetValue(Service, HttpClient);
    }

    public void Dispose()
    {
        HttpClient.Dispose();
        Service.Dispose();
    }

    protected void SetupMockResponse(HttpStatusCode statusCode, byte[]? content = null, string? errorContent = null)
    {
        MockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(new HttpResponseMessage
            {
                StatusCode = statusCode,
                Content = content != null ? new ByteArrayContent(content) : 
                         errorContent != null ? new StringContent(errorContent) : null
            });
    }

    protected void VerifyHttpRequest(string expectedQrContent)
    {
        MockHttpMessageHandler.Protected().Verify(
            "SendAsync",
            Times.Once(),
            ItExpr.Is<HttpRequestMessage>(req =>
                req.Method == HttpMethod.Post &&
                req.Content is MultipartFormDataContent &&
                req.Content.Headers.ContentType.MediaType == "multipart/form-data" &&
                Uri.UnescapeDataString(req.RequestUri.ToString()).Contains(expectedQrContent)),
            ItExpr.IsAny<CancellationToken>()
        );
    }
} 