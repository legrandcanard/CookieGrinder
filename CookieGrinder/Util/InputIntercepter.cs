using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CookieGrinder.Util
{
    internal class InputIntercepter
    {
        private GlobalKeyboardHook _globalKeyboardHook;

        public event EventHandler<GlobalKeyboardHookEventArgs> KeyPressed;

        public void SetupKeyboardHooks()
        {
            _globalKeyboardHook = new GlobalKeyboardHook();
            _globalKeyboardHook.KeyboardPressed += _globalKeyboardHook_KeyboardPressed;
        }

        private void _globalKeyboardHook_KeyboardPressed(object? sender, GlobalKeyboardHookEventArgs e)
        {
            KeyPressed?.Invoke(this, e);
        }

        public void Dispose()
        {
            _globalKeyboardHook.KeyboardPressed -= KeyPressed;
            _globalKeyboardHook?.Dispose();
        }
    }
}
