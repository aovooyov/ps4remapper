using System.ComponentModel;
using PS4Remapper.Hooks.States;

namespace PS4Remapper.Hooks.EventArgs
{
    public class KeyboardHookEventArgs : HandledEventArgs
    {
        public KeyboardState KeyboardState { get; private set; }
        public KeyboardHook.LowLevelKeyboardInputEvent KeyboardData { get; private set; }

        public KeyboardHookEventArgs(
            KeyboardHook.LowLevelKeyboardInputEvent keyboardData,
            KeyboardState keyboardState)
        {
            KeyboardData = keyboardData;
            KeyboardState = keyboardState;
        }
    }
}