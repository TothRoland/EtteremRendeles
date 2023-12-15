using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Étterem_rendelés
{
    public class adatok
    {
        public adatok(string nev, int mennyiseg, int ar)
        {
            this.nev = nev;
            this.mennyiseg = mennyiseg;
            this.ar = ar;
        }

        public string nev { get; set; }
        public int mennyiseg { get; set; }
        public int ar { get; set; }
    }
}
