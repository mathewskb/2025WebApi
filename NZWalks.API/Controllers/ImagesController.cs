using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.DTOs;
using NZWalks.API.Models.DTOs.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController (IImageRepository imageRepository) : ControllerBase
    {
        [HttpPost("upload")]
        public async Task<IActionResult> Upload(ImageUploadRequestDto request)
        {
            ValidateFileUpload(request);

            if (ModelState.IsValid)
            {
                //convert dto to domain model

                var imageDomainModel = new Image
                {
                    File = request.File,
                    FileName = request.FileName,
                    FileDescription = request.FileDescription,
                    FileExtension = Path.GetExtension(request.File.FileName),
                    FileSizeInBytes = request.File.Length
                };


                // upload image via repository

                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }

            return BadRequest(ModelState);


        }

        private void ValidateFileUpload(ImageUploadRequestDto request)
        {
            var allowedExtensions = new string[] { ".jpg", ".jpeg", ".png" };

            if (!allowedExtensions.Contains(Path.GetExtension(request.File.FileName)))
            {
                ModelState.AddModelError("file", "Unsupported version");
            }

            if (request.File.Length > 10485760)
            {
                ModelState.AddModelError("file", "file size exceeded the limit");
            }
        }
    }
}
