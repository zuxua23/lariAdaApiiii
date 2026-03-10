//namespace InventoryControl.PermissionHelper;

//using Microsoft.AspNetCore.Authorization;
//using System.Security.Claims;

//public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
//{
//    protected override Task HandleRequirementAsync(
//        AuthorizationHandlerContext context,
//        PermissionRequirement requirement)
//    {
//        var permissions = context.User
//            .FindAll("permission")
//            .Select(c => c.Value);

//        if (permissions.Contains(requirement.PermissionReq))
//        {
//            context.Succeed(requirement);
//        }

//        return Task.CompletedTask;
//    }
//}