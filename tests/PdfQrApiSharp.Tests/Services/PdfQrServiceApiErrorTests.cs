using System.Net;
using Xunit;
using PdfQrApiSharp.Tests.TestData;

namespace PdfQrApiSharp.Tests.Services;

public class PdfQrServiceApiErrorTests : BasePdfQrServiceTests
{
    /// <summary>
    /// Tests that an API error response is properly handled and returns an appropriate error message.
    /// </summary>
    [Fact]
    public async Task AddQrToPdfAsync_WithApiError_ReturnsError()
    {
        // Arrange
        var request = PdfQrTestDataFactory.CreateTestRequest();
        const string errorMessage = "Invalid PDF format";
        SetupMockResponse(HttpStatusCode.BadRequest, errorContent: errorMessage);

        // Act
        var response = await Service.AddQrToPdfAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Contains(PdfQrTestDataFactory.ApiRequestFailedMessage, response.ErrorMessage);
        Assert.Contains(errorMessage, response.ErrorMessage);
        Assert.Null(response.ModifiedPdf);
        VerifyHttpRequest(PdfQrTestDataFactory.TestQrContent);
    }

    /// <summary>
    /// Tests that different HTTP error status codes are properly handled.
    /// </summary>
    [Theory]
    [InlineData(HttpStatusCode.Unauthorized)]
    [InlineData(HttpStatusCode.Forbidden)]
    [InlineData(HttpStatusCode.NotFound)]
    [InlineData(HttpStatusCode.InternalServerError)]
    public async Task AddQrToPdfAsync_WithDifferentErrorStatusCodes_ReturnsError(HttpStatusCode statusCode)
    {
        // Arrange
        var request = PdfQrTestDataFactory.CreateTestRequest();
        const string errorMessage = "Error occurred";
        SetupMockResponse(statusCode, errorContent: errorMessage);

        // Act
        var response = await Service.AddQrToPdfAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Contains(PdfQrTestDataFactory.ApiRequestFailedMessage, response.ErrorMessage);
        Assert.Contains(statusCode.ToString(), response.ErrorMessage);
        Assert.Contains(errorMessage, response.ErrorMessage);
        Assert.Null(response.ModifiedPdf);
        VerifyHttpRequest(PdfQrTestDataFactory.TestQrContent);
    }
} 