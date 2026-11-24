
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using ShuttleGO.Models;

namespace ShuttleGO.Repositories
{
    public class RuteRepository
    {
        public void Tambah(Rute data)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"INSERT INTO Rute (Asal, Tujuan, Tarif)
                                VALUES ($Asal, $Tujuan, $Tarif)";
            cmd.Parameters.AddWithValue("$Asal", data.Asal);
            cmd.Parameters.AddWithValue("$Tujuan", data.Tujuan);
            cmd.Parameters.AddWithValue("$Tarif", data.Tarif);
            cmd.ExecuteNonQuery();
        }

        public List<Rute> GetAll()
        {
            var daftar = new List<Rute>();
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = "SELECT * FROM Rute";
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                daftar.Add(new Rute
                {
                    Id = reader.GetInt32(0),
                    Asal = reader.GetString(1),
                    Tujuan = reader.GetString(2),
                    Tarif = reader.GetDouble(3)
                });
            }

            return daftar;
        }

        public void Update(Rute data)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"UPDATE Rute SET Asal=$Asal,
                Tujuan=$Tujuan, Tarif=$Tarif WHERE Id=$Id";
            cmd.Parameters.AddWithValue("$Id", data.Id);
            cmd.Parameters.AddWithValue("$Asal", data.Asal);
            cmd.Parameters.AddWithValue("$Tujuan", data.Tujuan);
            cmd.Parameters.AddWithValue("$Tarif", data.Tarif);
            cmd.ExecuteNonQuery();
        }

        public void Hapus(int id)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM Rute WHERE Id=$Id";
            cmd.Parameters.AddWithValue("$Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
