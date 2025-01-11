using FlightDocsSystem.Data;
using System.Security.Claims;

namespace FlightDocsSystem.Middlewares
{
    public class CheckTokenMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckTokenMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //// Truong hop khong co token
            //if (string.IsNullOrEmpty(userId))
            //{
            //    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            //    await context.Response.WriteAsync("Unauthorized: Token is missing or invalid.");
            //    return;
            //}

            // Truong hop tai khoan user khong duoc active
            if (int.TryParse(userId, out int parsedUserId))
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var account = await dbContext.Accounts.FindAsync(parsedUserId);
                if (account?.IsActive == false)
                {
                    context.Response.StatusCode = StatusCodes.Status403Forbidden;
                    await context.Response.WriteAsync("Account is inactive.");
                    return;
                }
            }

            await _next(context);
        }

    }
}
