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
    public partial class OrganizatorForm : Form
    {
        public string Ime { get; private set; }
        public string KontaktInformacije { get; private set; }

        public OrganizatorForm()
        {
            InitializeComponent();
        }

        public OrganizatorForm(string ime, string kontaktInformacije) : this()
        {
            textBoxIme.Text = ime;
            textBoxKontakt.Text = kontaktInformacije;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Ime = textBoxIme.Text.Trim();
            KontaktInformacije = textBoxKontakt.Text.Trim();

            if (string.IsNullOrWhiteSpace(Ime))
            {
                MessageBox.Show("Ime organizatora je obavezno.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
