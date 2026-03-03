using InventoryControl.DTO;

namespace InventoryControl.Service.Implementations;

using InventoryControl.Database;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

public class ReaderService : IReaderService
{
    private readonly AppDBContext _db;

    public ReaderService(AppDBContext db)
    {
        _db = db;
    }

    public async Task<List<ReaderResponseDto>> GetAllAsync()
    {
        return await _db.Readers
            .Include(r => r.Location)
            .Select(r => new ReaderResponseDto
            {
                RdrId = r.RdrId,
                RdrName = r.Name,
                LocId = r.Location,
                LocationName = r.LocationNavigation.Name
            })
            .ToListAsync();
    }

    public async Task CreateAsync(ReaderDto dto, string createdBy)
    {
        var reader = new Reader
        {
            RdrId = dto.RdrId,
            Location = dto.LocId,
            Name = dto.RdrName,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        };

        _db.Readers.Add(reader);
        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(string id)
    {
        var reader = await _db.Readers.FindAsync(id);

        if (reader == null)
            throw new Exception("Reader tidak ditemukan");

        _db.Readers.Remove(reader);
        await _db.SaveChangesAsync();
    }
}