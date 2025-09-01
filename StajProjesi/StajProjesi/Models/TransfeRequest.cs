using System;
using System.ComponentModel.DataAnnotations;

namespace StajProjesi.Models
{
    public class TransferRequest
    {
        public int Id { get; set; }

        [Required]
        public int HospitalId { get; set; }
        public Hospital? Hospital { get; set; }

        [Required(ErrorMessage = "Hasta adı zorunludur.")]
        [StringLength(100)]
        public string PatientName { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        public TransferStatus Status { get; set; } = TransferStatus.Created;

        // Talebi oluşturan kullanıcının Id'si (AspNetUsers Id)
        [StringLength(256)]
        public string? RequestedByUserId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}