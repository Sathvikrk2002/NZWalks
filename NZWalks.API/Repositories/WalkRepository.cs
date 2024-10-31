using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDbContext dbContext;

        public WalkRepository(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<Walk> CreateWalkAsync(Walk addWalk)
        {
            await dbContext.AddAsync(addWalk);
            await dbContext.SaveChangesAsync();
            return addWalk;
        }

        public async Task<Walk?> DeleteWalkAsync(Guid id)
        {
            var walkDomainModel = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walkDomainModel == null)
            {
                return null;
            }
            dbContext.Walks.Remove(walkDomainModel);
            await dbContext.SaveChangesAsync();
            return walkDomainModel;
        }

        public async Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuary = null, string? sortBy = null, bool isAssending = true,
            int pageNumber = 1, int pageSize = 100)
        {
            var walks = dbContext.Walks.Include("Difficulty").Include("Region").AsQueryable();
            //Filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuary) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuary));
                }
            }
            //Sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if(sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAssending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = isAssending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }
            //Pagination
            var skipResult = (pageNumber - 1) * pageSize;
            return await walks.Skip(skipResult).Take(pageSize).ToListAsync();
        }

        public async Task<Walk?> GetWalkByIdAsync(Guid id)
        {
            return await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Walk> UpdateWalkAsync(Guid id, Walk walk)
        {
            var walkDomainModel = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (walkDomainModel == null)
            {
                return null;
            }

            walkDomainModel.Name = walk.Name;
            walkDomainModel.Description = walk.Description;
            walkDomainModel.LengthInKm = walk.LengthInKm;
            walkDomainModel.WalkImageUrl = walk.WalkImageUrl;
            walkDomainModel.DifficultyId = walk.DifficultyId;
            walkDomainModel.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();
            return walkDomainModel;
        }
    }
}
