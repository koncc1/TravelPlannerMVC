namespace TravelPlannerMVC.Middleware
{
    /// <summary>
    /// Custom middleware that checks if user is authenticated.
    /// Redirects unauthenticated users to login page.
    /// </summary>
    public class AuthCheckMiddleware
    {
        #region Fields

        private readonly RequestDelegate _next;

        #endregion

        #region Constructor

        public AuthCheckMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        #endregion

        #region Middleware Logic

        /// <summary>
        /// Executes authentication check for each HTTP request
        /// </summary>
        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.ToString().ToLower();

            // Allow login and register pages without auth check
            if (path.Contains("/account/login") || path.Contains("/account/register"))
            {
                await _next(context);
                return;
            }

            // If user is not authenticated → redirect to login
            if (context.User?.Identity == null || !context.User.Identity.IsAuthenticated)
            {
                context.Response.Redirect("/Account/Login");
                return;
            }

            // Continue pipeline
            await _next(context);
        }

        #endregion
    }
}