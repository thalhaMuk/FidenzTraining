using System.ComponentModel.DataAnnotations;

namespace FidenzLevelOne.Models
{
    public class LocationModel
    {
        [Required]
        public decimal Latitude { get; set; }

        [Required]
        public decimal Longitude { get; set; }

    }
}
