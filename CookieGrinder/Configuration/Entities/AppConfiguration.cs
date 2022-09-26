using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookieGrinder.Configuration.Entities
{
    public class AppConfiguration
    {
        public const int DEFAULT_SHUTDOWN_KEYCODE = 0x53; //C
        public const int DEFAULT_PAUSE_KEYCODE = 0x50; //P
        public const int DEFAULT_RESUME_KEYCODE = 0x53; //S

        public uint IntervalMs { get; set; } = 1000;
        public string ShutdownKey { get; set; } = "C";
        public string PasuseKey { get; set; } = "P";
        public string ResumeKey { get; set; } = "S";
    }
}
