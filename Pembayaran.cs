
namespace ShuttleGO.Models
{
    public class Pembayaran
    {
        public int Id { get; set; }
        public int BookingId { get; set; }
        public string Metode { get; set; } = string.Empty;
        public double Jumlah { get; set; }
        public string StatusPembayaran { get; set; } = "Belum Dibayar";
        public string? TanggalBayar { get; set; }
    }
}
