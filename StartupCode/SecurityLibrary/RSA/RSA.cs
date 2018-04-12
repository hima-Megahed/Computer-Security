using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SecurityLibrary.MainAlgorithms;

namespace SecurityLibrary.RSA
{
    public class RSA
    {
        public int Encrypt(int p, int q, int M, int e)
        {
            int N = p * q;

            int result = Convert.ToInt32(power(M, e, N));
            return result;
        }

        public int Decrypt(int p, int q, int C, int e)
        {
            int N = p * q;
            int totiont = (p - 1) * (q - 1);
            long D = get_GCD_and_Inverse(e, totiont)[1];
            int result = Convert.ToInt32(power(C, Convert.ToInt32(D), N));
            return result;
        }

        // ===================Helper functions=======================
        private long[] get_GCD_and_Inverse(long b, long m)
        {
            long[] result = new long[2];
            result[1] = -1; // In case there is not inverse
            long Q, A1 = 1, A2 = 0, A3 = m, B1 = 0, B2 = 1, B3 = b;

            while (true)
            {
                Q = Convert.ToInt64(Math.Floor(Convert.ToDecimal(A3) / B3));

                long tmpA1 = A1, tmpA2 = A2, tmpA3 = A3;
                A1 = B1;
                A2 = B2;
                A3 = B3;

                B1 = tmpA1 - Q * B1;
                B2 = tmpA2 - Q * B2;
                B3 = tmpA3 - Q * B3;

                if (B3 == 0)
                {
                    result[0] = A3;
                    result[1] = -1;
                    return result;
                }

                if (B3 == 1)
                {
                    result[0] = B3;
                    result[1] = B2;
                    if (B2 < 0)
                        result[1] += m;
                    return result;
                }
            }

        }

        private long power(int M, int E, int mod)
        {
            if (E == 0)
                return 1;
            if (E == 1)
                return M;

            var res = power(M, E / 2, mod) % mod;

            if (E % 2 == 0)
            {
                return (res * res) % mod;
            }

            return ((M % mod) * (power(M, E - 1, mod) % mod) % mod);
        }
    }

}
