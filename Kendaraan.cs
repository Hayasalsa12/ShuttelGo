
namespace ShuttleGO.Models
{
    public class Kendaraan
    {
        public int Id { get; set; }
        public string NamaKendaraan { get; set; } = string.Empty;
        public string PlatNomor { get; set; } = string.Empty;
        public int Kapasitas { get; set; } = 15;
    }
}
