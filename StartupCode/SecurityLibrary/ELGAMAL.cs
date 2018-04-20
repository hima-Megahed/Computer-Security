using System;
using System.Collections.Generic;

namespace SecurityLibrary
{
    public class ElGamal
    {
        /// <summary>
        /// Encryption
        /// </summary>
        /// <param name="alpha"></param>
        /// <param name="q"></param>
        /// <param name="y"></param>
        /// <param name="k"></param>
        /// <returns>list[0] = C1, List[1] = C2</returns>
        /// 
        public static long Extended_GCD(long A, long B)
        {
            long spare = B;
            if (A < B)
            {
                long temp = A;
                A = B;
                B = temp;
            }
            long r = B, q = 0, x0 = 1, y0 = 0, x1 = 0, y1 = 1, x = 0, y = 0;
            while (r > 1)
            {
                r = A % B;
                q = A / B;
                x = x0 - q * x1;
                y = y0 - q * y1;
                x0 = x1;
                y0 = y1;
                x1 = x;
                y1 = y;
                A = B;
                B = r;
            }
            while (y < 0)
                y += spare;
            return y;
        }
        static public long moduloPower(int a, int b, int n)
        {
            long x = 1, y = a;
            while (b > 0)
            {
                if (b % 2 == 1)
                {
                    x = (x * y) % n;
                }
                y = (y * y) % n;
                b /= 2;
            }
            return x % n;

        }
        public List<long> Encrypt(int q, int alpha, int y, int k, int m)
        {
            //throw new NotImplementedException();
            List<long> result = new List<long>();

            long K = (long)moduloPower(y, k, q);
            long c1 = (long)moduloPower(alpha, k, q);
            long c2 = (long)moduloPower((int)K * m, 1, q);

            result.Add(c1);
            result.Add(c2);
            return result;

        }
        public int Decrypt(int c1, int c2, int x, int q)
        {
            //throw new NotImplementedException();
            int K = (int)moduloPower(c1, x, q);
            int Kinv = (int)Extended_GCD(K, q);
            int M = (int)moduloPower(c2 * Kinv, 1, q);
            return M;

        }
    }
}
