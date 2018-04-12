using System;
using System.Collections.Generic;

namespace SecurityLibrary.MainAlgorithms
{
    /// <summary>
    /// The List<int> is row based. Which means that the key is given in row based manner.
    /// </summary>
    public class HillCipher : ICryptographicTechnique<List<int>, List<int>>
    {
        public List<int> Analyse(List<int> plainText, List<int> cipherText)
        {
            if (plainText.Count % 3 != 0)
                throw new InvalidAnlysisException();

            int[,] plain = get_data_analysis(plainText);
            int[,] cipher = get_data_analysis(cipherText);

            for (int i = 0; i < plain.GetLength(0); i++) // Crawel Columns
            {
                for (int j = i + 1; j < plain.GetLength(1); j++) // Crawel columns starting from i+1
                {
                    int[,] subPlain = new int[2, 2];
                    {
                        subPlain[0, 0] = plain[0, i];
                        subPlain[0, 1] = plain[0, j];
                        subPlain[1, 0] = plain[1, i];
                        subPlain[1, 1] = plain[1, j];
                    }

                    // Checking If subPlain has Inverse
                    int det = Determinant(subPlain);
                    int[] result = get_GCD_and_Inverse(det, 26);

                    if (result[1] != -1)
                    {
                        int[,] subcipher = new int[2, 2];
                        {
                            subcipher[0, 0] = cipher[0, i];
                            subcipher[0, 1] = cipher[0, j];
                            subcipher[1, 0] = cipher[1, i];
                            subcipher[1, 1] = cipher[1, j];
                        }
                        int tmp = subcipher[0, 1];
                        subcipher[0, 1] = subcipher[1, 0];
                        subcipher[1, 0] = tmp;

                        int[,] plainInverse = InverseMatrix(subPlain);
                        List<int> key = MultiplyMatrix(plainInverse, subcipher);
                        return key;
                    }
                    
                }
            }
            throw new Exception("Error Analysis!");
        }


        public List<int> Decrypt(List<int> cipherText, List<int> key)
        {
            int[,] mykey = get_key(key);
            int[,] mycipher = get_cipher(cipherText, key);
            int det = Determinant(mykey);
            int gcd = GCD(26, det);
            if (gcd == 1)
            {
                int[,] inv = InverseMatrix(mykey);
                inv = Transpose(inv);
                
                List<int> result = new List<int>();
                result = MultiplyMatrix(inv, mycipher);
                return result;
            }
            else
            {
                throw new InvalidAnlysisException();
            }
        }


        public List<int> Encrypt(List<int> plainText, List<int> key)
        {
            int[,] mykey = get_key(key);
            int[,] myplain = get_plain(plainText, key);
            List<int> result = MultiplyMatrix(mykey, myplain);
            return result;
        }


        public List<int> Analyse3By3Key(List<int> plainText, List<int> cipherText)
        {
            int[,] plain = get_plain(plainText);
            int[,] cipher = get_cipher(cipherText);

            int[,] plainInverse = InverseMatrix(plain);
            List<int> key = MultiplyMatrix(plainInverse, Transpose(cipher));

            return key;
        }

        // =============================Encrypt==================================
        private List<int> MultiplyMatrix(int[,] key, int[,] plain)
        {
            int[,] c = new int[key.GetLength(0), plain.GetLength(1)];
            for (int i = 0; i < c.GetLength(0); i++)
            {
                for (int j = 0; j < c.GetLength(1); j++)
                {
                    c[i, j] = 0;
                    for (int k = 0; k < key.GetLength(1); k++) // OR k<b.GetLength(0)
                        c[i, j] = (c[i, j] + (key[i, k] * plain[k, j])%26) %26;
                }
            }

            List<int> result = new List<int>();
            for (int i = 0; i < c.GetLength(1); i++)
            {
                for (int j = 0; j < c.GetLength(0); j++)
                {
                    result.Add(c[j,i]);
                }
            }
            return result;
        }

        private int[,] get_key(List<int> key)
        {
            int index = 0;
            int size = Convert.ToInt32(Math.Sqrt(key.Count));
            int[,] mykey = new int[size, size];
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    mykey[i, j] = key[index++];
                }
            }
            return mykey;
        }

        private int[,] get_plain(List<int> plain, List<int> key)
        {
            int size = Convert.ToInt32(Math.Sqrt(key.Count));
            int[,] myplain = plain.Count % size == 0 ? new int[size, plain.Count / size] : new int[size, (plain.Count / size) + 1];
            int ind = 0;

            for (int i = 0; i < myplain.GetLength(1); i++)
            {
                for (int j = 0; j < myplain.GetLength(0); j++)
                {
                    myplain[j, i] = plain[ind++];
                }
            }
            return myplain;
        }

        // =============================Decrypt Functions============================
        private int GCD(int a, int b)
        {
            while (a != 0 && b != 0)
            {
                if (a > b)
                    a %= b;
                else
                    b %= a;
            }

            return a == 0 ? b : a;
        }

        private int Determinant(int [,]arr1)
        {
            int det = 0;
            if (arr1.GetLength(0) == 2)
            {
                det = (arr1[0, 0] * arr1[1, 1]) - (arr1[0, 1] * arr1[1, 0]);
            }
            else
            {
                for (int i = 0; i < 3; i++)
                    det = det + (arr1[0, i] * (arr1[1, (i + 1) % 3] * arr1[2, (i + 2) % 3] - arr1[1, (i + 2) % 3] * arr1[2, (i + 1) % 3]));
            }
            det %= 26;
            if (det < 0)
                det += 26;
            return det ;
        }

        private int[,] InverseMatrix(int[,] matrix)
        {
            int d = Determinant(matrix);
            int[,] inverse = new int [matrix.GetLength(0), matrix.GetLength(1)];
            int[] res = get_GCD_and_Inverse(d, 26);

            if (matrix.GetLength(0) == 2)
            {
                // 2x2
                inverse[0, 0] = (res[1] * Convert.ToInt32(Math.Pow(-1, 0)) * matrix[1, 1]) % 26;
                inverse[0, 1] = (res[1] * Convert.ToInt32(Math.Pow(-1, 1)) * matrix[1, 0]) % 26;
                inverse[1, 0] = (res[1] * Convert.ToInt32(Math.Pow(-1, 1)) * matrix[0, 1]) % 26;
                inverse[1, 1] = (res[1] * Convert.ToInt32(Math.Pow(-1, 2)) * matrix[0, 0]) % 26;

                for (int i = 0; i < inverse.GetLength(0); i++)
                {
                    for (int j = 0; j < inverse.GetLength(1); j++)
                    {
                        if (inverse[i, j] < 0)
                        {
                            inverse[i, j] += 26;
                        }
                    }
                }
            }
            else
            {
                // 3x3
                inverse[0, 0] = (res[1] * Convert.ToInt32(Math.Pow(-1, 0)) * ((matrix[1,1] * matrix[2,2]) - (matrix[1,2] * matrix[2,1]))) % 26;
                inverse[0, 1] = (res[1] * Convert.ToInt32(Math.Pow(-1, 1)) * ((matrix[1,0] * matrix[2,2]) - (matrix[1,2] * matrix[2,0]))) % 26;
                inverse[0, 2] = (res[1] * Convert.ToInt32(Math.Pow(-1, 2)) * ((matrix[1,0] * matrix[2,1]) - (matrix[1,1] * matrix[2,0]))) % 26;
                inverse[1, 0] = (res[1] * Convert.ToInt32(Math.Pow(-1, 1)) * ((matrix[0,1] * matrix[2,2]) - (matrix[0,2] * matrix[2,1]))) % 26;
                inverse[1, 1] = (res[1] * Convert.ToInt32(Math.Pow(-1, 2)) * ((matrix[0,0] * matrix[2,2]) - (matrix[0,2] * matrix[2,0]))) % 26;
                inverse[1, 2] = (res[1] * Convert.ToInt32(Math.Pow(-1, 3)) * ((matrix[0,0] * matrix[2,1]) - (matrix[0,1] * matrix[2,0]))) % 26;
                inverse[2, 0] = (res[1] * Convert.ToInt32(Math.Pow(-1, 2)) * ((matrix[0,1] * matrix[1,2]) - (matrix[0,2] * matrix[1,1]))) % 26;
                inverse[2, 1] = (res[1] * Convert.ToInt32(Math.Pow(-1, 3)) * ((matrix[0,0] * matrix[1,2]) - (matrix[0,2] * matrix[1,0]))) % 26;
                inverse[2, 2] = (res[1] * Convert.ToInt32(Math.Pow(-1, 4)) * ((matrix[0,0] * matrix[1,1]) - (matrix[0,1] * matrix[1,0]))) % 26;

                for (int i = 0; i < inverse.GetLength(0); i++)
                {
                    for (int j = 0; j < inverse.GetLength(1); j++)
                    {
                        if (inverse[i, j] < 0)
                        {
                            inverse[i, j] += 26;
                        }
                    }
                }
            }
            return inverse;
        }

        private int[,] Transpose(int[,] inv)
        {
            int[,] result = new int[inv.GetLength(1), inv.GetLength(0)];
            for (int i = 0; i < inv.GetLength(0); i++)
            {
                for (int j = 0; j < inv.GetLength(1); j++)
                {
                    result[j, i] = inv[i, j];
                }
            }
            return result;
        }

        private int[,] get_cipher(List<int> cipherText, List<int> key)
        {
            int size = Convert.ToInt32(Math.Sqrt(key.Count));
            int[,] mycipher = cipherText.Count % size == 0 ? new int[size, cipherText.Count / size] : new int[size, (cipherText.Count / size) + 1];
            int ind = 0;

            for (int i = 0; i < mycipher.GetLength(1); i++)
            {
                for (int j = 0; j < mycipher.GetLength(0); j++)
                {
                    mycipher[j, i] = cipherText[ind++];
                }
            }
            return mycipher;
        }

        private int[] get_GCD_and_Inverse(int b, int m)
        {
            int [] result = new int[2];
            result[1] = -1; // In case there is not inverse
            int Q, A1=1, A2=0, A3=m, B1=0, B2=1, B3=b;

            while (true)
            {
                Q = A3 / B3;

                int tmpA1 = A1, tmpA2 = A2, tmpA3 = A3;
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
                    return result;
                }
            }
        }

        //===========================Analysis Functions===============================
        private int[,] get_plain(List<int> plainText)
        {
            int ind = 0;
            int[,] myplain = new int[3, plainText.Count/3];

            for (int i = 0; i < myplain.GetLength(1); i++)
            {
                for (int j = 0; j < myplain.GetLength(0); j++)
                {
                    myplain[j, i] = plainText[ind++];
                }
            }
            return myplain;
        }

        private int[,] get_cipher(List<int> cipherText)
        {
            int[,] mycipher = new int[3, cipherText.Count / 3] ;
            int ind = 0;

            for (int i = 0; i < mycipher.GetLength(1); i++)
            {
                for (int j = 0; j < mycipher.GetLength(0); j++)
                {
                    mycipher[j, i] = cipherText[ind++];
                }
            }
            return mycipher;
        }

        private int[,] get_data_analysis(List<int> data)
        {
            int[,] pre_data = new int[2, data.Count / 2];
            int ind = 0;

            for (int i = 0; i < pre_data.GetLength(1); i++)
            {
                for (int j = 0; j < pre_data.GetLength(0); j++)
                {
                    pre_data[j, i] = data[ind++];
                }
            }
            return pre_data;
        }
    }
}
