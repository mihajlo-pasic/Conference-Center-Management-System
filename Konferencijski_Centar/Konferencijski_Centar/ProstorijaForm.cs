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
    public partial class ProstorijaForm : Form
    {
        public string NazivProstorije { get; private set; }
        public int Kapacitet { get; private set; }

        public ProstorijaForm()
        {
            InitializeComponent();
        }

        // Konstruktor za izmenu prostorije
        public ProstorijaForm(string naziv, int kapacitet) : this()
        {
            textBox1.Text = naziv;
            numericUpDown1.Value = kapacitet;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            NazivProstorije = textBox1.Text.Trim();
            Kapacitet = (int)numericUpDown1.Value;

            if (string.IsNullOrWhiteSpace(NazivProstorije))
            {
                MessageBox.Show("Naziv prostorije je obavezan.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
