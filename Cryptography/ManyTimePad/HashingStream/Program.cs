using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HashingStream
{
    class Program
    {
        static void Main(string[] args)
        {
            var firstBlock = new byte[1024];
            var outFirstBlock = new byte[1024];
            using (var stream = File.Open("C:\\Users\\User\\Downloads\\6 - 2 - Generic birthday attack (16 min).mp4", FileMode.Open))
            {
                stream.Read(firstBlock, 0, 1024);
            }
            var hasher = SHA256.Create();
            hasher.TransformFinalBlock(firstBlock, 0, 1024);
            Console.WriteLine(BitConverter.ToString(hasher.Hash));
            Console.ReadLine();
        }
    }
}
