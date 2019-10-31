using System.IO.Ports;
using System.Threading;

//  Default arduino 'Serial' -> 'COM1'
//  Default arduino baudrate -> 9600

namespace SerialSample
{
    class Program
    {
        public static void Main()
        {
            const int defaultBaudRate = 9600;
            const string defaultPort = "COM1";

            SerialPort sp = new SerialPort(defaultPort, defaultBaudRate);         
            
            sp.Open();

            while (true)
            {
                sp.WriteLine("Hello World!");
                Thread.Sleep(3000);
            }
        }
    }
}