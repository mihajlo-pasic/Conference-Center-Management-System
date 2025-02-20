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
    public partial class Prikaz : Form
    {
        private int organizatorId = -1;
        private int dogadjajId = -1;
        private int sesijaId = -1;
        private int prostorijaId = -1;
        private int predavacId = -1;

        public Prikaz()
        {
            InitializeComponent();
        }

        // Ova metoda prima parametar za ID entiteta i tip entiteta (Organizator, Dogadjaj, Sesija, itd.)
        public void LoadData(int id, string type)
        {
            switch (type)
            {
                case "Organizator":
                    organizatorId = id;
                    LoadDogadjaje();
                    break;
                case "Dogadjaj":
                    dogadjajId = id;
                    LoadSesije();
                    break;
                case "Sesija":
                    sesijaId = id;
                    LoadUcesnici();
                    break;
                case "Prostorija":
                    prostorijaId = id;
                    LoadSesijeByProstorija();
                    break;
                case "Predavac":
                    predavacId = id;
                    LoadSesijeByPredavac();
                    break;
                default:
                    MessageBox.Show("Nepoznat tip entiteta.");
                    break;
            }
        }
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["konferencijski_centar"].ConnectionString;

        private void LoadDogadjaje()
        {
            // Upit koji prikazuje sve događaje vezane za organizatora
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(@"
                SELECT d.Id_Dogadjaja, d.Naziv, d.DatumPocetka, d.DatumZavrsetka
                FROM DOGADJAJ d
                JOIN ORGANIZATOR_DOGADJAJ od ON d.Id_Dogadjaja = od.DOGADJAJ_Id_Dogadjaja
                WHERE od.ORGANIZATOR_Id_Organizatora = @organizatorId", connection);
                command.Parameters.AddWithValue("@organizatorId", organizatorId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridViewPrikaz.DataSource = dataTable; // Poveži podatke sa dataGridView
            }
        }

        private void LoadSesije()
        {
            // Upit koji prikazuje sve sesije vezane za događaj
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(@"
                SELECT s.Id_Sesije, s.Naziv, s.Datum, s.VrijemePocetka, s.VrijemeZavrsetka
                FROM SESIJA s
                WHERE s.Id_Dogadjaja = @dogadjajId", connection);
                command.Parameters.AddWithValue("@dogadjajId", dogadjajId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridViewPrikaz.DataSource = dataTable;
            }
        }

        private void LoadUcesnici()
        {
            // Upit koji prikazuje sve učesnike za odabranu sesiju
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(@"
                SELECT u.Ime, u.Prezime, u.Email, u.Telefon
                FROM UCESNIK u
                JOIN PRIJAVA p ON u.Id_Ucesnika = p.Id_Ucesnika
                WHERE p.Id_Sesije = @sesijaId", connection);
                command.Parameters.AddWithValue("@sesijaId", sesijaId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridViewPrikaz.DataSource = dataTable;
            }
        }

        private void LoadSesijeByProstorija()
        {
            // Upit koji prikazuje sve sesije u prostoriji
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(@"
                SELECT s.Naziv, s.Datum, s.VrijemePocetka, s.VrijemeZavrsetka
                FROM SESIJA s
                WHERE s.PROSTORIJA_Id_Prostorije = @prostorijaId", connection);
                command.Parameters.AddWithValue("@prostorijaId", prostorijaId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridViewPrikaz.DataSource = dataTable;
            }
        }

        private void LoadSesijeByPredavac()
        {
            // Upit koji prikazuje sesije na kojima predavač drži predavanje
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(@"
                SELECT s.Naziv, s.Datum, s.VrijemePocetka, s.VrijemeZavrsetka
                FROM SESIJA s
                JOIN PREDAVAC_SESIJA ps ON s.Id_Sesije = ps.SESIJA_Id_Sesije
                WHERE ps.PREDAVAC_Id_Predavaca = @predavacId", connection);
                command.Parameters.AddWithValue("@predavacId", predavacId);

                MySqlDataAdapter adapter = new MySqlDataAdapter(command);
                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                dataGridViewPrikaz.DataSource = dataTable;
            }
        }
    }
}

