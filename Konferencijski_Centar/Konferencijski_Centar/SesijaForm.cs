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
    public partial class SesijaForm : Form
    {
        public string Naziv { get; private set; }
        public string Opis { get; private set; }
        public DateTime Datum { get; private set; }
        public TimeSpan VremePocetka { get; private set; }
        public TimeSpan VremeZavrsetka { get; private set; }
        public int ProstorijaId { get; private set; }
        public int DogadjajId { get; private set; }

        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["konferencijski_centar"].ConnectionString;

        public SesijaForm()
        {
            InitializeComponent();
            LoadDogadjaje();
            LoadProstorije();
        }

        public SesijaForm(string naziv, string opis, DateTime datum, TimeSpan vremePocetka, TimeSpan vremeZavrsetka, int prostorijaId, int dogadjajId) : this()
        {
            comboBoxDogadjaji.SelectedItem = dogadjajId; // Postavljanje odabranog događaja
            textBoxNaziv.Text = naziv;
            textBoxOpis.Text = opis;
            comboBoxDatum.SelectedValue = datum;
            comboBoxProstorije.SelectedItem = prostorijaId; // Postavljanje odabrane prostorije
            comboBoxVremePocetka.SelectedItem = vremePocetka;
            comboBoxVremeZavrsetka.SelectedItem = vremeZavrsetka;
        }

        private void LoadDogadjaje()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT Id_Dogadjaja, Naziv FROM DOGADJAJ", connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBoxDogadjaji.Items.Add(new { Id = reader["Id_Dogadjaja"], Naziv = reader["Naziv"] });
                }
                reader.Close();
            }
        }

        private void LoadProstorije()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand("SELECT Id_Prostorije, Naziv_Prostorije FROM PROSTORIJA", connection);
                MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    comboBoxProstorije.Items.Add(new { Id = reader["Id_Prostorije"], Naziv = reader["Naziv_Prostorije"] });
                }
                reader.Close();
            }
        }

        private void comboBoxDogadjaji_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopuniDostupneDatume();
            comboBoxDatum.Enabled = true; // Omogućite biranje datuma
        }

        private void comboBoxProstorije_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopuniDostupnaVremena();
        }

        private void PopuniDostupneDatume()
        {
            comboBoxDatum.Items.Clear();

            if (comboBoxDogadjaji.SelectedItem == null)
                return;

            int dogadjajId = ((dynamic)comboBoxDogadjaji.SelectedItem).Id;

            // Učitaj DatumPocetka i DatumZavrsetka za odabrani događaj
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(@"
            SELECT DatumPocetka, DatumZavrsetka 
            FROM DOGADJAJ 
            WHERE Id_Dogadjaja = @dogadjajId", connection);
                command.Parameters.AddWithValue("@dogadjajId", dogadjajId);
                MySqlDataReader reader = command.ExecuteReader();

                if (reader.Read())
                {
                    DateTime datumPocetka = DateTime.Parse(reader["DatumPocetka"].ToString());
                    DateTime datumZavrsetka = DateTime.Parse(reader["DatumZavrsetka"].ToString());

                    // Generisanje svih datuma između DatumPocetka i DatumZavrsetka
                    while (datumPocetka <= datumZavrsetka)
                    {
                        comboBoxDatum.Items.Add(datumPocetka.Date); // Dodajte samo datum (bez vremena)
                        datumPocetka = datumPocetka.AddDays(1); // Pređi na sledeći dan
                    }
                }
                reader.Close();
            }

            // Opcionalno: automatski selektujte prvi dostupni datum
            if (comboBoxDatum.Items.Count > 0)
            {
                comboBoxDatum.SelectedIndex = 0; // Automatski selektujte prvi datum
            }
        }



        private void PopuniDostupnaVremena()
        {
            comboBoxVremePocetka.Items.Clear();
            comboBoxVremeZavrsetka.Items.Clear();

            if (comboBoxProstorije.SelectedItem == null || comboBoxDogadjaji.SelectedItem == null || comboBoxDatum.SelectedItem == null)
                return;

            int prostorijaId = ((dynamic)comboBoxProstorije.SelectedItem).Id;
            DateTime datum = (DateTime)comboBoxDatum.SelectedItem; // Koristite comboBoxDatum za datum

            List<TimeSpan> svaMogucaVremena = new List<TimeSpan>();
            for (int h = 8; h <= 21; h++)
            {
                for (int m = 0; m < 60; m += 30)
                {
                    TimeSpan vreme = new TimeSpan(h, m, 0);
                    if (vreme == new TimeSpan(21, 0, 0)) continue;
                    svaMogucaVremena.Add(vreme); // Dodajemo sva moguća vremena u listu
                }
            }

            // Učitaj postojeće sesije za odabranu prostoriju i datum
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(@"SELECT VrijemePocetka, VrijemeZavrsetka 
                                                  FROM SESIJA 
                                                  WHERE PROSTORIJA_Id_Prostorije = @prostorijaId 
                                                  AND Datum = @datum
                                                  ORDER BY VrijemePocetka", connection);
                command.Parameters.AddWithValue("@prostorijaId", prostorijaId);
                command.Parameters.AddWithValue("@datum", datum);
                MySqlDataReader reader = command.ExecuteReader();

                Dictionary<TimeSpan, TimeSpan> zauzetaVremena = new Dictionary<TimeSpan, TimeSpan>();

                while (reader.Read())
                {
                    TimeSpan pocetak = TimeSpan.Parse(reader["VrijemePocetka"].ToString());
                    TimeSpan zavrsetak = TimeSpan.Parse(reader["VrijemeZavrsetka"].ToString());
                    zauzetaVremena.Add(pocetak, zavrsetak); // Dodavanje parova (početak, kraj) u dictionary
                }
                reader.Close();

                TimeSpan addedTime = new TimeSpan(0, 30, 0); // 30 minuta
                foreach (var vreme in zauzetaVremena.Keys)
                {
                    TimeSpan vrijeme = vreme;
                    while (vrijeme != zauzetaVremena[vreme])
                    {
                        if (svaMogucaVremena.Contains(vrijeme))
                        {
                            svaMogucaVremena.Remove(vrijeme); // Uklanjamo zauzeta vremena
                        }
                        vrijeme = vrijeme.Add(addedTime);
                    }
                }

                // Popunjavamo comboBoxVremePocetka sa slobodnim vremenima
                foreach (var vreme in svaMogucaVremena)
                {
                    comboBoxVremePocetka.Items.Add(vreme);
                }

            }
        }



        private void comboBoxVremePocetka_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Kada korisnik odabere vreme početka, popuni vreme završetka
            PopuniDostupnaVremenaZavrsetka();
            //comboBoxVremeZavrsetka.SelectedIndex = 0;
            
        }

        private void PopuniDostupnaVremenaZavrsetka()
        {
            comboBoxVremeZavrsetka.Items.Clear();

            if (comboBoxVremePocetka.SelectedItem == null)
                return;

            TimeSpan vremePocetka = (TimeSpan)comboBoxVremePocetka.SelectedItem;
            int prostorijaId = ((dynamic)comboBoxProstorije.SelectedItem).Id;
            DateTime datum = (DateTime)comboBoxDatum.SelectedItem;

            // Učitaj postojeće sesije za odabranu prostoriju i datum
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(@"SELECT VrijemePocetka, VrijemeZavrsetka 
                                                  FROM SESIJA 
                                                  WHERE PROSTORIJA_Id_Prostorije = @prostorijaId 
                                                  AND Datum = @datum
                                                  ORDER BY VrijemePocetka", connection);
                command.Parameters.AddWithValue("@prostorijaId", prostorijaId);
                command.Parameters.AddWithValue("@datum", datum);
                MySqlDataReader reader = command.ExecuteReader();

                List<TimeSpan> vremenaPocetka = new List<TimeSpan>();
                while (reader.Read())
                {
                    TimeSpan pocetak = TimeSpan.Parse(reader["VrijemePocetka"].ToString());
                    vremenaPocetka.Add(pocetak);
                }
                reader.Close();

                TimeSpan addedTime = new TimeSpan(0, 30, 0);
                TimeSpan vremeZavrsetka = vremePocetka.Add(addedTime);

                if (vremenaPocetka.Count == 0)
                {
                    while (vremeZavrsetka <= new TimeSpan(21, 0, 0))
                    {
                        comboBoxVremeZavrsetka.Items.Add(vremeZavrsetka);
                        vremeZavrsetka = vremeZavrsetka.Add(addedTime);
                    }
                }
                else
                {
                    foreach (TimeSpan vreme in vremenaPocetka)
                    {

                        if (vremeZavrsetka > vreme)
                        {
                            if (vreme != vremenaPocetka.Last()) continue;
                        }
                        if (vremeZavrsetka <= vreme)
                        {
                            while (vremeZavrsetka <= vreme)
                            {
                                comboBoxVremeZavrsetka.Items.Add(vremeZavrsetka);
                                vremeZavrsetka = vremeZavrsetka.Add(addedTime);
                            }
                            break;
                        }

                        while (vremeZavrsetka <= new TimeSpan(21, 0, 0))
                        {
                            comboBoxVremeZavrsetka.Items.Add(vremeZavrsetka);
                            vremeZavrsetka = vremeZavrsetka.Add(addedTime);
                        }
                    }
                }
            }
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            // Validacija i postavljanje podataka
            if (comboBoxDogadjaji.SelectedItem == null)
            {
                MessageBox.Show("Morate izabrati događaj.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Naziv = textBoxNaziv.Text.Trim();
            Opis = textBoxOpis.Text.Trim();
            Datum = (DateTime)comboBoxDatum.SelectedItem; // Uzmite datum iz comboBox
            DogadjajId = ((dynamic)comboBoxDogadjaji.SelectedItem).Id;
            ProstorijaId = ((dynamic)comboBoxProstorije.SelectedItem).Id;

            if (comboBoxVremePocetka.SelectedItem != null)
            {
                VremePocetka = (TimeSpan)comboBoxVremePocetka.SelectedItem;
            }

            if (comboBoxVremeZavrsetka.SelectedItem != null)
            {
                VremeZavrsetka = (TimeSpan)comboBoxVremeZavrsetka.SelectedItem;
            }

            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}
