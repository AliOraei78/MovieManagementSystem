using Microsoft.AspNetCore.Http; // Fixes RequestDelegate and HttpContext issues
using MovieManagementSystem.Infrastructure.Services;
using System.Threading.Tasks;

namespace MovieManagementSystem.Api.Middleware; // Change the namespace to the API project

public class TenantMiddleware
{
    private readonly RequestDelegate _next;

    public TenantMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context, CurrentTenant currentTenant)
    {
        // Fix CS0165 error: define tenantId variable inline where it is used
        if (context.Request.Headers.TryGetValue("X-Tenant-Id", out var tenantIdValue) &&
            int.TryParse(tenantIdValue, out int tenantId))
        {
            currentTenant.SetTenant(tenantId);
        }
        else
        {
            // Default value for testing
            currentTenant.SetTenant(1);
        }

        await _next(context); // After adding the using directive above, this error is resolved
    }
}
