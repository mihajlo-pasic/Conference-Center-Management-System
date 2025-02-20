using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencijski_Centar
{
    internal class PredavacSesija
    {
        public int PredavacIdPredavaca { get; set; }
        public int SesijaIdSesije { get; set; }

        // Podrazumevani konstruktor
        public PredavacSesija() { }

        // Konstruktor sa parametrima
        public PredavacSesija(int predavacIdPredavaca, int sesijaIdSesije)
        {
            PredavacIdPredavaca = predavacIdPredavaca;
            SesijaIdSesije = sesijaIdSesije;
        }
    }
}
