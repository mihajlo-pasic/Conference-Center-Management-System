using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencijski_Centar
{
    internal class Ucesnik
    {
        public int IdUcesnika { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string Email { get; set; }
        public string Telefon { get; set; }

        // Podrazumevani konstruktor
        public Ucesnik() { }

        // Konstruktor sa parametrima
        public Ucesnik(int idUcesnika, string ime, string prezime, string email, string telefon)
        {
            IdUcesnika = idUcesnika;
            Ime = ime;
            Prezime = prezime;
            Email = email;
            Telefon = telefon;
        }
    }
}
