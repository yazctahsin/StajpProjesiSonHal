using System;
using System.ComponentModel.DataAnnotations;

namespace StajProjesi.Models
{
    public class Hospital
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Ad zorunludur.")]
        [StringLength(100, ErrorMessage = "Ad en fazla 100 karakter olabilir.")]
        public string Name { get; set; } = string.Empty;

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(200)]
        public string? Address { get; set; }

        [StringLength(30)]
        public string? Phone { get; set; }

        [Range(-90, 90, ErrorMessage = "Enlem -90 ile 90 arasında olmalıdır.")]
        public double? Latitude { get; set; }

        [Range(-180, 180, ErrorMessage = "Boylam -180 ile 180 arasında olmalıdır.")]
        public double? Longitude { get; set; }

        public HospitalCapabilities Capabilities { get; set; } = HospitalCapabilities.None;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}