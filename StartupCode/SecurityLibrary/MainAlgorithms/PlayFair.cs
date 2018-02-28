using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary.MainAlgorithms
{
    public class PlayFair : ICryptographicTechnique<string, string>
    {
        private readonly char[,] _cipherAlphabet = new char[5, 5];
        private readonly char[] _alphabet =
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'
        };
        public string Decrypt(string cipherText, string key)
        {

            key = key.ToUpper();
            cipherText = cipherText.ToUpper();
            IntializeAlphabet(key);
            List<string> cipherTextArray = splitCipherText(cipherText);
            string plainText = GetPlainText(cipherTextArray);
            plainText = GetFormattedPlainText(plainText);




            return plainText;
        }

        public string Encrypt(string plainText, string key)
        {
            key = key.ToUpper();
            plainText = plainText.ToUpper();
            IntializeAlphabet(key);
            List<string> plainTextArray = splitPlainText(plainText);
            string cipherText = GetCipherText(plainTextArray);
            return cipherText;
        }

        public string Analyse(string plainText, string cipherText)
        {
            throw new NotImplementedException();
        }

        // ============= Encrypt Helper Functions ====================
        private bool CharExist(char c)
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (_cipherAlphabet[i, j] == 'J' && c == 'I')
                        return true;
                    if (_cipherAlphabet[i, j] == 'I' && c == 'J')
                        return true;
                    if (_cipherAlphabet[i, j] == c)
                        return true;
                }
            }
            return false;
        }

        private List<string> splitPlainText(string plainText)
        {
            // Checking for similar chars in each pair
            bool duplicate = true;
            while (duplicate)
            {
                duplicate = false;
                for (int i = 0; i < plainText.Length - 1; i = i + 2)
                {
                    if (plainText[i] == plainText[i + 1])
                    {
                        plainText = plainText.Insert(i + 1, "X");
                        duplicate = true;
                        break;
                    }
                }
            }

            // Checking if it has odd number of chars to add X at the end 
            if (plainText.Length % 2 != 0)
            {
                plainText = plainText.Insert(plainText.Length, "X");
            }

            // Add Delimeter between every 2 chars
            for (int i = 2; i < plainText.Length; i = i + 2)
            {
                plainText = plainText.Insert(i, "-");
                i++;
            }

            // Split 2 chars in Plain Text Array
            List<string> splittedPlainText = plainText.Split('-').ToList();

            return splittedPlainText;
        }

        private string GetCipherText(List<string> plainTextArray)
        {
            string cipher = "";
            foreach (string pair in plainTextArray)
            {
                cipher += GetCorrespondingCipherPairString(pair);
            }
            return cipher;
        }

        private string GetCorrespondingCipherPairString(string pair)
        {
            int char1I = -1, char2I = -1, char1J = -1, char2J = -1;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (_cipherAlphabet[i, j] == pair[0])
                    {
                        char1I = i;
                        char1J = j;
                    }
                    else if (_cipherAlphabet[i, j] == pair[1])
                    {
                        char2I = i;
                        char2J = j;
                    }
                }
            }

            // On the same ROW
            if (char1I == char2I && char1I != -1)
            {
                string text = "";
                if (char1J + 1 == 5)
                    text += _cipherAlphabet[char1I, 0];
                else
                    text += _cipherAlphabet[char1I, char1J + 1];
                if (char2J + 1 == 5)
                    text += _cipherAlphabet[char2I, 0];
                else
                {
                    text += _cipherAlphabet[char2I, char2J + 1];
                }
                return text;
            }

            // On the same COLUMN
            if (char1J == char2J && char1J != -1)
            {
                string text = "";
                if (char1I + 1 == 5)
                    text += _cipherAlphabet[0, char1J];
                else
                    text += _cipherAlphabet[char1I + 1, char1J];
                if (char2I + 1 == 5)
                    text += _cipherAlphabet[0, char2J];
                else
                {
                    text += _cipherAlphabet[char2I + 1, char2J];
                }
                return text;
            }

            // On diffrent ROW & COLUMN
            {
                string text = "";

                text += _cipherAlphabet[char1I, char2J];
                text += _cipherAlphabet[char2I, char1J];
                return text;
            }
        }

        // ============= Decrypt Helper Functions ====================
        private string GetPlainText(List<string> cipherTextArray)
        {
            string plain = "";
            foreach (string plainPair in cipherTextArray)
            {
                plain += GetCorrespondingPlainPairString(plainPair);
            }
            return plain;
        }

        private string GetCorrespondingPlainPairString(string plainPair)
        {
            int char1I = -1, char2I = -1, char1J = -1, char2J = -1;

            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (_cipherAlphabet[i, j] == plainPair[0])
                    {
                        char1I = i;
                        char1J = j;
                    }
                    else if (_cipherAlphabet[i, j] == plainPair[1])
                    {
                        char2I = i;
                        char2J = j;
                    }
                }
            }

            // On the same ROW
            if (char1I == char2I && char1I != -1)
            {
                string text = "";
                if (char1J == 0)
                    text += _cipherAlphabet[char1I, 4];
                else
                    text += _cipherAlphabet[char1I, char1J - 1];
                if (char2J == 0)
                    text += _cipherAlphabet[char2I, 4];
                else
                {
                    text += _cipherAlphabet[char2I, char2J - 1];
                }
                return text;
            }

            // On the same COLUMN
            if (char1J == char2J && char1J != -1)
            {
                string text = "";
                if (char1I == 0)
                    text += _cipherAlphabet[4, char1J];
                else
                    text += _cipherAlphabet[char1I - 1, char1J];
                if (char2I == 0)
                    text += _cipherAlphabet[4, char2J];
                else
                {
                    text += _cipherAlphabet[char2I - 1, char2J];
                }
                return text;
            }

            // On diffrent ROW & COLUMN
            {
                string text = "";

                text += _cipherAlphabet[char1I, char2J];
                text += _cipherAlphabet[char2I, char1J];
                return text;
            }
        }

        private List<string> splitCipherText(string cipherText)
        {
            // Add Delimeter between every 2 chars
            for (int i = 2; i < cipherText.Length; i = i + 2)
            {
                cipherText = cipherText.Insert(i, "-");
                i++;
            }

            // Split 2 chars in Plain Text Array
            List<string> splittedCipherText = cipherText.Split('-').ToList();

            return splittedCipherText;
        }

        private string GetFormattedPlainText(string plainText)
        {
            if (plainText[plainText.Length - 1] == 'X')
                plainText = plainText.Remove(plainText.Length - 1, 1);

            string s = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                if (i+1 < plainText.Length && plainText[i] == 'X' && plainText[i - 1] == plainText[i + 1] && i % 2 != 0)
                {
                }
                else
                {
                    s += plainText[i];
                }
            }

            return s;
        }

        // ============= Shared Helper Functions ====================
        private void IntializeAlphabet(string key)
        {
            // Intializing cipher alphabet array with % car
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    _cipherAlphabet[i, j] = '%';
                }
            }

            // Adding PlainText charachters to Cipher alphabet array
            bool endPlain = false;
            int pTindex = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (pTindex == key.Length)
                    {
                        endPlain = true;
                        break;
                    }
                    if (CharExist(key[pTindex]))
                    {
                        j--;
                    }
                    else
                    {
                        _cipherAlphabet[i, j] = key[pTindex];
                    }
                    pTindex++;
                }
                if (endPlain)
                    break;
            }

            // checking If cipher alphabet has empty cells
            int index = 0;
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 5; j++)
                {
                    if (_cipherAlphabet[i, j] == '%')
                    {
                        while (index < 25 && CharExist(_alphabet[index]))
                        {
                            index++;
                        }
                        if (!CharExist(_alphabet[index]))
                        {
                            _cipherAlphabet[i, j] = _alphabet[index];
                        }
                    }
                }
            }
        }
    }
}
