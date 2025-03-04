﻿using NZWalks.API.Models.DTOs.Domain;

namespace NZWalks.API.Interfaces
{
    public interface IWalkRepository
    {
        Task<Walk> CreateAsync(Walk walk);
        Task<IEnumerable<Walk>> GetAllAsync(string? filterOn = null, string? filterQuery = null,
                                string? sortBy = null, bool IsAscending = true,
                                int pageNumber = 1, int pageSize = 1000);

        Task<Walk?> GetByIdAsync(Guid id);

        Task<Walk?> UpdateAsync(Guid id, Walk walk);
        Task<Walk?> DeleteAsync(Guid id);
    }
}
