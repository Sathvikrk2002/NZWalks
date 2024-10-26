using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;

namespace NZWalks.API.Repositories
{
    public interface IRegionRepository
    {
        Task<List<Region>> GetAllRegionsAsync();
        Task<Region?> GetRegionByIdAsync(Guid id);
        Task<Region> CreateRegionAsync(Region addRegionRequestDto);
        Task<Region> UpdateRegionAsync(Guid id, Region updateDto);
        Task<Region?> DeleteRegionAsync(Guid id);
    }
}
