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

            _highTicker= new Ticker(SetHigh)
            {
                Microseconds = _highPulse
            };
            
            _lowTicker = new Ticker((SetLow))
            {
                Microseconds = _lowPulse
            };
            
            _highTicker.Start();
        }

        public void Stop()
        {
            _highTicker.Stop();
            
            while(_highTicker.Ticking)
                Thread.Sleep(0);
            
            _lowTicker.Stop();
            
            while(_lowTicker.Ticking)
                Thread.Sleep(0);
            
            _gpioController.Write(_pin, PinValue.Low);
        }

        private void SetHigh(object sender, ProgressChangedEventArgs e)
        {
            _gpioController.Write(_pin, PinValue.High);
            
            _highTicker.Stop();
            
            while(_highTicker.Ticking)
                Thread.Sleep(0);
            
            _lowTicker.Start();
        }

        private void SetLow(object sender, ProgressChangedEventArgs e)
        {
            _gpioController.Write(_pin, PinValue.Low);
            _lowTicker.Stop();
            
            while(_lowTicker.Ticking)
                Thread.Sleep(0);
            
            _highTicker.Start();
        }
    }
}