
namespace ShuttleGO.Models
{
    public class TitikJemput
    {
        public int Id { get; set; }
        public string NamaTitik { get; set; } = string.Empty;
        public string Alamat { get; set; } = string.Empty;
        public int RuteId { get; set; }
    }
}
