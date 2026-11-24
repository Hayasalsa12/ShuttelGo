
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using ShuttleGO.Models;

namespace ShuttleGO.Repositories
{
    public class TitikJemputRepository
    {
        public void Tambah(TitikJemput data)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"INSERT INTO TitikJemput
                (NamaTitik, Alamat, RuteId)
                VALUES ($Nama, $Alamat, $RuteId)";
            cmd.Parameters.AddWithValue("$Nama", data.NamaTitik);
            cmd.Parameters.AddWithValue("$Alamat", data.Alamat);
            cmd.Parameters.AddWithValue("$RuteId", data.RuteId);
            cmd.ExecuteNonQuery();
        }

        public List<TitikJemput> GetAll()
        {
            var daftar = new List<TitikJemput>();
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = "SELECT * FROM TitikJemput";
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                daftar.Add(new TitikJemput
                {
                    Id = reader.GetInt32(0),
                    NamaTitik = reader.GetString(1),
                    Alamat = reader.GetString(2),
                    RuteId = reader.GetInt32(3)
                });
            }

            return daftar;
        }

        public void Update(TitikJemput data)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"UPDATE TitikJemput SET
                NamaTitik=$Nama, Alamat=$Alamat, RuteId=$RuteId
                WHERE Id=$Id";
            cmd.Parameters.AddWithValue("$Id", data.Id);
            cmd.Parameters.AddWithValue("$Nama", data.NamaTitik);
            cmd.Parameters.AddWithValue("$Alamat", data.Alamat);
            cmd.Parameters.AddWithValue("$RuteId", data.RuteId);
            cmd.ExecuteNonQuery();
        }

        public void Hapus(int id)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = "DELETE FROM TitikJemput WHERE Id=$Id";
            cmd.Parameters.AddWithValue("$Id", id);
            cmd.ExecuteNonQuery();
        }
    }
}
