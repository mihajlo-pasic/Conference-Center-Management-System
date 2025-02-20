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
    public partial class PrijaveForm : Form
    {
        private int ucesnikId;

        public PrijaveForm(int ucesnikId)
        {
            InitializeComponent();
            this.ucesnikId = ucesnikId;
            LoadPrijave();
            LoadDogadjaje(); // Popuni comboBox1
        }

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["konferencijski_centar"].ConnectionString;

        private void LoadPrijave()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(@"
                SELECT s.Naziv AS Sesija, d.Naziv AS Dogadjaj, s.Datum, s.VrijemePocetka, s.VrijemeZavrsetka, p.Id_Prijave
                FROM PRIJAVA p
                JOIN SESIJA s ON p.Id_Sesije = s.Id_Sesije
                JOIN DOGADJAJ d ON s.Id_Dogadjaja = d.Id_Dogadjaja
                WHERE p.Id_Ucesnika = @ucesnikId", connection);
                command.Parameters.AddWithValue("@ucesnikId", ucesnikId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
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

        private void buttonPrijavi_Click(object sender, EventArgs e)
        {
            // Provera da li su odabrani događaj i sesija
            if (comboBox1.SelectedItem != null && comboBox2.SelectedItem != null)
            {
                int dogadjajId = ((dynamic)comboBox1.SelectedItem).Id;
                int sesijaId = ((dynamic)comboBox2.SelectedItem).Id;

                // Proveravamo da li je učesnik već prijavljen za ovu sesiju
                bool isAlreadyRegistered = false;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    MySqlCommand checkCommand = new MySqlCommand(@"
                SELECT COUNT(*) 
                FROM PRIJAVA 
                WHERE Id_Ucesnika = @ucesnikId AND Id_Sesije = @sesijaId", connection);

                    checkCommand.Parameters.AddWithValue("@ucesnikId", ucesnikId);
                    checkCommand.Parameters.AddWithValue("@sesijaId", sesijaId);

                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (count > 0)
                    {
                        isAlreadyRegistered = true;
                    }

                    connection.Close();
                }

                if (isAlreadyRegistered)
                {
                    MessageBox.Show("Učesnik je već prijavljen na ovu sesiju.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Ako nije prijavljen, dodajemo prijavu
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand(@"
                    INSERT INTO PRIJAVA (Id_Ucesnika, Id_Sesije) 
                    VALUES (@ucesnikId, @sesijaId)", connection);
                        command.Parameters.AddWithValue("@ucesnikId", ucesnikId);
                        command.Parameters.AddWithValue("@sesijaId", sesijaId);
                        command.ExecuteNonQuery();
                    }

                    LoadPrijave(); // Osvježavanje dataGridView1
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati događaj i sesiju.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonOdjavi_Click(object sender, EventArgs e)
        {
            // Odjavi učesnika sa sesije
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int prijavaId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id_Prijave"].Value);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("DELETE FROM PRIJAVA WHERE Id_Prijave = @prijavaId", connection);
                    command.Parameters.AddWithValue("@prijavaId", prijavaId);
                    command.ExecuteNonQuery();
                }

                LoadPrijave(); // Osvježavanje dataGridView1
            }
            else
            {
                MessageBox.Show("Morate selektovati prijavu za odjavu.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
