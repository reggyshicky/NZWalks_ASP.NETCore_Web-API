using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Migrations;
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



        public async Task<List<Walk>> GetAllAsync(string? filterOn, string? filterQuery = null,
            string? sortBy = null, bool isAscending = true)
        {
            var walks = _db.Walks.Include("Difficulty").Include("Region").AsQueryable();

            //filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }

            }

            //sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }
            return await walks.ToListAsync();
            //return await _db.Walks.Include(x=>x.Difficulty).Include(x=>x.Region).ToListAsync();
            //return await _db.Walks.Include("Difficulty").Include("Region").ToListAsync();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await _db.Walks.
                Include("Difficulty")
                .Include("Region")
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Walk?> UpdateWalkAsync(Guid id, Walk walk)
        {
            var existingwalk = await _db.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingwalk == null)
            {
                return null;
            }

            existingwalk.Name = walk.Name;
            existingwalk.Description = walk.Description;
            existingwalk.WalkImageUrl = walk.WalkImageUrl;
            existingwalk.RegionId = walk.RegionId;
            existingwalk.DifficultyId = walk.DifficultyId;
            existingwalk.LengthInKm = walk.LengthInKm;

            await _db.SaveChangesAsync();
            return existingwalk;
        }

        public async Task<Walk?> DeleteWalkAsync(Guid id)
        {
            var existingwalk = await _db.Walks.FirstOrDefaultAsync(x => x.Id == id);
            if (existingwalk == null)
            {
                return null;
            }
            _db.Walks.Remove(existingwalk);
            await _db.SaveChangesAsync();
            return existingwalk;

        }
    }
}
