using System;
using System.Collections.Generic;
using System.IO;

namespace GreedyAlgorithms
{
    class Program
    {
        private static void Main(string[] args)
        {
            //SchedulingAlgorithm.Run("c:\\temp\\homework2_1.txt", (w,l)=>w-l);
            var result = MST.Kruskal.Run(File.ReadLines("c:\\work\\largeEWG.txt"));
            Console.WriteLine("итого: {0}", result);
            Console.ReadLine();
        }
    }
}
