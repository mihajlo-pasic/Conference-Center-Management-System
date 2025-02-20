using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Konferencijski_Centar
{
    public partial class DogadjajForm : Form
    {
        public string Naziv { get; private set; }
        public string Opis { get; private set; }
        public DateTime DatumPocetka { get; private set; }
        public DateTime DatumZavrsetka { get; private set; }

        public DogadjajForm()
        {
            InitializeComponent();
        }

        // Konstruktor za izmenu događaja
        public DogadjajForm(string naziv, string opis, DateTime datumPocetka, DateTime datumZavrsetka) : this()
        {
            textBox1.Text = naziv;
            textBox2.Text = opis;
            dateTimePicker1.Value = datumPocetka; // Assuming dateTimePicker1 is used for "Datum pocetka"
            dateTimePicker2.Value = datumZavrsetka; // Assuming dateTimePicker2 is used for "Datum zavrsetka"
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Naziv = textBox1.Text.Trim();
            Opis = textBox2.Text.Trim();
            DatumPocetka = dateTimePicker1.Value;
            DatumZavrsetka = dateTimePicker2.Value;

            if (string.IsNullOrWhiteSpace(Naziv))
            {
                MessageBox.Show("Naziv događaja je obavezan.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
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
