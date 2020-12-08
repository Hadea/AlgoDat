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
            rndGen = null;
            if (ArrayToSort.Length < 15) foreach (var item in ArrayToSort) Console.Write(" " + item);
            Console.WriteLine("\nNun das Sortierte :D");
            GC.Collect();
            GC.WaitForPendingFinalizers();
            DateTime startTime = DateTime.Now;
            //SelectionSort(ArrayToSort);
            //byte[] buffer = MergeSort(ArrayToSort);
            //MergeSortScratch(ArrayToSort);
            MapSort(ArrayToSort);
            DateTime endTime = DateTime.Now;
            if (ArrayToSort.Length < 15) foreach (var item in ArrayToSort) Console.Write(" " + item);
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
            // 72353,0138 bei 100m unboosted

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

            while (leftID < leftSide.Length)
                result[resultID++] = leftSide[leftID++];

            while (rightID < rightSide.Length)
                result[resultID++] = rightSide[rightID++];

            return result;
        }


        static void MergeSortScratch(byte[] ArrayToSort)
        {
            MergeSortScratchWorker(ArrayToSort, new byte[ArrayToSort.Length], 0, ArrayToSort.Length - 1);
        }
        static void MergeSortScratchWorker(byte[] ArrayToSort, byte[] ScratchArray, int FirstElement, int LastElement)
        {
            // 46552,515 bei 100M unboosted
            // 11568,4818 bei 100M mit GC, boost und release
            // ################# DIVIDE #####################
            if (FirstElement == LastElement) return;

            int splitPoint = (LastElement - FirstElement) / 2 + FirstElement;
            int rightPointer = splitPoint + 1;

            MergeSortScratchWorker(ArrayToSort, ScratchArray, FirstElement, splitPoint);
            MergeSortScratchWorker(ArrayToSort, ScratchArray, rightPointer, LastElement);

            // ################ CONQUER #####################

            int leftPointer = FirstElement;
            int scratchPointer = leftPointer;

            // zusammenführen

            while (leftPointer <= splitPoint && rightPointer <= LastElement)
            {
                if (ArrayToSort[rightPointer] < ArrayToSort[leftPointer])
                    ScratchArray[scratchPointer++] = ArrayToSort[rightPointer++];
                else
                    ScratchArray[scratchPointer++] = ArrayToSort[leftPointer++];
            }

            // reste links

            while (leftPointer <= splitPoint)
                    ScratchArray[scratchPointer++] = ArrayToSort[leftPointer++];

            // reste rechts
            while(rightPointer <= LastElement)
                    ScratchArray[scratchPointer++] = ArrayToSort[rightPointer++];

            // ins original kopieren

            for (int counter = FirstElement; counter <= LastElement; counter++)
                ArrayToSort[counter] = ScratchArray[counter];
        }

        static void MapSort(byte[] ArrayToSort) //keine ahnung ob der Name passt
        {
            //  693,9298 bei 100M debug boosted

            // array aller möglichen werte erstellen
            int[] elementOccurance = new int[Byte.MaxValue+1];

            // original durchgehen und vorkommen der werte zählen
            for (int counter = 0; counter < ArrayToSort.Length; counter++)
                elementOccurance[ArrayToSort[counter]]++;

            // originalarray anhand des Zählarrays wieder aufbauen
            int origPointer = 0;
            for (int occurance = 0; occurance < elementOccurance.Length; occurance++)
            {
                for (int counter = 0; counter < elementOccurance[occurance]; counter++)
                {
                    ArrayToSort[origPointer++] = (byte)occurance;
                }
            }
        }
    }
}
