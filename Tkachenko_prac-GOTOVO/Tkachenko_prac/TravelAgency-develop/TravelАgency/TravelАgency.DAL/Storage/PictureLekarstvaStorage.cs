using Microsoft.EntityFrameworkCore;
using TravelАgency.DAL.Interfaces;
using TravelАgency.Domain.ModelsDb;

namespace TravelАgency.DAL.Storage
{
    public class PictureLekarstvaStorage : IBaseStorage<PictureLekarstvaDb>
    {
        public readonly ApplicationDbContext _db;

        public PictureLekarstvaStorage(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task Add(PictureLekarstvaDb item)
        {
            await _db.AddAsync(item);
            await _db.SaveChangesAsync();
        }

        public async Task Delete(PictureLekarstvaDb item)
        {
            _db.Remove(item);
            await _db.SaveChangesAsync();
        }

        public async Task<PictureLekarstvaDb> Get(Guid id)
        {
            return await _db.PicturesLekarstvaDb.FirstOrDefaultAsync(x => x.Id == id);
        }

        public IQueryable<PictureLekarstvaDb> GetAll()
        {
            return _db.PicturesLekarstvaDb;
        }

        public async Task<PictureLekarstvaDb> Update(PictureLekarstvaDb item)
        {
            _db.PicturesLekarstvaDb.Update(item);
            await _db.SaveChangesAsync();

            return item;
        }
    }
}