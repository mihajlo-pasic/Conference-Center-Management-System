using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencijski_Centar
{
    internal class Prostorija
    {
        public int IdProstorije { get; set; }
        public string NazivProstorije { get; set; }
        public int Kapacitet { get; set; }

        // Podrazumevani konstruktor
        public Prostorija() { }

        // Konstruktor sa parametrima
        public Prostorija(int idProstorije, string nazivProstorije, int kapacitet)
        {
            IdProstorije = idProstorije;
            NazivProstorije = nazivProstorije;
            Kapacitet = kapacitet;
        }
    }
}
