using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencijski_Centar
{
    internal class Prijava
    {
        public int IdPrijave { get; set; }
        public int? IdUcesnika { get; set; }
        public int? IdSesije { get; set; }

        // Podrazumevani konstruktor
        public Prijava() { }

        // Konstruktor sa parametrima
        public Prijava(int idPrijave, int? idUcesnika, int? idSesije)
        {
            IdPrijave = idPrijave;
            IdUcesnika = idUcesnika;
            IdSesije = idSesije;
        }
    }
}
