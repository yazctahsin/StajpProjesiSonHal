namespace StajProjesi.Models
{
    public enum TransferStatus
    {
        Created = 0,  // Talep oluşturuldu
        EnRoute = 1,  // Ekip yolda
        Arrived = 2,  // Hastaneye varıldı
        Completed = 3, // Nakil tamamlandı
        Canceled = 4  // İptal
    }
}