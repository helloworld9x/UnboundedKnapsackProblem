using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVKTQS_UnboundedKnapsackProblem
{
    public static class Utils
    {
        public static int CalculateWeight(string chromosome, int[] weights)
        {
            var gens = chromosome.Split(' ');
            var totalWeight = 0;
            for (int i = 0; i < gens.Length; i++)
            {
                totalWeight += ConvertStringToBinary(gens[i]) * weights[i];
            }

            return totalWeight;
        }

        public static int CalculateFitness(string chromosome, int[] profits)
        {
            var gens = chromosome.Split(' ');
            var totalFitness = 0;
            for (int i = 0; i < gens.Length; i++)
            {
                totalFitness += ConvertStringToBinary(gens[i]) * profits[i];
            }

            return totalFitness;
        }

        public static string CuttingGen(string chromosome, int end, int start = 0)
        {
            var gens = chromosome.Split(' ');
            var result = new List<string>();
            for (int i = 0; i < gens.Length; i++)
            {
                if (start == 0)
                {
                    if (i <= end)
                    {
                        result.Add(gens[i]);
                    }
                }
                else
                {
                    if (i > start && i <= end)
                    {
                        result.Add(gens[i]);
                    }
                }
            }
            if (!result.Any()) return string.Empty;

            return string.Join(" ", result);
        }

        public static int ConvertStringToBinary(string binary)
        {
            int result = 0;
            int k = 0;
            for (int i = binary.Length - 1; i >= 0; i--)
            {
                byte bit = 0;
                if (binary[i] == '1') bit = 1;
                result += (int)Math.Pow(2, k) * bit;
                k++;
            }
            return result;
        }

        public static int GetSizeOfGen(int maxBit)
        {
            string binary = Convert.ToString(maxBit, 2);
            return binary.Length;
        }

        public static string ConvertBinaryToGen(int bit, int sizeOfGen)
        {
            string binary = Convert.ToString(bit, 2);
            var result = binary.PadLeft(sizeOfGen, '0');
            return result;
        }
    }
}
