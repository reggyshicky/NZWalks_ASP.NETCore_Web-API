using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext _db;
        public SQLWalkRepository(NZWalksDbContext db)
        {
            this._db = db;
        }
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await _db.Walks.AddAsync(walk);
            await _db.SaveChangesAsync();
            return walk;

        }

        public async Task<List<Walk>> GetAllAsync()
        {
            //return await _db.Walks.Include(x=>x.Difficulty).Include(x=>x.Region).ToListAsync();
            return await _db.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _db.Walks.
                Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);

        }
    }
}
