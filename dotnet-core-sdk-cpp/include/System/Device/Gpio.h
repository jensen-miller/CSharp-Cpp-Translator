#ifndef DOTNET_SYSTEM_DEVICE_GPIO_H__
#define DOTNET_SYSTEM_DEVICE_GPIO_H__


namespace System
{
	namespace Device
	{
		namespace Gpio
		{
			//
			// Summary:
			//     Different numbering schemes supported by GPIO controllers and drivers.
			enum class PinNumberingScheme
			{
				//
				// Summary:
				//     The logical representation of the GPIOs. Refer to the microcontroller's datasheet
				//     to find this information.
				Logical = 0,
				//
				// Summary:
				//     The physical pin numbering that is usually accessible by the board headers.
				Board = 1
			};

			enum class PinValue
			{
				Low,
				High
			};

			enum class PinMode
			{
				Input,
				Output
			};


			class GpioController
			{
			public:
				//
				// Summary:
				//     Initializes a new instance of the System.Device.Gpio.GpioController class that
				//     will use the logical pin numbering scheme as default.
				GpioController() {}
				//
				// Summary:
				//     Initializes a new instance of the System.Device.Gpio.GpioController class that
				//     will use the specified numbering scheme. The controller will default to use the
				//     driver that best applies given the platform the program is executing on.
				//
				// Parameters:
				//   numberingScheme:
				//     The numbering scheme used to represent pins provided by the controller.
				GpioController(PinNumberingScheme numberingScheme);
				//
				// Summary:
				//     Initializes a new instance of the System.Device.Gpio.GpioController class that
				//     will use the specified numbering scheme and driver.
				//
				// Parameters:
				//   numberingScheme:
				//     The numbering scheme used to represent pins provided by the controller.
				//
				//   driver:
				//     The driver that manages all of the pin operations for the controller.
				//GpioController(PinNumberingScheme numberingScheme, GpioDriver driver);

				//
				// Summary:
				//     The numbering scheme used to represent pins provided by the controller.
				//PinNumberingScheme NumberingScheme{ get; }
					//
					// Summary:
					//     The number of pins provided by the controller.
				//int PinCount{ get; }

					//
					// Summary:
					//     Closes an open pin.
					//
					// Parameters:
					//   pinNumber:
					//     The pin number in the controller's numbering scheme.
				void ClosePin(int pinNumber);
				//
				void Dispose();
				//
				// Summary:
				//     Gets the mode of a pin.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				// Returns:
				//     The mode of the pin.
				PinMode GetPinMode(int pinNumber);
				//
				// Summary:
				//     Checks if a pin supports a specific mode.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				//   mode:
				//     The mode to check.
				//
				// Returns:
				//     The status if the pin supports the mode.
				bool IsPinModeSupported(int pinNumber, PinMode mode);
				//
				// Summary:
				//     Checks if a specific pin is open.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				// Returns:
				//     The status if the pin is open or closed.
				bool IsPinOpen(int pinNumber);
				//
				// Summary:
				//     Opens a pin in order for it to be ready to use.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				inline void OpenPin(int pinNumber)
				{
				
				}
				//
				// Summary:
				//     Opens a pin and sets it to a specific mode.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				//   mode:
				//     The mode to be set.
				inline void OpenPin(int pinNumber, PinMode mode)
				{
					
				}
				//
				// Summary:
				//     Read the given pins with the given pin numbers.
				//
				// Parameters:
				//   pinValuePairs:
				//     The pin/value pairs to read.
				//void Read(Span<PinValuePair> pinValuePairs);
				//
				// Summary:
				//     Reads the current value of a pin.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				// Returns:
				//     The value of the pin.
				PinValue Read(int pinNumber);
				//
				// Summary:
				//     Adds a callback that will be invoked when pinNumber has an event of type eventType.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				//   eventTypes:
				//     The event types to wait for.
				//
				//   callback:
				//     The callback method that will be invoked.
				//void RegisterCallbackForPinValueChangedEvent(int pinNumber, PinEventTypes eventTypes, PinChangeEventHandler callback);
				//
				// Summary:
				//     Sets the mode to a pin.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				//   mode:
				//     The mode to be set.
				void SetPinMode(int pinNumber, PinMode mode);
				//
				// Summary:
				//     Removes a callback that was being invoked for pin at pinNumber.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				//   callback:
				//     The callback method that will be invoked.
				//void UnregisterCallbackForPinValueChangedEvent(int pinNumber, PinChangeEventHandler callback);
				//
				// Summary:
				//     Blocks execution until an event of type eventType is received or a cancellation
				//     is requested.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				//   eventTypes:
				//     The event types to wait for.
				//
				//   cancellationToken:
				//     The cancellation token of when the operation should stop waiting for an event.
				//
				// Returns:
				//     A structure that contains the result of the waiting operation.
				//WaitForEventResult WaitForEvent(int pinNumber, PinEventTypes eventTypes, CancellationToken cancellationToken);
				//
				// Summary:
				//     Blocks execution until an event of type eventType is received or a period of
				//     time has expired.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				//   eventTypes:
				//     The event types to wait for.
				//
				//   timeout:
				//     The time to wait for the event.
				//
				// Returns:
				//     A structure that contains the result of the waiting operation.
				//WaitForEventResult WaitForEvent(int pinNumber, PinEventTypes eventTypes, TimeSpan timeout);
				//
				// Summary:
				//     Async call until an event of type eventType is received or a cancellation is
				//     requested.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				//   eventTypes:
				//     The event types to wait for.
				//
				//   token:
				//     The cancellation token of when the operation should stop waiting for an event.
				//
				// Returns:
				//     A task representing the operation of getting the structure that contains the
				//     result of the waiting operation
				//ValueTask<WaitForEventResult> WaitForEventAsync(int pinNumber, PinEventTypes eventTypes, CancellationToken token);
				//
				// Summary:
				//     Async call to wait until an event of type eventType is received or a period of
				//     time has expired.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				//   eventTypes:
				//     The event types to wait for.
				//
				//   timeout:
				//     The time to wait for the event.
				//
				// Returns:
				//     A task representing the operation of getting the structure that contains the
				//     result of the waiting operation.
				//ValueTask<WaitForEventResult> WaitForEventAsync(int pinNumber, PinEventTypes eventTypes, TimeSpan timeout);
				//
				// Summary:
				//     Writes a value to a pin.
				//
				// Parameters:
				//   pinNumber:
				//     The pin number in the controller's numbering scheme.
				//
				//   value:
				//     The value to be written to the pin.
				inline void Write(int pinNumber, PinValue value)
				{

				}
				//
				// Summary:
				//     Write the given pins with the given values.
				//
				// Parameters:
				//   pinValuePairs:
				//     The pin/value pairs to write.
				//void Write(ReadOnlySpan<PinValuePair> pinValuePairs);
			};
		}
	}
}



#endif	// !DOTNET_SYSTEM_DEVICE_GPIO_H__