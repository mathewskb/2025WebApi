using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class UpdateRegionRequestDto
    {
        [Required]
        [MaxLength(3, ErrorMessage = "Code has to be a MaxLength of 3 characters")]
        [MinLength(3, ErrorMessage = "Code has to be a Minlength of 3 characters")]
        public required string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Minlength is 100 characters")]
        public required string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
