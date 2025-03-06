using NZWalks.API.Models.DTOs.Domain;

namespace NZWalks.API.Repositories
{
    public interface IImageRepository
    {
        Task<Image> Upload(Image image);
    }
}
