using System.Net;
using System.Net.Http;
using Xunit;
using PdfQrApiSharp.Tests.TestData;

namespace PdfQrApiSharp.Tests.Services;

public class PdfQrServiceSuccessTests : BasePdfQrServiceTests
{
    /// <summary>
    /// Tests that a valid request with proper PDF and QR content returns a successful response.
    /// </summary>
    [Fact]
    public async Task AddQrToPdfAsync_WithValidRequest_ReturnsSuccess()
    {
        // Arrange
        var request = PdfQrTestDataFactory.CreateTestRequest();
        var expectedResponse = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D }; // Modified PDF content
        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var response = await Service.AddQrToPdfAsync(request);

        // Assert
        Assert.True(response.Success);
        Assert.Null(response.ErrorMessage);
        Assert.Equal(expectedResponse, response.ModifiedPdf);
        VerifyHttpRequest(PdfQrTestDataFactory.TestQrContent);
    }

    /// <summary>
    /// Tests that a request with special characters in QR content is properly encoded and handled.
    /// </summary>
    [Fact]
    public async Task AddQrToPdfAsync_WithSpecialCharactersInQrContent_ReturnsSuccess()
    {
        // Arrange
        var request = PdfQrTestDataFactory.CreateTestRequest(qrContent: PdfQrTestDataFactory.TestQrContentWithSpecialChars);
        var expectedResponse = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D };
        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var response = await Service.AddQrToPdfAsync(request);

        // Assert
        Assert.True(response.Success);
        Assert.Null(response.ErrorMessage);
        Assert.Equal(expectedResponse, response.ModifiedPdf);
        VerifyHttpRequest(PdfQrTestDataFactory.TestQrContentWithSpecialChars);
    }

    /// <summary>
    /// Tests that a request with Unicode characters in QR content is properly encoded and handled.
    /// </summary>
    [Fact]
    public async Task AddQrToPdfAsync_WithUnicodeInQrContent_ReturnsSuccess()
    {
        // Arrange
        var request = PdfQrTestDataFactory.CreateTestRequest(qrContent: PdfQrTestDataFactory.TestQrContentWithUnicode);
        var expectedResponse = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D };
        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var response = await Service.AddQrToPdfAsync(request);

        // Assert
        Assert.True(response.Success);
        Assert.Null(response.ErrorMessage);
        Assert.Equal(expectedResponse, response.ModifiedPdf);
        VerifyHttpRequest(PdfQrTestDataFactory.TestQrContentWithUnicode);
    }

    /// <summary>
    /// Tests that a large PDF file is handled correctly.
    /// </summary>
    [Fact]
    public async Task AddQrToPdfAsync_WithLargePdf_ReturnsSuccess()
    {
        // Arrange
        var request = PdfQrTestDataFactory.CreateTestRequest(pdfContent: PdfQrTestDataFactory.CreateLargePdfContent());
        var expectedResponse = new byte[] { 0x25, 0x50, 0x44, 0x46, 0x2D };
        SetupMockResponse(HttpStatusCode.OK, expectedResponse);

        // Act
        var response = await Service.AddQrToPdfAsync(request);

        // Assert
        Assert.True(response.Success);
        Assert.Null(response.ErrorMessage);
        Assert.Equal(expectedResponse, response.ModifiedPdf);
        VerifyHttpRequest(PdfQrTestDataFactory.TestQrContent);
    }
} 