using FlightDocsSystem.Data;
using System.Security.Claims;

namespace FlightDocsSystem.Middlewares
{
    public class CheckActiveAccountMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckActiveAccountMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IServiceProvider serviceProvider)
        {
            var userId = context.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (int.TryParse(userId, out int parsedUserId))
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

                var account = await dbContext.Accounts.FindAsync(parsedUserId);
                if (account?.IsActive.ToString() == "False")
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
