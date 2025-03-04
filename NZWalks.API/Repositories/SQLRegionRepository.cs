using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Interfaces;
using NZWalks.API.Models.DTOs.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLRegionRepository (NZWalksDbContext  dbContext) : IRegionRepository
    {
        public async Task<Region> CreateAsync(Region region)
        {
            await dbContext.Regions.AddAsync(region);
            await dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<Region?> DeleteAsync(Guid id)
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (region == null) return null;

            dbContext.Regions.Remove(region);
            await dbContext.SaveChangesAsync();

            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await dbContext.Regions.ToListAsync();
        }

        public async Task<Region?> GetByIdAsync(Guid id)
        {
            var region = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            return region;
        }

        public async Task<Region?> UpdateAsync(Guid id, Region region)
        {
            var exisitingregion = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            if (exisitingregion == null) return null;

            exisitingregion.Name = region.Name;
            exisitingregion.Code =  region.Code;
            exisitingregion.RegionImageUrl = region.RegionImageUrl;

            await dbContext.SaveChangesAsync();

            return exisitingregion;
               
        }
    }
}
