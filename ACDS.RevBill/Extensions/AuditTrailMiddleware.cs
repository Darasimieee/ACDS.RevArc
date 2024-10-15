using System.Text;
using ACDS.RevBill.Entities.Models;
using ACDS.RevBill.Helpers;

public class AuditTrailMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private string? _tenantId;

    public AuditTrailMiddleware(RequestDelegate next, IHttpContextAccessor httpContextAccessor)
    {
        _next = next;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Capture the request information
        var request = context.Request;
        var requestInfo = $"{request.Method} {request.Path}{request.QueryString}";

        // Capture the response information
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);

        // Capture the response status code
        var responseStatusCode = context.Response.StatusCode;

        // Get the response body
        responseBody.Seek(0, SeekOrigin.Begin);
        var responseBodyText = await new StreamReader(responseBody).ReadToEndAsync();
        responseBody.Seek(0, SeekOrigin.Begin);

        // Log the audit trail information (you can customize this according to your needs)
        var auditTrailMessage = $"{DateTime.UtcNow} | Request: {requestInfo} | Response: {responseStatusCode} | Response Body: {responseBodyText}";
        Console.WriteLine(auditTrailMessage);

        // Copy the response body back to the original stream
        await responseBody.CopyToAsync(originalBodyStream);
    }

    //public async Task Invoke(HttpContext httpContext, DataContext context)
    //{
    //    _tenantId = _httpContextAccessor.HttpContext?.User.Claims
    //         .FirstOrDefault(claim => claim.Type == "TenantId")?.Value;

    //    // Read the request body
    //    var requestBody = await ReadRequestBody(httpContext.Request);

    //    // Process the request and store the audit trail
    //    ProcessAuditTrail(httpContext, context, requestBody);

    //    // Call the next middleware in the pipeline
    //    await _next(httpContext).ConfigureAwait(false);
    //}

    //private async Task<string> ReadRequestBody(HttpRequest request)
    //{
    //    using (var reader = new StreamReader(request.Body, Encoding.UTF8, true, 1024, true))
    //    {
    //        return await reader.ReadToEndAsync();
    //    }
    //}

    //private void ProcessAuditTrail(HttpContext httpContext, DataContext context, string requestBody)
    //{
    //    // Get the necessary information from the HttpContext
    //    var requestPath = httpContext.Request.Path;
    //    var requestMethod = httpContext.Request.Method;
    //    //var user = httpContext.User.Identity.Name;
    //    var ipAddress = httpContext.Connection.RemoteIpAddress?.ToString();

    //    // Create a new AuditTrail object
    //    var auditTrail = new AuditTrail
    //    {
    //        TenantName = requestPath,
    //        EventType = requestMethod,
    //        //User = user,
    //        LastUpdatedDate = ipAddress,
    //        JsonData = requestBody,
    //        InsertedDate = DateTime.UtcNow
    //    };

    //    // Store the audit trail in the database using the Repository Context
    //    context.AuditTrail.Add(auditTrail);
    //    context.SaveChanges();
    //}
}