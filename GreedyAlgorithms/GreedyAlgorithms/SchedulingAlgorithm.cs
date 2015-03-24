using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace GreedyAlgorithms
{
    public class SchedulingAlgorithm
    {
        public static decimal Run(string filename, Func<decimal, int, decimal> calculateInvariant)
        {
            var jobs = LoadJobs(filename, calculateInvariant);
            var schedule = ScheduleJobs(jobs);
            return CalculateSumOfWeightedCompletionTimes(schedule);
        }

        private static decimal CalculateSumOfWeightedCompletionTimes(Heap<Job, Job> schedule)
        {
            var sum = (decimal)0;
            var completiontime = 0;
            while (schedule.Count > 0)
            {
                var job = schedule.ExtractMinValue().Key;
                Console.WriteLine("weight={0}, length={1}", job.Weight, job.Length);
                completiontime += job.Length;
                sum += completiontime*job.Weight;
                Console.WriteLine("intermediate sum = {0}", sum);
            }
            Console.WriteLine("Итого: {0}", sum);
            return sum;
        }

        private static void PrintSchedule(Heap<Job, Job> schedule)
        {
            while (schedule.Count>0)
            {
                var job = schedule.ExtractMinValue().Key;
                Console.WriteLine("weight={0}, length={1}", job.Weight, job.Length);
            }
        }

        private static Heap<Job,Job> ScheduleJobs(IEnumerable<Job> jobs)
        {
            var heap = new Heap<Job, Job>();
            foreach (var job in jobs)
            {
                heap.Insert(new KeyValuePair<Job, Job>(job, job));
            }
            return heap;
        }

        private static IEnumerable<Job> LoadJobs(string filename, Func<decimal, int, decimal> calculateInvariant)
        {
            var result = File.ReadAllLines(filename).Skip(1).Select(c =>
            {
                var values = c.Split(' ', '\t');
                Debug.Assert(values.Length == 2);
                return new Job(length: int.Parse(values[1]), weight: int.Parse(values[0]), calculateInvariant: calculateInvariant);
            });
            return result;
        }
    }
    internal struct Job : IEquatable<Job>, IComparable<Job>
    {
        public Job(int weight, int length, Func<decimal,int,decimal> calculateInvariant)
            : this()
        {
            _weight = weight;
            _length = length;
            _invariant = calculateInvariant(_weight,_length);
        }

        public decimal Weight
        {
            get { return _weight; }
        }

        public int Length
        {
            get { return _length; }
        }

        private readonly decimal _invariant;
        private readonly int _weight;
        private readonly int _length;

        public bool Equals(Job other)
        {
            return Weight == other.Weight && Length == other.Length;
        }

        public int CompareTo(Job other)
        {
            if (_invariant == other._invariant)
                return -Weight.CompareTo(other.Weight);
            if (_invariant > other._invariant)
                return -1;
            return 1;
        }
    }
}