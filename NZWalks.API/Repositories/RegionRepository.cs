using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;
        public RegionRepository(NZWalksDbContext dbContext)
        {
            this.nZWalksDbContext = dbContext;
        }

        public async Task<Region> CreateRegionAsync(Region region)
        {
            await nZWalksDbContext.AddAsync(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region?> DeleteRegionAsync( Guid id)
        {
            var regionDomainModel = await nZWalksDbContext.Region.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return null;
            }
            nZWalksDbContext.Region.Remove(regionDomainModel);
            await nZWalksDbContext.SaveChangesAsync();
            return regionDomainModel;
        }

        public async Task<List<Region>> GetAllRegionsAsync()
        {
            return await nZWalksDbContext.Region.ToListAsync();
        }

        public async Task<Region?> GetRegionByIdAsync(Guid id)
        {
            return await nZWalksDbContext.Region.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<Region?> UpdateRegionAsync(Guid id,  Region updateDto)
        {
            var regionDomainModel = await nZWalksDbContext.Region.FirstOrDefaultAsync(x => x.Id == id);

            if (regionDomainModel == null)
            {
                return null;
            }

            regionDomainModel.RegionImageUrl = updateDto.RegionImageUrl;
            regionDomainModel.Code = updateDto.Code;
            regionDomainModel.Name = updateDto.Name;

            await nZWalksDbContext.SaveChangesAsync();
            return regionDomainModel;
        }
    }
}
