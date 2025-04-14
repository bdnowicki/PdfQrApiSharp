using System.Net;
using Xunit;
using PdfQrApiSharp.Models;
using PdfQrApiSharp.Services;
using PdfQrApiSharp.Tests.TestData;

namespace PdfQrApiSharp.Tests.Services;

public class PdfQrServiceValidationTests : BasePdfQrServiceTests
{
    /// <summary>
    /// Tests that a request with null PDF content returns an appropriate error message.
    /// </summary>
    [Fact]
    public async Task AddQrToPdfAsync_WithNullPdfContent_ReturnsError()
    {
        // Arrange
        var request = new PdfQrRequest { PdfContent = null, QrContent = PdfQrTestDataFactory.TestQrContent };
        SetupMockResponse(HttpStatusCode.OK); // Set up mock response even though it won't be used

        // Act
        var response = await Service.AddQrToPdfAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Equal(PdfQrTestDataFactory.RequiredFieldsErrorMessage, response.ErrorMessage);
        Assert.Null(response.ModifiedPdf);
        // Don't verify HTTP request since validation happens before any request is made
    }

    /// <summary>
    /// Tests that a request with empty PDF content returns an appropriate error message.
    /// </summary>
    [Fact]
    public async Task AddQrToPdfAsync_WithEmptyPdfContent_ReturnsError()
    {
        // Arrange
        var request = new PdfQrRequest { PdfContent = Array.Empty<byte>(), QrContent = PdfQrTestDataFactory.TestQrContent };
        SetupMockResponse(HttpStatusCode.OK); // Set up mock response even though it won't be used

        // Act
        var response = await Service.AddQrToPdfAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Equal(PdfQrTestDataFactory.RequiredFieldsErrorMessage, response.ErrorMessage);
        Assert.Null(response.ModifiedPdf);
        // Don't verify HTTP request since validation happens before any request is made
    }

    /// <summary>
    /// Tests that a request with null QR content returns an appropriate error message.
    /// </summary>
    [Fact]
    public async Task AddQrToPdfAsync_WithNullQrContent_ReturnsError()
    {
        // Arrange
        var request = new PdfQrRequest { PdfContent = PdfQrTestDataFactory.CreateTestPdfContent(), QrContent = null };
        SetupMockResponse(HttpStatusCode.OK); // Set up mock response even though it won't be used

        // Act
        var response = await Service.AddQrToPdfAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Equal(PdfQrTestDataFactory.RequiredFieldsErrorMessage, response.ErrorMessage);
        Assert.Null(response.ModifiedPdf);
        // Don't verify HTTP request since validation happens before any request is made
    }

    /// <summary>
    /// Tests that the service can be constructed with a null config parameter.
    /// </summary>
    [Fact]
    public void Constructor_WithNullConfig_CreatesServiceWithDefaultConfig()
    {
        // Arrange & Act
        using var service = new PdfQrService(null);

        // Assert - if we got here without exceptions, the test passes
        Assert.NotNull(service);
    }
} 