
namespace ShuttleGO.Models
{
    public class Rute
    {
        public int Id { get; set; }
        public string Asal { get; set; } = string.Empty;
        public string Tujuan { get; set; } = string.Empty;
        public double Tarif { get; set; }
    }
}
