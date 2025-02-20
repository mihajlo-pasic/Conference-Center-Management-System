using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencijski_Centar
{
    internal class Sesija
    {
        public int IdSesije { get; set; }
        public int? IdDogadjaja { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public DateTime Datum { get; set; }
        public TimeSpan VrijemePocetka { get; set; }
        public TimeSpan VrijemeZavrsetka { get; set; }
        public int ProstorijaIdProstorije { get; set; }

        // Podrazumevani konstruktor
        public Sesija() { }

        // Konstruktor sa parametrima
        public Sesija(int idSesije, int? idDogadjaja, string naziv, string opis, DateTime datum, TimeSpan vrijemePocetka, TimeSpan vrijemeZavrsetka, int prostorijaIdProstorije)
        {
            IdSesije = idSesije;
            IdDogadjaja = idDogadjaja;
            Naziv = naziv;
            Opis = opis;
            Datum = datum;
            VrijemePocetka = vrijemePocetka;
            VrijemeZavrsetka = vrijemeZavrsetka;
            ProstorijaIdProstorije = prostorijaIdProstorije;
        }
    }
}
