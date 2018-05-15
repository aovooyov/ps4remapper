using System;
using System.Diagnostics;
using System.Drawing;
using PS4Remapper.Hooks;
using PS4Remapper.Hooks.EventArgs;
using PS4RemotePlayInterceptor;

namespace PS4Remapper
{
    public class Remapper
    {
        public bool IsDebugKeyboard { get; set; } = false;
        public bool IsDebugMouse { get; set; } = false;

        public Process RemotePlayProcess { get; set; }

        private KeyboardHook _keyboard;
        private MouseHook _mouse;

        private MouseRemapper _mouseRemapper;
        private KeyboardRemapper _keyboardRemapper;

        public MouseRemapper Mouse => _mouseRemapper;
        public KeyboardRemapper Keyboard => _keyboardRemapper;

        public DualShockState CurrentState { get; set; }

        public bool IsInjected { get; set; }
        public int PID { get; set; }

        public Remapper()
        {
            _keyboard = new KeyboardHook();
            _mouse = new MouseHook();

            _keyboard.KeyboardPressed += KeyboardOnKeyboardPressed;
            _mouse.MouseEvent += MouseOnMouseEvent;

            _mouseRemapper = new MouseRemapper(this);
            _keyboardRemapper = new KeyboardRemapper(this);

            Interceptor.InjectionMode = InjectionMode.Compatibility;
            Interceptor.Callback = new InterceptionDelegate(OnReceiveData);
        }

        public void Inject()
        {
            if (IsInjected)
            {
                return;
            }

            _mouse.Hook();
            _keyboard.Hook();

            PID = Interceptor.Inject();
            RemotePlayProcess = Process.GetProcessById(PID);

            IsInjected = true;
        }

        public void DebugKeyboard()
        {
            if (IsInjected)
            {
                return;
            }

            _keyboard.Hook();

            IsDebugKeyboard = true;
            IsInjected = true;
        }

        public void DebugMouse()
        {
            if (IsInjected)
            {
                return;
            }

            _mouse.Hook();

            IsDebugMouse = true;
            IsInjected = true;
        }

        public void Stop()
        {
            if (IsDebugKeyboard)
            {
                IsDebugKeyboard = false;
                IsInjected = false;

                _keyboard.UnHook();
                _keyboard.KeyboardPressed -= KeyboardOnKeyboardPressed;

                return;
            }

            if (IsDebugMouse)
            {
                IsDebugMouse = false;
                IsInjected = false;

                _mouse.UnHook();
                _mouse.MouseEvent -= MouseOnMouseEvent;

                return;
            }

            if (!IsInjected)
            {
                return;
            }

            IsInjected = false;

            _mouseRemapper.ShowCursorAndToolbar(true);

            _keyboard.UnHook();
            _mouse.UnHook();

            _keyboard.KeyboardPressed -= KeyboardOnKeyboardPressed;
            _mouse.MouseEvent -= MouseOnMouseEvent;

            Interceptor.StopInjection();
            PID = -1;
        }
        
        private void OnReceiveData(ref DualShockState state)
        {
            if (!IsInjected)
            {
                return;
            }

            if (CurrentState == null)
            {
                CurrentState = new DualShockState { Battery = 255 };
            }

            _mouseRemapper.OnReceiveData();
            state = CurrentState;
        }

        private void KeyboardOnKeyboardPressed(object sender, KeyboardHookEventArgs e)
        {
            if (!IsInjected)
            {
                return;
            }

            _keyboardRemapper.OnKeyPressed(sender, e);
        }
        
        private void MouseOnMouseEvent(object sender, MouseHookEventArgs e)
        {
            _mouseRemapper.OnMouseEvent(sender, e);
        }

        public void RefreshProcess()
        {
            try
            {
                if (RemotePlayProcess != null && !RemotePlayProcess.HasExited)
                {
                    RemotePlayProcess.Refresh();
                }
            }
            catch (Exception) { }
        }

        public bool CheckFocusedWindow()
        {
            if (RemotePlayProcess != null && WindowHook.IsProcessInForeground(RemotePlayProcess))
            {
                return true;
            }

            return IsDebugKeyboard || IsDebugKeyboard;
        }
    }
}