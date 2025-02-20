using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencijski_Centar
{
    internal class Dogadjaj
    {
        public int IdDogadjaja { get; set; }
        public string Naziv { get; set; }
        public string Opis { get; set; }
        public DateTime DatumPocetka { get; set; }
        public DateTime DatumZavrsetka { get; set; }

        // Podrazumevani konstruktor
        public Dogadjaj() { }

        // Konstruktor sa parametrima
        public Dogadjaj(int idDogadjaja, string naziv, string opis, DateTime datumPocetka, DateTime datumZavrsetka)
        {
            IdDogadjaja = idDogadjaja;
            Naziv = naziv;
            Opis = opis;
            DatumPocetka = datumPocetka;
            DatumZavrsetka = datumZavrsetka;
        }
    }
}
