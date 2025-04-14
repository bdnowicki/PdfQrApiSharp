using PdfQrApiSharp.Models;

namespace PdfQrApiSharp.Tests.TestData;

public static class PdfQrTestDataFactory
{
    public const string TestBaseUrl = "http://test.com";
    public const string TestApiVersion = "v1";
    public const string TestEndpoint = "add-qr-to-pdf";
    public const string TestQrContent = "https://example.com";
    public const string TestQrContentWithSpecialChars = "https://example.com?param=value&test=1";
    public const string TestQrContentWithUnicode = "https://example.com/测试";
    public const string RequiredFieldsErrorMessage = "PDF content and QR content are required";
    public const string ApiRequestFailedMessage = "API request failed";
    public const string NetworkErrorMessage = "Network error";
    public const string TimeoutErrorMessage = "The operation has timed out";

    public static byte[] CreateTestPdfContent()
    {
        return new byte[] { 0x25, 0x50, 0x44, 0x46 }; // PDF magic number
    }

    public static byte[] CreateLargePdfContent()
    {
        var largePdfContent = new byte[1024 * 1024]; // 1MB PDF
        for (int i = 0; i < largePdfContent.Length; i++)
        {
            largePdfContent[i] = 0x25;
        }
        largePdfContent[0] = 0x25;
        largePdfContent[1] = 0x50;
        largePdfContent[2] = 0x44;
        largePdfContent[3] = 0x46;
        return largePdfContent;
    }

    public static PdfQrRequest CreateTestRequest(byte[]? pdfContent = null, string? qrContent = null)
    {
        return new PdfQrRequest
        {
            PdfContent = pdfContent ?? CreateTestPdfContent(),
            QrContent = qrContent ?? TestQrContent
        };
    }

    public static PdfQrApiConfig CreateTestConfig()
    {
        return new PdfQrApiConfig
        {
            BaseUrl = TestBaseUrl,
            ApiVersion = TestApiVersion,
            Endpoint = TestEndpoint,
            Timeout = TimeSpan.FromSeconds(30)
        };
    }
} 