
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using ShuttleGO.Models;

namespace ShuttleGO.Repositories
{
    public class AdminRepository
    {
        // CREATE
        public void Tambah(Admin admin)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Admin
                (NamaAdmin, Username, Password)
                VALUES
                ($NamaAdmin, $Username, $Password)";

            cmd.Parameters.AddWithValue("$NamaAdmin", admin.Nama);
            cmd.Parameters.AddWithValue("$Username", admin.Email);
            cmd.Parameters.AddWithValue("$Password", admin.Password);

            cmd.ExecuteNonQuery();
        }

        // READ ALL
        public List<Admin> GetAll()
        {
            var daftar = new List<Admin>();

            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                SELECT IdAdmin, NamaAdmin, Username, Password
                FROM Admin";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                daftar.Add(new Admin
                {
                    Id = reader.GetInt32(0),
                    Nama = reader.GetString(1),
                    Email = reader.GetString(2),
                    Password = reader.GetString(3)
                });
            }

            return daftar;
        }

        // READ BY USERNAME
        public Admin? GetByEmail(string email)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                SELECT IdAdmin, NamaAdmin, Username, Password
                FROM Admin
                WHERE Username = $Username";

            cmd.Parameters.AddWithValue("$Username", email);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Admin
            {
                Id = reader.GetInt32(0),
                Nama = reader.GetString(1),
                Email = reader.GetString(2),
                Password = reader.GetString(3)
            };
        }

        // UPDATE
        public void Update(Admin admin)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                UPDATE Admin
                SET NamaAdmin = $NamaAdmin,
                    Username = $Username,
                    Password = $Password
                WHERE IdAdmin = $IdAdmin";

            cmd.Parameters.AddWithValue("$IdAdmin", admin.Id);
            cmd.Parameters.AddWithValue("$NamaAdmin", admin.Nama);
            cmd.Parameters.AddWithValue("$Username", admin.Email);
            cmd.Parameters.AddWithValue("$Password", admin.Password);

            cmd.ExecuteNonQuery();
        }

        // DELETE
        public void Hapus(int id)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                DELETE FROM Admin
                WHERE IdAdmin = $IdAdmin";

            cmd.Parameters.AddWithValue("$IdAdmin", id);

            cmd.ExecuteNonQuery();
        }
    }
}
