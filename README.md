# CSharp-Cpp-Translator
A program for translating C# into C++ for portability (specifically running C# programs on devices that only support C++)

## Introduction
As a developer in the IoT sector, I am constantly switching languages; switching from managed to unmanaged day to day due to the nature
of cloud clients and bare-metal frameworks. While most frameworks, firmware, and RTOSes are implemented in C, the lexical and syntatical advantages
of c++ allows the developer to easily implement object-oriented designs, and tailor the API for ease-of-use. The templated nature of c++ also makes
the libraries more generic while not at the cost of type safety. With c++11 in mind, C# seemed more possible to run on MCUs that only support c++11
(as highest language). Thus I have written a translator that will translate/compile C# code (specific to dotnet-iot) into c++11 code, which can then
be optimized by GNU, Clang, or MSVC for any MCU. The supplementary [SDK](https://github.com/jensen-miller/dotnet-iot-sdk-cpp) will provide an interfacing PAL between the C# code and c++ code. The PAL will
have platform specific implementations in c++ for now.

As of right now, the target IDE is VS2019. This [setup tutorial](https://github.com/jensen-miller/CSharp-Cpp-Translator/blob/master/README.md) will explain how to use the translator in a basic dotnet project template.

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
