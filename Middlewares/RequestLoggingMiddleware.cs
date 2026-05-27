namespace TravelPlannerMVC.Middleware
{
    public class AuthCheckMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();

            if (path.Contains("/account/login") || path.Contains("/account/register"))
            {
                await _next(context);
                return;
            }

            if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/Account/Login");
                return;
            }

            await _next(context);
        }
    }
}