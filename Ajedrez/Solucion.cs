using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Ajedrez
{
    class Solucion
    {
        internal bool[,] sol;
        internal int nK;
        internal Solucion(bool[,] s, int n)
        {
            sol = s;
            nK = n;
        }
    }
}
