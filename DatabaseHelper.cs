
using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace ShuttleGO
{
    public class DatabaseHelper
    {
        private static readonly string databasePath =
            Path.Combine(AppContext.BaseDirectory, "shuttlego.db");

        private static readonly string connectionString =
            $"Data Source={databasePath}";

        public static SqliteConnection GetConnection()
        {
            var connection = new SqliteConnection(connectionString);
            connection.Open();

            using var command = connection.CreateCommand();
            command.CommandText = "PRAGMA foreign_keys = ON;";
            command.ExecuteNonQuery();

            return connection;
        }

        public static void InitializeDatabase()
        {
            using var connection = GetConnection();
            using var command = connection.CreateCommand();

            Console.WriteLine("DATABASE AKTIF: " + databasePath);

            command.CommandText = @"
                CREATE TABLE IF NOT EXISTS Penumpang (
                    IdPenumpang INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nama TEXT NOT NULL,
                    NoHP TEXT NOT NULL,
                    Email TEXT,
                    PasswordHash TEXT
                );

                CREATE TABLE IF NOT EXISTS Admin (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nama TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Driver (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nama TEXT NOT NULL,
                    NoHP TEXT NOT NULL,
                    Email TEXT NOT NULL UNIQUE,
                    Password TEXT NOT NULL DEFAULT ''
                );

                CREATE TABLE IF NOT EXISTS Kendaraan (
                    IdKendaraan INTEGER PRIMARY KEY AUTOINCREMENT,
                    NamaKendaraan TEXT NOT NULL,
                    PlatNomor TEXT NOT NULL UNIQUE,
                    Kapasitas INTEGER NOT NULL,
                    IdDriver INTEGER
                );

                CREATE TABLE IF NOT EXISTS Rute (
                    IdRute INTEGER PRIMARY KEY AUTOINCREMENT,
                    Asal TEXT NOT NULL,
                    Tujuan TEXT NOT NULL,
                    Tarif REAL NOT NULL
                );

                CREATE TABLE IF NOT EXISTS TitikJemput (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NamaTitik TEXT NOT NULL,
                    Alamat TEXT NOT NULL,
                    RuteId INTEGER NOT NULL
                );

                CREATE TABLE IF NOT EXISTS Jadwal (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    KendaraanId INTEGER NOT NULL,
                    RuteId INTEGER NOT NULL,
                    TanggalBerangkat TEXT NOT NULL,
                    JamBerangkat TEXT NOT NULL,
                    DriverId INTEGER,
                    Status TEXT DEFAULT 'Tersedia'
                );

                CREATE TABLE IF NOT EXISTS Booking (
                    IdBooking INTEGER PRIMARY KEY AUTOINCREMENT,
                    KodeBooking TEXT NOT NULL,
                    IdPenumpang INTEGER NOT NULL,
                    IdJadwal INTEGER NOT NULL,
                    NomorKursi TEXT NOT NULL,
                    StatusBooking TEXT DEFAULT 'Menunggu',
                    TanggalBooking TEXT NOT NULL,
                    TitikJemputId INTEGER,
                    TotalBiaya REAL NOT NULL DEFAULT 0
                );

                CREATE TABLE IF NOT EXISTS Pembayaran (
                    IdPembayaran INTEGER PRIMARY KEY AUTOINCREMENT,
                    IdBooking INTEGER NOT NULL UNIQUE,
                    MetodePembayaran TEXT NOT NULL,
                    Jumlah REAL NOT NULL,
                    StatusPembayaran TEXT DEFAULT 'Menunggu',
                    TanggalPembayaran TEXT,
                    FOREIGN KEY (IdBooking)
                        REFERENCES Booking(IdBooking)
                );
            ";

            command.ExecuteNonQuery();

            // MIGRASI BOOKING: MENAMBAHKAN KOLOM TANPA MENGHAPUS DATA
            TambahKolomJikaBelumAda(
                connection, "Booking", "TitikJemputId",
                "INTEGER"
            );

            TambahKolomJikaBelumAda(
                connection, "Booking", "TotalBiaya",
                "REAL NOT NULL DEFAULT 0"
            );

            PeriksaKolomDriver(connection);

            Console.WriteLine("Database berhasil disiapkan!");
        }

        private static bool KolomAda(
            SqliteConnection connection,
            string namaTabel,
            string namaKolom)
        {
            using var command = connection.CreateCommand();

            // Nama tabel berasal dari pemanggilan internal program.
            command.CommandText =
                $"PRAGMA table_info('{namaTabel}');";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                string kolom = reader.GetString(1);

                if (kolom.Equals(
                    namaKolom,
                    StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private static void TambahKolomJikaBelumAda(
            SqliteConnection connection,
            string namaTabel,
            string namaKolom,
            string definisiKolom)
        {
            if (KolomAda(connection, namaTabel, namaKolom))
                return;

            using var command = connection.CreateCommand();

            command.CommandText =
                $"ALTER TABLE {namaTabel} ADD COLUMN {namaKolom} {definisiKolom};";

            command.ExecuteNonQuery();

            Console.WriteLine(
                $"Kolom {namaKolom} berhasil ditambahkan ke {namaTabel}."
            );
        }

        private static void PeriksaKolomDriver(
            SqliteConnection connection)
        {
            if (!KolomAda(connection, "Driver", "Password"))
            {
                TambahKolomJikaBelumAda(
                    connection,
                    "Driver",
                    "Password",
                    "TEXT NOT NULL DEFAULT ''"
                );
            }

            Console.WriteLine(
                "Struktur tabel Driver berhasil diperiksa!"
            );
        }

        public static void PeriksaStrukturDriver()
        {
            using var connection = GetConnection();
            using var command = connection.CreateCommand();

            command.CommandText = "PRAGMA table_info('Driver');";

            using var reader = command.ExecuteReader();

            Console.WriteLine("\n=== STRUKTUR TABEL DRIVER ===");

            while (reader.Read())
            {
                Console.WriteLine(
                    $"Kolom: {reader["name"]} | Tipe: {reader["type"]}"
                );
            }
        }
    }
}
