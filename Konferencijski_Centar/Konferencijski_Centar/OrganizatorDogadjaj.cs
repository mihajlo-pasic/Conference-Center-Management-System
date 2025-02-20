using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Konferencijski_Centar
{
    internal class OrganizatorDogadjaj
    {
        public int OrganizatorIdOrganizatora { get; set; }
        public int DogadjajIdDogadjaja { get; set; }

        // Podrazumevani konstruktor
        public OrganizatorDogadjaj() { }

        // Konstruktor sa parametrima
        public OrganizatorDogadjaj(int organizatorIdOrganizatora, int dogadjajIdDogadjaja)
        {
            OrganizatorIdOrganizatora = organizatorIdOrganizatora;
            DogadjajIdDogadjaja = dogadjajIdDogadjaja;
        }
    }
}
