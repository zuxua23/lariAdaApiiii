using InventoryControl.Entity;
using Microsoft.EntityFrameworkCore;
using System;

namespace InventoryControl.Database.Seeder;

public class SeedAccess
{
    public static async Task Initialize(IServiceProvider serviceProvider)
    {
        using var context = new AppDBContext(
            serviceProvider.GetRequiredService<DbContextOptions<AppDBContext>>());

        var systemUser = await context.Users
    .FirstOrDefaultAsync(u => u.Username == "SYSTEM");
        if (!context.Permissions.Any())
        {
            var permissions = new List<Permission>
            {
                new Permission {Id = Guid.NewGuid().ToString(), Code = "MASTER_ITEM_VIEW", Name = "View Master Item", CreatedBy = "SYSTEM", PerId = "PER001"},
                new Permission {Id = Guid.NewGuid().ToString(), Code = "MASTER_ITEM_CREATE", Name = "Create Master Item", CreatedBy = "SYSTEM",PerId = "PER002"  },
                new Permission {Id = Guid.NewGuid().ToString(), Code = "MASTER_ITEM_UPDATE", Name = "Update Master Item", CreatedBy = "SYSTEM", PerId = "PER003"},
                new Permission {Id = Guid.NewGuid().ToString(), Code = "MASTER_ITEM_DELETE", Name = "Delete Master Item", CreatedBy = "SYSTEM", PerId = "PER004"},

                new Permission {Id = Guid.NewGuid().ToString(), Code = "TRANS_STOCK_IN", Name = "Stock In", CreatedBy = "SYSTEM", PerId = "PER005"},
                new Permission {Id = Guid.NewGuid().ToString(), Code = "TRANS_STOCK_OUT", Name = "Stock Out", CreatedBy = "SYSTEM", PerId = "PER006"},

                new Permission {Id = Guid.NewGuid().ToString(), Code = "PRINT_TAG", Name = "Print Tag", CreatedBy = "SYSTEM", PerId = "PER007"},
                new Permission {Id = Guid.NewGuid().ToString(), Code = "REPRINT_TAG", Name = "Reprint Tag", CreatedBy = "SYSTEM"       , PerId = "PER008"}
            };

            context.Permissions.AddRange(permissions);
            await context.SaveChangesAsync();
        }

        if (!context.Roles.Any())
        {
            var operatorRole = new Role
            {
                Id = Guid.NewGuid().ToString(),
                RolId="ROL001",
                Code = "OPERATOR",
                Name = "Operator"
            };

            var adminRole = new Role
            {
                Id = Guid.NewGuid().ToString(),
                RolId="ROL002",
                Code = "ADMIN",
                Name = "Administrator"
            };

            context.Roles.AddRange(operatorRole, adminRole);
            await context.SaveChangesAsync();

            // Assign permission ke role
            var stockInPermission = context.Permissions
                .First(p => p.Code == "TRANS_STOCK_IN");

            context.RolePermissions.Add(new Role_Permission
            {
                RolId = operatorRole.RolId,
                PerId = stockInPermission.PerId
            });

            var allPermissions = context.Permissions.ToList();

            foreach (var permission in allPermissions)
            {
                context.RolePermissions.Add(new Role_Permission
                {
                    RolId = adminRole.RolId,
                    PerId = permission.PerId
                });
            }

            await context.SaveChangesAsync();
        }
    }
}