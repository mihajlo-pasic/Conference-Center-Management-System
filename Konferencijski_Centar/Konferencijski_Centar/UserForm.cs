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
    public partial class UserForm : Form
    {
        public UserForm()
        {
            InitializeComponent();
        }

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["konferencijski_centar"].ConnectionString;

        private void LoadData()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Učitavanje organizatora
                MySqlCommand command = new MySqlCommand("SELECT Id_Organizatora, Ime, Kontakt_Informacije FROM ORGANIZATOR ORDER BY Id_Organizatora", connection);
                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;

                // Učitavanje događaja
                MySqlCommand commandDogadjaji = new MySqlCommand("SELECT Id_Dogadjaja, Naziv, Opis, DatumPocetka, DatumZavrsetka FROM DOGADJAJ ORDER BY Id_Dogadjaja", connection);
                MySqlDataAdapter adapterDogadjaji = new MySqlDataAdapter(commandDogadjaji);
                DataTable dataTableDogadjaji = new DataTable();
                adapterDogadjaji.Fill(dataTableDogadjaji);
                dataGridView5.DataSource = dataTableDogadjaji;

                // Učitavanje sesija
                MySqlCommand commandSesije = new MySqlCommand(@"
                SELECT 
                    S.Id_Sesije, 
                    D.Naziv AS NazivDogadjaja, 
                    S.Naziv, 
                    S.Opis, 
                    S.Datum, 
                    S.VrijemePocetka, 
                    S.VrijemeZavrsetka, 
                    P.Naziv_Prostorije AS Prostorija
                FROM 
                    SESIJA S
                JOIN 
                    DOGADJAJ D ON S.Id_Dogadjaja = D.Id_Dogadjaja
                JOIN 
                    PROSTORIJA P ON S.PROSTORIJA_Id_Prostorije = P.Id_Prostorije
                ORDER BY 
                    S.Id_Sesije", connection);
                MySqlDataAdapter adapterSesije = new MySqlDataAdapter(commandSesije);
                DataTable dataTableSesije = new DataTable();
                adapterSesije.Fill(dataTableSesije);
                dataGridView6.DataSource = dataTableSesije;


                // Učitavanje prostorija
                MySqlCommand commandProstorije = new MySqlCommand("SELECT Id_Prostorije, Naziv_Prostorije, Kapacitet FROM PROSTORIJA ORDER BY Id_Prostorije", connection);
                MySqlDataAdapter adapterProstorije = new MySqlDataAdapter(commandProstorije);
                DataTable dataTableProstorije = new DataTable();
                adapterProstorije.Fill(dataTableProstorije);
                dataGridView4.DataSource = dataTableProstorije;


                // Učitavanje predavača
                MySqlCommand commandPredavaci = new MySqlCommand("SELECT Id_Predavaca, Ime, Prezime, Tema_Predavanja FROM PREDAVAC ORDER BY Id_Predavaca", connection);
                MySqlDataAdapter adapterPredavaci = new MySqlDataAdapter(commandPredavaci);
                DataTable dataTablePredavaci = new DataTable();
                adapterPredavaci.Fill(dataTablePredavaci);
                dataGridView2.DataSource = dataTablePredavaci;

                // Učitavanje učesnika
                MySqlCommand commandUcesnici = new MySqlCommand("SELECT Id_Ucesnika, Ime, Prezime, Email, Telefon FROM UCESNIK ORDER BY Id_Ucesnika", connection);
                MySqlDataAdapter adapterUcesnici = new MySqlDataAdapter(commandUcesnici);
                DataTable dataTableUcesnici = new DataTable();
                adapterUcesnici.Fill(dataTableUcesnici);
                dataGridView3.DataSource = dataTableUcesnici;

                connection.Close();
            }
        }

        private void button9_Click(object sender, EventArgs e) // Dugme za dodavanje učesnika
        {
            using (UcesnikForm ucesnikForm = new UcesnikForm())
            {
                if (ucesnikForm.ShowDialog() == DialogResult.OK)
                {
                    string ime = ucesnikForm.Ime;
                    string prezime = ucesnikForm.Prezime;
                    string email = ucesnikForm.Email;
                    string telefon = ucesnikForm.Telefon;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // Proverava da li već postoji učesnik sa istim imenom i prezimenom
                        MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM UCESNIK WHERE Ime = @ime AND Prezime = @prezime", connection);
                        checkCommand.Parameters.AddWithValue("@ime", ime);
                        checkCommand.Parameters.AddWithValue("@prezime", prezime);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Učesnik sa tim imenom i prezimenom već postoji.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Ako ne postoji, dodaj novog učesnika
                        MySqlCommand command = new MySqlCommand("INSERT INTO UCESNIK (Ime, Prezime, Email, Telefon) VALUES (@ime, @prezime, @email, @telefon)", connection);
                        command.Parameters.AddWithValue("@ime", ime);
                        command.Parameters.AddWithValue("@prezime", prezime);
                        command.Parameters.AddWithValue("@email", email);
                        command.Parameters.AddWithValue("@telefon", telefon);
                        command.ExecuteNonQuery();

                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }
        }

        private void button8_Click(object sender, EventArgs e) // Dugme za izmenu
        {
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati učesnika za izmenu.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView3.SelectedRows[0];
            int ucesnikId = Convert.ToInt32(selectedRow.Cells["Id_Ucesnika"].Value);
            string ime = selectedRow.Cells["Ime"].Value.ToString();
            string prezime = selectedRow.Cells["Prezime"].Value.ToString();
            string email = selectedRow.Cells["Email"].Value.ToString();
            string telefon = selectedRow.Cells["Telefon"].Value.ToString();

            using (UcesnikForm ucesnikForm = new UcesnikForm(ime, prezime, email, telefon))
            {
                if (ucesnikForm.ShowDialog() == DialogResult.OK)
                {
                    // Ažuriranje učesnika u bazi podataka
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("UPDATE UCESNIK SET Ime = @ime, Prezime = @prezime, Email = @email, Telefon = @telefon WHERE Id_Ucesnika = @id", connection);
                        command.Parameters.AddWithValue("@ime", ucesnikForm.Ime);
                        command.Parameters.AddWithValue("@prezime", ucesnikForm.Prezime);
                        command.Parameters.AddWithValue("@email", ucesnikForm.Email);
                        command.Parameters.AddWithValue("@telefon", ucesnikForm.Telefon);
                        command.Parameters.AddWithValue("@id", ucesnikId);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }
        }

        private void button7_Click(object sender, EventArgs e) // Dugme za uklanjanje
        {
            if (dataGridView3.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati učesnika za uklanjanje.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView3.SelectedRows[0];
            int ucesnikId = Convert.ToInt32(selectedRow.Cells["Id_Ucesnika"].Value);

            // Potvrda brisanja
            DialogResult result = MessageBox.Show("Da li ste sigurni da želite ukloniti ovog učesnika i sve njegove prijave?", "Potvrda brisanja", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Prvo brisanje svih prijava za učesnika
                    MySqlCommand deletePrijaveCommand = new MySqlCommand("DELETE FROM PRIJAVA WHERE Id_Ucesnika = @id", connection);
                    deletePrijaveCommand.Parameters.AddWithValue("@id", ucesnikId);
                    deletePrijaveCommand.ExecuteNonQuery();

                    // Zatim brisanje učesnika
                    MySqlCommand command = new MySqlCommand("DELETE FROM UCESNIK WHERE Id_Ucesnika = @id", connection);
                    command.Parameters.AddWithValue("@id", ucesnikId);
                    command.ExecuteNonQuery();

                    connection.Close();
                }

                LoadData(); // Osvježavanje DataGridView
            }
        }

        private void UserForm_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void button21_Click(object sender, EventArgs e)
        {
            PrijavaForm login = new PrijavaForm();
            this.Hide();
            login.Show();
        }

        private void UserForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            string searchValue = textBox3.Text.Trim();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SELECT Id_Ucesnika, Ime, Prezime, Email, Telefon FROM UCESNIK WHERE Ime LIKE @searchValue OR Prezime LIKE @searchValue", connection);
                command.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView3.DataSource = dataTable;

                connection.Close();
            }
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int organizatorId = Convert.ToInt32(dataGridView1.Rows[e.RowIndex].Cells["Id_Organizatora"].Value);

                // Otvorite formu za organizatora
                Prikaz prikazForm = new Prikaz();
                prikazForm.LoadData(organizatorId, "Organizator");
                prikazForm.Show();
            }
        }

        private void dataGridView5_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridView5.Rows[e.RowIndex].Cells["Id_Dogadjaja"].Value);
                Prikaz prikazFormDogadjaj = new Prikaz();
                prikazFormDogadjaj.LoadData(id, "Dogadjaj");
                prikazFormDogadjaj.Show();
            }
        }

        private void dataGridView6_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridView6.Rows[e.RowIndex].Cells["Id_Sesije"].Value);
                Prikaz prikazFormDogadjaj = new Prikaz();
                prikazFormDogadjaj.LoadData(id, "Sesija");
                prikazFormDogadjaj.Show();
            }
        }

        private void dataGridView4_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridView4.Rows[e.RowIndex].Cells["Id_Prostorije"].Value);
                Prikaz prikazFormProstorija = new Prikaz();
                prikazFormProstorija.LoadData(id, "Prostorija");
                prikazFormProstorija.Show();
            }
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                int id = Convert.ToInt32(dataGridView2.Rows[e.RowIndex].Cells["Id_Predavaca"].Value);
                Prikaz prikazFormPredavac = new Prikaz();
                prikazFormPredavac.LoadData(id, "Predavac");
                prikazFormPredavac.Show();
            }
        }

        private void dataGridView3_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                // Uzmi ID učesnika sa odabranog reda
                int ucesnikId = Convert.ToInt32(dataGridView3.Rows[e.RowIndex].Cells["Id_Ucesnika"].Value);

                // Otvori formu za prijave učesnika
                PrijaveForm prijaveForm = new PrijaveForm(ucesnikId);
                prijaveForm.Show();
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            string searchValue = textBox1.Text.Trim();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SELECT Id_Organizatora, Ime, Kontakt_Informacije FROM ORGANIZATOR WHERE Ime LIKE @searchValue", connection);
                command.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView1.DataSource = dataTable;

                connection.Close();
            }
        }

        private void textBox5_TextChanged(object sender, EventArgs e)
        {
            string searchValue = textBox5.Text.Trim();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SELECT Id_Dogadjaja, Naziv, Opis, DatumPocetka, DatumZavrsetka FROM DOGADJAJ WHERE Naziv LIKE @searchValue OR Opis LIKE @searchValue", connection);
                command.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView5.DataSource = dataTable;

                connection.Close();
            }
        }

        private void textBox6_TextChanged(object sender, EventArgs e)
        {
            string searchValue = textBox6.Text.Trim();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SELECT Id_Sesije, Id_Dogadjaja, Naziv, Opis, Datum, VrijemePocetka, VrijemeZavrsetka FROM SESIJA WHERE Naziv LIKE @searchValue OR Opis LIKE @searchValue", connection);
                command.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView6.DataSource = dataTable;

                connection.Close();
            }
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {
            string searchValue = textBox4.Text.Trim();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SELECT Id_Prostorije, Naziv_Prostorije, Kapacitet FROM PROSTORIJA WHERE Naziv_Prostorije LIKE @searchValue", connection);
                command.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView4.DataSource = dataTable;

                connection.Close();
            }
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            string searchValue = textBox2.Text.Trim();

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                MySqlCommand command = new MySqlCommand("SELECT Id_Predavaca, Ime, Prezime, Tema_Predavanja FROM PREDAVAC WHERE Ime LIKE @searchValue OR Prezime LIKE @searchValue", connection);
                command.Parameters.AddWithValue("@searchValue", "%" + searchValue + "%");

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridView2.DataSource = dataTable;

                connection.Close();
            }
        }
    }
}
