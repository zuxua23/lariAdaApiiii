using InventoryControl.Models;
using System.Text.Json;

namespace InventoryControl.Handler;

public interface ICommandHandler
{
    string TrxType { get; }

    Task HandleAsync(string action, JsonElement data);
}