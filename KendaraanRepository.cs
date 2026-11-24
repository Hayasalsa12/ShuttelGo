
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using ShuttleGO.Models;

namespace ShuttleGO.Repositories
{
    public class KendaraanRepository
    {
        // CREATE: Menambahkan kendaraan
        public void Tambah(Kendaraan kendaraan)
        {
            using var connection = DatabaseHelper.GetConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Kendaraan
                (NamaKendaraan, PlatNomor, Kapasitas)
                VALUES
                (@nama, @plat, @kapasitas)";

            command.Parameters.AddWithValue("@nama", kendaraan.NamaKendaraan);
            command.Parameters.AddWithValue("@plat", kendaraan.PlatNomor);
            command.Parameters.AddWithValue("@kapasitas", kendaraan.Kapasitas);

            command.ExecuteNonQuery();
        }

        // READ: Mengambil semua data kendaraan
        public List<Kendaraan> GetAll()
        {
            var daftarKendaraan = new List<Kendaraan>();

            using var connection = DatabaseHelper.GetConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT
                    IdKendaraan,
                    NamaKendaraan,
                    PlatNomor,
                    Kapasitas
                FROM Kendaraan";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                daftarKendaraan.Add(new Kendaraan
                {
                    Id = reader.GetInt32(0),
                    NamaKendaraan = reader.GetString(1),
                    PlatNomor = reader.GetString(2),
                    Kapasitas = reader.GetInt32(3)
                });
            }

            return daftarKendaraan;
        }

        // UPDATE: Mengubah data kendaraan
        public void Update(Kendaraan kendaraan)
        {
            using var connection = DatabaseHelper.GetConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                UPDATE Kendaraan
                SET NamaKendaraan = @nama,
                    PlatNomor = @plat,
                    Kapasitas = @kapasitas
                WHERE IdKendaraan = @id";

            command.Parameters.AddWithValue("@nama", kendaraan.NamaKendaraan);
            command.Parameters.AddWithValue("@plat", kendaraan.PlatNomor);
            command.Parameters.AddWithValue("@kapasitas", kendaraan.Kapasitas);
            command.Parameters.AddWithValue("@id", kendaraan.Id);

            command.ExecuteNonQuery();
        }

        // DELETE: Menghapus data kendaraan
        public void Hapus(int id)
        {
            using var connection = DatabaseHelper.GetConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                DELETE FROM Kendaraan
                WHERE IdKendaraan = @id";

            command.Parameters.AddWithValue("@id", id);

            command.ExecuteNonQuery();
        }
    }
}
