namespace InventoryControl.Service.Implementations;

using InventoryControl.Database;
using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

public class StockTakingService : IStockTakingService
{
    private readonly AppDBContext _db;

    public StockTakingService(AppDBContext db)
    {
        _db = db;
    }

    public async Task<string> CreateAsync(StockTakingCreateDto dto, string user)
    {
        var st = new StockTaking
        {
            SttId = Guid.NewGuid().ToString(),
            Remark = dto.Remark,
            Status = "OPEN",
            CreatedBy = user,
            CreatedAt = DateTime.UtcNow
        };

        _db.StockTakings.Add(st);
        await _db.SaveChangesAsync();

        return st.SttId;
    }

    public async Task<List<Tag>> GetStockDataAsync()
    {
        return await _db.Tags
            .Where(t => t.Status == "IN_STOCK")
            .ToListAsync();
    }

    public async Task ScanAsync(StockTakingScanDto dto)
    {
        _db.StockTakingDetails.Add(new StockTakingDetail
        {
            StdId = Guid.NewGuid().ToString(),
            SttId = dto.SttId,
            TagId = dto.TagId,
            Action = "FOUND"
        });

        await _db.SaveChangesAsync();
    }

    public async Task RemoveAsync(StockTakingRemoveDto dto)
    {
        _db.StockTakingDetails.Add(new StockTakingDetail
        {
            StdId = Guid.NewGuid().ToString(),
            SttId = dto.SttId,
            TagId = dto.TagId,
            Action = "REMOVE"
        });

        await _db.SaveChangesAsync();
    }

    public async Task ManualAddAsync(StockTakingManualAddDto dto)
    {
        _db.StockTakingDetails.Add(new StockTakingDetail
        {
            StdId = Guid.NewGuid().ToString(),
            SttId = dto.SttId,
            ItemId = dto.ItemId,
            Remark = dto.Remark,
            Action = "ADD_MANUAL"
        });

        await _db.SaveChangesAsync();
    }

    public async Task FinalizeAsync(StockTakingFinalizeDto dto, string user)
    {
        using var trx = await _db.Database.BeginTransactionAsync();

        var st = await _db.StockTakings
            .FirstOrDefaultAsync(x => x.SttId == dto.SttId && x.Status == "OPEN");

        if (st == null)
            throw new Exception("Session tidak aktif");

        var details = await _db.StockTakingDetails
            .Where(d => d.SttId == dto.SttId)
            .ToListAsync();

        var removeList = details.Where(d => d.Action == "REMOVE");

        foreach (var remove in removeList)
        {
            var tag = await _db.Tags
                .FirstOrDefaultAsync(t => t.TagId == remove.TagId);

            if (tag != null && tag.Status == "IN_STOCK")
            {
                tag.Status = "OUT";
                tag.UpdatedBy = user;
                tag.UpdatedAt = DateTime.UtcNow;
            }
        }

        _db.Transactions.Add(new Transaction
        {
            TrsId = Guid.NewGuid().ToString(),
            TrsType = "STOCK_ADJUSTMENT",
            ReferenceId = dto.SttId,
            CreatedBy = user,
            CreatedAt = DateTime.UtcNow
        });

        st.Status = "COMPLETED";

        await _db.SaveChangesAsync();
        await trx.CommitAsync();
    }
}