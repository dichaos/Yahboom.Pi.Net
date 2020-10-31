using System;
using System.ComponentModel;
using System.Device.Gpio;
using System.Threading;

namespace Robot.PWM
{
    public class PWM
    {
        private readonly GpioController _gpioController;
        private readonly int _pin;
        private readonly long _minPulseCycle;
        private readonly long _maxPulseCycle;

        private Ticker _highTicker;
        private Ticker _lowTicker;
        
        public PWM(GpioController gpioController, int pin, long minPulseCycle, long maxPulseCycle)
        {
            _gpioController = gpioController;
            _pin = pin;
            _minPulseCycle = minPulseCycle;
            _maxPulseCycle = maxPulseCycle;
            
            _gpioController.OpenPin(pin, PinMode.Output);
        }

        public void SetDutyCycle(long pulse)
        {
            if (pulse < _minPulseCycle)
                pulse = _minPulseCycle;

            if (pulse > _maxPulseCycle)
                pulse = _maxPulseCycle;

            long _highPulse = pulse;
            long _lowPulse = _maxPulseCycle - pulse;

            _highTicker = new Ticker(_highPulse);
            _lowTicker = new Ticker(_lowPulse);

            for(int i = 0; i < 300; i++)
            {
                SetHigh();
                SetLow();
            }
        }

        public void Stop()
        {
            _gpioController.Write(_pin, PinValue.Low);
        }

        private void SetHigh()
        {
            _gpioController.Write(_pin, PinValue.High);
            _highTicker.Wait();
            
        }

        private void SetLow()
        {
            _gpioController.Write(_pin, PinValue.Low);
            _lowTicker.Wait();
        }
    }
}