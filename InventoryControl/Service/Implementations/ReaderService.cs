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
            .Include(r => r.LocationNavigation)
            .Select(r => new ReaderResponseDto
            {
                RdrId = r.RdrId,
                RdrName = r.Name,
                LocId = r.LocationId,
                LocationName = r.LocationNavigation.Name
            })
            .ToListAsync();
    }

    public async Task CreateAsync(ReaderDto dto, string createdBy)
    {
        if (string.IsNullOrWhiteSpace(dto.RdrId))
            throw new Exception("Reader ID tidak boleh kosong");

        var existingReader = await _db.Readers
            .FirstOrDefaultAsync(x => x.RdrId == dto.RdrId);

        if (existingReader != null)
            throw new Exception("Reader sudah terdaftar");

        var location = await _db.Locations
            .FirstOrDefaultAsync(x => x.Id == dto.LocId);

        if (location == null)
            throw new Exception("Location tidak ditemukan");

        var reader = new Reader
        {
            Id = Guid.NewGuid().ToString(),
            RdrId = dto.RdrId,
            LocationId = location.Id,
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