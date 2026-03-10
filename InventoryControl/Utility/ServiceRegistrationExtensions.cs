namespace InventoryControl.Utility;

using System.Reflection;


public static class ServiceRegistrationExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var serviceTypes = assembly.GetTypes()
            .Where(t =>
                t.IsClass &&
                !t.IsAbstract &&
                !typeof(BackgroundService).IsAssignableFrom(t) &&
                !typeof(IHostedService).IsAssignableFrom(t));

        foreach (var implementation in serviceTypes)
        {
            var interfaces = implementation.GetInterfaces();

            foreach (var serviceInterface in interfaces)
            {
                if (serviceInterface.Name.EndsWith("Service"))
                {
                    services.AddScoped(serviceInterface, implementation);
                }
            }
        }

        return services;
    }
}