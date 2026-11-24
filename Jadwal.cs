
namespace ShuttleGO.Models
{
    public class Jadwal
    {
        public int Id { get; set; }
        public int KendaraanId { get; set; }
        public int RuteId { get; set; }
        public int? DriverId { get; set; }
        public string TanggalBerangkat { get; set; } = string.Empty;
        public string JamBerangkat { get; set; } = string.Empty;
        public string Status { get; set; } = "Tersedia";
    }
}
