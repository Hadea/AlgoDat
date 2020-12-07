using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;

namespace Wiederholung
{

    interface ICharacter
    {
        public void Die();
    }


    class Auto
    {
        public const int Doors = 5;

        public static int NumberOfCars;
        public Auto()
        {
            NumberOfCars++;
        }

        public void Draw()
        {
            Console.WriteLine("Hallo ich bin ein Auto");
        }

        public virtual void OpenDoor()
        {
            Console.WriteLine("Tür geht nach vorn auf");
        }

        internal void PrintNumber()
        {
            Console.WriteLine(NumberOfCars);
        }
    }

    sealed class Delorean : Auto
    {
        public Action DoIt;

        public sealed override void OpenDoor()
        {
            Console.WriteLine("Tür geht nach oben auf");
        }

        public void Addition(in int NumberA, in int NumberB, out int result)
        {
            result = NumberA + NumberB;
            Console.WriteLine(result);
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

    }


    interface IDrawable
    {
        public void Draw();
    }


    class PathfindingException : Exception
    {
        public int OpenFields;
        public int ClosedFields;

        public override string ToString()
        {
            return "Irgendwas ist schiefgegangen:" + OpenFields + " " + ClosedFields + " " + StackTrace;
        }
    }


    enum SomeStates
    {
        Alpha,
        Bravo,
        Charlie
    }

    [StructLayout(LayoutKind.Explicit)]
    struct UnionDemo
    {
        [FieldOffset(0)] public decimal b;
        [FieldOffset(0)] public Content c;
    }

    [StructLayout(LayoutKind.Sequential)]
    struct Content
    {
        public int NumberA;
        public short NumberB;
        public short NumberC;
    }



    class Program
    {
        [DllImport("Kernel32.dll")]
        static extern SafeFileHandle CreateFile(
            string filename,
            [MarshalAs(UnmanagedType.U4)] uint fileAccess);

        static void Main()
        {
            Console.WriteLine(Auto.NumberOfCars);

            List<Auto> stau = new();
            stau.Add(new Auto());
            Console.WriteLine(Auto.NumberOfCars);
            stau.Add(new Auto());
            stau.Add(new Delorean());
            Console.WriteLine(Auto.NumberOfCars);
            stau.Add(new Auto());
            stau.Add(new Delorean());
            Console.WriteLine(Auto.NumberOfCars);


            UnionDemo union;


            foreach (var item in stau)
            {
                item.OpenDoor();
                item.PrintNumber();
            }

            Console.ReadLine();

        }
    }
}
