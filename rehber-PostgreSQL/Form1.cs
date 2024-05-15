using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Npgsql;
namespace rehber_PostgreSQL
{

    public partial class Form1 : Form
    {
        string resimYolu;
        public Form1()
        {
            InitializeComponent();
            Class1 class1 = new Class1("Host=localhost;Port=5432;Username=omer;Password=4002;Database=kişiler");
            class1.VerileriGoster(dataGridView1);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "Resim dosyaları|*.jpg;*.jpeg;*.png;*.bmp"; // Sadece resim dosyalarını göster
                    openFileDialog.Title = "Resim Seç";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string resimYolu = openFileDialog.FileName;
                        pictureBox1.Image = Image.FromFile(resimYolu); // Resmi PictureBox'ta göster
                        pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
                    }

                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Class1 class1 = new Class1("Host=localhost;Port=5432;Username=omer;Password=4002;Database=kişiler");
            string ad = textBox1.Text;
            string soyad = textBox2.Text;
            string telefon = textBox3.Text;
            string telefon2 = textBox4.Text;
            string adres = textBox5.Text;




            class1.ekle(ad, soyad, telefon, telefon2, adres, resimYolu);

            MessageBox.Show("Kişi başarıyla kaydedildi.");
        }
         
        private void Güncelle_btn_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0) // Ensure at least one row is selected
            {
                using (var connection = new NpgsqlConnection("Host=localhost;Port=5432;Username=omer;Password=4002;Database=kişiler"))
                {
                    connection.Open();

                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        string updateSql = @"UPDATE kişibilgileri
                                 SET ad = @ad, soyad = @soyad, telefon = @telefon, telefon2 = @telefon2, adres = @adres, resimyolu = @resimYolu
                                 WHERE id = @id";

                        using (var command = new NpgsqlCommand(updateSql, connection))
                        {
                            command.Parameters.Add("@ad", NpgsqlTypes.NpgsqlDbType.Text).Value = row.Cells["ad"].Value.ToString();
                            command.Parameters.Add("@soyad", NpgsqlTypes.NpgsqlDbType.Text).Value = row.Cells["soyad"].Value.ToString();
                            command.Parameters.Add("@telefon", NpgsqlTypes.NpgsqlDbType.Text).Value = row.Cells["telefon"].Value.ToString();
                            command.Parameters.Add("@telefon2", NpgsqlTypes.NpgsqlDbType.Text).Value = row.Cells["telefon2"].Value.ToString();
                            command.Parameters.Add("@adres", NpgsqlTypes.NpgsqlDbType.Text).Value = row.Cells["adres"].Value.ToString();
                            command.Parameters.Add("@resimYolu", NpgsqlTypes.NpgsqlDbType.Text).Value = row.Cells["resimyolu"].Value.ToString();
                            command.Parameters.Add("@id", NpgsqlTypes.NpgsqlDbType.Bigint).Value = Convert.ToInt64(row.Cells["id"].Value);

                            command.ExecuteNonQuery();
                        }
                        MessageBox.Show("Güncelleme işlemi tamamlandı");

                        dataGridView1.Refresh();
                    }
                }
            }
            else 
            {
                MessageBox.Show("Lütfen güncellemek için bir satır seçin.", "Seçim Gerekli", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0) 
            {
                
                dataGridView1.EditMode = DataGridViewEditMode.EditOnEnter;
            }
        }
   
        private void button3_Click(object sender, EventArgs e)

        {
          

            if (dataGridView1.SelectedRows.Count > 0) // En az bir satır seçildiğinden emin olun
            {
                Class1 class1 = new Class1("Host=localhost;Port=5432;Username=omer;Password=4002;Database=kişiler");
                // Kullanıcıya silme işlemini onaylamasını isteyin
                DialogResult result = MessageBox.Show("Seçili kişiyi silmek istediğinizden emin misiniz?", "Kişi Silme", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

                // Kullanıcı eveti seçtiyse
                if (result == DialogResult.Yes)
                {
                    // Seçili satırlar üzerinde döngü
                    foreach (DataGridViewRow row in dataGridView1.SelectedRows)
                    {
                        // Seçili kişinin id'sini al
                        int id = Convert.ToInt32(row.Cells["id"].Value);

                        // Veritabanından kişiyi sil
                        class1.Delete(id);

                        // DataGridView'i güncelle
                        dataGridView1.Rows.Remove(row);
                    }
                }
            }
            else
            {
                MessageBox.Show("Lütfen silmek için bir kişi seçin.", "Seçim Gerekli", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            // DataGridView'de bir satır seçildiğinde çalışacak kod
            if (dataGridView1.SelectedRows.Count > 0) // En az bir satır seçildiğinden emin olun
            {
                // Seçili satırı alın
                DataGridViewRow selectedRow = dataGridView1.SelectedRows[0];

                // Seçili satırın resim yolunu alın
                string resimYolu = selectedRow.Cells["resimyolu"].Value.ToString();

                // Resim yolunu kontrol edin
                if (File.Exists(resimYolu))
                {
                    // Resim yolunu PictureBox'a yükle
                    pictureBox1.Image = Image.FromFile(resimYolu);
                    pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;

                    // Bir saniye bekleyin
                    System.Threading.Thread.Sleep(1000);
                }
                else
                {
                    // Hatalı resim yolu olduğunu belirtin
                    MessageBox.Show("Hatalı resim yolu: " + resimYolu, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                // DataGridView'de hiç satır seçilmediyse, PictureBox'ı temizleyin
                pictureBox1.Image = null;
            }
        }
    }
    }

