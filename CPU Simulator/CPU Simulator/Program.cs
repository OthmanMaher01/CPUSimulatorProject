using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CPU_Simulator
{
    class Program
    {
        static void Main(string[] args)
        {
            String input;

            Console.WriteLine("Number of processors : ");
            input = Console.ReadLine();
            int numOfP = Convert.ToInt32(input);

            Console.WriteLine("Number of clock cycles : ");
            input = Console.ReadLine();
            int numOfC = Convert.ToInt32(input);

            Console.WriteLine("The path for the tasks file : ");
            input = Console.ReadLine();
            string path = input;

            Simulator simulator = new Simulator(numOfP, numOfC, path);
            simulator.start();
            Console.ReadKey();
        }
    }
}
