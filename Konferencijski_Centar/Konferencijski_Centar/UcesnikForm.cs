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
    public partial class UcesnikForm : Form
    {
        public string Ime { get; private set; }
        public string Prezime { get; private set; }
        public string Email { get; private set; }
        public string Telefon { get; private set; }

        public UcesnikForm()
        {
            InitializeComponent();
        }

        // Konstruktor za izmenu učesnika
        public UcesnikForm(string ime, string prezime, string email, string telefon) : this()
        {
            textBox1.Text = ime;
            textBox2.Text = prezime;
            textBox3.Text = email;
            textBox4.Text = telefon;
        }

        private void buttonOK_Click(object sender, EventArgs e)
        {
            Ime = textBox1.Text.Trim();
            Prezime = textBox2.Text.Trim();
            Email = textBox3.Text.Trim();
            Telefon = textBox4.Text.Trim();

            if (string.IsNullOrWhiteSpace(Ime) || string.IsNullOrWhiteSpace(Prezime))
            {
                MessageBox.Show("Ime i prezime učesnika su obavezni.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Provera validnosti email-a (opciono)
            if (!IsValidEmail(Email))
            {
                MessageBox.Show("Unesite važeću email adresu.", "Greška", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        // Opcionalna metoda za validaciju email adrese
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}

