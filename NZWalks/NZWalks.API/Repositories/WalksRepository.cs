using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalksRepository : IWalksRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public WalksRepository(NZWalksDbContext nzWalksDbContex)
        {
            nZWalksDbContext = nzWalksDbContex;
        }

        public async Task<Walks> AddAsync(Walks walk)
        {
            walk.Id = Guid.NewGuid();
            await nZWalksDbContext.Walks.AddAsync(walk);
            await nZWalksDbContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walks> DeleteAsync(Guid id)
        {
            var walk2Del = await nZWalksDbContext.Walks.FindAsync(id);

            if (walk2Del == null)
            {
                return null;
            }

            // Delete the walk 
            nZWalksDbContext.Walks.Remove(walk2Del);
            await nZWalksDbContext.SaveChangesAsync();
            return walk2Del;
        }

        public async Task<IEnumerable<Walks>> GetAllAsync()
        {
            return await
                nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public async Task<Walks> GetAsync(Guid id)
        {
            return await nZWalksDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walks> UpdateAsync(Guid id, Walks walk)
        {
            // Find the requested Walk using a primary key
            var existingWalk = await nZWalksDbContext.Walks.FindAsync(id);

            // If null then not found
            if (existingWalk == null)
            {
                return null;
            }

            existingWalk.Name = walk.Name;
            existingWalk.Length = walk.Length;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;

            await nZWalksDbContext.SaveChangesAsync();

            return existingWalk;
        }
    }
}
