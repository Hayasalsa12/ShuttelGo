
namespace ShuttleGO.Models
{
    public class Driver : Pengguna
    {
        public string NoHP { get; set; } = string.Empty;

        public override void TampilkanInfo()
        {
            Console.WriteLine("=== DATA DRIVER ===");
            base.TampilkanInfo();
            Console.WriteLine($"No. HP: {NoHP}");
        }
    }
}
