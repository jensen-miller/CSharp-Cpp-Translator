#include <System\\Device\\Gpio.h>
using namespace System::Device::Gpio;
#include <System\\Threading.h>
using namespace System::Threading;

namespace BlinkSample
{
	class Program
	{
		const int ledPin = 17;

		const int lightTimeInMilliseconds = 1000;

		const int dimTimeInMillisecons = 200;

		static void Main()
		{
			GpioController controller = new GpioController();
			controller.OpenPin(ledPin, PinMode::Output);
			while (true)
			{
				controller.Write(ledPin, PinValue.High);
				Thread::Sleep(lightTimeInMilliseconds);
				controller.Write(ledPin, PinValue.Low);
				Thread::Sleep(dimTimeInMillisecons);
			}
		}

	};

}



int main()
{
	BlinkSample::Program::Main();
}