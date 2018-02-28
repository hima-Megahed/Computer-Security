using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary.MainAlgorithms
{
    public class Monoalphabetic : ICryptographicTechnique<string, string>
    {
        private Dictionary<char, char> _alphabet =  new Dictionary<char, char>();
        public string Analyse(string plainText, string cipherText)
        {
            cipherText = cipherText.ToLower();
            string key = "";
            List<char> alphabet= new List<char>()
            {
                'a',
                'b',
                'c',
                'd',
                'e',
                'f',
                'g',
                'h',
                'i',
                'j',
                'k',
                'l',
                'm',
                'n',
                'o',
                'p',
                'q',
                'r',
                's',
                't',
                'u',
                'v',
                'w',
                'x',
                'y',
                'z'
            };
            for (int i = 0; i < alphabet.Count; i++)
            {
               _alphabet.Add(alphabet[i], '#');
            }
            for (int i = 0; i < plainText.Length; i++)
            {
                _alphabet[plainText[i]] = cipherText[i];
            }
            int tmpInd = 0;
            foreach (var AlphaKay in _alphabet.Keys.ToList())
            {
                if (_alphabet[AlphaKay] == '#')
                {
                    for (int i = 0; i < alphabet.Count; i++)
                    {
                        if (!_alphabet.ContainsValue(alphabet[i]))
                        {
                            _alphabet[AlphaKay] = alphabet[i];
                            break;
                        }
                    }
                }

                key += _alphabet[AlphaKay];
            }
            return key;
        }

        public string Decrypt(string cipherText, string key)
        {
            key = key.ToUpper();
            Intialize(key);
            string plain = "";

            foreach (char c in cipherText)
            {
                plain += _alphabet.FirstOrDefault(x => x.Value == c).Key;
            }
            return plain;
        }

        public string Encrypt(string plainText, string key)
        {
            key = key.ToUpper();
            Intialize(key);
            string cipher = "";
            for (int i = 0; i < plainText.Length; i++)
            {
                cipher += _alphabet[plainText[i]];
            }
            return cipher;
        }

        /// <summary>
        /// Frequency Information:
        /// E   12.51%
        /// T	9.25
        /// A	8.04
        /// O	7.60
        /// I	7.26
        /// N	7.09
        /// S	6.54
        /// R	6.12
        /// H	5.49
        /// L	4.14
        /// D	3.99
        /// C	3.06
        /// U	2.71
        /// M	2.53
        /// F	2.30
        /// P	2.00
        /// G	1.96
        /// W	1.92
        /// Y	1.73
        /// B	1.54
        /// V	0.99
        /// K	0.67
        /// X	0.19
        /// J	0.16
        /// Q	0.11
        /// Z	0.09
        /// </summary>
        /// <param name="cipher"></param>
        /// <returns>Plain text</returns>
        public string AnalyseUsingCharFrequency(string cipher)
        {
            cipher = cipher.ToLower();
            Dictionary<double, char> CharFrequency = new Dictionary<double, char>();
            {
                CharFrequency.Add(12.51,'e');
                CharFrequency.Add(9.25,'t');
                CharFrequency.Add(8.04,'a');
                CharFrequency.Add(7.60,'o');
                CharFrequency.Add(7.26,'i');
                CharFrequency.Add(7.09,'n');
                CharFrequency.Add(6.54,'s');
                CharFrequency.Add(6.12,'r');
                CharFrequency.Add(5.49,'h');
                CharFrequency.Add(4.14,'l');
                CharFrequency.Add(3.99,'d');
                CharFrequency.Add(3.06,'c');
                CharFrequency.Add(2.71,'u');
                CharFrequency.Add(2.53,'m');
                CharFrequency.Add(2.30,'f');
                CharFrequency.Add(2.00,'p');
                CharFrequency.Add(1.96,'g');
                CharFrequency.Add(1.92,'w');
                CharFrequency.Add(1.73,'y');
                CharFrequency.Add(1.54,'b');
                CharFrequency.Add(0.99,'v');
                CharFrequency.Add(0.67,'k');
                CharFrequency.Add(0.19,'x');
                CharFrequency.Add(0.16,'j');
                CharFrequency.Add(0.11,'q');
                CharFrequency.Add(0.09,'z');
            }
            Dictionary<char, int> CipherCharsCount = new Dictionary<char, int>();
            {
                CipherCharsCount.Add('e',0);
                CipherCharsCount.Add('t',0);
                CipherCharsCount.Add('a',0);
                CipherCharsCount.Add('o',0);
                CipherCharsCount.Add('i',0);
                CipherCharsCount.Add('n',0);
                CipherCharsCount.Add('s',0);
                CipherCharsCount.Add('r',0);
                CipherCharsCount.Add('h',0);
                CipherCharsCount.Add('l',0);
                CipherCharsCount.Add('d',0);
                CipherCharsCount.Add('c',0);
                CipherCharsCount.Add('u',0);
                CipherCharsCount.Add('m',0);
                CipherCharsCount.Add('f',0);
                CipherCharsCount.Add('p',0);
                CipherCharsCount.Add('g',0);
                CipherCharsCount.Add('w',0);
                CipherCharsCount.Add('y',0);
                CipherCharsCount.Add('b',0);
                CipherCharsCount.Add('v',0);
                CipherCharsCount.Add('k',0);
                CipherCharsCount.Add('x',0);
                CipherCharsCount.Add('j',0);
                CipherCharsCount.Add('q',0);
                CipherCharsCount.Add('z',0);
            }
            Dictionary<char,char> map = new Dictionary<char, char>();
            {
                map.Add('a', '#');
                map.Add('b', '#');
                map.Add('c', '#');
                map.Add('d', '#');
                map.Add('e', '#');
                map.Add('f', '#');
                map.Add('g', '#');
                map.Add('h', '#');
                map.Add('i', '#');
                map.Add('j', '#');
                map.Add('k', '#');
                map.Add('l', '#');
                map.Add('m', '#');
                map.Add('n', '#');
                map.Add('o', '#');
                map.Add('p', '#');
                map.Add('q', '#');
                map.Add('r', '#');
                map.Add('s', '#');
                map.Add('t', '#');
                map.Add('u', '#');
                map.Add('v', '#');
                map.Add('w', '#');
                map.Add('x', '#');
                map.Add('y', '#');
                map.Add('z', '#');  
            }

            foreach (var CharCountkey in CipherCharsCount.Keys.ToList())
            {
                int counter = 0;
                foreach (char c in cipher)
                {
                    if (CharCountkey == c)
                        counter++;
                }
                CipherCharsCount[CharCountkey] = counter;
            }
            var sortedDict = from entry in CipherCharsCount orderby entry.Value descending select entry;

            var ArrayCharFrequency = CharFrequency.ToList();
            int CharFreqind = 0;
            foreach (var mapkey in sortedDict)
            {
                map[mapkey.Key] = ArrayCharFrequency[CharFreqind].Value;
                CharFreqind++;
            }

            string plain = "";
            foreach (var c in cipher)
            {
                plain += map[c];
            }

            return plain;
        }

        // ============ Helper Encrypt Functions ===========================
        private void Intialize(string key)
        {
            int ind = 0;
            for (char c = 'a'; c <= 'z'; c++)
            {
                _alphabet.Add(c, key[ind++]);
            }
        }
    }
}
