using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpROSE.Loginserver
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.Title = "Experimental ROSE Online - Build: " + " 0.1 " + " [Memory Usage: " + GC.GetTotalMemory(false) / 1024 + "KB]";

            Core.Main.Boot();

            Console.ReadKey();
        }
    }
}
