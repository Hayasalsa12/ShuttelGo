
namespace ShuttleGO.Models
{
    public class Penumpang : Pengguna
    {
        public string NoHP { get; set; } = string.Empty;

        public override void TampilkanInfo()
        {
            Console.WriteLine("=== DATA PENUMPANG ===");
            base.TampilkanInfo();
            Console.WriteLine($"No. HP: {NoHP}");
        }
    }
}
