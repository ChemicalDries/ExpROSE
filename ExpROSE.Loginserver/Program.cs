using System;

namespace ExpROSE.Loginserver
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.Title = "Experimental ROSE Online Emulator - Build: " + " 0.1 " + " [Memory Usage: " + GC.GetTotalMemory(false) / 1024 + "KB]";

            Core.Main.Boot();

            Console.ReadKey();
        }
    }
}