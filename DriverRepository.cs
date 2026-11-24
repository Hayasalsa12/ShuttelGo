
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using ShuttleGO.Models;

namespace ShuttleGO.Repositories
{
    public class DriverRepository
    {
        // MENAMBAHKAN DRIVER
        public void Tambah(Driver driver)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                INSERT INTO Driver (Nama, NoHP, Email, Password)
                VALUES ($Nama, $NoHP, $Email, $Password)";

            cmd.Parameters.AddWithValue("$Nama", driver.Nama);
            cmd.Parameters.AddWithValue("$NoHP", driver.NoHP);
            cmd.Parameters.AddWithValue("$Email", driver.Email);
            cmd.Parameters.AddWithValue("$Password", driver.Password);

            cmd.ExecuteNonQuery();
        }

        // MENAMPILKAN SEMUA DRIVER
        public List<Driver> GetAll()
        {
            var daftar = new List<Driver>();

            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                SELECT Id, Nama, NoHP, Email, Password
                FROM Driver";

            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                daftar.Add(new Driver
                {
                    Id = reader.GetInt32(0),
                    Nama = reader.GetString(1),
                    NoHP = reader.GetString(2),
                    Email = reader.GetString(3),
                    Password = reader.GetString(4)
                });
            }

            return daftar;
        }

        // MENCARI DRIVER BERDASARKAN ID
        public Driver? GetById(int id)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                SELECT Id, Nama, NoHP, Email, Password
                FROM Driver
                WHERE Id = $Id";

            cmd.Parameters.AddWithValue("$Id", id);

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Driver
            {
                Id = reader.GetInt32(0),
                Nama = reader.GetString(1),
                NoHP = reader.GetString(2),
                Email = reader.GetString(3),
                Password = reader.GetString(4)
            };
        }

        // MENCARI DRIVER BERDASARKAN EMAIL
        public Driver? GetByEmail(string email)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                SELECT Id, Nama, NoHP, Email, Password
                FROM Driver
                WHERE Email = $Email COLLATE NOCASE";

            cmd.Parameters.AddWithValue("$Email", email.Trim());

            using var reader = cmd.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Driver
            {
                Id = reader.GetInt32(0),
                Nama = reader.GetString(1),
                NoHP = reader.GetString(2),
                Email = reader.GetString(3),
                Password = reader.GetString(4)
            };
        }

        // MENGATUR PASSWORD DRIVER BERDASARKAN EMAIL
        public bool PerbaruiPassword(string email, string password)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                UPDATE Driver
                SET Password = $Password
                WHERE Email = $Email COLLATE NOCASE";

            cmd.Parameters.AddWithValue("$Email", email.Trim());
            cmd.Parameters.AddWithValue("$Password", password);

            int hasil = cmd.ExecuteNonQuery();

            return hasil > 0;
        }

        // MENGUBAH DATA DRIVER
        public void Update(Driver driver)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                UPDATE Driver
                SET Nama = $Nama,
                    NoHP = $NoHP,
                    Email = $Email,
                    Password = $Password
                WHERE Id = $Id";

            cmd.Parameters.AddWithValue("$Id", driver.Id);
            cmd.Parameters.AddWithValue("$Nama", driver.Nama);
            cmd.Parameters.AddWithValue("$NoHP", driver.NoHP);
            cmd.Parameters.AddWithValue("$Email", driver.Email);
            cmd.Parameters.AddWithValue("$Password", driver.Password);

            cmd.ExecuteNonQuery();
        }

        // MENGHAPUS DRIVER
        public void Hapus(int id)
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                DELETE FROM Driver
                WHERE Id = $Id";

            cmd.Parameters.AddWithValue("$Id", id);

            cmd.ExecuteNonQuery();
        }

        // MENAMPILKAN DATA DRIVER
        public void TampilkanDataDriver()
        {
            using var con = DatabaseHelper.GetConnection();
            using var cmd = con.CreateCommand();

            cmd.CommandText = @"
                SELECT Id, Nama, NoHP, Email
                FROM Driver";

            using var reader = cmd.ExecuteReader();

            Console.WriteLine("\n=== DATA DRIVER TERDAFTAR ===");

            bool adaData = false;

            while (reader.Read())
            {
                adaData = true;

                Console.WriteLine($"ID    : {reader["Id"]}");
                Console.WriteLine($"Nama  : {reader["Nama"]}");
                Console.WriteLine($"No HP : {reader["NoHP"]}");
                Console.WriteLine($"Email : {reader["Email"]}");
                Console.WriteLine("-----------------------------");
            }

            if (!adaData)
            {
                Console.WriteLine("Belum ada akun Driver.");
            }
        }
    }
}
