using NZWalks.API.Data;
using NZWalks.API.Models.DTOs.Domain;

namespace NZWalks.API.Repositories
{
    public class LocalImageRepository (IWebHostEnvironment env, 
                IHttpContextAccessor httpContextAccessor,
                NZWalksDbContext dbContext): IImageRepository
    {
        
        public async Task<Image> Upload(Image image)
        {

            var localFilePath = Path.Combine(env.ContentRootPath, "Images",
                    $"{image.FileName}{image.FileExtension}");

            // Upload image to local path in API Project/Images
            using var stream = new FileStream(localFilePath, FileMode.Create);
            await image.File.CopyToAsync(stream);

            var urlFilepath = $"{httpContextAccessor.HttpContext.Request.Scheme}://{httpContextAccessor.HttpContext.Request.Host}{httpContextAccessor.HttpContext.Request.PathBase}/Images/{image.FileName}{image.FileExtension}";

            image.FilePath = urlFilepath;

            // save to database
            await dbContext.Images.AddAsync(image);
            await dbContext.SaveChangesAsync();

            return image;



        }
    }
}
