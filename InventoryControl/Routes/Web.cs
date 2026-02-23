namespace InventoryControl.Routes;

public static class WebRoutes
{
    public static void Map(WebApplication app)
    {
        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Auth}/{action=Login}/{id?}"
        );
    }
}
