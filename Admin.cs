
namespace ShuttleGO.Models
{
    public class Admin : Pengguna
    {
        public override void TampilkanInfo()
        {
            Console.WriteLine("=== DATA ADMIN ===");
            base.TampilkanInfo();
        }
    }
}
