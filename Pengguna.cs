using System;

namespace ShuttleGO.Models
{
    public abstract class Pengguna
    {
        public int Id { get; set; }
        public string Nama { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;

        public virtual void TampilkanInfo()
        {
            Console.WriteLine($"Nama  : {Nama}");
            Console.WriteLine($"Email : {Email}");
        }
    }
}