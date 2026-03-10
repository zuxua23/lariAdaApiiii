namespace InventoryControl.Utility;

using InventoryControl.Handler;
using System.Reflection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var handlerTypes = assembly.GetTypes()
            .Where(t =>
                typeof(ICommandHandler).IsAssignableFrom(t) &&
                t.IsClass &&
                !t.IsAbstract &&
                !t.IsGenericType &&
                !t.Name.StartsWith("<"));

        foreach (var type in handlerTypes)
        {
            services.AddScoped(typeof(ICommandHandler), type);
        }

        return services;
    }
}