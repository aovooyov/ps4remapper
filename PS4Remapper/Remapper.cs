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

            _mouse.Hook();
        }

        public void Inject()
        {
            if (IsInjected)
            {
                return;
            }

            _keyboard.Hook();

            PID = Interceptor.Inject();
            RemotePlayProcess = Process.GetProcessById(PID);

            IsInjected = true;
        }

        public void Stop()
        {
            if (!IsInjected)
            {
                return;
            }

            IsInjected = false;

            _mouseRemapper.ShowCursorAndToolbar(true);

            _keyboard.UnHook();
            //_mouse.UnHook();

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
                CurrentState = new DualShockState() { Battery = 255 };
            }

            _mouseRemapper.OnReceiveData();
            state = CurrentState;

            //Debug.WriteLine($"{state.RX} {state.RY} {state.Cross}", "Callback");
        }

        private void KeyboardOnKeyboardPressed(object sender, KeyboardHookEventArgs e)
        {
            //Debug.WriteLine($"{e.KeyboardData.VirtualCode}", "KEY");
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
            if (RemotePlayProcess == null)
            {
                return false;
            }

            return WindowHook.IsProcessInForeground(RemotePlayProcess);
        }
    }
}