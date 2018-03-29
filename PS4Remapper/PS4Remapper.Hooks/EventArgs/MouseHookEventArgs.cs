using System.ComponentModel;
using PS4Remapper.Hooks.States;

namespace PS4Remapper.Hooks.EventArgs
{
    public class MouseHookEventArgs : HandledEventArgs
    {
        public MouseState MouseState { get; private set; }
        public MouseHook.LowLevelMouseInputEvent MouseData { get; private set; }

        public MouseHookEventArgs(
            MouseHook.LowLevelMouseInputEvent mouseData,
            MouseState mouseState)
        {
            MouseData = mouseData;
            MouseState = mouseState;
        }
    }
}