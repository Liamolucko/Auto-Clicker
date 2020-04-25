using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace AutoClicker
{
    public class ViewModel : BindableBase
    {
        public ViewModel(Clicker clicker)
        {
            this.clicker = clicker;
        }

        private readonly Clicker clicker;

        /// <summary>
        /// The current click interval
        /// </summary>
        public double ClickInterval
        {
            get { return clicker.ClickInterval / unitValues[units]; }
            set
            {
                if (ClickInterval == value) return;
                if (double.IsNaN(value))
                {
                    value = 0;
                }
                clicker.ClickInterval = value * unitValues[units];
                if (value > MaximumClickInterval && Units == "Milliseconds")
                {
                    Units = "Seconds";
                }
                else if (value < MinimumClickInterval && Units == "Seconds")
                {
                    Units = "Milliseconds";
                }
                OnPropertyChanged("ClickInterval");
            }
        }

        private readonly Dictionary<string, double> unitValues = new Dictionary<string, double>
        {
            {"Milliseconds", 1},
            {"Seconds", 1000}
        };
        private string units = "Milliseconds";
        /// <summary>
        /// The units in which the click interval is currently served.
        /// </summary>
        public string Units
        {
            get { return units; }
            set
            {
                if (units == value) return;
                SetProperty(ref units, value, "Units");
                var tempClickInterval = clicker.ClickInterval;
                OnPropertyChanged("MinimumClickInterval");
                OnPropertyChanged("MaximumClickInterval");
                OnPropertyChanged("ClickIntervalStepFrequency");
                clicker.ClickInterval = tempClickInterval;
                OnPropertyChanged("ClickInterval");
            }
        }

        private readonly Dictionary<string, double> minimumClickIntervals = new Dictionary<string, double>
        {
            { "Milliseconds", 1 },
            { "Seconds", 0.1 }
        };
        public double MinimumClickInterval { get { return minimumClickIntervals[units]; } }

        private readonly Dictionary<string, double> maximumClickIntervals = new Dictionary<string, double>
        {
            { "Milliseconds", 1000 },
            { "Seconds", 30 }
        };
        public double MaximumClickInterval { get { return maximumClickIntervals[units]; } }

        private readonly Dictionary<string, double> clickIntervalStepFrequencies = new Dictionary<string, double>
        {
            {"Milliseconds", 1 },
            {"Seconds", 0.1 }
        };
        public double ClickIntervalStepFrequency { get { return clickIntervalStepFrequencies[units]; } }

        public bool IsClicking { get { return clicker.IsRunning; } }
        public bool ControlsEnabled { get { return !clicker.IsRunning; } }

        public void ToggleClicking()
        {
            if (clicker.IsRunning)
            {
                clicker.Stop();
            }
            else
            {
                clicker.Start();
            }
            OnPropertyChanged("IsClicking");
            OnPropertyChanged("ControlsEnabled");
        }
    }
}
