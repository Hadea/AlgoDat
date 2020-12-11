using System;
using System.Linq;
using System.Threading.Tasks;

namespace Algo
{
    class Program
    {
        static void Main()
        {
            byte[] ArrayToSort = new byte[2000];
            Random rndGen = new Random();
            TimeSpan fullElapsedTime = TimeSpan.Zero;
            rndGen.NextBytes(ArrayToSort);
            //SelectionSort(ArrayToSort);
            //MergeSortScratch(ArrayToSort);
            //MergeSortThreaded(ArrayToSort);
            MergeSortV2threaded(ArrayToSort);
            for (int repeat = 0; repeat < 1000; repeat++)
            {
                rndGen.NextBytes(ArrayToSort);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                DateTime startTime = DateTime.Now;
                //SelectionSort(ArrayToSort);
                //byte[] buffer = MergeSort(ArrayToSort);
                //MergeSortScratch(ArrayToSort);
                //MapSort(ArrayToSort);
                //MergeSortThreaded(ArrayToSort);
                MergeSortV2threaded(ArrayToSort);
                DateTime endTime = DateTime.Now;
                fullElapsedTime += (endTime - startTime);
                for (int counter = 0; counter < ArrayToSort.Length - 1; counter++)
                {
                    if (ArrayToSort[counter] > ArrayToSort[counter + 1])
                    {
                        Console.WriteLine("error");
                        Console.ReadLine();
                        return;
                    }
                }
            }
            Console.WriteLine("\nDas Sortieren von {1} Elementen dauerte {0}", fullElapsedTime.TotalMilliseconds / 1000, ArrayToSort.Length);
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
            MergeSortScratchWorker(ArrayToSort, new byte[ArrayToSort.Length], 0, ArrayToSort.Length - 1, ArrayToSort.Length / 2);
        }
        static void MergeSortScratchWorker(byte[] ArrayToSort, byte[] ScratchArray, int FirstElement, int LastElement, int splitPoint)
        {
            // 46552,515 bei 100M unboosted
            // 11568,4818 bei 100M mit GC, boost und release
            // ################# DIVIDE #####################
            if (FirstElement == LastElement) return;
            int rightPointer = splitPoint + 1;

            MergeSortScratchWorker(ArrayToSort, ScratchArray, FirstElement, splitPoint, (splitPoint - FirstElement) / 2 + FirstElement);
            MergeSortScratchWorker(ArrayToSort, ScratchArray, rightPointer, LastElement, (LastElement - rightPointer) / 2 + rightPointer);

            MergeSortScratchWorkerMerge(ArrayToSort, ScratchArray, FirstElement, LastElement, splitPoint);

        }
        static void MergeSortScratchWorkerMerge(byte[] ArrayToSort, byte[] ScratchArray, int FirstElement, int LastElement, int splitPoint)
        {
            // ################ CONQUER #####################

            int leftPointer = FirstElement;
            int scratchPointer = leftPointer;
            int rightPointer = splitPoint + 1;
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
            while (rightPointer <= LastElement)
                ScratchArray[scratchPointer++] = ArrayToSort[rightPointer++];

            // ins original kopieren

            for (int counter = FirstElement; counter <= LastElement; counter++)
                ArrayToSort[counter] = ScratchArray[counter];
        }

        static void MapSort(byte[] ArrayToSort) //keine ahnung ob der Name passt
        {
            //  693,9298 bei 100M debug boosted

            // array aller möglichen werte erstellen
            int[] elementOccurance = new int[Byte.MaxValue + 1];

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

        static void MergeSortThreaded(byte[] ArrayToSort)
        {
            throw new NotImplementedException("Unsafe with array length below 1500, do not use! ");
            // 30494,7437 bei 100M debug, boost, singlethread
            // 16947,968 bei 100M debug, boost, dualthreaded
            // 10099,9445 bei 100M debug, boost, quadthreaded
            //  9441,0841 bei 100M debug, boost, quadthreaded/dualmerge

            //aufteilen auf power of two arrays, maximal aber anzahl der Kerne des Prozessors

            // normalen merge sort auf 4 unterschiedlichen bereichen starten, jeweils in einem thread
            byte[] scratch = new byte[ArrayToSort.Length];

            Parallel.Invoke(
            () => MergeSortScratchWorker(ArrayToSort, scratch, 0, ArrayToSort.Length / 8 * 2, ArrayToSort.Length / 8),
            () => MergeSortScratchWorker(ArrayToSort, scratch, ArrayToSort.Length / 8 * 2 + 1, ArrayToSort.Length / 8 * 4, GetSplitPoint(ArrayToSort.Length / 8 * 2 + 1, ArrayToSort.Length / 8 * 4)),
            () => MergeSortScratchWorker(ArrayToSort, scratch, ArrayToSort.Length / 8 * 4 + 1, ArrayToSort.Length / 8 * 6, GetSplitPoint(ArrayToSort.Length / 8 * 4 + 1, ArrayToSort.Length / 8 * 6)),
            () => MergeSortScratchWorker(ArrayToSort, scratch, ArrayToSort.Length / 8 * 6 + 1, ArrayToSort.Length - 1, GetSplitPoint(ArrayToSort.Length / 8 * 6 + 1, ArrayToSort.Length - 1))
            );
            // die 4 sortierten teilarrays zusammenführen

            Parallel.Invoke(
            () => MergeSortScratchWorkerMerge(ArrayToSort, scratch, 0, ArrayToSort.Length / 8 * 4, GetSplitPoint(0, ArrayToSort.Length / 8 * 4)),
            () => MergeSortScratchWorkerMerge(ArrayToSort, scratch, ArrayToSort.Length / 8 * 4 + 1, ArrayToSort.Length - 1, GetSplitPoint(ArrayToSort.Length / 8 * 4 + 1, ArrayToSort.Length - 1))
            );
            MergeSortScratchWorkerMerge(ArrayToSort, scratch, 0, ArrayToSort.Length - 1, ArrayToSort.Length / 8 * 4);

            int GetSplitPoint(int first, int last)
            {
                return (last - first) / 2 + first;
            }
        }

        static void Sort(byte[] ArrayToSort)
        {
            if (ArrayToSort.Length < 100)
            {
                SelectionSort(ArrayToSort);
            }
            else if (ArrayToSort.Length < 2000)
            {
                MergeSortScratch(ArrayToSort);
            }
            else
            {
                MergeSortV2threaded(ArrayToSort);
            }
        }


        static void MergeSortV2threaded(byte[] ArrayToSort)
        {
            byte[] scratchArray = new byte[ArrayToSort.Length];
            int segmentGroesse = ArrayToSort.Length / 4;
            Parallel.Invoke(
                () => MergeSortV2Split(ArrayToSort, scratchArray, segmentGroesse * 0, segmentGroesse * 1 - 1),
                () => MergeSortV2Split(ArrayToSort, scratchArray, segmentGroesse * 1, segmentGroesse * 2 - 1),
                () => MergeSortV2Split(ArrayToSort, scratchArray, segmentGroesse * 2, segmentGroesse * 3 - 1),
                () => MergeSortV2Split(ArrayToSort, scratchArray, segmentGroesse * 3, ArrayToSort.Length - 1));

            Parallel.Invoke(
                () => MergeSortV2Merge(ArrayToSort, scratchArray, segmentGroesse * 0, segmentGroesse * 1 - 1, segmentGroesse * 1, segmentGroesse * 2 - 1),
                () => MergeSortV2Merge(ArrayToSort, scratchArray, segmentGroesse * 2, segmentGroesse * 3 - 1, segmentGroesse * 3, ArrayToSort.Length - 1));

            MergeSortV2Merge(ArrayToSort, scratchArray, segmentGroesse * 0, segmentGroesse * 2 - 1, segmentGroesse * 2, ArrayToSort.Length - 1);

        }

        static void MergeSortV2Split(byte[] ArrayToSort, byte[] ScratchArray, int pBeginInclusive, int pEndInvlusive)
        {
            if (pEndInvlusive - pBeginInclusive > 0)
            {
                int splitPoint = (pEndInvlusive + 1 - pBeginInclusive) / 2 + pBeginInclusive;
                MergeSortV2Split(ArrayToSort, ScratchArray, pBeginInclusive, splitPoint - 1);
                MergeSortV2Split(ArrayToSort, ScratchArray, splitPoint, pEndInvlusive);
                MergeSortV2Merge(ArrayToSort, ScratchArray, pBeginInclusive, splitPoint - 1, splitPoint, pEndInvlusive);
            }
        }


        static void MergeSortV2Merge(byte[] ArrayToSort, byte[] ScratchArray, int pLinksAnfang, int pLinksEnde, int pRechtsAnfang, int pRechtsEnde) // Conquer
        {

            int posLeft = pLinksAnfang;
            int posRight = pRechtsAnfang;
            int scratchPointer = pLinksAnfang;

            while (posLeft <= pLinksEnde && posRight <= pRechtsEnde)
            {
                if (ArrayToSort[posLeft] < ArrayToSort[posRight])
                {
                    ScratchArray[scratchPointer] = ArrayToSort[posLeft];
                    ++posLeft;
                    ++scratchPointer;
                }
                else
                {
                    ScratchArray[scratchPointer] = ArrayToSort[posRight];
                    ++posRight;
                    ++scratchPointer;
                }
            }

            while (posLeft <= pLinksEnde)
            {
                ScratchArray[scratchPointer] = ArrayToSort[posLeft];
                ++scratchPointer;
                ++posLeft;
            }

            while (posRight <= pRechtsEnde)
            {
                ScratchArray[scratchPointer] = ArrayToSort[posRight];
                ++scratchPointer;
                ++posRight;
            }

            Array.Copy(ScratchArray, pLinksAnfang, ArrayToSort, pLinksAnfang, pRechtsEnde - pLinksAnfang + 1);
        }


    }
}
