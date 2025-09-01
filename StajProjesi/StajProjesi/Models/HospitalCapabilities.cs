using System;

namespace StajProjesi.Models
{
    [Flags]
    public enum HospitalCapabilities : long
    {
        None = 0,
        Emergency = 1 << 0,         // Acil
        Trauma = 1 << 1,            // Travma
        WoundCare = 1 << 2,         // Yara bakımı
        BurnUnit = 1 << 3,          // Yanık
        Cardiology = 1 << 4,        // Kardiyoloji
        Neurology = 1 << 5,         // Nöroloji
        InfectiousDisease = 1 << 6, // Enfeksiyon
        Pediatrics = 1 << 7,        // Çocuk
        Maternity = 1 << 8,         // Kadın doğum
        Orthopedics = 1 << 9,       // Ortopedi
        Oncology = 1 << 10,         // Onkoloji
        Psychiatry = 1 << 11        // Psikiyatri
    }
}