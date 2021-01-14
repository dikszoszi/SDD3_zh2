using System;
using System.Collections.Generic;

namespace HandballTeams
{
    public static class MyExtensions
    {
        public static void PrintToConsole<T>(this IEnumerable<T> input, string str = "")
        {
            if (input is null)
            {
                Console.WriteLine("Nothing to print!");
                return;
            }

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("\n\tBEGIN: " + str);
            Console.ResetColor();

            foreach (T item in input) Console.WriteLine(item.ToString());

            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine('\t' + str + " END.\t(Press a key)");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
