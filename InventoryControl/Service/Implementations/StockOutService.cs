namespace InventoryControl.Service.Implementations;

using InventoryControl.Database;
using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

public class StockOutService : IStockOutService
{
    private readonly AppDBContext _db;

    public StockOutService(AppDBContext db)
    {
        _db = db;
    }

    public async Task StockOutAsync(StockOutDto dto, string user)
    {
        using var trx = await _db.Database.BeginTransactionAsync();

        var doData = await _db.DOs
            .Include(d => d.Details)
            .FirstOrDefaultAsync(d => d.DoId == dto.DoId && !d.IsDelete);

        if (doData == null)
            throw new Exception("DO tidak ditemukan");

        var reservedDetails = await _db.TransactionDetails
            .Include(td => td.Tag)
            .Include(td => td.Transaction)
            .Where(td =>
                td.Transaction.TrsType == "STOCK_PREPARATION" &&
                td.Transaction.ReferenceId == dto.DoId)
            .ToListAsync();

        if (!reservedDetails.Any())
            throw new Exception("Tidak ada tag yang diprepare untuk DO ini");

        foreach (var doDetail in doData.Details)
        {
            var reservedCount = reservedDetails
                .Count(x => x.ItemId == doDetail.ItemId);

            if (reservedCount != doDetail.QtyRequired)
                throw new Exception($"Qty item {doDetail.ItemId} belum terpenuhi");
        }

        var trxHeader = new Transaction
        {
            TrsId = Guid.NewGuid().ToString(),
            TrsType = "STOCK_OUT",
            ReferenceId = dto.DoId,
            RdrId = dto.ReaderId,
            CreatedBy = user,
            CreatedAt = DateTime.UtcNow
        };

        _db.Transactions.Add(trxHeader);

        var lastHistory = await _db.Histories
            .OrderByDescending(x => x.HisId)
            .FirstOrDefaultAsync();

        int lastNumber = 0;

        if (lastHistory != null)
            lastNumber = int.Parse(lastHistory.HisId.Substring(3));

        foreach (var detail in reservedDetails)
        {
            var tag = detail.Tag;

            if (tag.Status != "RESERVED")
                throw new Exception($"Tag {tag.TagId} tidak dalam status RESERVED");

            tag.Status = "OUT";
            tag.UpdatedBy = user;
            tag.UpdatedAt = DateTime.UtcNow;

            _db.TransactionDetails.Add(new Transaction_Detail
            {
                TrdId = Guid.NewGuid().ToString(),
                TrsId = trxHeader.TrsId,
                TagId = detail.TagId,
                ItemId = detail.ItemId
            });

            lastNumber++;

            _db.Histories.Add(new HistoryPrint
            {
                Id = Guid.NewGuid().ToString(),
                HisId = $"HIS{lastNumber:D5}",
                TagId = detail.TagId,
                ItemId = detail.ItemId,
                Type = "STOCK_OUT",
                Reference = dto.DoId,
                Action = "OUT",
                CreatedBy = user,
                CreatedAt = DateTime.UtcNow
            });
        }

        doData.Status = "COMPLETED";

        await _db.SaveChangesAsync();
        await trx.CommitAsync();
    }
}