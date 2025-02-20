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
    public partial class AdminForm : Form
    {
        public AdminForm()
        {
            InitializeComponent();
        }

        private void tabControl1_Click(object sender, EventArgs e)
        {
            LoadData();
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

        private void AdminForm_Load(object sender, EventArgs e)
        {
            LoadData();
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

        private void button1_Click(object sender, EventArgs e)
        {
            using (OrganizatorForm organizatorForm = new OrganizatorForm())
            {
                if (organizatorForm.ShowDialog() == DialogResult.OK)
                {
                    string ime = organizatorForm.Ime;
                    string kontaktInformacije = organizatorForm.KontaktInformacije;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // Proverava da li već postoji organizator sa istim imenom
                        MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM ORGANIZATOR WHERE Ime = @ime", connection);
                        checkCommand.Parameters.AddWithValue("@ime", ime);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Organizator sa tim imenom već postoji.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Ako ne postoji, dodaj novog organizatora
                        MySqlCommand command = new MySqlCommand("INSERT INTO ORGANIZATOR (Ime, Kontakt_Informacije) VALUES (@ime, @kontakt)", connection);
                        command.Parameters.AddWithValue("@ime", ime);
                        command.Parameters.AddWithValue("@kontakt", kontaktInformacije);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati organizatora za izmenu.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int organizatorId = Convert.ToInt32(selectedRow.Cells["Id_Organizatora"].Value);
            string ime = selectedRow.Cells["Ime"].Value.ToString();
            string kontaktInformacije = selectedRow.Cells["Kontakt_Informacije"].Value.ToString();

            using (OrganizatorForm organizatorForm = new OrganizatorForm(ime, kontaktInformacije))
            {
                if (organizatorForm.ShowDialog() == DialogResult.OK)
                {
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("UPDATE ORGANIZATOR SET Ime = @ime, Kontakt_Informacije = @kontakt WHERE Id_Organizatora = @id", connection);
                        command.Parameters.AddWithValue("@ime", organizatorForm.Ime);
                        command.Parameters.AddWithValue("@kontakt", organizatorForm.KontaktInformacije);
                        command.Parameters.AddWithValue("@id", organizatorId);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati organizatora za uklanjanje.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView1.SelectedRows[0];
            int organizatorId = Convert.ToInt32(selectedRow.Cells["Id_Organizatora"].Value);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Proverite da li postoji događaj vezan za organizatora
                MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM ORGANIZATOR_DOGADJAJ WHERE ORGANIZATOR_Id_Organizatora = @id", connection);
                checkCommand.Parameters.AddWithValue("@id", organizatorId);
                int eventCount = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (eventCount > 0)
                {
                    MessageBox.Show("Organizator se ne može ukloniti jer je vezan za postojeći događaj.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Potvrda brisanja
                DialogResult result = MessageBox.Show("Da li ste sigurni da želite ukloniti ovog organizatora?", "Potvrda brisanja", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    // Preporučuje se prvo brisanje iz povezanih tabela
                    MySqlCommand deleteReferencesCommand = new MySqlCommand("DELETE FROM ORGANIZATOR_DOGADJAJ WHERE ORGANIZATOR_Id_Organizatora = @id", connection);
                    deleteReferencesCommand.Parameters.AddWithValue("@id", organizatorId);
                    deleteReferencesCommand.ExecuteNonQuery();

                    // Zatim brisanje organizatora
                    MySqlCommand command = new MySqlCommand("DELETE FROM ORGANIZATOR WHERE Id_Organizatora = @id", connection);
                    command.Parameters.AddWithValue("@id", organizatorId);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            LoadData(); // Osvježavanje DataGridView
        }

        private void AdminForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
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

        private void button6_Click(object sender, EventArgs e)
        {
            using (PredavacForm predavacForm = new PredavacForm())
            {
                if (predavacForm.ShowDialog() == DialogResult.OK)
                {
                    string ime = predavacForm.Ime;
                    string prezime = predavacForm.Prezime;
                    string temaPredavanja = predavacForm.TemaPredavanja;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // Proverava da li već postoji predavač sa istim imenom i prezimenom
                        MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM PREDAVAC WHERE Ime = @ime AND Prezime = @prezime", connection);
                        checkCommand.Parameters.AddWithValue("@ime", ime);
                        checkCommand.Parameters.AddWithValue("@prezime", prezime);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Predavač sa istim imenom i prezimenom već postoji.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Ako ne postoji, dodaj novog predavača
                        MySqlCommand command = new MySqlCommand("INSERT INTO PREDAVAC (Ime, Prezime, Tema_Predavanja) VALUES (@ime, @prezime, @tema)", connection);
                        command.Parameters.AddWithValue("@ime", ime);
                        command.Parameters.AddWithValue("@prezime", prezime);
                        command.Parameters.AddWithValue("@tema", temaPredavanja);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }

        }

        private void button5_Click(object sender, EventArgs e) // Dugme za izmenu
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati predavača za izmenu.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView2.SelectedRows[0];
            int predavacId = Convert.ToInt32(selectedRow.Cells["Id_Predavaca"].Value);
            string ime = selectedRow.Cells["Ime"].Value.ToString();
            string prezime = selectedRow.Cells["Prezime"].Value.ToString();
            string tema = selectedRow.Cells["Tema_Predavanja"].Value.ToString();

            using (PredavacForm predavacForm = new PredavacForm(ime, prezime, tema))
            {
                if (predavacForm.ShowDialog() == DialogResult.OK)
                {
                    // Ažuriranje predavača u bazi podataka
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("UPDATE PREDAVAC SET Ime = @ime, Prezime = @prezime, Tema_Predavanja = @tema WHERE Id_Predavaca = @id", connection);
                        command.Parameters.AddWithValue("@ime", predavacForm.Ime);
                        command.Parameters.AddWithValue("@prezime", predavacForm.Prezime);
                        command.Parameters.AddWithValue("@tema", predavacForm.TemaPredavanja);
                        command.Parameters.AddWithValue("@id", predavacId);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }
        }

        private void button4_Click(object sender, EventArgs e) // Dugme za uklanjanje
        {
            if (dataGridView2.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati predavača za uklanjanje.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView2.SelectedRows[0];
            int predavacId = Convert.ToInt32(selectedRow.Cells["Id_Predavaca"].Value);

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Proverava da li postoji sesija vezana za predavača
                MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM PREDAVAC_SESIJA WHERE PREDAVAC_Id_Predavaca = @id", connection);
                checkCommand.Parameters.AddWithValue("@id", predavacId);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                if (count > 0)
                {
                    MessageBox.Show("Predavač se ne može ukloniti jer ima vezanih sesija.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Potvrda brisanja
                DialogResult result = MessageBox.Show("Da li ste sigurni da želite ukloniti ovog predavača?", "Potvrda brisanja", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (result == DialogResult.Yes)
                {
                    // Zatim brisanje predavača
                    MySqlCommand command = new MySqlCommand("DELETE FROM PREDAVAC WHERE Id_Predavaca = @id", connection);
                    command.Parameters.AddWithValue("@id", predavacId);
                    command.ExecuteNonQuery();
                }

                connection.Close();
            }

            LoadData(); // Osvježavanje DataGridView
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

        private void button12_Click(object sender, EventArgs e) // Dugme za dodavanje prostorije
        {
            using (ProstorijaForm prostorijaForm = new ProstorijaForm())
            {
                if (prostorijaForm.ShowDialog() == DialogResult.OK)
                {
                    string nazivProstorije = prostorijaForm.NazivProstorije;
                    int kapacitet = (int)prostorijaForm.Kapacitet; // Konvertovanje na int

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // Proverava da li već postoji prostorija sa istim nazivom
                        MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM PROSTORIJA WHERE Naziv_Prostorije = @naziv", connection);
                        checkCommand.Parameters.AddWithValue("@naziv", nazivProstorije);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Prostorija sa tim nazivom već postoji.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Ako ne postoji, dodaj novu prostoriju
                        MySqlCommand command = new MySqlCommand("INSERT INTO PROSTORIJA (Naziv_Prostorije, Kapacitet) VALUES (@naziv, @kapacitet)", connection);
                        command.Parameters.AddWithValue("@naziv", nazivProstorije);
                        command.Parameters.AddWithValue("@kapacitet", kapacitet);
                        command.ExecuteNonQuery();

                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }
        }

        private void button11_Click(object sender, EventArgs e) // Dugme za izmenu
        {
            if (dataGridView4.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati prostoriju za izmenu.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView4.SelectedRows[0];
            int prostorijaId = Convert.ToInt32(selectedRow.Cells["Id_Prostorije"].Value);
            string nazivProstorije = selectedRow.Cells["Naziv_Prostorije"].Value.ToString();
            int kapacitet = Convert.ToInt32(selectedRow.Cells["Kapacitet"].Value);

            using (ProstorijaForm prostorijaForm = new ProstorijaForm(nazivProstorije, kapacitet))
            {
                if (prostorijaForm.ShowDialog() == DialogResult.OK)
                {
                    // Ažuriranje prostorije u bazi podataka
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("UPDATE PROSTORIJA SET Naziv_Prostorije = @naziv, Kapacitet = @kapacitet WHERE Id_Prostorije = @id", connection);
                        command.Parameters.AddWithValue("@naziv", prostorijaForm.NazivProstorije);
                        command.Parameters.AddWithValue("@kapacitet", prostorijaForm.Kapacitet);
                        command.Parameters.AddWithValue("@id", prostorijaId);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }
        }

        private void button10_Click(object sender, EventArgs e) // Dugme za uklanjanje
        {
            if (dataGridView4.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati prostoriju za uklanjanje.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView4.SelectedRows[0];
            int prostorijaId = Convert.ToInt32(selectedRow.Cells["Id_Prostorije"].Value);

            // Potvrda brisanja
            DialogResult result = MessageBox.Show("Da li ste sigurni da želite ukloniti ovu prostoriju?", "Potvrda brisanja", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // TODO Ovo pogledati da li ce ovako ici
                    // Prvo brisanje iz povezanih tabela ako je potrebno
                    MySqlCommand deleteReferencesCommand = new MySqlCommand("DELETE FROM SESIJA WHERE PROSTORIJA_Id_Prostorije = @id", connection);
                    deleteReferencesCommand.Parameters.AddWithValue("@id", prostorijaId);
                    deleteReferencesCommand.ExecuteNonQuery();

                    // Zatim brisanje prostorije
                    MySqlCommand command = new MySqlCommand("DELETE FROM PROSTORIJA WHERE Id_Prostorije = @id", connection);
                    command.Parameters.AddWithValue("@id", prostorijaId);
                    command.ExecuteNonQuery();

                    connection.Close();
                }

                LoadData(); // Osvježavanje DataGridView
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

        private void button15_Click(object sender, EventArgs e) // Dugme za dodavanje događaja
        {
            using (DogadjajForm dogadjajForm = new DogadjajForm())
            {
                if (dogadjajForm.ShowDialog() == DialogResult.OK)
                {
                    string naziv = dogadjajForm.Naziv;
                    string opis = dogadjajForm.Opis;
                    DateTime datumPocetka = dogadjajForm.DatumPocetka;
                    DateTime datumZavrsetka = dogadjajForm.DatumZavrsetka;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        // Proverava da li već postoji događaj sa istim nazivom
                        MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM DOGADJAJ WHERE Naziv = @naziv", connection);
                        checkCommand.Parameters.AddWithValue("@naziv", naziv);
                        int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        if (count > 0)
                        {
                            MessageBox.Show("Događaj sa tim nazivom već postoji.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Ako ne postoji, dodaj novi događaj
                        MySqlCommand command = new MySqlCommand("INSERT INTO DOGADJAJ (Naziv, Opis, DatumPocetka, DatumZavrsetka) VALUES (@naziv, @opis, @datumPocetka, @datumZavrsetka)", connection);
                        command.Parameters.AddWithValue("@naziv", naziv);
                        command.Parameters.AddWithValue("@opis", opis);
                        command.Parameters.AddWithValue("@datumPocetka", datumPocetka);
                        command.Parameters.AddWithValue("@datumZavrsetka", datumZavrsetka);
                        command.ExecuteNonQuery();

                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }
        }

        private void button14_Click(object sender, EventArgs e) // Dugme za izmenu
        {
            if (dataGridView5.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati događaj za izmenu.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView5.SelectedRows[0];
            int dogadjajId = Convert.ToInt32(selectedRow.Cells["Id_Dogadjaja"].Value);
            string naziv = selectedRow.Cells["Naziv"].Value.ToString();
            string opis = selectedRow.Cells["Opis"].Value.ToString();
            DateTime datumPocetka = Convert.ToDateTime(selectedRow.Cells["DatumPocetka"].Value);
            DateTime datumZavrsetka = Convert.ToDateTime(selectedRow.Cells["DatumZavrsetka"].Value);

            using (DogadjajForm dogadjajForm = new DogadjajForm(naziv, opis, datumPocetka, datumZavrsetka))
            {
                if (dogadjajForm.ShowDialog() == DialogResult.OK)
                {
                    // Ažuriranje događaja u bazi podataka
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("UPDATE DOGADJAJ SET Naziv = @naziv, Opis = @opis, DatumPocetka = @datumPocetka, DatumZavrsetka = @datumZavrsetka WHERE Id_Dogadjaja = @id", connection);
                        command.Parameters.AddWithValue("@naziv", dogadjajForm.Naziv);
                        command.Parameters.AddWithValue("@opis", dogadjajForm.Opis);
                        command.Parameters.AddWithValue("@datumPocetka", dogadjajForm.DatumPocetka);
                        command.Parameters.AddWithValue("@datumZavrsetka", dogadjajForm.DatumZavrsetka);
                        command.Parameters.AddWithValue("@id", dogadjajId);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }
        }

        private void button13_Click(object sender, EventArgs e) // Dugme za uklanjanje
        {
            if (dataGridView5.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati događaj za uklanjanje.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView5.SelectedRows[0];
            int dogadjajId = Convert.ToInt32(selectedRow.Cells["Id_Dogadjaja"].Value);

            // Potvrda brisanja
            DialogResult result = MessageBox.Show("Da li ste sigurni da želite ukloniti ovaj događaj?", "Potvrda brisanja", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Prvo brisanje iz povezanih tabela ako je potrebno
                    MySqlCommand deleteReferencesCommand = new MySqlCommand("DELETE FROM SESIJA WHERE Id_Dogadjaja = @id", connection);
                    deleteReferencesCommand.Parameters.AddWithValue("@id", dogadjajId);
                    deleteReferencesCommand.ExecuteNonQuery();

                    // Zatim brisanje događaja
                    MySqlCommand command = new MySqlCommand("DELETE FROM DOGADJAJ WHERE Id_Dogadjaja = @id", connection);
                    command.Parameters.AddWithValue("@id", dogadjajId);
                    command.ExecuteNonQuery();

                    connection.Close();
                }

                LoadData(); // Osvježavanje DataGridView
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

        private void button18_Click(object sender, EventArgs e) // Dugme za dodavanje sesije
        {
            using (SesijaForm sesijaForm = new SesijaForm())
            {
                if (sesijaForm.ShowDialog() == DialogResult.OK)
                {
                    string naziv = sesijaForm.Naziv;
                    string opis = sesijaForm.Opis;
                    DateTime datum = sesijaForm.Datum;
                    int prostorijaId = sesijaForm.ProstorijaId;
                    TimeSpan vremePocetka = sesijaForm.VremePocetka;
                    TimeSpan vremeZavrsetka = sesijaForm.VremeZavrsetka;
                    int dogadjajId = sesijaForm.DogadjajId;

                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();

                        //// Proverava da li već postoji sesija sa istim nazivom u isto vreme
                        //MySqlCommand checkCommand = new MySqlCommand("SELECT COUNT(*) FROM SESIJA WHERE PROSTORIJA_Id_Prostorije = @prostorijaId AND Datum = @datum AND ((VrijemePocetka <= @vremeZavrsetka AND VrijemeZavrsetka >= @vremePocetka))", connection);
                        //checkCommand.Parameters.AddWithValue("@prostorijaId", prostorijaId);
                        //checkCommand.Parameters.AddWithValue("@datum", datum);
                        //checkCommand.Parameters.AddWithValue("@vremePocetka", vremePocetka);
                        //checkCommand.Parameters.AddWithValue("@vremeZavrsetka", vremeZavrsetka);
                        //int count = Convert.ToInt32(checkCommand.ExecuteScalar());

                        //if (count > 0)
                        //{
                        //    MessageBox.Show("Već postoji sesija u odabranoj prostoriji u to vreme.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //    return;
                        //}

                        // Ako ne postoji, dodaj novu sesiju
                        MySqlCommand command = new MySqlCommand("INSERT INTO SESIJA (Id_Dogadjaja, Naziv, Opis, Datum, VrijemePocetka, VrijemeZavrsetka, PROSTORIJA_Id_Prostorije) VALUES (@idDogadjaja, @naziv, @opis, @datum, @vremePocetka, @vremeZavrsetka, @prostorijaId)", connection);
                        command.Parameters.AddWithValue("@idDogadjaja", dogadjajId);
                        command.Parameters.AddWithValue("@naziv", naziv);
                        command.Parameters.AddWithValue("@opis", opis);
                        command.Parameters.AddWithValue("@datum", datum);
                        command.Parameters.AddWithValue("@vremePocetka", vremePocetka);
                        command.Parameters.AddWithValue("@vremeZavrsetka", vremeZavrsetka);
                        command.Parameters.AddWithValue("@prostorijaId", prostorijaId);
                        command.ExecuteNonQuery();

                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }
        }

        // Pronalaženje ID prostorije prema nazivu
        private int GetProstorijaIdByNaziv(string nazivProstorije)
        {
            int prostorijaId = -1;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT Id_Prostorije FROM PROSTORIJA WHERE Naziv_Prostorije = @naziv", connection);
                command.Parameters.AddWithValue("@naziv", nazivProstorije);

                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    prostorijaId = Convert.ToInt32(reader["Id_Prostorije"]);
                }
                reader.Close();
            }

            return prostorijaId;
        }

        // Pronalaženje ID događaja prema nazivu
        private int GetDogadjajIdByNaziv(string nazivDogadjaja)
        {
            int dogadjajId = -1;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT Id_Dogadjaja FROM DOGADJAJ WHERE Naziv = @naziv", connection);
                command.Parameters.AddWithValue("@naziv", nazivDogadjaja);

                MySqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    dogadjajId = Convert.ToInt32(reader["Id_Dogadjaja"]);
                }
                reader.Close();
            }

            return dogadjajId;
        }

        private void button17_Click(object sender, EventArgs e) // Dugme za izmenu
        {
            if (dataGridView6.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati sesiju za izmenu.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView6.SelectedRows[0];
            int sesijaId = Convert.ToInt32(selectedRow.Cells["Id_Sesije"].Value);
            string naziv = selectedRow.Cells["Naziv"].Value.ToString();
            string opis = selectedRow.Cells["Opis"].Value.ToString();
            DateTime datum = Convert.ToDateTime(selectedRow.Cells["Datum"].Value);
            TimeSpan vremePocetka = TimeSpan.Parse(selectedRow.Cells["VrijemePocetka"].Value.ToString());
            TimeSpan vremeZavrsetka = TimeSpan.Parse(selectedRow.Cells["VrijemeZavrsetka"].Value.ToString());
            //int prostorijaId = Convert.ToInt32(selectedRow.Cells["Prostorija"].Value); // Pretpostavljamo da se ovde čuva ID prostorije
            //int dogadjajId = Convert.ToInt32(selectedRow.Cells["NazivDogadjaja"].Value); // Pretpostavljamo da se ovde čuva ID događaja
            int prostorijaId = GetProstorijaIdByNaziv(selectedRow.Cells["Prostorija"].Value.ToString());
            int dogadjajId = GetDogadjajIdByNaziv(selectedRow.Cells["NazivDogadjaja"].Value.ToString());


            using (SesijaForm sesijaForm = new SesijaForm(naziv, opis, datum, vremePocetka, vremeZavrsetka, prostorijaId, dogadjajId))
            {
                if (sesijaForm.ShowDialog() == DialogResult.OK)
                {
                    // Ažuriranje sesije u bazi podataka
                    using (MySqlConnection connection = new MySqlConnection(connectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("UPDATE SESIJA SET Id_Dogadjaja = @idDogadjaja, Naziv = @naziv, Opis = @opis, Datum = @datum, VrijemePocetka = @vremePocetka, VrijemeZavrsetka = @vremeZavrsetka, PROSTORIJA_Id_Prostorije = @prostorijaId WHERE Id_Sesije = @id", connection);
                        command.Parameters.AddWithValue("@idDogadjaja", sesijaForm.DogadjajId);
                        command.Parameters.AddWithValue("@naziv", sesijaForm.Naziv);
                        command.Parameters.AddWithValue("@opis", sesijaForm.Opis);
                        command.Parameters.AddWithValue("@datum", sesijaForm.Datum);
                        command.Parameters.AddWithValue("@vremePocetka", sesijaForm.VremePocetka);
                        command.Parameters.AddWithValue("@vremeZavrsetka", sesijaForm.VremeZavrsetka);
                        command.Parameters.AddWithValue("@prostorijaId", sesijaForm.ProstorijaId);
                        command.Parameters.AddWithValue("@id", sesijaId);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }

                    LoadData(); // Osvježavanje DataGridView
                }
            }
        }


        private void button16_Click(object sender, EventArgs e) // Dugme za uklanjanje
        {
            if (dataGridView6.SelectedRows.Count == 0)
            {
                MessageBox.Show("Morate selektovati sesiju za uklanjanje.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedRow = dataGridView6.SelectedRows[0];
            int sesijaId = Convert.ToInt32(selectedRow.Cells["Id_Sesije"].Value);

            // Potvrda brisanja
            DialogResult result = MessageBox.Show("Da li ste sigurni da želite ukloniti ovu sesiju?", "Potvrda brisanja", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result == DialogResult.Yes)
            {
                using (MySqlConnection connection = new MySqlConnection(connectionString))
                {
                    connection.Open();

                    // Prvo brisanje povezanih podataka iz tabele PRIJAVA
                    MySqlCommand deletePrijaveCommand = new MySqlCommand("DELETE FROM PRIJAVA WHERE Id_Sesije = @id", connection);
                    deletePrijaveCommand.Parameters.AddWithValue("@id", sesijaId);
                    deletePrijaveCommand.ExecuteNonQuery();

                    // Prvo brisanje povezanih podataka iz tabele PREDAVAC_SESIJA
                    MySqlCommand deletePredavacSesijaCommand = new MySqlCommand("DELETE FROM PREDAVAC_SESIJA WHERE SESIJA_Id_Sesije = @id", connection);
                    deletePredavacSesijaCommand.Parameters.AddWithValue("@id", sesijaId);
                    deletePredavacSesijaCommand.ExecuteNonQuery();

                    // Zatim brisanje sesije
                    MySqlCommand command = new MySqlCommand("DELETE FROM SESIJA WHERE Id_Sesije = @id", connection);
                    command.Parameters.AddWithValue("@id", sesijaId);
                    command.ExecuteNonQuery();

                    connection.Close();
                }

                LoadData(); // Osvježavanje DataGridView
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

        private void button20_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Dobijamo ID organizatora sa selektovanog reda
                int organizatorId = Convert.ToInt32(dataGridView1.SelectedRows[0].Cells["Id_Organizatora"].Value);

                // Otvorimo formu za povezivanje organizatora sa događajem
                UvezivanjeForm uvezivanjeForm = new UvezivanjeForm(organizatorId);
                uvezivanjeForm.Show();
            }
            else
            {
                MessageBox.Show("Morate selektovati organizatora.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button19_Click(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedRows.Count > 0)
            {
                // Dobijamo ID predavača sa selektovanog reda
                int predavacId = Convert.ToInt32(dataGridView2.SelectedRows[0].Cells["Id_Predavaca"].Value);

                // Otvorimo formu za povezivanje predavača sa sesijom
                UvezivanjePredavacSesija uvezivanjeForm = new UvezivanjePredavacSesija(predavacId);
                uvezivanjeForm.Show();
            }
            else
            {
                MessageBox.Show("Morate selektovati predavača.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button21_Click(object sender, EventArgs e)
        {
            PrijavaForm login = new PrijavaForm();
            this.Hide();
            login.Show();
        }
    }
}
