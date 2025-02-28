using AutoMapper;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Models.DTOs.Domain;

namespace NZWalks.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // regions 
            CreateMap<Region, RegionDto>().ReverseMap();
            CreateMap<AddRegionRequestDto, Region>().ReverseMap();
            CreateMap<UpdateRegionRequestDto, Region>().ReverseMap();

            // difficulty
            CreateMap<Difficulty, DifficultyDto>().ReverseMap();

            // walks
            CreateMap<AddWalkRequestDto, Walk>().ReverseMap();
            CreateMap<Walk, WalkDto>().ReverseMap();
            // CreateMap<UpdateWalkRequestDto, WalkDto>().ReverseMap();
            CreateMap<UpdateWalkRequestDto, Walk>().ReverseMap();


        }
    }
}
