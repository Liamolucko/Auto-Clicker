using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System.Threading;
using Windows.UI.Input.Preview.Injection;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.ApplicationModel.Core;
using Windows.Foundation;

namespace AutoClicker
{
    public class Clicker
    {
        private ThreadPoolTimer timer;
        private InputInjector injector;

        public Clicker() { }

        /// <summary>
        /// Starts clicking at current click interval. The interval will not change unless restarted.
        /// </summary>
        public void Start()
        {
            if (IsRunning)
            {
                timer.Cancel();
            }
            timer = ThreadPoolTimer.CreatePeriodicTimer((source) =>
            {
                injector = InputInjector.TryCreate();
                injector.InjectMouseInput(new InjectedInputMouseInfo[]
                {
                    new InjectedInputMouseInfo
                    {
                        MouseOptions = InjectedInputMouseOptions.LeftDown
                    },
                    new InjectedInputMouseInfo
                    {
                        MouseOptions = InjectedInputMouseOptions.LeftUp
                    },
                });
            }, TimeSpan.FromMilliseconds(ClickInterval));
            IsRunning = true;
        }

        public void Stop()
        {
            if (!IsRunning) { return; }
            timer.Cancel();
            IsRunning = false;
        }

        /// <summary>
        ///     Click interval in seconds.
        /// </summary>
        public double ClickInterval { get; set; } = 100;

        public bool IsRunning { get; set; } = false;
    }
}
