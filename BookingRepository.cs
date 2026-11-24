
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using ShuttleGO.Models;

namespace ShuttleGO.Repositories
{
    public class BookingRepository
    {
        // CREATE
        public void Tambah(Booking data)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Booking
                (
                    KodeBooking,
                    IdPenumpang,
                    IdJadwal,
                    NomorKursi,
                    StatusBooking,
                    TanggalBooking,
                    TitikJemputId,
                    TotalBiaya
                )
                VALUES
                (
                    $Kode,
                    $Penumpang,
                    $Jadwal,
                    $Kursi,
                    $Status,
                    $Tanggal,
                    $Titik,
                    $Biaya
                )";

            cmd.Parameters.AddWithValue(
                "$Kode",
                string.IsNullOrWhiteSpace(data.KodeBooking)
                    ? Guid.NewGuid().ToString("N")
                        .Substring(0, 10).ToUpper()
                    : data.KodeBooking
            );

            cmd.Parameters.AddWithValue("$Penumpang", data.PenumpangId);
            cmd.Parameters.AddWithValue("$Jadwal", data.JadwalId);
            cmd.Parameters.AddWithValue("$Kursi", data.NomorKursi.ToString());
            cmd.Parameters.AddWithValue(
                "$Status",
                string.IsNullOrWhiteSpace(data.StatusPemesanan)
                    ? "Menunggu"
                    : data.StatusPemesanan
            );
            cmd.Parameters.AddWithValue("$Tanggal", data.TanggalPesan);
            cmd.Parameters.AddWithValue("$Titik", data.TitikJemputId);
            cmd.Parameters.AddWithValue("$Biaya", data.TotalBiaya);

            cmd.ExecuteNonQuery();
        }

        // MEMBACA DATA DARI READER
        private Booking BacaBooking(SqliteDataReader reader)
        {
            return new Booking
            {
                Id = Convert.ToInt32(reader["IdBooking"]),
                KodeBooking = reader["KodeBooking"]?.ToString() ?? "",
                PenumpangId = Convert.ToInt32(reader["IdPenumpang"]),
                JadwalId = Convert.ToInt32(reader["IdJadwal"]),
                NomorKursi = Convert.ToInt32(reader["NomorKursi"]),
                StatusPemesanan =
                    reader["StatusBooking"]?.ToString() ?? "Menunggu",
                TanggalPesan =
                    reader["TanggalBooking"]?.ToString() ?? "",
                TitikJemputId = reader["TitikJemputId"] == DBNull.Value
                    ? 0
                    : Convert.ToInt32(reader["TitikJemputId"]),
                TotalBiaya = reader["TotalBiaya"] == DBNull.Value
                    ? 0
                    : Convert.ToDouble(reader["TotalBiaya"])
            };
        }

        // READ ALL
        public List<Booking> GetAll()
        {
            var daftar = new List<Booking>();

            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                SELECT
                    IdBooking,
                    KodeBooking,
                    IdPenumpang,
                    IdJadwal,
                    NomorKursi,
                    StatusBooking,
                    TanggalBooking,
                    TitikJemputId,
                    TotalBiaya
                FROM Booking
                ORDER BY IdBooking DESC";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                daftar.Add(BacaBooking(reader));
            }

            return daftar;
        }

        // READ BY PENUMPANG
        public List<Booking> GetByPenumpang(int penumpangId)
        {
            var daftar = new List<Booking>();

            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                SELECT
                    IdBooking,
                    KodeBooking,
                    IdPenumpang,
                    IdJadwal,
                    NomorKursi,
                    StatusBooking,
                    TanggalBooking,
                    TitikJemputId,
                    TotalBiaya
                FROM Booking
                WHERE IdPenumpang = $Id
                ORDER BY IdBooking DESC";

            cmd.Parameters.AddWithValue("$Id", penumpangId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                daftar.Add(BacaBooking(reader));
            }

            return daftar;
        }

        // UPDATE STATUS
        public void UpdateStatus(int id, string status)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                UPDATE Booking
                SET StatusBooking = $Status
                WHERE IdBooking = $Id";

            cmd.Parameters.AddWithValue("$Id", id);
            cmd.Parameters.AddWithValue("$Status", status);

            cmd.ExecuteNonQuery();
        }

        // DELETE
        public void Hapus(int id)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                DELETE FROM Booking
                WHERE IdBooking = $Id";

            cmd.Parameters.AddWithValue("$Id", id);

            cmd.ExecuteNonQuery();
        }

        // MENGAMBIL KURSI YANG SUDAH TERISI
        public List<int> GetKursiTerisi(int jadwalId)
        {
            var kursi = new List<int>();

            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                SELECT NomorKursi
                FROM Booking
                WHERE IdJadwal = $JadwalId
                AND StatusBooking <> 'Dibatalkan'";

            cmd.Parameters.AddWithValue("$JadwalId", jadwalId);

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                if (int.TryParse(
                    reader["NomorKursi"]?.ToString(),
                    out int nomorKursi))
                {
                    kursi.Add(nomorKursi);
                }
            }

            return kursi;
        }
    }
}
