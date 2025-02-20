using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencijski_Centar
{
    internal class Predavac
    {
        public int IdPredavaca { get; set; }
        public string Ime { get; set; }
        public string Prezime { get; set; }
        public string TemaPredavanja { get; set; }

        // Podrazumevani konstruktor
        public Predavac() { }

        // Konstruktor sa parametrima
        public Predavac(int idPredavaca, string ime, string prezime, string temaPredavanja)
        {
            IdPredavaca = idPredavaca;
            Ime = ime;
            Prezime = prezime;
            TemaPredavanja = temaPredavanja;
        }
    }
}
