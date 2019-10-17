﻿using System.Device.Gpio;
using System.Threading;




namespace BlinkSample
{
    class Program
    {
        const int ledPin = 17;
        const int lightTimeInMilliseconds = 1000;
        const int dimTimeInMillisecons = 200;

        static void Main()
        {
            //  Get an instance of the GPIO controller
            GpioController controller = new GpioController();

            //  Open the pin for IO
            controller.OpenPin(ledPin, PinMode.Output);

            while(true)
            {
                controller.Write(ledPin, PinValue.High);
                Thread.Sleep(lightTimeInMilliseconds);
                controller.Write(ledPin, PinValue.Low);
                Thread.Sleep(dimTimeInMillisecons);
            }
        }
    }
}



//  References:
//
//  https://github.com/dotnet/iot/blob/master/samples/led-blink/README.md