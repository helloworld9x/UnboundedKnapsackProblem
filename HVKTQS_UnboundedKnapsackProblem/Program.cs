﻿using System;
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

        public static int Generation;
        static Random rnd = new Random();

        static void Main(string[] args)
        {
            //Input data.
            Input();
            PreProcess();

            // GAs
            InitPopulation();

            for (int i = 0; i < Generation; i++)
            {
                // Sort By Fitness.
                SortChromosomesByFitness();

                // Take 70% Chromosomes
                var Take70Percent = PopulationSize * 70 / 100;
                Chromosomes = Chromosomes.Take(Take70Percent).ToList();
                Fitness = Fitness.Take(Take70Percent).ToList();

                while (Chromosomes.Count <= PopulationSize)
                {
                    // Parent Selection.
                    var parent = Parent();
                    var parentAIndex = parent.Item1;
                    var parentBIndex = parent.Item2;

                    //Single-point
                    //var division = rnd.Next(2, N);
                    var division = 9;
                    var childA = Utils.CuttingGen(Chromosomes[parentAIndex], division)
                        + " " + Utils.CuttingGen(Chromosomes[parentBIndex], N, division);

                    var childB = Utils.CuttingGen(Chromosomes[parentBIndex], division)
                       + " " + Utils.CuttingGen(Chromosomes[parentAIndex], N, division);

                    var totalWeightA = Utils.CalculateWeight(childA, Weight);
                    if (totalWeightA > 0 && totalWeightA <= C)
                    {
                        Chromosomes.Add(childA);
                        Fitness.Add(Utils.CalculateFitness(childA, Profits));
                    }

                    var totalWeightB = Utils.CalculateWeight(childB, Weight);
                    if (totalWeightB > 0 && totalWeightB <= C)
                    {
                        Chromosomes.Add(childB);
                        Fitness.Add(Utils.CalculateFitness(childB, Profits));
                    }
                }
            }



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
            Generation = 100;
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
            var size = Chromosomes.Count();
            //Tournament Selection
            var parentIndex = rnd.Next(0, size);
            var bestFitness = Fitness[parentIndex];

            var round = rnd.Next(2, size);
            for (int i = 1; i <= round; i++)
            {
                var index = rnd.Next(0, size);
                if (index != parentIndex && bestFitness < Fitness[index])
                {
                    parentIndex = index;
                    bestFitness = Fitness[index];
                }
            }

            return parentIndex;
        }

        static void SortChromosomesByFitness()
        {
            // One by one move boundary of unsorted subarray
            var n = Chromosomes.Count;

            for (int i = 0; i < n - 1; i++)
            {
                // Find the minimum element in unsorted array
                int min_idx = i;
                for (int j = i + 1; j < n; j++)
                    if (Fitness[j] > Fitness[min_idx])
                        min_idx = j;

                // Swap the found minimum element with the first
                // element
                string temp = Chromosomes[min_idx];
                Chromosomes[min_idx] = Chromosomes[i];
                Chromosomes[i] = temp;

                int indexTemp = Fitness[min_idx];
                Fitness[min_idx] = Fitness[i];
                Fitness[i] = indexTemp;
            }
        }
    }
}
