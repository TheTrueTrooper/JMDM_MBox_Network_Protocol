using JMDM_Network_Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
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
                Tester1.Reset();
                Thread.Sleep(1000);
                Tester1.SetAxis(new uint[] { 0xc350, 0xc350, 0xc350, 0xc350, 0xc350, 0xc350 }, JMDM_Network_Protocol.Enums.Channels.Set6Axis);
                //Thread.Sleep(1000);
                Tester1.SetAxis(new uint[] { 0xF, 0xF, 0xF, 0xF, 0xF, 0xF }, JMDM_Network_Protocol.Enums.Channels.Set6Axis);
                //Thread.Sleep(1000);
                Tester1.SetAxis(new uint[] { 0xCFFF, 0xCFFF, 0xCFFF, 0xCFFF, 0xCFFF, 0xCFFF }, JMDM_Network_Protocol.Enums.Channels.Set6Axis);
                //Thread.Sleep(1000);
                Tester1.SetAxis(new uint[] { 0xc350, 0xc350, 0xc350, 0xc350, 0xc350, 0xc350 }, JMDM_Network_Protocol.Enums.Channels.Set6Axis);
                //Thread.Sleep(1000);
                Tester1.SetAxis(new uint[] { 0xF, 0xF, 0xF, 0xF, 0xF, 0xF }, JMDM_Network_Protocol.Enums.Channels.Set6Axis);
                //Thread.Sleep(1000);
                Tester1.SetAxis(new uint[] { 0xCFFF, 0xCFFF, 0xCFFF, 0xCFFF, 0xCFFF, 0xCFFF }, JMDM_Network_Protocol.Enums.Channels.Set6Axis);
            });
            Task ReciveTest = new Task(() =>
            {
                while (true)
                    Console.WriteLine($"Tester:{BitConverter.ToString(Tester1.Receive())}");
            });
            ReciveTest.Start();
            SendTest.Start();
            Console.ReadKey();
        }
    }
}
