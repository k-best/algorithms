using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace ManyTimePad
{
    class Program
    {
        

        private static readonly char[] charsandspace = new[]
        {
            'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V',
            'W', 'X', 'Y', 'Z', 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r',
            's', 't', 'u', 'v', 'w', 'x', 'y', 'z', (char) 0
        };

        static void Main(string[] args)
        {
            InitLogger();
            var worker = new Worker();
            worker.DeduceKey();
            worker.UpdateKeyFromPartiallyDecryptedMessage(0);
            //worker.UpdateKeyFromPartiallyDecryptedMessage(3);
            worker.UpdateKeyFromPartiallyDecryptedMessage(1);
            //worker.UpdateKeyFromPartiallyDecryptedMessage(5);
            for (int i = 0; i < 11; i++)
            {
                worker.DecryptCipher(i);
            }
            
            //var ciphers = new[]
            //{cipher1, cipher2, cipher3, cipher4, cipher5, cipher6, cipher7, cipher8, cipher9, cipher10};
            //var minLength = ciphers.Select(c => c.Length).Min();
            //var key = new byte[minLength]; 
            //for (int i = 0; i < 10; i++)
            //{
            //    var ci = ciphers[i];
            //    var xors = ciphers.Select(c => XorStrings(ci, c)).ToArray();
            //    var spaceidexes = GetSpaceIndexes(xors);
            //    foreach (var spaceidex in spaceidexes)
            //    {
            //        key[spaceidex] = (byte)((byte)ci[spaceidex] ^ 32);
            //    }
            //}
            //Console.WriteLine(string.Join(",", key));
            //var charkey = new string(key.Select(c => (char)c).ToArray());
            //Console.WriteLine(charkey);

            //foreach (var cipher in ciphers)
            //{
            //    Console.WriteLine(XorStringOnKey(cipher,key));
            //    Console.ReadLine();
            //}
            Console.ReadLine();
        }

        private static void InitLogger()
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget();
            config.AddTarget("file", target);
            target.FileName = "{basedir}/log.txt";
            target.Layout = "${message}";
            var rule2 = new LoggingRule("*", LogLevel.Trace, target);
            config.LoggingRules.Add(rule2);
            LogManager.Configuration = config;
        }

        public static IEnumerable<int> GetSpaceIndexes(IEnumerable<string> xors)
        {
            var minlength = xors.Select(c => c.Length).Min();
            for (int i = 0; i < minlength; i++)
            {
                if (xors.All(c => charsandspace.Contains(c[i])))
                    yield return i;
            }
        }

        public static IEnumerable<int> GetSpaceIndexes(string str1, string str2, string str3, string str4, string str5, string str6,
            string str7, string str8, string str9)
        {
            var lengthes = new[]
            {
                str1.Length, str2.Length, str3.Length, str4.Length, str5.Length, str6.Length, str7.Length, str8.Length, str9.Length
            };
            var minlength = lengthes.Min();
            for (int i = 0; i < minlength; i++)
            {
                if (charsandspace.Contains(str1[i])
                    && charsandspace.Contains(str2[i])
                    && charsandspace.Contains(str3[i])
                    && charsandspace.Contains(str4[i])
                    && charsandspace.Contains(str5[i])
                    && charsandspace.Contains(str6[i])
                    && charsandspace.Contains(str7[i])
                    && charsandspace.Contains(str8[i])
                    && charsandspace.Contains(str9[i]))
                    yield return i;
            }
        }

        public static string XorStrings(string str1, string str2)
        {
            var sb = new StringBuilder();
            var length = str1.Length > str2.Length ? str2.Length : str1.Length;
            for (int i = 0; i < length; i+=2)
            {
                var num1 = GetSymbolToInt(i, str1);
                var num2 = GetSymbolToInt(i, str2);
                var xoredNum = num1 ^ num2;
                sb.Append((char)xoredNum);
            }
            return sb.ToString();
        }

        public static string XorStringOnKey(string str, byte[] key)
        {
            var sb = new StringBuilder();
            for (int i = 0; i < key.Length; i++)
            {

                var num1 = GetSymbolToInt(i, str);
                var xoredNum = num1 ^ key[i];
                sb.Append((char)xoredNum);
            }
            return sb.ToString();
        }

        public static int GetSymbolToInt(int index, string hex)
        {
            string hexdec = hex.Substring(index, 2);//string of one octet in hex
            int number = int.Parse(hexdec, NumberStyles.HexNumber);//the number the hex represented
            return number;
        }

        public static string HexToString(string hex)
        {
            var sb = new StringBuilder();//to hold our result;
            for (int i = 0; i < hex.Length; i += 2)//chunks of two - I'm just going to let an exception happen if there is an odd-length input, or any other error
            {
                string hexdec = hex.Substring(i, 2);//string of one octet in hex
                int number = int.Parse(hexdec, NumberStyles.HexNumber);//the number the hex represented
                char charToAdd = (char)number;//coerce into a character
                sb.Append(charToAdd);//add it to the string being built
            }
            return sb.ToString();//the string we built up.
        }
    }
}
