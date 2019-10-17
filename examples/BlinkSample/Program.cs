using System.Device.Gpio;
using System.Threading;




namespace BlinkSample
{
    class Program
    {                
        public static void Main()
        {
            const int LedPin = 17;
            const int LightTimeInMilliseconds = 1000;
            const int DimTimeInMilliseconds = 200;

            //  Get an instance of the GPIO controller
            GpioController controller = new GpioController();

            //  Open the pin for IO
            controller.OpenPin(LedPin, PinMode.Output);

            while(true)
            {
                controller.Write(LedPin, PinValue.High);
                Thread.Sleep(LightTimeInMilliseconds);
                controller.Write(LedPin, PinValue.Low);
                Thread.Sleep(DimTimeInMilliseconds);
            }
        }
    }
}



//  References:
//
//  https://github.com/dotnet/iot/blob/master/samples/led-blink/README.md