using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DynamicProgramming
{
    class Program
    {
        static void Main(string[] args)
        {
            var textreader = new StreamReader(typeof(Program).Assembly.GetManifestResourceStream("DynamicProgramming.knapsack_big.txt"));
            var result = new KnapSack().CalculateKnapSackProblem(textreader);
            Console.WriteLine(result);
            Console.ReadLine();
        }
    }
}
