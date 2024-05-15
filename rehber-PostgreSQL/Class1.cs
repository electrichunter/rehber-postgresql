using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
namespace rehber_PostgreSQL
{
    internal class Class1
    {

        string connString = "Host=localhost;Port=5432;Username=omer;Password=4002;Database=kişiler";

        public Class1(string connectionString)
        {
            connString = connectionString;
        }
        public void Delete(long id)
        {
            using (var connection = new NpgsqlConnection(connString))
            {
                connection.Open();
                string deleteSql = "DELETE FROM kişibilgileri WHERE id = @id";

                using (var command = new NpgsqlCommand(deleteSql, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.ExecuteNonQuery();
                }
            }
        }


            public void VerileriGoster(DataGridView dataGridView)
        {
            DataTable dt = VerileriGetir();
            dataGridView.DataSource = dt;
        }
        private DataTable VerileriGetir()
        {
            using (NpgsqlConnection conn = new NpgsqlConnection(connString))
            {
                conn.Open();
                string sql = "SELECT * FROM kişibilgileri";

                using (NpgsqlDataAdapter da = new NpgsqlDataAdapter(sql, conn))
                {
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                    return dt;
                }
            }
        }


        private string Random(int length)
        {
            Random random = new Random();
            const string chars = "0123456789qwertyuıopğüasdfghjklşiçmnbvcxz";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        public void ekle(string ad, string soyad, string telefon, string telefon2, string adres, string resimYolu)
        {
            using (var conn = new NpgsqlConnection(connString))
            {
                conn.Open();

                string sql = @"INSERT INTO kişibilgileri(ad, soyad, telefon, telefon2, adres, resimyolu)
                           VALUES (@ad, @soyad, @telefon, @telefon2, @adres, @resimYolu)";

                using (var cmd = new NpgsqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("ad", ad);
                    cmd.Parameters.AddWithValue("soyad", soyad);
                    cmd.Parameters.AddWithValue("telefon", telefon);
                    cmd.Parameters.AddWithValue("telefon2", telefon2);
                    cmd.Parameters.AddWithValue("adres", adres);

                    
                    if (string.IsNullOrEmpty(resimYolu))
                    {
                        string resimAdi = "varsayilan.png";
                        resimYolu = Path.Combine(@"C:\Users\ömer\Documents\resim\", resimAdi);
                    }

                    cmd.Parameters.AddWithValue("resimYolu", resimYolu);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
