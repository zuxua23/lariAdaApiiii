namespace InventoryControl.Service.Implementations;

using InventoryControl.Database;
using InventoryControl.DTO;
using InventoryControl.Entity;
using InventoryControl.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

public class StockInService : IStockInService
{
    private readonly AppDBContext _db;

    public StockInService(AppDBContext db)
    {
        _db = db;
    }

    //public async Task StockInAsync(StockInRequestDto dto, string user)
    //{
    //    var reader = await _db.Readers
    //        .FirstOrDefaultAsync(r => r.RdrId == dto.ReaderId);

    //    if (reader == null)
    //        throw new Exception("Reader tidak ditemukan");

    //    var tag = await _db.Tags
    //        .FirstOrDefaultAsync(t => t.EpcTag == dto.EpcTag);

    //    if (tag == null)
    //        throw new Exception("Tag tidak ditemukan");

    //    if (tag.Status == "IN_STOCK")
    //        throw new Exception("Tag sudah di stock");

    //    if (tag.Status == "OUT")
    //        throw new Exception("Tag sudah keluar");

    //    tag.Status = "IN_STOCK";
    //    tag.Curent_Location = reader.Location;
    //    tag.UpdatedBy = user;
    //    tag.UpdatedAt = DateTime.UtcNow;

    //    var transaction = new Transaction
    //    {
    //        TrsId = Guid.NewGuid().ToString(),
    //        TrsType = "STOCK_IN",
    //        ReferenceId = tag.TagId,
    //        RdrId = reader.RdrId,
    //        CreatedBy = user,
    //        CreatedAt = DateTime.UtcNow
    //    };

    //    _db.Transactions.Add(transaction);

    //    await _db.SaveChangesAsync();
    //}

    public async Task StockInAsync(StockInDto dto, string user)
    {
        using var trx = await _db.Database.BeginTransactionAsync();

        if (!dto.SelectedTagIds.Any())
            throw new Exception("Tidak ada tag yang dipilih");

        var selected = dto.SelectedTagIds.OrderBy(x => x);
        var scanned = dto.ScannedTagIds.OrderBy(x => x);

        if (!selected.SequenceEqual(scanned))
            throw new Exception("Semua tag harus discan sebelum save");

        var tags = await _db.Tags
            .Where(t => dto.SelectedTagIds.Contains(t.TagId))
            .ToListAsync();

        foreach (var tag in tags)
        {
            if (tag.Status != "STANDBY")
                throw new Exception($"Tag {tag.TagId} tidak dalam status STANDBY");

            tag.Status = "IN_STOCK";
            tag.UpdatedBy = user;
            tag.UpdatedAt = DateTime.UtcNow;
        }

        var trxHeader = new Transaction
        {
            TrsId = Guid.NewGuid().ToString(),
            TrsType = "STOCK_IN",
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
                Type = "STOCK_IN",
                Reference = trxHeader.TrsId,
                Action = "IN_STOCK",
                CreatedBy = user,
                CreatedAt = DateTime.UtcNow
            });
        }

        await _db.SaveChangesAsync();
        await trx.CommitAsync();
    }
}