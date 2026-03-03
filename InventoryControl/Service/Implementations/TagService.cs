namespace InventoryControl.Service.Implementations;

using InventoryControl.Database;
using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

public class TagService : ITagService
{
    private readonly AppDBContext _db;

    public TagService(AppDBContext db)
    {
        _db = db;
    }

    public async Task<List<TagResponseDto>> GetAllAsync()
    {
        return await _db.Tags
            .Include(t => t.Item)
            .Include(t => t.Location)
            .Select(t => new TagResponseDto
            {
                TagId = t.TagId,
                ItemId = t.ItmId,
                ItemName = t.Item.Name,
                Status = t.Status,
                CurrentLocation = t.Location.Name
            })
            .ToListAsync();
    }

    public async Task CreateAsync(TagCreateDto dto, string createdBy)
    {
        var tag = new Tag
        {
            TagId = Guid.NewGuid().ToString(),
            ItmId = dto.ItemId,
            EpcTag = dto.EpcTag,
            Status = "PRINTED",
            Curent_Location = dto.CurrentLocId,
            CreatedBy = createdBy,
            CreatedAt = DateTime.UtcNow
        };

        _db.Tags.Add(tag);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(string tagId, string status)
    {
        var tag = await _db.Tags.FindAsync(tagId);

        if (tag == null)
            throw new Exception("Tag tidak ditemukan");

        tag.Status = status;
        tag.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }
}