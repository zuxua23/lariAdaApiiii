namespace InventoryControl.PermissionHelper;

using Microsoft.AspNetCore.Authorization;

public class PermissionRequirement : IAuthorizationRequirement
{
    public string PermissionReq { get; }

    public PermissionRequirement(string permission)
    {
        PermissionReq = permission;
    }
}