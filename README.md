# CSharp-Cpp-Translator
A program for translating C# into C++ for portability (specifically running C# programs on devices that only support C++)

##  Example

``` cs
using System.Device.Gpio;
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

int main()
{
	BlinkSample::Program::Main();
}

```

=======
# To-Do

- [ ] Collect code heuristics.
- [ ] Output is Compiling.
- [x] Format output for better reading.
- [ ] Finish all visitors.
- [x] Command line parsing of arguments.
- [ ] Advanced command line arguments.
