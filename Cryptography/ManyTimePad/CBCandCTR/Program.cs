using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using NLog;
using NLog.Config;
using NLog.Targets;

namespace CBCandCTR
{
    class Program
    {
        private const string cbcKey = "140b41b22a29beb4061bda66b6747e14";
        private const string cbcCipherText1 = @"4ca00ff4c898d61e1edbf1800618fb2828a226d160dad07883d04e008a7897ee2e4b7465d5290d0c0e6c6822236e1daafb94ffe0c5da05d9476be028ad7c1d81";

        private const string cbcCipherText2 =
            "5b68629feb8606f9a6667670b75b38a5b4832d0f26e1ab7da33249de7d4afc48e713ac646ace36e872ad5fb8a512428a6e21364b0c374df45503473c5242a253";

        private const string CTRKey = "36f18357be4dbd77f050515c73fcf9f2";

        private const string CTRCipherText1 =
            "69dda8455c7dd4254bf353b773304eec0ec7702330098ce7f7520d1cbbb20fc388d1b0adb5054dbd7370849dbf0b88d393f252e764f1f5f7ad97ef79d59ce29f5f51eeca32eabedd9afa9329";

        private const string CTRCipherText2 =
            "770b80259ec33beb2561358a9f2dc617e46218c0a53cbeca695ae45faa8952aa0e311bde9d4e01726d3184c34451";

        static void Main(string[] args)
        {
            InitLogger();
            var worker = new Worker();
            //worker.DecryptCbc(cbcKey, cbcCipherText1);
            //worker.DecryptCbc(cbcKey, cbcCipherText2);
            worker.DecryptCTR(CTRKey, CTRCipherText1);
            worker.DecryptCTR(CTRKey, CTRCipherText2);
            Console.ReadLine();
        }

        private static void InitLogger()
        {
            var config = new LoggingConfiguration();
            var target = new FileTarget();
            config.AddTarget("file", target);
            target.FileName = "{basedir}/logCbcAndCtr.txt";
            target.Layout = "${message}";
            var rule2 = new LoggingRule("*", LogLevel.Trace, target);
            config.LoggingRules.Add(rule2);
            LogManager.Configuration = config;
        }
    }


    internal class Worker
    {
        private static Aes _crypter;
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        public Worker()
        {
            InitAes();
        }

        internal void DecryptCbc(string hexKey, string cipherText)
        {
            var cipher = HexStringToByteArray(cipherText).ToArray();
            _crypter.Key = HexStringToByteArray(hexKey).ToArray();
            _crypter.IV = new byte[16];
            var decryptor = _crypter.CreateDecryptor();
            var messageArray = new byte[cipher.Length-16];
            for (int i = 0; i < cipher.Length/16-1; i++)
            {
                var IV = cipher.Skip(i * 16).Take(16).ToArray();
                decryptor.TransformBlock(cipher, (i + 1) * 16, 16, messageArray, i * 16);
                XorWithIV(IV, messageArray, i * 16, 16);
            }
            var paddingLength = messageArray[cipher.Length - 17];
            var resultstring = ByteArrayToString(messageArray, paddingLength);
            Console.WriteLine(resultstring);
            Console.WriteLine(paddingLength);
            _logger.Info(resultstring);
        }

        internal void DecryptCTR(string hexKey, string cipherText)
        {
            var cipher = HexStringToByteArray(cipherText).ToArray();
            _crypter.Key = HexStringToByteArray(hexKey).ToArray();
            _crypter.IV = new byte[16];
            var blockCount = cipher.Length/16;
            var IV = cipher.Take(16).ToArray();
            var encryptor = _crypter.CreateEncryptor();
            var expandedIV = new byte[blockCount*16];
            for (int i = 0; i < blockCount; i++)
            {
                encryptor.TransformBlock(IV, 0, 16, expandedIV, i*16);
                IncreaseIV(IV);
            }
            var messageArray = cipher.Skip(16).ToArray();
            XorWithIV(expandedIV.Take(cipher.Length - 16).ToArray(), messageArray, 0, cipher.Length - 16);
            var resultString = ByteArrayToString(messageArray, 0);
            Console.WriteLine(resultString);
            _logger.Info(resultString);
        }

        private void IncreaseIV(byte[] iv)
        {
            for (var i = iv.Length-1; i >= 0; i--)
            {
                var value = iv[i];
                if (value != byte.MaxValue)
                {
                    iv[i] = (byte)(value+1);
                    return;
                }
                iv[i] = 0;
            }
        }

        private static void InitAes()
        {
            _crypter = Aes.Create();
            _crypter.Mode = CipherMode.ECB;
            _crypter.Padding = PaddingMode.None;
        }

        public IEnumerable<byte> HexStringToByteArray(string str)
        {
            for (var i = 0; i < str.Length; i += 2)
            {
                var hexdec = str.Substring(i, 2);//string of one octet in hex
                yield return byte.Parse(hexdec, NumberStyles.HexNumber);
            }
        }

        public void XorWithIV(byte[] IV, byte[] target, int offset, int length)
        {
            if(IV.Length!=length)
                throw new ArgumentException("Неверная длина инициализирующего вектора");
            for (var i = 0; i < length; i++)
            {
                target[offset + i] = (byte)(IV[i] ^ target[offset + i]);
            }
        }

        public string ByteArrayToString(byte[] array, int paddingLength)
        {
            var sb = new StringBuilder();
            foreach (var c in array.Take(array.Length-paddingLength))
            {
                sb.Append((char)c);
            }
            return sb.ToString();
        }
    }
}
