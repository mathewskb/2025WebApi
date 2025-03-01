using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class AddRegionRequestDto
    {
        [Required]
        [MaxLength(3, ErrorMessage = "Code has to be a MaxLength of 3 characters")]
        [MinLength(3, ErrorMessage = "Code has to be a Minlength of 3 characters")]
        public string Code { get; set; }

        [Required]
        [MaxLength(100, ErrorMessage = "Minlength is 100 characters")]
        public string Name { get; set; }

        public string? RegionImageUrl { get; set; }
    }
}
