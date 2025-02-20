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
    public partial class UvezivanjeForm : Form
    {
        private int organizatorId;

        public UvezivanjeForm(int organizatorId)
        {
            InitializeComponent();
            this.organizatorId = organizatorId;
            LoadConnectedEvents();
            LoadAvailableEvents();
        }
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["konferencijski_centar"].ConnectionString;

        private void LoadConnectedEvents()
        {
            // Učitaj događaje koji su već povezani sa organizatorom
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(@"
                SELECT d.Id_Dogadjaja, d.Naziv
                FROM DOGADJAJ d
                JOIN ORGANIZATOR_DOGADJAJ od ON d.Id_Dogadjaja = od.DOGADJAJ_Id_Dogadjaja
                WHERE od.ORGANIZATOR_Id_Organizatora = @organizatorId", connection);
                command.Parameters.AddWithValue("@organizatorId", organizatorId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
            }
        }

        private void LoadAvailableEvents()
        {
            // Učitaj sve događaje koji nisu povezani sa organizatorom
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

        private void buttonUvezi_Click(object sender, EventArgs e)
        {
            // Dodavanje veze organizatora sa odabranim događajem
            if (comboBox1.SelectedItem != null)
            {
                int dogadjajId = ((dynamic)comboBox1.SelectedItem).Id;

                // Provera da li već postoji veza između organizatora i događaja
                bool isAlreadyConnected = false;

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    MySqlCommand checkCommand = new MySqlCommand(@"
                SELECT COUNT(*) 
                FROM ORGANIZATOR_DOGADJAJ 
                WHERE ORGANIZATOR_Id_Organizatora = @organizatorId AND DOGADJAJ_Id_Dogadjaja = @dogadjajId", connection);

                    checkCommand.Parameters.AddWithValue("@organizatorId", organizatorId);
                    checkCommand.Parameters.AddWithValue("@dogadjajId", dogadjajId);

                    int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                    if (count > 0)
                    {
                        isAlreadyConnected = true;
                    }

                    connection.Close();
                }

                if (isAlreadyConnected)
                {
                    // Ako je događaj već uparen, obavesti korisnika
                    MessageBox.Show("Događaj je već povezan sa organizatorom.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    // Ako nije povezano, dodajemo vezu
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand(@"
                    INSERT INTO ORGANIZATOR_DOGADJAJ (ORGANIZATOR_Id_Organizatora, DOGADJAJ_Id_Dogadjaja) 
                    VALUES (@organizatorId, @dogadjajId)", connection);
                        command.Parameters.AddWithValue("@organizatorId", organizatorId);
                        command.Parameters.AddWithValue("@dogadjajId", dogadjajId);
                        command.ExecuteNonQuery();
                    }

                    LoadConnectedEvents(); // Osvježavanje podataka u dataGridView1
                    comboBox1.Items.Clear(); // Očisti comboBox1
                    LoadAvailableEvents(); // Ponovo učitaj sve dostupne događaje
                }
            }
            else
            {
                MessageBox.Show("Morate izabrati događaj za povezivanje.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void buttonOdvezi_Click(object sender, EventArgs e)
        {
            // Odvezivanje organizatora od selektovanog događaja
            if (dataGridView1.SelectedRows.Count > 0)
            {
                int dogadjajId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id_Dogadjaja"].Value);

                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("DELETE FROM ORGANIZATOR_DOGADJAJ WHERE ORGANIZATOR_Id_Organizatora = @organizatorId AND DOGADJAJ_Id_Dogadjaja = @dogadjajId", connection);
                    command.Parameters.AddWithValue("@organizatorId", organizatorId);
                    command.Parameters.AddWithValue("@dogadjajId", dogadjajId);
                    command.ExecuteNonQuery();
                }

                LoadConnectedEvents(); // Osvježavanje podataka u dataGridView1
            }
            else
            {
                MessageBox.Show("Morate selektovati događaj za odvezivanje.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
