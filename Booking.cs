
namespace ShuttleGO.Models
{
    public class Booking
    {
        public int Id { get; set; }

        public string KodeBooking { get; set; } = string.Empty;

        public int PenumpangId { get; set; }

        public int JadwalId { get; set; }

        public int TitikJemputId { get; set; }

        public int NomorKursi { get; set; }

        public double TotalBiaya { get; set; }

        public string StatusPemesanan { get; set; } = "Menunggu";

        public string TanggalPesan { get; set; } = string.Empty;
    }
}
