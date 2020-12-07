using System;
using System.Linq;

namespace Algo
{
    class Program
    {
        static void Main()
        {
            byte[] ArrayToSort = new byte[100000000];
            Random rndGen = new Random();
            rndGen.NextBytes(ArrayToSort);
            if (ArrayToSort.Length < 15) foreach (var item in ArrayToSort) Console.Write(" " + item);
            Console.WriteLine("\nNun das Sortierte :D");
            DateTime startTime = DateTime.Now;
            //SelectionSort(ArrayToSort);
            byte[] buffer = MergeSort(ArrayToSort);
            DateTime endTime = DateTime.Now;
            if (ArrayToSort.Length < 15) foreach (var item in buffer) Console.Write(" " + item);
            Console.WriteLine("\nDas Sortieren dauerte " + (endTime - startTime).TotalMilliseconds);
            Console.ReadLine();
        }

        static void SelectionSort(byte[] ArrayToSort)
        {
            // 23609,5496 bei 100k
            // 23216,0438 bei 100k nach optimierung
            for (int outer = 0; outer < ArrayToSort.Length - 1; outer++)
            {
                int smallestID = outer;
                for (int inner = outer + 1; inner < ArrayToSort.Length; inner++)
                {
                    if (ArrayToSort[smallestID] > ArrayToSort[inner])
                    {
                        smallestID = inner;
                    }
                }

                if (smallestID != outer)
                {
                    byte buffer = ArrayToSort[smallestID];
                    ArrayToSort[smallestID] = ArrayToSort[outer];
                    ArrayToSort[outer] = buffer;
                }
            }
        }

        static byte[] MergeSort(byte[] ArrayToSort)
        {
            // 134,8332 bei 100k


            // ################# DIVIDE #################
            if (ArrayToSort.Length <= 1) return ArrayToSort;

            byte[] leftSide = MergeSort(ArrayToSort.Take(ArrayToSort.Length / 2).ToArray());
            byte[] rightSide = MergeSort(ArrayToSort.Skip(ArrayToSort.Length / 2).ToArray());

            // ################# CONQUER ################

            int leftID = 0;
            int rightID = 0;
            int resultID = 0;

            byte[] result = new byte[ArrayToSort.Length];

            while (leftID < leftSide.Length && rightID < rightSide.Length)
                if (rightSide[rightID] < leftSide[leftID])
                    result[resultID++] = rightSide[rightID++];
                else
                    result[resultID++] = leftSide[leftID++];

            for (; leftID < leftSide.Length; leftID++)
                result[resultID++] = leftSide[leftID];

            while (rightID < rightSide.Length)
                result[resultID++] = rightSide[rightID++];

            return result;

        }
    }
}
