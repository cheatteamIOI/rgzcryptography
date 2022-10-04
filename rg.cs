using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace budilnik
{
    class rg
    {
        public int Uniform(int a, int b)
        {
            Random randsgener = new Random();
            int rands = randsgener.Next(a, b);
            return a + rands * (b - a) / b;
        }
    }
}
