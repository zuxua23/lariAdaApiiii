namespace InventoryControl.Routes;

public static class Web
{
    public static void Map(WebApplication app)
    {
        app.MapControllerRoute(
            name: "login-page",
            pattern: "/login",
            defaults: new { controller = "Auth", action = "Index" }
        ).WithMetadata(new HttpMethodMetadata(new[] { "GET" }));

        app.MapControllerRoute(
            name: "login-post",
            pattern: "/login",
            defaults: new { controller = "Auth", action = "Login" })
            .WithMetadata(new HttpMethodMetadata(new[] { "POST" }));


        app.MapControllerRoute(
            name: "logout",
            pattern: "/logout",
            defaults: new { controller = "Auth", action = "Logout" });

        app.MapControllerRoute(
        name: "home",
        pattern: "/home",
        defaults: new { controller = "Home", action = "Index" })
        .AddEndpointFilter(async (context, next) =>
        {
            // Jika user sudah login, redirect ke dashboard
            if (context.HttpContext.Session.GetString("is_login") != "OK")
            {
                context.HttpContext.Response.Redirect("/");
                return Results.Empty;
            }
            return await next(context);
        })
        .WithMetadata(new HttpMethodMetadata(new[] { "GET" }));

    }
}