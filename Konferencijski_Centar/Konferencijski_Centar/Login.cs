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
    public partial class PrijavaForm : Form
    {
        public PrijavaForm()
        {
            InitializeComponent();
        }

        private void buttonPrijava_Click(object sender, EventArgs e)
        {
            string username = textBoxKorisnickoIme.Text;
            string password = textBoxLozinka.Text;

            // Proveri korisnika u bazi
            (bool userExists, bool isAdmin) = CheckUserCredentials(username, password);

            if (userExists)
            {
                if (isAdmin)
                {
                    AdminForm adminForm = new AdminForm();
                    adminForm.Show();
                }
                else
                {
                    UserForm userForm = new UserForm();
                    userForm.Show();
                }
                this.Hide();
            }
            else
            {
                MessageBox.Show("Neispravno korisničko ime ili lozinka.");
            }

        }
        private string connectionString = ConfigurationManager.ConnectionStrings["konferencijski_centar"].ConnectionString;

        private (bool, bool) CheckUserCredentials(string username, string password)
        {
            bool userExists = false;
            bool isAdmin = false;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                string query = "SELECT Is_Admin FROM KORISNIK WHERE Korisnicko_Ime = @username AND Lozinka = @password";
                MySqlCommand command = new MySqlCommand(query, connection);
                command.Parameters.AddWithValue("@username", username);
                command.Parameters.AddWithValue("@password", password);

                using (MySqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        userExists = true;
                        isAdmin = reader.GetBoolean("Is_Admin");
                    }
                }
            }

            return (userExists, isAdmin);
        }

        private void PrijavaForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}
