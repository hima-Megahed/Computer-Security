using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SecurityLibrary.RC4
{
    /// <summary>
    /// If the string starts with 0x.... then it's Hexadecimal not string
    /// </summary>
    public class RC4 : CryptographicTechnique
    {
        public override string Decrypt(string cipherText, string key)
        {
            if (Hexa(cipherText) && Hexa(key))
            {
                cipherText = HexToString(cipherText);
                key = HexToString(key);
                byte[] Tbyte = StringToByte(cipherText);
                byte[] KeyByte = StringToByte(key);
                string result = RC4Algorithm(Tbyte, KeyByte);
                string ResInHex = StringToHex(result);
                return ResInHex;
            }
            else
            {
                byte[] Tbyte = StringToByte(cipherText);
                byte[] KeyByte = StringToByte(key);
                string result = RC4Algorithm(Tbyte, KeyByte);
                return result;
            }
        }

        public override string Encrypt(string plainText, string key)
        {
            if (Hexa(plainText) && Hexa(key))
            {
                plainText = HexToString(plainText);
                key = HexToString(key);
                byte[] Tbyte = Encoding.ASCII.GetBytes(plainText);
                byte[] KeyByte = Encoding.ASCII.GetBytes(key);
                string result = RC4Algorithm(Tbyte, KeyByte);
                string ResInHex = StringToHex(result);
                return ResInHex;
            }
            else
            {
                byte[] Tbyte = Encoding.ASCII.GetBytes(plainText);
                byte[] KeyByte = Encoding.ASCII.GetBytes(key);
                string result = RC4Algorithm(Tbyte, KeyByte);
                return result;
            }
        }

        private bool Hexa(string text)
        {
            return text[0] == '0' && (text[1] == 'x' || text[1] == 'X');
        }

        private string HexToString(string HexText)
        {
            string tmp = "";
            for (int i = 2; i < HexText.Length; i += 2)
            {
                string hexChar = HexText.Substring(i, 2);
                int val = Convert.ToInt32(hexChar, 16);
                tmp += (char)val;
            }
            return tmp;
        }

        private string StringToHex(string text)
        {
            string Hex = "0X";
            char[] values = text.ToCharArray();
            foreach (char letter in values)
            {
                int value = Convert.ToInt32(letter);
                string hexOutput = $"{value:X}";
                Hex += hexOutput;
            }
            return Hex;
        }

        private string RC4Algorithm(byte[] Tbyte, byte[] keyByte)
        {
            byte[] S = new byte[256];
            byte[] T = new byte[256];
            byte tmp;
            int i = 0, j = 0;

            // RC4 A- Intialization of S & T
            for (; i < 256; i++)
            {
                S[i] = (byte)i;
                T[i] = keyByte[i % keyByte.Length];
            }
            // RC4 B- Intial permutation of S
            for (i = 0; i < 256; i++)
            {
                j = (j + S[i] + T[i]) % 256;
                tmp = S[i];
                S[i] = S[j];
                S[j] = tmp;
            }
            // RC4 C&D- Generation of key stream S & XOR with S
            i = j = 0;
            for (int x = 0; x < Tbyte.Length; x++)
            {
                i = (i + 1) % 256;
                j = (j + S[i]) % 256;
                tmp = S[i];
                S[i] = S[j];
                S[j] = tmp;
                int t = (S[i] + S[j]) % 256;
                Tbyte[x] ^= S[t];
            }

            // Converting Byte array to string
            return Tbyte.Aggregate("", (current, t) => current + (char)t);
        }

        private byte[] StringToByte(string text)
        {
            byte[] bytes = new byte[text.Length];
            for (int i = 0; i < text.Length; i++)
            {
                bytes[i] = (byte)text[i];
            }
            return bytes;
        }
    }
}
