
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using ShuttleGO.Models;

namespace ShuttleGO.Repositories
{
    public class PembayaranRepository
    {
        // CREATE
        public void Tambah(Pembayaran data)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Pembayaran
                (
                    IdBooking,
                    MetodePembayaran,
                    Jumlah,
                    StatusPembayaran,
                    TanggalPembayaran
                )
                VALUES
                (
                    $Booking,
                    $Metode,
                    $Jumlah,
                    $Status,
                    $Tanggal
                )";

            cmd.Parameters.AddWithValue("$Booking", data.BookingId);
            cmd.Parameters.AddWithValue("$Metode", data.Metode);
            cmd.Parameters.AddWithValue("$Jumlah", data.Jumlah);
            cmd.Parameters.AddWithValue("$Status", data.StatusPembayaran);
            cmd.Parameters.AddWithValue(
                "$Tanggal",
                (object?)data.TanggalBayar ?? DBNull.Value
            );

            cmd.ExecuteNonQuery();
        }

        // READ ALL
        public List<Pembayaran> GetAll()
        {
            var daftar = new List<Pembayaran>();

            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                SELECT
                    IdPembayaran,
                    IdBooking,
                    MetodePembayaran,
                    Jumlah,
                    StatusPembayaran,
                    TanggalPembayaran
                FROM Pembayaran
                ORDER BY IdPembayaran DESC";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                daftar.Add(new Pembayaran
                {
                    Id = Convert.ToInt32(reader["IdPembayaran"]),
                    BookingId = Convert.ToInt32(reader["IdBooking"]),
                    Metode = reader["MetodePembayaran"]?.ToString() ?? "",
                    Jumlah = Convert.ToDouble(reader["Jumlah"]),
                    StatusPembayaran =
                        reader["StatusPembayaran"]?.ToString()
                        ?? "Menunggu",
                    TanggalBayar =
                        reader["TanggalPembayaran"] == DBNull.Value
                            ? null
                            : reader["TanggalPembayaran"].ToString()
                });
            }

            return daftar;
        }

        // UPDATE STATUS
        public void UpdateStatus(
            int id,
            string status,
            string? tanggalBayar)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                UPDATE Pembayaran
                SET
                    StatusPembayaran = $Status,
                    TanggalPembayaran = $Tanggal
                WHERE IdPembayaran = $Id";

            cmd.Parameters.AddWithValue("$Id", id);
            cmd.Parameters.AddWithValue("$Status", status);
            cmd.Parameters.AddWithValue(
                "$Tanggal",
                (object?)tanggalBayar ?? DBNull.Value
            );

            cmd.ExecuteNonQuery();
        }

        // DELETE
        public void Hapus(int id)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                DELETE FROM Pembayaran
                WHERE IdPembayaran = $Id";

            cmd.Parameters.AddWithValue("$Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
