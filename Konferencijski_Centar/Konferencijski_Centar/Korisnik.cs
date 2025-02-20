using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencijski_Centar
{
    internal class Korisnik
    {
        public int IdKorisnika { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string KorisnickoIme { get; set; }
        public string Lozinka { get; set; }
        public bool IsAdmin { get; set; }

        // Podrazumevani konstruktor
        public Korisnik() { }

        // Konstruktor sa parametrima
        public Korisnik(int idKorisnika, string ime, string prezime, string korisnickoIme, string lozinka, bool isAdmin)
        {
            IdKorisnika = idKorisnika;
            Ime = ime;
            Prezime = prezime;
            KorisnickoIme = korisnickoIme;
            Lozinka = lozinka;
            IsAdmin = isAdmin;
        }
    }
}
