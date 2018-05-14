using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVKTQS_UnboundedKnapsackProblem
{
    class Program
    {
        public static int N; // n items.
        public static int[] Weight;
        public static int[] Profits;
        public static Dictionary<int, string[]> CombinationBitOfItem;

        public static int C; //Capacity of Knapsack.

        public static int PopulationSize;
        public static List<string> Chromosomes;
        public static List<int> Fitness;

        static Random rnd = new Random();

        static void Main(string[] args)
        {
            //Input data.
            Input();
            PreProcess();

            // GAs
            InitPopulation();

            // Parent Selection.
            var parent = Parent();
            var parentA = parent.Item1;
            var parentB = parent.Item2;

            
        }

        static void Input()
        {
            N = 10;
            Weight = new int[] { 22, 50, 20, 10, 15, 16, 17, 21, 8, 25 };
            Profits = new int[] { 1078, 2350, 920, 440, 645, 656, 816, 903, 1125, 400 };
            C = 100;
            CombinationBitOfItem = new Dictionary<int, string[]>();
            PopulationSize = 20;
            Chromosomes = new List<string>();
            Fitness = new List<int>();
        }

        static void PreProcess()
        {
            for (int i = 0; i < N; i++)
            {
                var weight = Weight[i];
                var profit = Profits[i];
                var l = (C / weight) + 1;
                var binaries = new List<string>();
                var maxSizeOfGen = Utils.GetSizeOfGen(l);
                for (int j = 0; j < l; j++)
                {
                    string binary = Utils.ConvertBinaryToGen(j, maxSizeOfGen);
                    binaries.Add(binary);
                }
                CombinationBitOfItem.Add(i, binaries.ToArray());
            }
        }

        static void InitPopulation()
        {
            var i = 0;
            while (i < PopulationSize)
            {
                var strs = new List<string>();
                var chromosomesWeight = 0;
                var chromosomesFitness = 0;
                foreach (var item in CombinationBitOfItem)
                {
                    int r = rnd.Next(item.Value.Length);
                    var gen = item.Value[r];
                    // Calculate Total Weight of chromosome.
                    var quantity = Utils.ConvertStringToBinary(gen);

                    chromosomesWeight += quantity * Weight[item.Key];

                    // Calculate Fitness of chromosome.
                    chromosomesFitness += quantity * Profits[item.Key];

                    strs.Add(item.Value[r]);
                }
                if (chromosomesWeight > C) continue;

                Chromosomes.Add(string.Join(" ", strs));
                Fitness.Add(chromosomesFitness);

                i++;
            }
        }

        static Tuple<int, int> Parent()
        {
            var parentA = ParentSelection();
            var parentB = ParentSelection();
            if (parentA == parentB) return Parent();
            return new Tuple<int, int>(parentA, parentB);
        }
        static int ParentSelection()
        {
            //Tournament Selection
            var parentIndex = rnd.Next(0, PopulationSize);
            var bestFitness = Fitness[parentIndex];

            var round = rnd.Next(2, PopulationSize);
            for (int i = 1; i <= round; i++)
            {
                var index = rnd.Next(0, PopulationSize);
                if (index != parentIndex && bestFitness < Fitness[index])
                {
                    parentIndex = index;
                    bestFitness = Fitness[index];
                }
            }

            return parentIndex;
        }
    }
}
