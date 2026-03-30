namespace InventoryControl.Utility;


using System.Text.Json;

public static class PermissionHelper
{
    public static bool HasPermission(HttpContext context, string permission)
    {
        var json = context.Session.GetString("Permissions");

        if (string.IsNullOrEmpty(json))
            return false;

        var permissions = JsonSerializer.Deserialize<List<string>>(json);

        return permissions.Contains(permission);
    }

    public static bool HasAnyPermission(HttpContext context, params string[] perms)
    {
        var json = context.Session.GetString("Permissions");

        if (string.IsNullOrEmpty(json))
            return false;

        var permissions = JsonSerializer.Deserialize<List<string>>(json);

        return perms.Any(p => permissions.Contains(p));
    }
}