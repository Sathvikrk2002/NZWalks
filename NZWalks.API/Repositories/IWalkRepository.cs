using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<List<Walk>> GetAllWalksAsync(string? filterOn = null, string? filterQuary = null, string? sortBy = null, bool isAssending = true, int pageNumber = 1,  int pageSize = 100);
        Task<Walk?> GetWalkByIdAsync(Guid id);
        Task<Walk> CreateWalkAsync(Walk addWalk);
        Task<Walk> UpdateWalkAsync(Guid id, Walk walk);
        Task<Walk?> DeleteWalkAsync(Guid id);
    }
}
