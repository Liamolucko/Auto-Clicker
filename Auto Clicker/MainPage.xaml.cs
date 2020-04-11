using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace Auto_Clicker
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private readonly NumberBox[] clickIntervalUnitBoxes;
        private readonly Dictionary<NumberBox, int> overflowPoints;

        public MainPage()
        {
            InitializeComponent();
            clickIntervalUnitBoxes = new NumberBox[] { Milliseconds, Seconds, Minutes, Hours };
            overflowPoints = new Dictionary<NumberBox, int>
            {
                {Milliseconds, 1000},
                {Seconds, 60},
                {Minutes, 60}
            };
        }

        public void HandleOverflows(NumberBox sender, NumberBoxValueChangedEventArgs e)
        {
            if (clickIntervalUnitBoxes != null && !Double.IsNaN(e.NewValue))
            {
                // Check for overflow
                if (sender != Hours)
                {
                    int overflowPoint = overflowPoints[sender];
                    if (e.NewValue > overflowPoints[sender])
                    {
                        NumberBox overflowBox = clickIntervalUnitBoxes[Array.IndexOf(clickIntervalUnitBoxes, sender) + 1];
                        overflowBox.Value = Math.Floor(e.NewValue / overflowPoint);
                        sender.Value = e.NewValue % overflowPoint;
                    }
                }

                // Check for non-integer number
                double fraction = e.NewValue % 1;
                if (sender != Milliseconds && fraction != 0)
                {
                    NumberBox overflowBox = clickIntervalUnitBoxes[Array.IndexOf(clickIntervalUnitBoxes, sender) - 1];
                    overflowBox.Value = fraction * overflowPoints[overflowBox];
                    sender.Value = Math.Floor(e.NewValue);
                }
            }
        }
    }
}
