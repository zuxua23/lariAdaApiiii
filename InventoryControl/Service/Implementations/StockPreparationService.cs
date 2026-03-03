namespace InventoryControl.Service.Implementations;

using InventoryControl.Database;
using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

public class StockPreparationService : IStockPreparationService
{
    private readonly AppDBContext _db;

    public StockPreparationService(AppDBContext db)
    {
        _db = db;
    }

    public async Task PrepareAsync(StockPreparationRequestDto dto, string user)
    {
        var doData = await _db.DOs
            .Include(d => d.Details)
            .FirstOrDefaultAsync(d => d.DoId == dto.DoId && !d.IsDelete);

        if (doData == null)
            throw new Exception("DO tidak ditemukan");

        var tag = await _db.Tags
            .FirstOrDefaultAsync(t => t.EpcTag == dto.EpcTag);

        if (tag == null)
            throw new Exception("Tag tidak ditemukan");

        if (tag.Status != "IN_STOCK")
            throw new Exception("Tag tidak dalam status IN_STOCK");

        var detail = doData.Details
            .FirstOrDefault(d => d.ItemId == tag.ItmId);

        if (detail == null)
            throw new Exception("Item tidak ada dalam DO");

        var reservedCount = await _db.TransactionDetails
            .Where(td =>
                td.ItemId == tag.ItmId &&
                td.Transaction.TrsType == "STOCK_PREPARATION" &&
                td.Transaction.ReferenceId == dto.DoId)
            .CountAsync();

        if (reservedCount >= detail.QtyRequired)
            throw new Exception("Qty item sudah terpenuhi untuk DO ini");

        var transaction = new Transaction
        {
            TrsId = Guid.NewGuid().ToString(),
            TrsType = "STOCK_PREPARATION",
            ReferenceId = dto.DoId,
            RdrId = dto.ReaderId,
            CreatedBy = user,
            CreatedAt = DateTime.UtcNow
        };

        _db.Transactions.Add(transaction);

        var trDetail = new Transaction_Detail
        {
            TrdId = Guid.NewGuid().ToString(),
            TrsId = transaction.TrsId,
            TagId = tag.TagId,
            ItemId = tag.ItmId
        };

        _db.TransactionDetails.Add(trDetail);

        tag.Status = "RESERVED";
        tag.UpdatedBy = user;
        tag.UpdatedAt = DateTime.UtcNow;

        await _db.SaveChangesAsync();
    }

    public async Task PrepareAsync(StockPreparationDto dto, string user)
    {
        using var trx = await _db.Database.BeginTransactionAsync();

        if (!dto.TagIds.Any())
            throw new Exception("Tidak ada tag yang diprepare");

        var doData = await _db.DOs
            .Include(d => d.Details)
            .FirstOrDefaultAsync(d => d.DoId == dto.DoId && d.IsDelete == false);

        if (doData == null)
            throw new Exception("DO tidak ditemukan");

        var tags = await _db.Tags
            .Where(t => dto.TagIds.Contains(t.TagId))
            .ToListAsync();

        foreach (var tag in tags)
        {
            if (tag.Status != "IN_STOCK")
                throw new Exception($"Tag {tag.TagId} belum ada di stock");

            var doDetail = doData.Details
                .FirstOrDefault(x => x.ItemId == tag.ItmId);

            if (doDetail == null)
                throw new Exception($"Item {tag.ItmId} tidak ada di DO");

            var reservedCount = await _db.Tags
                .CountAsync(t => t.Status == "RESERVED"
                                 && t.ItmId == tag.ItmId);

            if (reservedCount >= doDetail.QtyRequired)
                throw new Exception($"Qty item {tag.ItmId} sudah terpenuhi");

            tag.Status = "RESERVED";
            tag.UpdatedBy = user;
            tag.UpdatedAt = DateTime.UtcNow;
        }

        var trxHeader = new Transaction
        {
            TrsId = Guid.NewGuid().ToString(),
            TrsType = "STOCK_PREPARATION",
            ReferenceId = dto.DoId,
            RdrId = dto.ReaderId,
            CreatedBy = user,
            CreatedAt = DateTime.UtcNow
        };

        _db.Transactions.Add(trxHeader);

        foreach (var tag in tags)
        {
            _db.TransactionDetails.Add(new Transaction_Detail
            {
                TrdId = Guid.NewGuid().ToString(),
                TrsId = trxHeader.TrsId,
                TagId = tag.TagId,
                ItemId = tag.ItmId
            });

            _db.Histories.Add(new HistoryPrint
            {
                HisId = Guid.NewGuid().ToString(),
                TagId = tag.TagId,
                ItmId = tag.ItmId,
                Type = "STOCK_PREPARATION",
                Reference = dto.DoId,
                Action = "RESERVED",
                CreatedBy = user,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync();
        await trx.CommitAsync();
    }
}
