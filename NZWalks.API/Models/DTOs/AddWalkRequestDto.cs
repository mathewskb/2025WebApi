using System.ComponentModel.DataAnnotations;

namespace NZWalks.API.Models.DTOs
{
    public class AddWalkRequestDto
    {
        [Required]
        [MaxLength(100, ErrorMessage ="Maximum of 100 characters")]
        public string Name { get; set; }
        [Required]
        [MaxLength(1000)]
        public string Description { get; set; }
        [Required]
        [Range(2, 100)]
        public double LengthInKm { get; set; }
        [Required]
        public string? WalkImageUrl { get; set; }
        [Required]
        public Guid RegionId { get; set; }
        [Required]
        public Guid DifficultyId { get; set; }

        
    }
}
