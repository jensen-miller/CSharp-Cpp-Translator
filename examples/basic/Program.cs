using MicroSystem;
using MicroSystem.Controllers;
using MicroSystem.Communications;


namespace SampleProject
{
    class Program
    {
        static void Main()
        {
            SerialConnection serialConnection = SerialController.GetDefault().OpenConnection(115200);
            serialConnection.Write("Hello, World!", 13);
        }
    }
}
