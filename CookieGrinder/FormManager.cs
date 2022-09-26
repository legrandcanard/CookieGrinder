using CookieGrinder.Configuration;
using CookieGrinder.Configuration.Entities;
using CookieGrinder.Util;

namespace CookieGrinder
{
    internal static class FormManager
    {
        #region Control Keys
        const string CTL_KEY_IN_PROGRESS = "InProgressLabel";
        const string CTL_KEY_START_BTN = "StartButton";
        const string CTL_KEY_STOP_BTN = "StopButton";
        #endregion

        static IntervalJob _intervalJob;
        static Form _mainForm;

        static int _shutdownKey { get; set; }
        static int _pasuseKey { get; set; }
        static int _resumeKey { get; set; }

        [STAThread]
        public static void Main()
        {
            ApplicationConfiguration.Initialize();
            AppConfiguration configuration = new ConfiguratonManager().GetConfiguration();
            SetupKeys(configuration);

            var intercepter = new InputIntercepter();
            intercepter.SetupKeyboardHooks();
            intercepter.KeyPressed += OnKeyPressed;

            var job = _intervalJob = new IntervalJob(configuration.IntervalMs);
            job.Elpsed += (e, args) => MouseSimulator.LeftMouseClick(1, 1);

            var form = _mainForm = new FormMain();
            var startButton = form.Controls[CTL_KEY_START_BTN];
            startButton.Click += OnStartClick;

            var stopButton = form.Controls[CTL_KEY_STOP_BTN];
            stopButton.Click += OnStopClick;
            stopButton.Enabled = false;

            SetupHotKeyHelpersOnForm();

            Application.Run(form);
        }

        static void OnStartClick(object sender, EventArgs eventArgs)
        {
            _mainForm.Controls[CTL_KEY_IN_PROGRESS].Show();
            _mainForm.Controls[CTL_KEY_START_BTN].Enabled = false;
            _mainForm.Controls[CTL_KEY_STOP_BTN].Enabled = true;
            _intervalJob.Start();
        }

        static void OnStopClick(object sender, EventArgs eventArgs)
        {
            _mainForm.Controls[CTL_KEY_IN_PROGRESS].Hide();
            _mainForm.Controls[CTL_KEY_START_BTN].Enabled = true;
            _mainForm.Controls[CTL_KEY_STOP_BTN].Enabled = false;
            _intervalJob.Stop();
        }

        static void OnKeyPressed(object sender, GlobalKeyboardHookEventArgs e)
        {
            if (e.KeyboardData.Flags != GlobalKeyboardHook.LlkhfAltdown)
                return;

            if (e.KeyboardData.VirtualCode == _shutdownKey)
            {
                Application.Exit();
            }
            else if (e.KeyboardData.VirtualCode == _pasuseKey)
            {
                OnStopClick(sender, e);
            }
            else if (e.KeyboardData.VirtualCode == _resumeKey)
            {
                OnStartClick(sender, e);
            }

            e.Handled = true;
        }

        static void SetupKeys(AppConfiguration appConfiguration)
        {
            _shutdownKey = Enum.TryParse<Keys>(appConfiguration.ShutdownKey, out Keys shutdownKey) ? (int)shutdownKey : AppConfiguration.DEFAULT_SHUTDOWN_KEYCODE;
            _pasuseKey = Enum.TryParse<Keys>(appConfiguration.PasuseKey, out Keys pasuseKey) ? (int)pasuseKey : AppConfiguration.DEFAULT_PAUSE_KEYCODE;
            _resumeKey = Enum.TryParse<Keys>(appConfiguration.ResumeKey, out Keys resumeKey) ? (int)resumeKey : AppConfiguration.DEFAULT_RESUME_KEYCODE;
        }

        static void SetupHotKeyHelpersOnForm()
        {
            const string ctlKey = "Alt";
            _mainForm.Controls["label2"].Text = $"Terminate: {ctlKey}+{(Keys)_shutdownKey}";
            _mainForm.Controls["label3"].Text = $"Pause    : {ctlKey}+{(Keys)_pasuseKey}";
            _mainForm.Controls["label6"].Text = $"Resume   : {ctlKey}+{(Keys)_resumeKey}";
        }
    }
}