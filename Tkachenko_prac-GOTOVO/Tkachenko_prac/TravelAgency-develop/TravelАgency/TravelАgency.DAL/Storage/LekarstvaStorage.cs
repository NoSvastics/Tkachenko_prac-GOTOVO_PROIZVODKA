using Microsoft.EntityFrameworkCore;
using TravelАgency.DAL.Interfaces;
using TravelАgency.Domain.ModelsDb;

namespace TravelАgency.DAL.Storage
{
    public class LekarstvaStorage : IBaseStorage<LekarstvaDb>
    {
        public readonly ApplicationDbContext _db;

        public LekarstvaStorage(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Add(LekarstvaDb item)
        {
            await _db.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(LekarstvaDb item)
        {
            _db.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<LekarstvaDb> Get(Guid id)
        {
            return await _db.LekarstvaDb.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<LekarstvaDb> GetAll()
        {
            return _db.LekarstvaDb;
        }

        public async Task<LekarstvaDb> Update(LekarstvaDb item)
        {
            _db.LekarstvaDb.Update(item);
            await _db.SaveChangesAsync();

            return item;
        }
    }
}