using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Konferencijski_Centar
{
    public partial class UvezivanjePredavacSesija : Form
    {
        private int predavacId;

        public UvezivanjePredavacSesija(int predavacId)
        {
            InitializeComponent();
            this.predavacId = predavacId;
            LoadConnectedSessions();
            LoadDogadjaje();
            //LoadAvailableSessions();
        }

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["konferencijski_centar"].ConnectionString;

        private void LoadConnectedSessions()
        {
            // Učitaj sesije koje su već povezane sa predavačem
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand(@"
            SELECT 
                s.Id_Sesije, 
                s.Naziv AS Sesija, 
                d.Naziv AS Dogadjaj, 
                s.Datum, 
                s.VrijemePocetka, 
                s.VrijemeZavrsetka, 
                p.Naziv_Prostorije
            FROM 
                SESIJA s
            JOIN 
                DOGADJAJ d ON s.Id_Dogadjaja = d.Id_Dogadjaja
            JOIN 
                PROSTORIJA p ON s.PROSTORIJA_Id_Prostorije = p.Id_Prostorije
            JOIN 
                PREDAVAC_SESIJA ps ON s.Id_Sesije = ps.SESIJA_Id_Sesije
            WHERE 
                ps.PREDAVAC_Id_Predavaca = @predavacId", connection);

                command.Parameters.AddWithValue("@predavacId", predavacId); // predavacId je ID predavača koji ste dobili

                // Napravite adapter za učitavanje podataka
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                // Prikazivanje podataka u dataGridView
                dataGridView1.DataSource = dataTable;
            }
        }


        private void LoadDogadjaje()
        {
            // Popuni comboBox1 sa svim dostupnim događajima
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT Id_Dogadjaja, Naziv FROM DOGADJAJ", connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(new { Id = reader["Id_Dogadjaja"], Naziv = reader["Naziv"] });
                }
                reader.Close();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kada korisnik izabere događaj, popuni dostupne sesije
            if (comboBox1.SelectedItem != null)
            {
                int dogadjajId = ((dynamic)comboBox1.SelectedItem).Id;
                LoadSesije(dogadjajId);
            }
        }

        private void LoadSesije(int dogadjajId)
        {
            // Popuni comboBox2 sa sesijama povezanim sa odabranim događajem
            comboBox2.Items.Clear();
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(@"
                SELECT Id_Sesije, Naziv 
                FROM SESIJA 
                WHERE Id_Dogadjaja = @dogadjajId", connection);
                command.Parameters.AddWithValue("@dogadjajId", dogadjajId);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox2.Items.Add(new { Id = reader["Id_Sesije"], Naziv = reader["Naziv"] });
                }
                reader.Close();
            }
        }

        private void LoadAvailableSessions()
        {
            // Učitaj sve dostupne sesije
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT Id_Sesije, Naziv FROM SESIJA", connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBox1.Items.Add(new { Id = reader["Id_Sesije"], Naziv = reader["Naziv"] });
                }
                reader.Close();
            }
        }

        private void buttonUvezi_Click(object sender, EventArgs e)
        {
            // Proverite da li je sesija izabrana u comboBox2
            if (comboBox2.SelectedItem != null)
            {
                int sesijaId = ((dynamic)comboBox2.SelectedItem).Id; // Koristite comboBox2 da biste uzeli selektovanu sesiju

                // Provera da li već postoji veza između predavača i sesije
                bool isAlreadyConnected = false;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    MySqlCommand checkCommand = new MySqlCommand(@"
            SELECT COUNT(*) 
            FROM PREDAVAC_SESIJA 
            WHERE PREDAVAC_Id_Predavaca = @predavacId AND SESIJA_Id_Sesije = @sesijaId", connection);

                    checkCommand.Parameters.AddWithValue("@predavacId", predavacId);
                    checkCommand.Parameters.AddWithValue("@sesijaId", sesijaId);

                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (count > 0)
                    {
                        isAlreadyConnected = true;
                    }

                    connection.Close();
                }

                if (isAlreadyConnected)
                {
                    MessageBox.Show("Predavač je već povezan sa ovom sesijom.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Dodavanje veze predavača sa sesijom
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand(@"
                INSERT INTO PREDAVAC_SESIJA (PREDAVAC_Id_Predavaca, SESIJA_Id_Sesije) 
                VALUES (@predavacId, @sesijaId)", connection);
                        command.Parameters.AddWithValue("@predavacId", predavacId);
                        command.Parameters.AddWithValue("@sesijaId", sesijaId);
                        command.ExecuteNonQuery();
                    }

                    LoadConnectedSessions(); // Osvježavanje podataka u dataGridView1
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati sesiju za povezivanje.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonOdvezi_Click(object sender, EventArgs e)
        {
            // Odvezivanje predavača od selektovane sesije
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int sesijaId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id_Sesije"].Value);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("DELETE FROM PREDAVAC_SESIJA WHERE PREDAVAC_Id_Predavaca = @predavacId AND SESIJA_Id_Sesije = @sesijaId", connection);
                    command.Parameters.AddWithValue("@predavacId", predavacId);
                    command.Parameters.AddWithValue("@sesijaId", sesijaId);
                    command.ExecuteNonQuery();
                }

                LoadConnectedSessions(); // Osvježavanje podataka u dataGridView1
            }
            else
            {
                MessageBox.Show("Morate selektovati sesiju za odvezivanje.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
