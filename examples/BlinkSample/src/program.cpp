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
			const int LedPin = 13;
			const int LightTimeInMilliseconds = 1000;
			const int DimTimeInMilliseconds = 300;
			 GpioController &controller = *new GpioController();
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



void setup() {
	BlinkSample::Program::Main();
}

void loop() {
}