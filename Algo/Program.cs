using System;

namespace Algo
{
    class Program
    {
        static void Main()
        {
            byte[] ArrayToSort = new byte[100000];
            Random rndGen = new Random();
            rndGen.NextBytes(ArrayToSort);
            if (ArrayToSort.Length < 15) foreach (var item in ArrayToSort) Console.Write(" " + item);
            Console.WriteLine("\nNun das Sortierte :D");
            DateTime startTime = DateTime.Now;
            SelectionSort(ArrayToSort);
            DateTime endTime = DateTime.Now;
            if (ArrayToSort.Length < 15) foreach (var item in ArrayToSort) Console.Write(" " + item);
            Console.WriteLine("\nDas Sortieren dauerte " + (endTime - startTime).TotalMilliseconds);
            Console.ReadLine();
        }

        static void SelectionSort(byte[] ArrayToSort)
        {
            // 23609,5496 bei 100k

            for (int outer = 0; outer < ArrayToSort.Length - 1; outer++)
            {
                for (int inner = outer + 1; inner < ArrayToSort.Length; inner++)
                {
                    if (ArrayToSort[outer] > ArrayToSort[inner])
                    {
                        byte buffer = ArrayToSort[inner];
                        ArrayToSort[inner] = ArrayToSort[outer];
                        ArrayToSort[outer] = buffer;
                    }
                }
            }
        }

    }
}
