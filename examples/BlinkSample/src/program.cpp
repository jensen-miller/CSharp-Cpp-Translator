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