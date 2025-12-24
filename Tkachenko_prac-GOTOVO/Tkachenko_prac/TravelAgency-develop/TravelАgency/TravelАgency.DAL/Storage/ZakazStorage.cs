using Microsoft.EntityFrameworkCore;
using TravelАgency.DAL.Interfaces;
using TravelАgency.Domain.ModelsDb;

namespace TravelАgency.DAL.Storage
{
    public class ZakazStorage : IBaseStorage<ZakazDb>
    {
        public readonly ApplicationDbContext _db;

        public ZakazStorage(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Add(ZakazDb item)
        {
            await _db.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(ZakazDb item)
        {
            _db.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<ZakazDb> Get(Guid id)
        {
            return await _db.ZakazDb.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<ZakazDb> GetAll()
        {
            return _db.ZakazDb;
        }

        public async Task<ZakazDb> Update(ZakazDb item)
        {
            _db.ZakazDb.Update(item);
            await _db.SaveChangesAsync();

            return item;
        }
    }
}