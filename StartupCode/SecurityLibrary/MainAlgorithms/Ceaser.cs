using System;
using System.Collections.Generic;
using System.Linq;

namespace SecurityLibrary.MainAlgorithms
{
    public class Ceaser : ICryptographicTechnique<string, int>
    {
        private readonly Dictionary<char, int> _alphabet = new Dictionary<char, int>()
        {
            {'A', 0},
            {'B', 1},
            {'C', 2},
            {'D', 3},
            {'E', 4},
            {'F', 5},
            {'G', 6},
            {'H', 7},
            {'I', 8},
            {'J', 9},
            {'K', 10},
            {'L', 11},
            {'M', 12},
            {'N', 13},
            {'O', 14},
            {'P', 15},
            {'Q', 16},
            {'R', 17},
            {'S', 18},
            {'T', 19},
            {'U', 20},
            {'V', 21},
            {'W', 22},
            {'X', 23},
            {'Y', 24},
            {'Z', 25},
        };

        public string Encrypt(string plainText, int key)
        {
            string tmp = "";
            plainText = plainText.ToUpper();

            foreach (var c in plainText)
            {
                int index = (_alphabet[c] + key) % 26;
                char tmpChar = _alphabet.FirstOrDefault(x => x.Value == index).Key;
                tmp += tmpChar;
            }
            return tmp;
        }

        public string Decrypt(string cipherText, int key)
        {
            string tmp = "";

            foreach (var c in cipherText)
            {
                int index = (_alphabet[c] - key) % 26;
                index = (index < 0) ? (index + 26) : index;
                char tmpChar = _alphabet.FirstOrDefault(x => x.Value == index).Key;
                tmp += tmpChar;
            }
            return tmp;
        }

        public int Analyse(string plainText, string cipherText)
        {
            plainText = plainText.ToUpper();
            var p = _alphabet[plainText[0]];
            var c = _alphabet[cipherText[0]];
            return ((c - p) < 0)? ((c - p) + 26) : (c - p);
        }
    }
}
