using InventoryControl.Database;
using InventoryControl.DTO;
using InventoryControl.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace InventoryControl.Service.Implementations;

public class TransactionService : ITransactionService
{

    private readonly AppDBContext _db;

    public TransactionService(AppDBContext db)
    {
        _db = db;
    }


    public async Task<List<TransactionHistoryDto>> GetHistory(
    DateTime? fromDate,
    DateTime? toDate,
    string? txType)
    {
        var query =
            from t in _db.Transactions

            join td in _db.TransactionDetails on t.TrsId equals td.TrsId
            join tag in _db.Tags on td.TagId equals tag.Id
            join item in _db.Items on td.ItemId equals item.Id

            join reader in _db.Readers on t.ReaderId equals reader.Id into readerJoin
            from reader in readerJoin.DefaultIfEmpty()

            join d in _db.DOs on t.ReferenceId equals d.DoId into doJoin
            from d in doJoin.DefaultIfEmpty()

            join loc in _db.Locations on tag.LocationId equals loc.Id into locJoin
            from loc in locJoin.DefaultIfEmpty()

            select new TransactionHistoryDto
            {
                TxDate = t.CreatedAt,
                TxType = t.TrsType,

                DoNumber = d != null ? d.DoNumber : "-",           
                ReaderName = reader != null ? reader.Name : "-",

                TagId = tag.TagId,
                ItemName = item.Name,

                LocationName = loc != null ? loc.Name : "-"     
            };

        if (fromDate.HasValue)
            query = query.Where(x => x.TxDate >= fromDate);

        if (toDate.HasValue)
            query = query.Where(x => x.TxDate <= toDate);

        if (!string.IsNullOrEmpty(txType))
            query = query.Where(x => x.TxType == txType);

        return await query
            .OrderByDescending(x => x.TxDate)
            .ToListAsync();
    }
}
