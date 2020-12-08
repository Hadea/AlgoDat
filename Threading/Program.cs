using System;
using System.Threading.Tasks;

namespace Threading
{
    class Program
    {
        static void Main()
        {
            Parallel.For(0, 30, (counter) => Console.Write(" {0,3}",counter));
            Console.WriteLine();

            Parallel.Invoke(
                () => PrintSomething("1sdhb"),
                () => PrintSomething("2ege"),
                () => PrintSomething("3edwe"),
                () => PrintSomething("4sadsad"),
                () => PrintSomething("5grg"),
                () => PrintSomething("6ddwefrg"));

            Console.ReadLine();
        }

        static void PrintSomething(string Content)
        {
            Console.WriteLine(Content);
        }
    }
}
