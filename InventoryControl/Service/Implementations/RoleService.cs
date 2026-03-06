namespace InventoryControl.Service.Implementations;

using InventoryControl.Database;
using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using InventoryControl.Utility;
using Microsoft.EntityFrameworkCore;

public class RoleService : IRoleService
{
    private readonly AppDBContext _db;

    public RoleService(AppDBContext db)
    {
        _db = db;
    }

    public async Task<List<RoleResponseDto>> GetAllAsync()
    {
        try
        {
            var result = await _db.Roles
                .Where(x => x.IsDelete == false)
                .Select(x => new RoleResponseDto
                {
                    Id = x.Id,
                    Code = x.Code,
                    Name = x.Name
                })
                .ToListAsync();

            DailyFileLogger.Info($"GetAllAsync Role berhasil, total {result.Count} role.");

            return result;
        }
        catch (Exception ex)
        {
            DailyFileLogger.Error("Error di GetAllAsync Role.", ex);
            throw;
        }
    }

    public async Task CreateAsync(RoleDto dto, string createdBy)
    {
        try
        {
            var exists = await _db.Roles
                .AnyAsync(x => x.Code == dto.Code && x.IsDelete == false);

            if (exists)
            {
                DailyFileLogger.Warn($"CreateAsync Role gagal: Role dengan Code {dto.Code} sudah ada.");
                throw new Exception("Role sudah ada.");
            }

            var role = new Role
            {
                Id = Guid.NewGuid().ToString(),
                Code = dto.Code,
                Name = dto.Name,
                IsDelete = false
            };

            _db.Roles.Add(role);
            await _db.SaveChangesAsync();

            DailyFileLogger.Info($"CreateAsync Role berhasil. Code: {dto.Code}, CreatedBy: {createdBy}");
        }
        catch (Exception ex)
        {
            DailyFileLogger.Error($"Error di CreateAsync Role untuk Code {dto.Code}.", ex);
            throw;
        }
    }

    public async Task AssignPermissionsAsync(string roleId, List<string> permissionIds)
    {
        try
        {
            var role = await _db.Roles.FindAsync(roleId);

            if (role == null || role.IsDelete)
            {
                DailyFileLogger.Warn($"AssignPermissionsAsync gagal: Role {roleId} tidak ditemukan.");
                throw new Exception("Role tidak ditemukan.");
            }

            foreach (var permissionId in permissionIds)
            {
                bool exists = await _db.RolePermissions
                    .AnyAsync(rp => rp.RoleId == roleId && rp.PermissionId == permissionId);

                if (!exists)
                {
                    _db.RolePermissions.Add(new Role_Permission
                    {
                        Id = Guid.NewGuid().ToString(),
                        RoleId = roleId,
                        PermissionId = permissionId,
                        CreatedAt = DateTime.UtcNow
                    });

                    DailyFileLogger.Info($"Permission {permissionId} berhasil ditambahkan ke Role {roleId}");
                }
            }

            await _db.SaveChangesAsync();

            DailyFileLogger.Info($"AssignPermissionsAsync selesai untuk Role {roleId}");
        }
        catch (Exception ex)
        {
            DailyFileLogger.Error($"Error di AssignPermissionsAsync untuk Role {roleId}.", ex);
            throw;
        }
    }

    public async Task AssignRolesToUserAsync(string userId, List<string> roleIds)
    {
        try
        {
            var user = await _db.Users.FindAsync(userId);

            if (user == null || user.IsDelete)
            {
                DailyFileLogger.Warn($"AssignRolesToUserAsync gagal: User {userId} tidak ditemukan.");
                throw new Exception("User tidak ditemukan.");
            }

            foreach (var roleId in roleIds)
            {
                bool exists = await _db.UserRoles
                    .AnyAsync(ur => ur.UserId == userId && ur.RoleId == roleId);

                if (!exists)
                {
                    _db.UserRoles.Add(new User_Role
                    {
                        Id = Guid.NewGuid().ToString(),
                        UserId = userId,
                        RoleId = roleId,
                        CreatedAt = DateTime.UtcNow
                    });

                    DailyFileLogger.Info($"Role {roleId} berhasil di-assign ke User {userId}");
                }
            }

            await _db.SaveChangesAsync();

            DailyFileLogger.Info($"AssignRolesToUserAsync selesai untuk User {userId}");
        }
        catch (Exception ex)
        {
            DailyFileLogger.Error($"Error di AssignRolesToUserAsync untuk User {userId}.", ex);
            throw;
        }
    }
}