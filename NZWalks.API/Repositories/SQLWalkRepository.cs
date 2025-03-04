using Microsoft.AspNetCore.Hosting.StaticWebAssets;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Interfaces;
using NZWalks.API.Models.DTOs.Domain;

namespace NZWalks.API.Repositories
{
    public class SQLWalkRepository(NZWalksDbContext dbContext) : IWalkRepository
    {
        public async Task<Walk> CreateAsync(Walk walk)
        {
            await dbContext.Walks.AddAsync(walk);
            await dbContext.SaveChangesAsync();

            return walk;
        }


        public async Task<Walk?> DeleteAsync(Guid id)
        {
            var result = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (result == null) return null;

            dbContext.Walks.Remove(result);
            await dbContext.SaveChangesAsync();

            return result;
        }

        public async Task<IEnumerable<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
                        string? sortBy = null, bool IsAscending = true,
                        int pageNumber = 1, int pageSize = 1000)
        {
            // before 
            // return await dbContext.Walks.Include("Region").Include("Difficulty").ToListAsync();


            var walks = dbContext.Walks.Include("Region").Include("Difficulty").AsQueryable();

            // filtering
            if (string.IsNullOrWhiteSpace(filterOn) == false && string.IsNullOrWhiteSpace(filterQuery) == false)
            {
                if (filterOn.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Name.Contains(filterQuery));
                }
                else if (filterOn.Equals("Description", StringComparison.OrdinalIgnoreCase))
                {
                    walks = walks.Where(x => x.Description.Contains(filterQuery));
                }
            }


            // sorting
            if (string.IsNullOrWhiteSpace(sortBy) == false)
            {
                if (sortBy.Equals("Name", StringComparison.OrdinalIgnoreCase))
                {
                    walks = IsAscending ? walks.OrderBy(x => x.Name) : walks.OrderByDescending(x => x.Name);
                }
                else if (sortBy.Equals("Length", StringComparison.OrdinalIgnoreCase))
                {
                    walks = IsAscending ? walks.OrderBy(x => x.LengthInKm) : walks.OrderByDescending(x => x.LengthInKm);
                }
            }

            // pagination
            var skipresults = (pageNumber - 1) * pageSize;
            walks = walks.Skip(skipresults).Take(pageSize);

            return await walks.ToListAsync();
        }

        public Task<IEnumerable<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null, string? sortBy = null, bool? IsAscending = true)
        {
            throw new NotImplementedException();
        }

        public async Task<Walk?> GetByIdAsync(Guid id)
        {
            return await dbContext.Walks
                .Include("Region")
                .Include("Difficulty")
                .FirstOrDefaultAsync(x => x.Id == id);

        }

        public async Task<Walk?> UpdateAsync(Guid id, Walk walk)
        {
            var existingwalk = await dbContext.Walks.FirstOrDefaultAsync(x => x.Id == id);

            if (existingwalk == null) return null;

            existingwalk.Name = walk.Name;
            existingwalk.Description = walk.Description;
            existingwalk.LengthInKm = walk.LengthInKm;
            existingwalk.WalkImageUrl = walk.WalkImageUrl;
            existingwalk.DifficultyId = walk.DifficultyId;
            existingwalk.RegionId = walk.RegionId;

            await dbContext.SaveChangesAsync();

            return existingwalk;

        }
    }
}
