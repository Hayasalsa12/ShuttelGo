
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using ShuttleGO.Models;

namespace ShuttleGO.Repositories
{
    public class PenumpangRepository
    {
        public void Tambah(Penumpang penumpang)
        {
            using var connection = DatabaseHelper.GetConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                INSERT INTO Penumpang
                (Nama, NoHP, Email, PasswordHash)
                VALUES ($Nama, $NoHP, $Email, $Password)";

            command.Parameters.AddWithValue("$Nama", penumpang.Nama);
            command.Parameters.AddWithValue("$NoHP", penumpang.NoHP);
            command.Parameters.AddWithValue("$Email",
                (object?)penumpang.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("$Password",
                (object?)penumpang.Password ?? DBNull.Value);

            command.ExecuteNonQuery();
        }

        public List<Penumpang> GetAll()
        {
            var daftar = new List<Penumpang>();

            using var connection = DatabaseHelper.GetConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT IdPenumpang, Nama, NoHP, Email, PasswordHash
                FROM Penumpang";

            using var reader = command.ExecuteReader();

            while (reader.Read())
            {
                daftar.Add(new Penumpang
                {
                    Id = reader.GetInt32(0),
                    Nama = reader.GetString(1),
                    NoHP = reader.GetString(2),
                    Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                    Password = reader.IsDBNull(4) ? "" : reader.GetString(4)
                });
            }

            return daftar;
        }

        public Penumpang? GetById(int id)
        {
            using var connection = DatabaseHelper.GetConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT IdPenumpang, Nama, NoHP, Email, PasswordHash
                FROM Penumpang
                WHERE IdPenumpang = $Id";

            command.Parameters.AddWithValue("$Id", id);

            using var reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Penumpang
            {
                Id = reader.GetInt32(0),
                Nama = reader.GetString(1),
                NoHP = reader.GetString(2),
                Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Password = reader.IsDBNull(4) ? "" : reader.GetString(4)
            };
        }

        public Penumpang? GetByEmail(string email)
        {
            using var connection = DatabaseHelper.GetConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                SELECT IdPenumpang, Nama, NoHP, Email, PasswordHash
                FROM Penumpang
                WHERE Email = $Email COLLATE NOCASE";

            command.Parameters.AddWithValue("$Email", email.Trim());

            using var reader = command.ExecuteReader();

            if (!reader.Read())
                return null;

            return new Penumpang
            {
                Id = reader.GetInt32(0),
                Nama = reader.GetString(1),
                NoHP = reader.GetString(2),
                Email = reader.IsDBNull(3) ? "" : reader.GetString(3),
                Password = reader.IsDBNull(4) ? "" : reader.GetString(4)
            };
        }

        public void Update(Penumpang penumpang)
        {
            using var connection = DatabaseHelper.GetConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                UPDATE Penumpang
                SET Nama = $Nama,
                    NoHP = $NoHP,
                    Email = $Email,
                    PasswordHash = $Password
                WHERE IdPenumpang = $Id";

            command.Parameters.AddWithValue("$Id", penumpang.Id);
            command.Parameters.AddWithValue("$Nama", penumpang.Nama);
            command.Parameters.AddWithValue("$NoHP", penumpang.NoHP);
            command.Parameters.AddWithValue("$Email",
                (object?)penumpang.Email ?? DBNull.Value);
            command.Parameters.AddWithValue("$Password",
                (object?)penumpang.Password ?? DBNull.Value);

            command.ExecuteNonQuery();
        }

        public void Hapus(int id)
        {
            using var connection = DatabaseHelper.GetConnection();
            using var command = connection.CreateCommand();

            command.CommandText = @"
                DELETE FROM Penumpang
                WHERE IdPenumpang = $Id";

            command.Parameters.AddWithValue("$Id", id);

            command.ExecuteNonQuery();
        }
    }
}
