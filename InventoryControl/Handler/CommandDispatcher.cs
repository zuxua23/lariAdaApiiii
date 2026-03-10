using InventoryControl.DTO;
using InventoryControl.Models;

namespace InventoryControl.Handler;
public class CommandDispatcher
{
    private readonly IEnumerable<ICommandHandler> _handlers;

    public CommandDispatcher(IEnumerable<ICommandHandler> handlers)
    {
        _handlers = handlers;
    }

    public async Task DispatchAsync(Message message)
    {

        var handler = _handlers.FirstOrDefault(h =>
            h.TrxType.Equals(message.TrxType, StringComparison.OrdinalIgnoreCase));
        Console.WriteLine($"Dispatching {message.TrxType} -> {message.Action}");
        Console.WriteLine($"Handlers count: {_handlers.Count()}");
        if (handler == null)
            throw new Exception($"Handler not found for {message.TrxType}");

        await handler.HandleAsync(message.Action, message.Data);
    }
}