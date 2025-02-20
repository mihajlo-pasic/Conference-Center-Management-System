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
    public partial class PredavacForm : Form
    {
        public string Ime { get; private set; }
        public string Prezime { get; private set; }
        public string TemaPredavanja { get; private set; }

        public PredavacForm()
        {
            InitializeComponent();
        }

        // Konstruktor za izmenu predavača
        public PredavacForm(string ime, string prezime, string tema) : this()
        {
            textBoxImePredavaca.Text = ime;
            textBoxPrezimePredavaca.Text = prezime;
            textBoxTemaPredavanjaPredavaca.Text = tema;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Ime = textBoxImePredavaca.Text.Trim();
            Prezime = textBoxPrezimePredavaca.Text.Trim();
            TemaPredavanja = textBoxTemaPredavanjaPredavaca.Text.Trim();

            if (string.IsNullOrWhiteSpace(Ime) || string.IsNullOrWhiteSpace(Prezime))
            {
                MessageBox.Show("Ime i prezime predavača su obavezni.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
