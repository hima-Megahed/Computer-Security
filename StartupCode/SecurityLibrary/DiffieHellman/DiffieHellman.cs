using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.DiffieHellman
{
    public class DiffieHellman 
    {
        public List<int> GetKeys(int q, int alpha, int xa, int xb)
        {
            List<int> result = new List<int>();
            int YA = (int)power(alpha, xa, q);
            int YB = (int)power(alpha, xb, q);

            int K1 = (int)power(YB, xa, q);
            int K2 = (int)power(YA, xb, q);
            result.Add(K1);
            result.Add(K2);

            return result;
        }

        private long power(int M, int E, int mod)
        {
            long res;
            if (E == 0)
                return 1;
            if (E == 1)
                return M;

            if (E % 2 == 0)
            {
                res = power(M, E / 2, mod) % mod;
                return (res * res) % mod;
            }
            else
                return ((M % mod) * (power(M, E - 1, mod) % mod) % mod);
        }
    }
}
