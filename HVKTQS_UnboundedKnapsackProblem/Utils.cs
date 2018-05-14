using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HVKTQS_UnboundedKnapsackProblem
{
    public static class Utils
    {
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
