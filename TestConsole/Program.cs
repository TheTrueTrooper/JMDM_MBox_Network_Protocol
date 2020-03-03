using JMDM_Network_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            JMDM_MBOXUDP Tester1 = new JMDM_MBOXUDP("255.255.255.255");
            Task SendTest = new Task(() =>
            {
                while (true)
                    Tester1.SetAxis(new uint[] { 0xc350, 0xc350, 0xc350, 0xc350, 0xc350, 0xc350 }, JMDM_Network_Protocol.Enums.Channels.Set6Axis);
            });
            Task ReciveTest = new Task(() =>
            {
                while (true)
                    Console.WriteLine($"Tester:{BitConverter.ToString(Tester1.Receive())}");
            });
            Console.ReadKey();
        }
    }
}
