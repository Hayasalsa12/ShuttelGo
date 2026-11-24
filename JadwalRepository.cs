
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using ShuttleGO.Models;

namespace ShuttleGO.Repositories
{
    public class JadwalRepository
    {
        public void Tambah(Jadwal data)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Jadwal
                (KendaraanId, RuteId, DriverId, TanggalBerangkat,
                 JamBerangkat, Status)
                VALUES
                ($Kendaraan, $Rute, $Driver, $Tanggal, $Jam, $Status)";

            cmd.Parameters.AddWithValue("$Kendaraan", data.KendaraanId);
            cmd.Parameters.AddWithValue("$Rute", data.RuteId);
            cmd.Parameters.AddWithValue(
                "$Driver",
                data.DriverId.HasValue
                    ? (object)data.DriverId.Value
                    : DBNull.Value
            );
            cmd.Parameters.AddWithValue("$Tanggal", data.TanggalBerangkat);
            cmd.Parameters.AddWithValue("$Jam", data.JamBerangkat);
            cmd.Parameters.AddWithValue("$Status", data.Status ?? "Tersedia");

            cmd.ExecuteNonQuery();
        }

        public List<Jadwal> GetAll()
        {
            var daftar = new List<Jadwal>();

            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                SELECT
                    Id,
                    KendaraanId,
                    RuteId,
                    DriverId,
                    TanggalBerangkat,
                    JamBerangkat,
                    Status
                FROM Jadwal";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                daftar.Add(new Jadwal
                {
                    Id = reader.GetInt32(0),
                    KendaraanId = reader.GetInt32(1),
                    RuteId = reader.GetInt32(2),

                    DriverId = reader.IsDBNull(3)
                        ? (int?)null
                        : reader.GetInt32(3),

                    TanggalBerangkat = reader.IsDBNull(4)
                        ? ""
                        : reader.GetString(4),

                    JamBerangkat = reader.IsDBNull(5)
                        ? ""
                        : reader.GetString(5),

                    Status = reader.IsDBNull(6)
                        ? "Tersedia"
                        : reader.GetString(6)
                });
            }

            return daftar;
        }

        public void Update(Jadwal data)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                UPDATE Jadwal SET
                    KendaraanId = $Kendaraan,
                    RuteId = $Rute,
                    DriverId = $Driver,
                    TanggalBerangkat = $Tanggal,
                    JamBerangkat = $Jam,
                    Status = $Status
                WHERE Id = $Id";

            cmd.Parameters.AddWithValue("$Id", data.Id);
            cmd.Parameters.AddWithValue("$Kendaraan", data.KendaraanId);
            cmd.Parameters.AddWithValue("$Rute", data.RuteId);

            cmd.Parameters.AddWithValue(
                "$Driver",
                data.DriverId.HasValue
                    ? (object)data.DriverId.Value
                    : DBNull.Value
            );

            cmd.Parameters.AddWithValue("$Tanggal", data.TanggalBerangkat);
            cmd.Parameters.AddWithValue("$Jam", data.JamBerangkat);
            cmd.Parameters.AddWithValue("$Status", data.Status ?? "Tersedia");

            cmd.ExecuteNonQuery();
        }

        public void Hapus(int id)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM Jadwal WHERE Id = $Id";
            cmd.Parameters.AddWithValue("$Id", id);

            cmd.ExecuteNonQuery();
        }
    }
}
