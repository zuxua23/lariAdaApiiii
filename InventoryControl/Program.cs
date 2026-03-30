using InventoryControl.Database;
using InventoryControl.Database.Seeder;
using InventoryControl.Utility;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient();


#region DATABASE
builder.Services.AddDbContext<AppDBContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")));
#endregion


#region SESSION CONFIG 
builder.Services.AddDistributedMemoryCache();

builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30); 
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
#endregion

#region DEPENDENCY INJECTION
builder.Services.AddApplicationServices();
builder.Services.AddControllersWithViews();
#endregion

#region MVC + API
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
});
#endregion



var app = builder.Build();

#region SEEDER
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedAccess.Initialize(services);
}
#endregion

#region MIDDLEWARE PIPELINE

app.UseStaticFiles();
app.UseRouting();
app.UseSession();
app.UseAuthorization();
#endregion


#region SESSION AUTH MIDDLEWARE 
app.Use(async (context, next) =>
{
    var path = context.Request.Path;

    if (path.StartsWithSegments("/login") ||
          path.StartsWithSegments("/auth/login") ||
          path.StartsWithSegments("/auth/logout") ||
          path.StartsWithSegments("/css") ||
          path.StartsWithSegments("/js") ||
          path.StartsWithSegments("/lib") ||
          path.StartsWithSegments("/images"))
    { await next();
        return;
    }
        var userId = context.Session.GetString("UserId");

        if (string.IsNullOrEmpty(userId))
        {
            context.Response.StatusCode = 401;

            if (path.StartsWithSegments("/api") || path.StartsWithSegments("/auth"))
            {
                context.Response.ContentType = "application/json";

                var result = JsonConvert.SerializeObject(new
                {
                    status = 401,
                    code = "UNAUTHORIZED",
                    message = "Silakan login terlebih dahulu"
                });
                await context.Response.WriteAsync(result);
            return;

            }

            context.Response.Redirect("/login");
            return;
        }
    

    await next();
});
#endregion


//#region API FORBIDDEN (403) CUSTOM
//app.Use(async (context, next) =>
//{
//    await next();

//    if (context.Response.StatusCode == 403 &&
//        context.Request.Path.StartsWithSegments("/api"))
//    {
//        context.Response.ContentType = "application/json";

//        var result = JsonConvert.SerializeObject(new
//        {
//            status = 403,
//            code = "FORBIDDEN",
//            message = "Anda tidak memiliki akses"
//        });

//        await context.Response.WriteAsync(result);
//    }
//});
//#endregion

#region ROUTING
// API
app.MapControllers();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}"
);
#endregion

app.Run();
