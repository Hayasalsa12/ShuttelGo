using System;
using System.Linq;
using ShuttleGO.Models;
using ShuttleGO.Repositories;

namespace ShuttleGO
{
    class Program
    {
        static PenumpangRepository repository = new PenumpangRepository();
        static AdminRepository adminRepository = new AdminRepository();
        static DriverRepository driverRepository = new DriverRepository();
        static KendaraanRepository kendaraanRepository = new KendaraanRepository();
        static RuteRepository ruteRepository = new RuteRepository();
        static TitikJemputRepository titikRepository = new TitikJemputRepository();
        static JadwalRepository jadwalRepository = new JadwalRepository();
        static BookingRepository bookingRepository = new BookingRepository();
        static PembayaranRepository pembayaranRepository = new PembayaranRepository();



        static void Main(string[] args)
        {
            // MENYIAPKAN DATABASE
            try
            {
                DatabaseHelper.InitializeDatabase();

                Console.WriteLine("Database berhasil disiapkan!");

                // MEMERIKSA AKUN DRIVER
                var driver = driverRepository.GetByEmail("erlin@gmail");

                // MEMBUAT AKUN DRIVER JIKA BELUM ADA
                if (driver == null)
                {
                    Driver driverBaru = new Driver
                    {
                        Nama = "Erlin",
                        NoHP = "12345",
                        Email = "erlin@gmail",
                        Password = "erlin123"
                    };

                    driverRepository.Tambah(driverBaru);

                    Console.WriteLine("Akun Driver berhasil dibuat!");
                }
                else if (string.IsNullOrWhiteSpace(driver.Password))
                {
                    // MENGISI PASSWORD JIKA MASIH KOSONG
                    driverRepository.PerbaruiPassword(
                        "erlin@gmail",
                        "erlin123"
                    );

                    Console.WriteLine("Password Driver berhasil diperbarui!");
                }
                else
                {
                    Console.WriteLine("Akun Driver sudah tersedia.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal menyiapkan database!");
                Console.WriteLine(ex.Message);
                Console.WriteLine("\nTekan Enter untuk keluar...");
                Console.ReadLine();
                return;
            }

            // MENU UTAMA SHUTTLE GO
            bool berjalan = true;

            while (berjalan)
            {
                Console.Clear();

                Console.WriteLine("==================================");
                Console.WriteLine("             SHUTTLE GO");
                Console.WriteLine("       YOUR JOURNEY, MADE EASIER");
                Console.WriteLine("==================================");
                Console.WriteLine("1. Registrasi Pengguna");
                Console.WriteLine("2. Login Pengguna");
                Console.WriteLine("3. Login Admin");
                Console.WriteLine("4. Login Driver");
                Console.WriteLine("5. Keluar");
                Console.WriteLine("==================================");

                string pilihan = Input("Pilih menu: ");

                switch (pilihan)
                {
                    case "1":
                        RegistrasiPengguna();
                        break;

                    case "2":
                        LoginPengguna();
                        break;

                    case "3":
                        LoginAdmin();
                        break;

                    case "4":
                        LoginDriver();
                        break;

                    case "5":
                        berjalan = false;
                        Console.WriteLine(
                            "Terima kasih telah menggunakan Shuttle GO!"
                        );
                        break;

                    default:
                        Console.WriteLine("Pilihan tidak tersedia!");
                        Jeda();
                        break;
                }
            }
        }


        // =========================================
        // INPUT DAN BANTUAN
        // =========================================

        static string Input(string pesan)
        {
            Console.Write(pesan);
            return Console.ReadLine() ?? "";
        }

        static int InputInt(string pesan)
        {
            while (true)
            {
                if (int.TryParse(Input(pesan), out int nilai))
                    return nilai;

                Console.WriteLine("Masukkan angka yang valid!");
            }
        }

        static double InputDouble(string pesan)
        {
            while (true)
            {
                if (double.TryParse(Input(pesan), out double nilai))
                    return nilai;

                Console.WriteLine("Masukkan angka yang valid!");
            }
        }

        static bool Konfirmasi()
        {
            return Input("Yakin? (y/n): ").Trim().ToLower() == "y";
        }

        static void Jeda()
        {
            Console.WriteLine("\nTekan Enter untuk melanjutkan...");
            Console.ReadLine();
        }

        static void Garis()
        {
            Console.WriteLine("----------------------------------");
        }

        // =========================================
        // REGISTRASI DAN LOGIN
        // =========================================

        static void RegistrasiPengguna()
        {
            Console.Clear();
            Console.WriteLine("=== REGISTRASI PENGGUNA ===");

            var penumpang = new Penumpang
            {
                Nama = Input("Nama lengkap : "),
                NoHP = Input("Nomor HP     : "),
                Email = Input("Email        : "),
                Password = Input("Password     : ")
            };

            if (string.IsNullOrWhiteSpace(penumpang.Nama) ||
                string.IsNullOrWhiteSpace(penumpang.NoHP) ||
                string.IsNullOrWhiteSpace(penumpang.Email) ||
                string.IsNullOrWhiteSpace(penumpang.Password))
            {
                Console.WriteLine("Semua data wajib diisi!");
                Jeda();
                return;
            }

            try
            {
                if (repository.GetByEmail(penumpang.Email) != null)
                {
                    Console.WriteLine("Email sudah terdaftar!");
                }
                else
                {
                    repository.Tambah(penumpang);
                    Console.WriteLine("Registrasi berhasil!");
                    Console.WriteLine("Silakan login menggunakan akun tersebut.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Registrasi gagal: " + ex.Message);
            }

            Jeda();
        }

        static void LoginPengguna()
        {
            Console.Clear();
            Console.WriteLine("=== LOGIN PENGGUNA ===");

            string email = Input("Email    : ");
            string password = Input("Password : ");

            try
            {
                var pengguna = repository.GetByEmail(email);

                if (pengguna != null && pengguna.Password == password)
                {
                    Console.WriteLine("Login berhasil!");
                    Console.WriteLine("Selamat datang, " + pengguna.Nama);
                    Jeda();
                    MenuPengguna(pengguna);
                }
                else
                {
                    Console.WriteLine("Email atau password salah!");
                    Jeda();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login gagal: " + ex.Message);
                Jeda();
            }
        }

        static void LoginAdmin()
        {
            Console.Clear();
            Console.WriteLine("=== LOGIN ADMIN ===");

            string email = Input("Email    : ");
            string password = Input("Password : ");

            try
            {
                var admin = adminRepository.GetByEmail(email);

                if (admin != null && admin.Password == password)
                {
                    Console.WriteLine("Login berhasil!");
                    Console.WriteLine("Selamat datang, " + admin.Nama);
                    Jeda();
                    MenuAdmin();
                }
                else
                {
                    Console.WriteLine("Email atau password salah!");
                    Jeda();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login gagal: " + ex.Message);
                Jeda();
            }
        }

        static void LoginDriver()
        {
            Console.Clear();
            Console.WriteLine("=== LOGIN DRIVER ===");

            string email = Input("Email    : ");
            string password = Input("Password : ");

            try
            {
                var driver = driverRepository.GetByEmail(email);

                if (driver != null && driver.Password == password)
                {
                    Console.WriteLine("Login berhasil!");
                    Console.WriteLine("Selamat datang, " + driver.Nama);
                    Jeda();
                    MenuDriver(driver);
                }
                else
                {
                    Console.WriteLine("Email atau password salah!");
                    Jeda();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Login gagal: " + ex.Message);
                Jeda();
            }
        }

        // =========================================
        // MENU ADMIN
        // =========================================

        static void MenuAdmin()
        {
            bool kembali = false;

            while (!kembali)
            {
                Console.Clear();
                Console.WriteLine("========== MENU ADMIN ==========");
                Console.WriteLine("1. Kelola Penumpang");
                Console.WriteLine("2. Kelola Kendaraan");
                Console.WriteLine("3. Kelola Rute");
                Console.WriteLine("4. Kelola Titik Jemput");
                Console.WriteLine("5. Kelola Jadwal");
                Console.WriteLine("6. Kelola Booking");
                Console.WriteLine("7. Kelola Pembayaran");
                Console.WriteLine("8. Logout");

                switch (Input("Pilih menu: "))
                {
                    case "1": MenuPenumpang(); break;
                    case "2": MenuKendaraan(); break;
                    case "3": MenuRute(); break;
                    case "4": MenuTitikJemput(); break;
                    case "5": MenuJadwal(); break;
                    case "6": MenuBooking(); break;
                    case "7": MenuPembayaran(); break;
                    case "8": kembali = true; break;
                    default: Console.WriteLine("Pilihan tidak tersedia!"); Jeda(); break;
                }
            }
        }

        // =========================================
        // KELOLA PENUMPANG
        // =========================================

        static void MenuPenumpang()
        {
            bool kembali = false;

            while (!kembali)
            {
                Console.Clear();
                Console.WriteLine("=== KELOLA PENUMPANG ===");
                Console.WriteLine("1. Tambah");
                Console.WriteLine("2. Lihat");
                Console.WriteLine("3. Ubah");
                Console.WriteLine("4. Hapus");
                Console.WriteLine("5. Kembali");

                switch (Input("Pilih: "))
                {
                    case "1": TambahPenumpang(); break;
                    case "2": LihatPenumpang(); break;
                    case "3": UbahPenumpang(); break;
                    case "4": HapusPenumpang(); break;
                    case "5": kembali = true; break;
                    default: Console.WriteLine("Pilihan tidak tersedia!"); Jeda(); break;
                }
            }
        }

        static void LihatPenumpang()
        {
            Console.Clear();
            Console.WriteLine("=== DATA PENUMPANG ===");

            try
            {
                var daftar = repository.GetAll();

                if (daftar.Count == 0)
                    Console.WriteLine("Belum ada data penumpang.");

                foreach (var p in daftar)
                {
                    Garis();
                    Console.WriteLine("ID    : " + p.Id);
                    Console.WriteLine("Nama  : " + p.Nama);
                    Console.WriteLine("No HP : " + p.NoHP);
                    Console.WriteLine("Email : " + p.Email);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal mengambil data: " + ex.Message);
            }

            Jeda();
        }

        static void TambahPenumpang()
        {
            Console.Clear();
            Console.WriteLine("=== TAMBAH PENUMPANG ===");

            var p = new Penumpang
            {
                Nama = Input("Nama     : "),
                NoHP = Input("No HP    : "),
                Email = Input("Email    : "),
                Password = Input("Password : ")
            };

            if (string.IsNullOrWhiteSpace(p.Nama) ||
                string.IsNullOrWhiteSpace(p.NoHP) ||
                string.IsNullOrWhiteSpace(p.Email) ||
                string.IsNullOrWhiteSpace(p.Password))
            {
                Console.WriteLine("Semua data wajib diisi!");
            }
            else
            {
                try
                {
                    repository.Tambah(p);
                    Console.WriteLine("Penumpang berhasil ditambahkan!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Gagal menambahkan: " + ex.Message);
                }
            }

            Jeda();
        }

        static void UbahPenumpang()
        {
            Console.Clear();
            LihatPenumpangTanpaJeda();

            int id = InputInt("ID penumpang: ");

            try
            {
                var p = repository.GetById(id);

                if (p == null)
                {
                    Console.WriteLine("Penumpang tidak ditemukan!");
                    Jeda();
                    return;
                }

                p.Nama = Input("Nama baru     : ");
                p.NoHP = Input("No HP baru    : ");
                p.Email = Input("Email baru    : ");

                string password = Input("Password baru (kosong jika tidak diubah): ");

                if (!string.IsNullOrWhiteSpace(password))
                    p.Password = password;

                repository.Update(p);
                Console.WriteLine("Data penumpang berhasil diubah!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal mengubah data: " + ex.Message);
            }

            Jeda();
        }

        static void LihatPenumpangTanpaJeda()
        {
            var daftar = repository.GetAll();

            foreach (var p in daftar)
                Console.WriteLine($"{p.Id}. {p.Nama} - {p.Email}");
        }

        static void HapusPenumpang()
        {
            Console.Clear();
            LihatPenumpangTanpaJeda();

            int id = InputInt("\nID penumpang: ");

            if (!Konfirmasi())
            {
                Console.WriteLine("Penghapusan dibatalkan.");
                Jeda();
                return;
            }

            try
            {
                repository.Hapus(id);
                Console.WriteLine("Perintah penghapusan selesai.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal menghapus. Data mungkin masih digunakan oleh booking.");
                Console.WriteLine(ex.Message);
            }

            Jeda();
        }

        // =========================================
        // KELOLA KENDARAAN
        // =========================================

        static void MenuKendaraan()
        {
            bool kembali = false;

            while (!kembali)
            {
                Console.Clear();
                Console.WriteLine("=== KELOLA KENDARAAN ===");
                Console.WriteLine("1. Tambah");
                Console.WriteLine("2. Lihat");
                Console.WriteLine("3. Ubah");
                Console.WriteLine("4. Hapus");
                Console.WriteLine("5. Kembali");

                switch (Input("Pilih: "))
                {
                    case "1": TambahKendaraan(); break;
                    case "2": LihatKendaraan(); break;
                    case "3": UbahKendaraan(); break;
                    case "4": HapusKendaraan(); break;
                    case "5": kembali = true; break;
                    default: Console.WriteLine("Pilihan tidak tersedia!"); Jeda(); break;
                }
            }
        }

        static void LihatKendaraan()
        {
            Console.Clear();
            Console.WriteLine("=== DATA KENDARAAN ===");

            try
            {
                var daftar = kendaraanRepository.GetAll();

                if (daftar.Count == 0)
                    Console.WriteLine("Belum ada kendaraan.");

                foreach (var k in daftar)
                {
                    Garis();
                    Console.WriteLine("ID       : " + k.Id);
                    Console.WriteLine("Nama     : " + k.NamaKendaraan);
                    Console.WriteLine("Plat     : " + k.PlatNomor);
                    Console.WriteLine("Kapasitas: " + k.Kapasitas + " kursi");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Jeda();
        }

        static void TambahKendaraan()
        {
            Console.Clear();
            Console.WriteLine("=== TAMBAH KENDARAAN ===");

            var k = new Kendaraan
            {
                NamaKendaraan = Input("Nama kendaraan: "),
                PlatNomor = Input("Plat nomor    : "),
                Kapasitas = 15
            };

            if (string.IsNullOrWhiteSpace(k.NamaKendaraan) ||
                string.IsNullOrWhiteSpace(k.PlatNomor))
            {
                Console.WriteLine("Data wajib diisi!");
            }
            else
            {
                try
                {
                    kendaraanRepository.Tambah(k);
                    Console.WriteLine("Kendaraan berhasil ditambahkan!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Gagal menambahkan: " + ex.Message);
                }
            }

            Jeda();
        }

        static void UbahKendaraan()
        {
            Console.Clear();
            TampilkanKendaraanTanpaJeda();

            int id = InputInt("\nID kendaraan: ");

            try
            {
                var k = kendaraanRepository.GetAll().FirstOrDefault(x => x.Id == id);

                if (k == null)
                {
                    Console.WriteLine("Kendaraan tidak ditemukan!");
                    Jeda();
                    return;
                }

                k.NamaKendaraan = Input("Nama baru: ");
                k.PlatNomor = Input("Plat baru: ");
                k.Kapasitas = 15;

                kendaraanRepository.Update(k);
                Console.WriteLine("Kendaraan berhasil diubah!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal mengubah: " + ex.Message);
            }

            Jeda();
        }

        static void TampilkanKendaraanTanpaJeda()
        {
            foreach (var k in kendaraanRepository.GetAll())
                Console.WriteLine($"{k.Id}. {k.NamaKendaraan} - {k.PlatNomor}");
        }

        static void HapusKendaraan()
        {
            Console.Clear();
            TampilkanKendaraanTanpaJeda();

            int id = InputInt("\nID kendaraan: ");

            if (!Konfirmasi())
            {
                Console.WriteLine("Penghapusan dibatalkan.");
                Jeda();
                return;
            }

            try
            {
                kendaraanRepository.Hapus(id);
                Console.WriteLine("Perintah penghapusan selesai.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal menghapus. Kendaraan mungkin masih digunakan oleh jadwal.");
                Console.WriteLine(ex.Message);
            }

            Jeda();
        }

        // =========================================
        // KELOLA RUTE
        // =========================================

        static void MenuRute()
        {
            bool kembali = false;

            while (!kembali)
            {
                Console.Clear();
                Console.WriteLine("=== KELOLA RUTE ===");
                Console.WriteLine("1. Tambah");
                Console.WriteLine("2. Lihat");
                Console.WriteLine("3. Ubah");
                Console.WriteLine("4. Hapus");
                Console.WriteLine("5. Kembali");

                switch (Input("Pilih: "))
                {
                    case "1": TambahRute(); break;
                    case "2": LihatRute(); break;
                    case "3": UbahRute(); break;
                    case "4": HapusRute(); break;
                    case "5": kembali = true; break;
                    default: Console.WriteLine("Pilihan tidak tersedia!"); Jeda(); break;
                }
            }
        }

        static void LihatRute()
        {
            Console.Clear();
            Console.WriteLine("=== DATA RUTE ===");

            foreach (var r in ruteRepository.GetAll())
            {
                Garis();
                Console.WriteLine($"ID     : {r.Id}");
                Console.WriteLine($"Asal   : {r.Asal}");
                Console.WriteLine($"Tujuan : {r.Tujuan}");
                Console.WriteLine($"Tarif  : Rp {r.Tarif:N0}");
            }

            Jeda();
        }

        static void TambahRute()
        {
            Console.Clear();
            Console.WriteLine("=== TAMBAH RUTE ===");

            var r = new Rute
            {
                Asal = Input("Kota asal   : "),
                Tujuan = Input("Kota tujuan : "),
                Tarif = InputDouble("Tarif       : Rp ")
            };

            if (string.IsNullOrWhiteSpace(r.Asal) ||
                string.IsNullOrWhiteSpace(r.Tujuan) ||
                r.Tarif <= 0)
            {
                Console.WriteLine("Data rute tidak valid!");
            }
            else
            {
                try
                {
                    ruteRepository.Tambah(r);
                    Console.WriteLine("Rute berhasil ditambahkan!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Jeda();
        }

        static void UbahRute()
        {
            Console.Clear();
            TampilkanRuteTanpaJeda();

            int id = InputInt("\nID rute: ");

            try
            {
                var r = ruteRepository.GetAll().FirstOrDefault(x => x.Id == id);

                if (r == null)
                {
                    Console.WriteLine("Rute tidak ditemukan!");
                    Jeda();
                    return;
                }

                r.Asal = Input("Asal baru   : ");
                r.Tujuan = Input("Tujuan baru : ");
                r.Tarif = InputDouble("Tarif baru  : Rp ");

                ruteRepository.Update(r);
                Console.WriteLine("Rute berhasil diubah!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal mengubah rute: " + ex.Message);
            }

            Jeda();
        }

        static void TampilkanRuteTanpaJeda()
        {
            foreach (var r in ruteRepository.GetAll())
                Console.WriteLine($"{r.Id}. {r.Asal} - {r.Tujuan} | Rp {r.Tarif:N0}");
        }

        static void HapusRute()
        {
            Console.Clear();
            TampilkanRuteTanpaJeda();

            int id = InputInt("\nID rute: ");

            if (!Konfirmasi())
            {
                Console.WriteLine("Penghapusan dibatalkan.");
                Jeda();
                return;
            }

            try
            {
                ruteRepository.Hapus(id);
                Console.WriteLine("Perintah penghapusan selesai.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal menghapus. Rute mungkin masih digunakan oleh jadwal atau titik jemput.");
                Console.WriteLine(ex.Message);
            }

            Jeda();
        }

        // =========================================
        // KELOLA TITIK JEMPUT
        // =========================================

        static void MenuTitikJemput()
        {
            bool kembali = false;

            while (!kembali)
            {
                Console.Clear();
                Console.WriteLine("=== KELOLA TITIK JEMPUT ===");
                Console.WriteLine("1. Tambah titik jemput");
                Console.WriteLine("2. Lihat titik jemput");
                Console.WriteLine("3. Kembali");

                switch (Input("Pilih: "))
                {
                    case "1": TambahTitikJemput(); break;
                    case "2": LihatTitikJemput(); break;
                    case "3": kembali = true; break;
                    default: Console.WriteLine("Pilihan tidak tersedia!"); Jeda(); break;
                }
            }
        }

        static void TambahTitikJemput()
        {
            Console.Clear();
            Console.WriteLine("=== TAMBAH TITIK JEMPUT ===");
            TampilkanRuteTanpaJeda();

            int ruteId = InputInt("\nID rute: ");

            if (!ruteRepository.GetAll().Any(r => r.Id == ruteId))
            {
                Console.WriteLine("Rute tidak ditemukan!");
                Jeda();
                return;
            }

            var titik = new TitikJemput
            {
                NamaTitik = Input("Nama titik jemput: "),
                Alamat = Input("Alamat            : "),
                RuteId = ruteId
            };

            if (string.IsNullOrWhiteSpace(titik.NamaTitik) ||
                string.IsNullOrWhiteSpace(titik.Alamat))
            {
                Console.WriteLine("Nama titik dan alamat wajib diisi!");
            }
            else
            {
                try
                {
                    titikRepository.Tambah(titik);
                    Console.WriteLine("Titik jemput berhasil ditambahkan!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Jeda();
        }

        static void LihatTitikJemput()
        {
            Console.Clear();
            Console.WriteLine("=== DATA TITIK JEMPUT ===");

            var daftarRute = ruteRepository.GetAll();
            var daftarTitik = titikRepository.GetAll();

            if (daftarTitik.Count == 0)
                Console.WriteLine("Belum ada titik jemput.");

            foreach (var t in daftarTitik)
            {
                var r = daftarRute.FirstOrDefault(x => x.Id == t.RuteId);

                Garis();
                Console.WriteLine("ID      : " + t.Id);
                Console.WriteLine("Nama    : " + t.NamaTitik);
                Console.WriteLine("Alamat  : " + t.Alamat);
                Console.WriteLine("Rute    : " + (r == null ? "-" : r.Asal + " - " + r.Tujuan));
            }

            Jeda();
        }

        // =========================================
        // KELOLA JADWAL
        // =========================================

        static void MenuJadwal()
        {
            bool kembali = false;

            while (!kembali)
            {
                Console.Clear();
                Console.WriteLine("=== KELOLA JADWAL ===");
                Console.WriteLine("1. Tambah");
                Console.WriteLine("2. Lihat");
                Console.WriteLine("3. Ubah");
                Console.WriteLine("4. Hapus");
                Console.WriteLine("5. Kembali");

                switch (Input("Pilih: "))
                {
                    case "1": TambahJadwal(); break;
                    case "2": LihatJadwalAdmin(); break;
                    case "3": UbahJadwal(); break;
                    case "4": HapusJadwal(); break;
                    case "5": kembali = true; break;
                    default: Console.WriteLine("Pilihan tidak tersedia!"); Jeda(); break;
                }
            }
        }

        static void TambahJadwal()
        {
            Console.Clear();
            Console.WriteLine("=== TAMBAH JADWAL ===");

            TampilkanKendaraanTanpaJeda();
            int kendaraanId = InputInt("\nID kendaraan: ");

            TampilkanRuteTanpaJeda();
            int ruteId = InputInt("ID rute: ");

            if (!kendaraanRepository.GetAll().Any(k => k.Id == kendaraanId) ||
                !ruteRepository.GetAll().Any(r => r.Id == ruteId))
            {
                Console.WriteLine("Kendaraan atau rute tidak ditemukan!");
                Jeda();
                return;
            }

            Console.WriteLine("\nDAFTAR DRIVER:");
            foreach (var d in driverRepository.GetAll())
                Console.WriteLine($"{d.Id}. {d.Nama}");

            string inputDriver = Input("ID driver (kosong jika belum ditentukan): ");
            int? driverId = null;

            if (!string.IsNullOrWhiteSpace(inputDriver))
            {
                if (!int.TryParse(inputDriver, out int parsed) ||
                    !driverRepository.GetAll().Any(d => d.Id == parsed))
                {
                    Console.WriteLine("Driver tidak ditemukan!");
                    Jeda();
                    return;
                }

                driverId = parsed;
            }

            var jadwal = new Jadwal
            {
                KendaraanId = kendaraanId,
                RuteId = ruteId,
                DriverId = driverId,
                TanggalBerangkat = Input("Tanggal (YYYY-MM-DD): "),
                JamBerangkat = Input("Jam (HH:MM): "),
                Status = "Tersedia"
            };

            if (string.IsNullOrWhiteSpace(jadwal.TanggalBerangkat) ||
                string.IsNullOrWhiteSpace(jadwal.JamBerangkat))
            {
                Console.WriteLine("Tanggal dan jam wajib diisi!");
            }
            else
            {
                try
                {
                    jadwalRepository.Tambah(jadwal);
                    Console.WriteLine("Jadwal berhasil ditambahkan!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Gagal menambahkan jadwal: " + ex.Message);
                }
            }

            Jeda();
        }

        static void LihatJadwalAdmin()
        {
            Console.Clear();
            TampilkanJadwalUntukPengguna();
            Jeda();
        }

        static void UbahJadwal()
        {
            Console.Clear();
            TampilkanJadwalUntukPengguna();

            int id = InputInt("\nID jadwal: ");

            try
            {
                var j = jadwalRepository.GetAll().FirstOrDefault(x => x.Id == id);

                if (j == null)
                {
                    Console.WriteLine("Jadwal tidak ditemukan!");
                    Jeda();
                    return;
                }

                TampilkanKendaraanTanpaJeda();
                int kendaraanId = InputInt("ID kendaraan baru: ");

                TampilkanRuteTanpaJeda();
                int ruteId = InputInt("ID rute baru: ");

                if (!kendaraanRepository.GetAll().Any(k => k.Id == kendaraanId) ||
                    !ruteRepository.GetAll().Any(r => r.Id == ruteId))
                {
                    Console.WriteLine("Kendaraan atau rute tidak ditemukan!");
                    Jeda();
                    return;
                }

                j.KendaraanId = kendaraanId;
                j.RuteId = ruteId;
                j.TanggalBerangkat = Input("Tanggal baru (YYYY-MM-DD): ");
                j.JamBerangkat = Input("Jam baru (HH:MM): ");
                j.Status = Input("Status jadwal: ");

                jadwalRepository.Update(j);
                Console.WriteLine("Jadwal berhasil diubah!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal mengubah jadwal: " + ex.Message);
            }

            Jeda();
        }

        static void HapusJadwal()
        {
            Console.Clear();
            TampilkanJadwalUntukPengguna();

            int id = InputInt("\nID jadwal: ");

            if (!Konfirmasi())
            {
                Console.WriteLine("Penghapusan dibatalkan.");
                Jeda();
                return;
            }

            try
            {
                jadwalRepository.Hapus(id);
                Console.WriteLine("Perintah penghapusan selesai.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal menghapus. Jadwal mungkin sudah memiliki booking.");
                Console.WriteLine(ex.Message);
            }

            Jeda();
        }

        static void TampilkanJadwalUntukPengguna()
        {
            var jadwalList = jadwalRepository.GetAll();
            var ruteList = ruteRepository.GetAll();
            var kendaraanList = kendaraanRepository.GetAll();
            var driverList = driverRepository.GetAll();

            if (jadwalList.Count == 0)
            {
                Console.WriteLine("Belum ada jadwal keberangkatan.");
                return;
            }

            foreach (var j in jadwalList)
            {
                var r = ruteList.FirstOrDefault(x => x.Id == j.RuteId);
                var k = kendaraanList.FirstOrDefault(x => x.Id == j.KendaraanId);
                var d = j.DriverId.HasValue
                    ? driverList.FirstOrDefault(x => x.Id == j.DriverId.Value)
                    : null;

                Garis();
                Console.WriteLine("ID Jadwal : " + j.Id);
                Console.WriteLine("Rute      : " + (r == null ? "-" : r.Asal + " - " + r.Tujuan));
                Console.WriteLine("Tarif     : Rp " + (r == null ? 0 : r.Tarif).ToString("N0"));
                Console.WriteLine("Kendaraan : " + (k == null ? "-" : k.NamaKendaraan));
                Console.WriteLine("Plat      : " + (k == null ? "-" : k.PlatNomor));
                Console.WriteLine("Driver    : " + (d == null ? "Belum ditentukan" : d.Nama));
                Console.WriteLine("Tanggal   : " + j.TanggalBerangkat);
                Console.WriteLine("Jam       : " + j.JamBerangkat);
                Console.WriteLine("Status    : " + j.Status);
            }

            Garis();
        }

        // =========================================
        // KELOLA BOOKING
        // =========================================

        static void MenuBooking()
        {
            bool kembali = false;

            while (!kembali)
            {
                Console.Clear();
                Console.WriteLine("=== KELOLA BOOKING ===");
                Console.WriteLine("1. Lihat booking");
                Console.WriteLine("2. Ubah status booking");
                Console.WriteLine("3. Hapus booking");
                Console.WriteLine("4. Kembali");

                switch (Input("Pilih: "))
                {
                    case "1": LihatBookingAdmin(); break;
                    case "2": UbahStatusBookingAdmin(); break;
                    case "3": HapusBookingAdmin(); break;
                    case "4": kembali = true; break;
                    default: Console.WriteLine("Pilihan tidak tersedia!"); Jeda(); break;
                }
            }
        }

        static void LihatBookingAdmin()
        {
            Console.Clear();
            Console.WriteLine("=== DATA BOOKING ===");

            var daftar = bookingRepository.GetAll();

            if (daftar.Count == 0)
                Console.WriteLine("Belum ada booking.");

            foreach (var b in daftar)
            {
                Garis();
                Console.WriteLine("ID Booking   : " + b.Id);
                Console.WriteLine("ID Penumpang : " + b.PenumpangId);
                Console.WriteLine("ID Jadwal    : " + b.JadwalId);
                Console.WriteLine("ID Titik     : " + b.TitikJemputId);
                Console.WriteLine("Nomor Kursi  : " + b.NomorKursi);
                Console.WriteLine("Total Biaya  : Rp " + b.TotalBiaya.ToString("N0"));
                Console.WriteLine("Status       : " + b.StatusPemesanan);
                Console.WriteLine("Tanggal      : " + b.TanggalPesan);
            }

            Jeda();
        }

        static void UbahStatusBookingAdmin()
        {
            Console.Clear();
            LihatBookingTanpaJeda();

            int id = InputInt("\nID booking: ");

            Console.WriteLine("1. Menunggu");
            Console.WriteLine("2. Berhasil");
            Console.WriteLine("3. Dibatalkan");

            string pilihan = Input("Pilih status: ");
            string status = pilihan switch
            {
                "1" => "Menunggu",
                "2" => "Berhasil",
                "3" => "Dibatalkan",
                _ => ""
            };

            if (status == "")
            {
                Console.WriteLine("Status tidak valid!");
            }
            else
            {
                try
                {
                    bookingRepository.UpdateStatus(id, status);
                    Console.WriteLine("Status booking berhasil diperbarui!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            Jeda();
        }

        static void LihatBookingTanpaJeda()
        {
            foreach (var b in bookingRepository.GetAll())
                Console.WriteLine($"{b.Id}. Jadwal {b.JadwalId} - Kursi {b.NomorKursi} - {b.StatusPemesanan}");
        }

        static void HapusBookingAdmin()
        {
            Console.Clear();
            LihatBookingTanpaJeda();

            int id = InputInt("\nID booking: ");

            if (!Konfirmasi())
            {
                Console.WriteLine("Penghapusan dibatalkan.");
                Jeda();
                return;
            }

            try
            {
                bookingRepository.Hapus(id);
                Console.WriteLine("Perintah penghapusan selesai.");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal menghapus booking. Hapus pembayaran terkait terlebih dahulu jika ada.");
                Console.WriteLine(ex.Message);
            }

            Jeda();
        }

        // =========================================
        // KELOLA PEMBAYARAN
        // =========================================

        static void MenuPembayaran()
        {
            bool kembali = false;

            while (!kembali)
            {
                Console.Clear();
                Console.WriteLine("=== KELOLA PEMBAYARAN ===");
                Console.WriteLine("1. Tambah pembayaran");
                Console.WriteLine("2. Lihat pembayaran");
                Console.WriteLine("3. Ubah status pembayaran");
                Console.WriteLine("4. Hapus pembayaran");
                Console.WriteLine("5. Kembali");

                switch (Input("Pilih: "))
                {
                    case "1": TambahPembayaranAdmin(); break;
                    case "2": LihatPembayaranAdmin(); break;
                    case "3": UbahStatusPembayaranAdmin(); break;
                    case "4": HapusPembayaranAdmin(); break;
                    case "5": kembali = true; break;
                    default: Console.WriteLine("Pilihan tidak tersedia!"); Jeda(); break;
                }
            }
        }

        static void TambahPembayaranAdmin()
        {
            Console.Clear();
            Console.WriteLine("=== TAMBAH PEMBAYARAN ===");
            LihatBookingTanpaJeda();

            int bookingId = InputInt("\nID booking: ");

            var booking = bookingRepository.GetAll()
                .FirstOrDefault(b => b.Id == bookingId);

            if (booking == null)
            {
                Console.WriteLine("Booking tidak ditemukan!");
                Jeda();
                return;
            }

            if (pembayaranRepository.GetAll().Any(p => p.BookingId == bookingId))
            {
                Console.WriteLine("Booking ini sudah memiliki pembayaran!");
                Jeda();
                return;
            }

            Console.WriteLine("1. QRIS");
            string pilihan = Input("Metode pembayaran: ");
            string metode = pilihan == "1" ? "QRIS" : "";

            if (metode == "")
            {
                Console.WriteLine("Metode tidak valid!");
                Jeda();
                return;
            }

            var pembayaran = new Pembayaran
            {
                BookingId = bookingId,
                Metode = metode,
                Jumlah = booking.TotalBiaya,
                StatusPembayaran = "Belum Dibayar",
                TanggalBayar = null
            };

            try
            {
                pembayaranRepository.Tambah(pembayaran);
                Console.WriteLine("Data pembayaran berhasil ditambahkan!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal menambahkan pembayaran: " + ex.Message);
            }

            Jeda();
        }

        static void LihatPembayaranAdmin()
        {
            Console.Clear();
            Console.WriteLine("=== DATA PEMBAYARAN ===");

            var daftar = pembayaranRepository.GetAll();

            if (daftar.Count == 0)
                Console.WriteLine("Belum ada pembayaran.");

            foreach (var p in daftar)
            {
                Garis();
                Console.WriteLine("ID Pembayaran : " + p.Id);
                Console.WriteLine("ID Booking    : " + p.BookingId);
                Console.WriteLine("Metode        : " + p.Metode);
                Console.WriteLine("Jumlah        : Rp " + p.Jumlah.ToString("N0"));
                Console.WriteLine("Status        : " + p.StatusPembayaran);
                Console.WriteLine("Tanggal       : " + (p.TanggalBayar ?? "-"));
            }

            Jeda();
        }

        static void UbahStatusPembayaranAdmin()
        {
            Console.Clear();

            foreach (var p in pembayaranRepository.GetAll())
                Console.WriteLine($"{p.Id}. Booking {p.BookingId} - {p.StatusPembayaran}");

            int id = InputInt("\nID pembayaran: ");

            Console.WriteLine("1. Belum Dibayar");
            Console.WriteLine("2. Berhasil");
            Console.WriteLine("3. Dibatalkan");

            string pilihan = Input("Pilih status: ");

            string status = pilihan switch
            {
                "1" => "Belum Dibayar",
                "2" => "Berhasil",
                "3" => "Dibatalkan",
                _ => ""
            };

            if (status == "")
            {
                Console.WriteLine("Status tidak valid!");
                Jeda();
                return;
            }

            string? tanggal = status == "Berhasil"
                ? DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                : null;

            try
            {
                pembayaranRepository.UpdateStatus(id, status, tanggal);
                Console.WriteLine("Status pembayaran berhasil diperbarui!");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Gagal mengubah status: " + ex.Message);
            }

            Jeda();
        }

        static void HapusPembayaranAdmin()
        {
            Console.Clear();

            foreach (var p in pembayaranRepository.GetAll())
                Console.WriteLine($"{p.Id}. Booking {p.BookingId} - {p.StatusPembayaran}");

            int id = InputInt("\nID pembayaran: ");

            if (!Konfirmasi())
            {
                Console.WriteLine("Penghapusan dibatalkan.");
                Jeda();
                return;
            }

            try
            {
                pembayaranRepository.Hapus(id);
                Console.WriteLine("Perintah penghapusan selesai.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Jeda();
        }

        // =========================================
        // MENU PENGGUNA
        // =========================================

        static void MenuPengguna(Penumpang pengguna)
        {
            bool kembali = false;

            while (!kembali)
            {
                Console.Clear();
                Console.WriteLine("========== MENU PENUMPANG ==========");
                Console.WriteLine("Nama: " + pengguna.Nama);
                Console.WriteLine("1. Lihat jadwal shuttle");
                Console.WriteLine("2. Booking shuttle");
                Console.WriteLine("3. Riwayat booking");
                Console.WriteLine("4. Status pembayaran");
                Console.WriteLine("5. Logout");

                switch (Input("Pilih menu: "))
                {
                    case "1": LihatJadwalPengguna(); break;
                    case "2": BookingPengguna(pengguna); break;
                    case "3": RiwayatBookingPengguna(pengguna); break;
                    case "4": StatusPembayaranPengguna(pengguna); break;
                    case "5": kembali = true; break;
                    default: Console.WriteLine("Pilihan tidak tersedia!"); Jeda(); break;
                }
            }
        }

        static void LihatJadwalPengguna()
        {
            Console.Clear();
            Console.WriteLine("=== JADWAL SHUTTLE ===");
            TampilkanJadwalUntukPengguna();
            Jeda();
        }

        // =========================================
        // BOOKING PENGGUNA
        // =========================================

        static void BookingPengguna(Penumpang pengguna)
        {
            Console.Clear();
            Console.WriteLine("=== BOOKING SHUTTLE ===");

            try
            {
                TampilkanJadwalUntukPengguna();

                int jadwalId = InputInt("\nPilih ID jadwal: ");

                var jadwal = jadwalRepository.GetAll()
                    .FirstOrDefault(j => j.Id == jadwalId);

                if (jadwal == null || jadwal.Status != "Tersedia")
                {
                    Console.WriteLine("Jadwal tidak tersedia!");
                    Jeda();
                    return;
                }

                var rute = ruteRepository.GetAll()
                    .FirstOrDefault(r => r.Id == jadwal.RuteId);

                if (rute == null)
                {
                    Console.WriteLine("Data rute tidak ditemukan!");
                    Jeda();
                    return;
                }

                var titikList = titikRepository.GetAll()
                    .Where(t => t.RuteId == jadwal.RuteId)
                    .ToList();

                if (titikList.Count == 0)
                {
                    Console.WriteLine("Belum ada titik jemput untuk rute ini.");
                    Console.WriteLine("Silakan hubungi admin.");
                    Jeda();
                    return;
                }

                Console.WriteLine("\n=== PILIH TITIK JEMPUT ===");

                foreach (var t in titikList)
                    Console.WriteLine($"{t.Id}. {t.NamaTitik} - {t.Alamat}");

                int titikId = InputInt("Pilih ID titik jemput: ");

                if (!titikList.Any(t => t.Id == titikId))
                {
                    Console.WriteLine("Titik jemput tidak sesuai dengan rute!");
                    Jeda();
                    return;
                }

                Console.WriteLine("\n=== KETERSEDIAAN KURSI ===");

                var kursiTerisi = bookingRepository.GetKursiTerisi(jadwalId);

                for (int i = 1; i <= 15; i++)
                {
                    string statusKursi = kursiTerisi.Contains(i) ? "[Terisi]" : "[Kosong]";
                    Console.WriteLine($"Kursi {i,2} : {statusKursi}");
                }

                int nomorKursi = InputInt("\nPilih nomor kursi (1-15): ");

                if (nomorKursi < 1 || nomorKursi > 15)
                {
                    Console.WriteLine("Nomor kursi harus 1 sampai 15!");
                    Jeda();
                    return;
                }

                if (kursiTerisi.Contains(nomorKursi))
                {
                    Console.WriteLine("Kursi sudah dipesan!");
                    Jeda();
                    return;
                }

                var booking = new Booking
                {
                    PenumpangId = pengguna.Id,
                    JadwalId = jadwalId,
                    TitikJemputId = titikId,
                    NomorKursi = nomorKursi,
                    TotalBiaya = rute.Tarif,
                    StatusPemesanan = "Menunggu",
                    TanggalPesan = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                };

                bookingRepository.Tambah(booking);

                Console.WriteLine("\nBooking berhasil dibuat!");
                Console.WriteLine("Rute        : " + rute.Asal + " - " + rute.Tujuan);
                Console.WriteLine("Nomor Kursi : " + nomorKursi);
                Console.WriteLine("Total Biaya : Rp " + rute.Tarif.ToString("N0"));
                Console.WriteLine("Status      : Menunggu");
            }
            catch (Exception ex)
            {
                Console.WriteLine("Booking gagal: " + ex.Message);
            }

            Jeda();
        }

        // =========================================
        // RIWAYAT BOOKING
        // =========================================

        static void RiwayatBookingPengguna(Penumpang pengguna)
        {
            Console.Clear();
            Console.WriteLine("=== RIWAYAT BOOKING ===");

            var daftar = bookingRepository.GetByPenumpang(pengguna.Id);

            if (daftar.Count == 0)
                Console.WriteLine("Kamu belum memiliki riwayat booking.");

            foreach (var b in daftar)
            {
                Garis();
                Console.WriteLine("ID Booking  : " + b.Id);
                Console.WriteLine("ID Jadwal   : " + b.JadwalId);
                Console.WriteLine("Nomor Kursi : " + b.NomorKursi);
                Console.WriteLine("Total Biaya : Rp " + b.TotalBiaya.ToString("N0"));
                Console.WriteLine("Status      : " + b.StatusPemesanan);
                Console.WriteLine("Tanggal     : " + b.TanggalPesan);
            }

            Jeda();
        }

        // =========================================
        // STATUS PEMBAYARAN PENGGUNA
        // =========================================

        static void StatusPembayaranPengguna(Penumpang pengguna)
        {
            Console.Clear();
            Console.WriteLine("=== STATUS PEMBAYARAN ===");

            var daftarBooking = bookingRepository.GetByPenumpang(pengguna.Id);
            var daftarPembayaran = pembayaranRepository.GetAll();

            if (daftarBooking.Count == 0)
            {
                Console.WriteLine("Kamu belum memiliki booking.");
                Jeda();
                return;
            }

            foreach (var b in daftarBooking)
            {
                var p = daftarPembayaran.FirstOrDefault(x => x.BookingId == b.Id);

                Garis();
                Console.WriteLine("ID Booking       : " + b.Id);
                Console.WriteLine("Status Booking   : " + b.StatusPemesanan);

                if (p == null)
                {
                    Console.WriteLine("Pembayaran       : Belum ada data pembayaran");
                }
                else
                {
                    Console.WriteLine("Metode           : " + p.Metode);
                    Console.WriteLine("Jumlah           : Rp " + p.Jumlah.ToString("N0"));
                    Console.WriteLine("Status Pembayaran: " + p.StatusPembayaran);
                    Console.WriteLine("Tanggal Bayar    : " + (p.TanggalBayar ?? "-"));
                }
            }

            Jeda();
        }

        // =========================================
        // MENU DRIVER
        // =========================================

        static void MenuDriver(Driver driver)
        {
            bool kembali = false;

            while (!kembali)
            {
                Console.Clear();
                Console.WriteLine("========== MENU DRIVER ==========");
                Console.WriteLine("Nama: " + driver.Nama);
                Console.WriteLine("1. Lihat jadwal tugas");
                Console.WriteLine("2. Lihat daftar penumpang");
                Console.WriteLine("3. Logout");

                switch (Input("Pilih menu: "))
                {
                    case "1": LihatJadwalDriver(driver); break;
                    case "2": LihatPenumpangDriver(driver); break;
                    case "3": kembali = true; break;
                    default: Console.WriteLine("Pilihan tidak tersedia!"); Jeda(); break;
                }
            }
        }

        static void LihatJadwalDriver(Driver driver)
        {
            Console.Clear();
            Console.WriteLine("=== JADWAL TUGAS DRIVER ===");

            var daftar = jadwalRepository.GetAll()
                .Where(j => j.DriverId == driver.Id)
                .ToList();

            if (daftar.Count == 0)
                Console.WriteLine("Belum ada jadwal tugas.");

            foreach (var j in daftar)
            {
                var r = ruteRepository.GetAll().FirstOrDefault(x => x.Id == j.RuteId);
                var k = kendaraanRepository.GetAll().FirstOrDefault(x => x.Id == j.KendaraanId);

                Garis();
                Console.WriteLine("ID Jadwal : " + j.Id);
                Console.WriteLine("Rute      : " + (r == null ? "-" : r.Asal + " - " + r.Tujuan));
                Console.WriteLine("Kendaraan : " + (k == null ? "-" : k.NamaKendaraan));
                Console.WriteLine("Tanggal   : " + j.TanggalBerangkat);
                Console.WriteLine("Jam       : " + j.JamBerangkat);
                Console.WriteLine("Status    : " + j.Status);
            }

            Jeda();
        }

        static void LihatPenumpangDriver(Driver driver)
        {
            Console.Clear();
            Console.WriteLine("=== DAFTAR PENUMPANG ===");

            var jadwalDriver = jadwalRepository.GetAll()
                .Where(j => j.DriverId == driver.Id)
                .ToList();

            var semuaBooking = bookingRepository.GetAll();
            var semuaPenumpang = repository.GetAll();

            bool ditemukan = false;

            foreach (var j in jadwalDriver)
            {
                var bookingJadwal = semuaBooking
                    .Where(b => b.JadwalId == j.Id &&
                                b.StatusPemesanan != "Dibatalkan")
                    .ToList();

                Console.WriteLine("\nJadwal ID: " + j.Id);

                foreach (var b in bookingJadwal)
                {
                    var p = semuaPenumpang.FirstOrDefault(x => x.Id == b.PenumpangId);

                    Console.WriteLine("----------------------------------");
                    Console.WriteLine("Nama        : " + (p == null ? "-" : p.Nama));
                    Console.WriteLine("Nomor Kursi : " + b.NomorKursi);
                    Console.WriteLine("ID Booking  : " + b.Id);
                    ditemukan = true;
                }
            }

            if (!ditemukan)
                Console.WriteLine("Belum ada penumpang pada jadwal tugas.");

            Jeda();
        }
    }
}
