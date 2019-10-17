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

```

#### output

``` cpp
#include <System\\Device\\Gpio.h>
using namespace System::Device::Gpio;
#include <System\\Threading.h>
using namespace System::Threading;

namespace BlinkSample
{
	class Program
	{
		public:
		static void Main()
		{
			const int LedPin = 17;
			const int LightTimeInMilliseconds = 1000;
			const int DimTimeInMilliseconds = 200;
			GpioController controller = new GpioController();
			controller.OpenPin(LedPin, PinMode::Output);
			while (true)
			{
				controller.Write(LedPin, PinValue::High);
				Thread::Sleep(LightTimeInMilliseconds);
				controller.Write(LedPin, PinValue::Low);
				Thread::Sleep(DimTimeInMilliseconds);
			}
		}

	};

}


int main()
{
	BlinkSample::Program::Main();
}

```


## To-Do

- [ ] Collect code heuristics.
- [ ] Output is Compiling.
- [x] Format output for better reading.
- [ ] Finish all visitors.
- [x] Command line parsing of arguments.
- [ ] Advanced command line arguments.
