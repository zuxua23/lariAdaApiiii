namespace InventoryControl.Service.Implementations;

using InventoryControl.Database;
using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using InventoryControl.Utility;
using Microsoft.EntityFrameworkCore;
using System;

public class PermissionService : IPermissionService
{
    private readonly AppDBContext _db;

    public PermissionService(AppDBContext db)
    {
        _db = db;
    }


    public async Task Create(RoleRequestDto dto, string user)
    {
        var role = new Role
        {
            Id = Guid.NewGuid().ToString(),
            Code = dto.RoleCode,
            Name = dto.RoleName
        };

        _db.Roles.Add(role);

        await SavePermissions(role.Id, dto.Permissions, user);

        await _db.SaveChangesAsync();
    }

    public async Task Update(string id, RoleRequestDto dto, string user)
    {
        var role = await _db.Roles.FindAsync(id);
        if (role == null) throw new Exception("Role not found");

        role.Name = dto.RoleName;
        role.Code = dto.RoleCode;

        // hapus permission lama
        var existing = _db.RolePermissions.Where(x => x.RoleId == id);
        _db.RolePermissions.RemoveRange(existing);

        await SavePermissions(id, dto.Permissions, user);

        await _db.SaveChangesAsync();
    }

    private async Task SavePermissions(string roleId, Dictionary<string, List<string>> permissions, string user)
    {
        foreach (var module in permissions)
        {
            var moduleKey = module.Key;

            foreach (var action in module.Value)
            {
                var permission = await _db.Permissions
                    .Include(p => p.Module)
                    .FirstOrDefaultAsync(x =>
                        x.Module.ModuleKey == moduleKey &&
                        x.Operation == action);

                if (permission == null)
                    continue;

                _db.RolePermissions.Add(new Role_Permission
                {
                    Id = Guid.NewGuid().ToString(),
                    RoleId = roleId,
                    PermissionId = permission.Id
                });
            }
        }
    }


    public async Task<RoleResponseDto> GetById(string id)
    {
        var role = await _db.Roles.FindAsync(id);

        var rolePermissions = await _db.RolePermissions
            .Where(x => x.RoleId == id)
            .Include(x => x.Permission)
            .ThenInclude(p => p.Module)
            .ToListAsync();

        var permissions = rolePermissions
            .GroupBy(x => x.Permission.Module.ModuleKey)
            .ToDictionary(
                g => g.Key,
                g => g.Select(x => x.Permission.Operation).ToList()
            );

        return new RoleResponseDto
        {
            Id = role.Id,
            RoleCode = role.Code,   
            RoleName = role.Name,
            Permissions = permissions
        };
    }

    public async Task<object> GetModules()
    {
        return await _db.Modules
            .Where(m => m.IsActive)
            .Select(m => new
            {
                moduleKey = m.ModuleKey,
                moduleName = m.ModuleName,
                permissions = m.Permissions
                    .Select(p => p.Operation)
                    .Distinct()
                    .ToList()
            })
            .ToListAsync();
    }

    public async Task<List<Role>> GetAll()  
    {
        return await _db.Roles.ToListAsync();
    }

    public async Task Delete(string id)
    {
        var role = await _db.Roles.FindAsync(id);
        if (role == null) return;

        var permissions = _db.RolePermissions.Where(x => x.RoleId == id);
        _db.RolePermissions.RemoveRange(permissions);

        _db.Roles.Remove(role);

        await _db.SaveChangesAsync();
    }
}