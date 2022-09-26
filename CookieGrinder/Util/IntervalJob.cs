using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookieGrinder.Util
{
    public class IntervalJob : IDisposable
    {
        protected System.Threading.Timer _timer;
        public event EventHandler Elpsed;

        public uint IntervalMs { get; set; }

        public IntervalJob(uint periodMSec)
        {
            IntervalMs = periodMSec;
        }

        public void Start()
        {
            if (_timer != null)
                return; //Already started
            if (IntervalMs > 1)
                _timer = new System.Threading.Timer(InvokeElapsed, null, TimeSpan.Zero, TimeSpan.FromMilliseconds(IntervalMs));
        }

        private void InvokeElapsed(object? e)
            => Elpsed?.Invoke(this, new EventArgs());

        public void Stop()
        {
            _timer?.Dispose();
            _timer = null;
        }

        public void Dispose()
        {
            Stop();
        }
    }
}
