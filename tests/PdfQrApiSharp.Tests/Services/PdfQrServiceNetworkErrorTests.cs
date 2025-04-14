using System;
using System.Net.Http;
using Moq;
using Moq.Protected;
using Xunit;
using PdfQrApiSharp.Services;
using PdfQrApiSharp.Tests.TestData;

namespace PdfQrApiSharp.Tests.Services;

public class PdfQrServiceNetworkErrorTests : BasePdfQrServiceTests
{
    /// <summary>
    /// Tests that a network error is properly handled and returns an appropriate error message.
    /// </summary>
    [Fact]
    public async Task AddQrToPdfAsync_WithNetworkError_ReturnsError()
    {
        // Arrange
        var request = PdfQrTestDataFactory.CreateTestRequest();
        MockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new HttpRequestException(PdfQrTestDataFactory.NetworkErrorMessage));

        // Act
        var response = await Service.AddQrToPdfAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Contains(PdfQrTestDataFactory.NetworkErrorMessage, response.ErrorMessage);
        Assert.Null(response.ModifiedPdf);
    }

    /// <summary>
    /// Tests that a timeout error is properly handled and returns an appropriate error message.
    /// </summary>
    [Fact]
    public async Task AddQrToPdfAsync_WithTimeout_ReturnsError()
    {
        // Arrange
        var request = PdfQrTestDataFactory.CreateTestRequest();
        MockHttpMessageHandler.Protected()
            .Setup<Task<HttpResponseMessage>>(
                "SendAsync",
                ItExpr.IsAny<HttpRequestMessage>(),
                ItExpr.IsAny<CancellationToken>())
            .ThrowsAsync(new TaskCanceledException(PdfQrTestDataFactory.TimeoutErrorMessage));

        // Act
        var response = await Service.AddQrToPdfAsync(request);

        // Assert
        Assert.False(response.Success);
        Assert.Contains(PdfQrTestDataFactory.TimeoutErrorMessage, response.ErrorMessage);
        Assert.Null(response.ModifiedPdf);
    }

    /// <summary>
    /// Tests that the Dispose method properly disposes of resources.
    /// </summary>
    [Fact]
    public async Task Dispose_WhenCalled_DisposesResources()
    {
        // Act
        Service.Dispose();

        // Assert - verify that HttpClient is disposed by checking if it throws ObjectDisposedException
        await Assert.ThrowsAsync<ObjectDisposedException>(() => HttpClient.GetAsync("http://test.com"));
    }
} 