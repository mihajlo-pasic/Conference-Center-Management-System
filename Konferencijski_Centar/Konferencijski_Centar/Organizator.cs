using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencijski_Centar
{
    internal class Organizator
    {
        public int IdOrganizatora { get; set; }
        public string Ime { get; set; }
        public string KontaktInformacije { get; set; }

        // Podrazumevani konstruktor
        public Organizator() { }

        // Konstruktor sa parametrima
        public Organizator(int idOrganizatora, string ime, string kontaktInformacije)
        {
            IdOrganizatora = idOrganizatora;
            Ime = ime;
            KontaktInformacije = kontaktInformacije;
        }
    }
}
