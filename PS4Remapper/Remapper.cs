﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using PS4Remapper.Classes;
using PS4Remapper.Hooks;
using PS4Remapper.Hooks.EventArgs;
using PS4RemotePlayInterceptor;

namespace PS4Remapper
{
    public class Remapper
    {
        private static readonly Lazy<Remapper> _remapper = new Lazy<Remapper>(() => new Remapper());

        public static Remapper Instance => _remapper.Value;

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

        public List<MapAction> Map { get; private set; }

        public Remapper()
        {
            Map = new List<MapAction>
            {
                new MapAction("L Left", Keys.A, "LX", 0),
                new MapAction("L Right", Keys.D, "LX", 255),
                new MapAction("L Up", Keys.W, "LY", 0),
                new MapAction("L Down", Keys.S, "LY", 255),

                new MapAction("R Left", Keys.J, "RX", 0),
                new MapAction("R Right", Keys.L, "RX", 255),
                new MapAction("R Up", Keys.I, "RY", 0),
                new MapAction("R Down", Keys.K, "RY", 255),

                new MapAction("R1", Keys.E, "R1", true),
                new MapAction("L1", Keys.Q, "L1", true),
                new MapAction("L2", Keys.U, "L2", 255),
                new MapAction("R2", Keys.O, "R2", 255),

                new MapAction("Triangle", Keys.C, "Triangle", true),
                new MapAction("Circle", Keys.Escape, "Circle", true),
                new MapAction("Cross", Keys.Enter, "Cross", true),
                new MapAction("Square", Keys.V, "Square", true),

                new MapAction("DPad Up", Keys.Up, "DPad_Up", true),
                new MapAction("DPad Down", Keys.Down, "DPad_Down", true),
                new MapAction("DPad Left", Keys.Left, "DPad_Left", true),
                new MapAction("DPad Right", Keys.Right, "DPad_Right", true),

                new MapAction("L3", Keys.N, "L3", true),
                new MapAction("R3", Keys.M, "R3", true),

                new MapAction("Share", Keys.LControlKey, "Share", true),
                new MapAction("Options", Keys.Z, "Options", true),
                new MapAction("PS", Keys.LShiftKey, "PS", true),

                new MapAction("Touch Button", Keys.T, "TouchButton", true)
            };
            
            _keyboard = new KeyboardHook();
            _mouse = new MouseHook();
            
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

            _keyboard.KeyboardPressed += KeyboardOnKeyboardPressed;
            _mouse.MouseEvent += MouseOnMouseEvent;

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
            _keyboard.KeyboardPressed += KeyboardOnKeyboardPressed;

            IsDebugKeyboard = true;
            IsInjected = true;
        }

        public void DebugMouse(Process process)
        {
            if (IsInjected)
            {
                return;
            }

            _mouse.Hook();
            _mouse.MouseEvent += MouseOnMouseEvent;

            PID = process.Id;
            RemotePlayProcess = process;

            //Cursor.Clip = IsDebugMouse
            //    ? Screen.PrimaryScreen.Bounds
            //    : Screen.FromHandle(_remapper.RemotePlayProcess.MainWindowHandle).Bounds;
            
            IsDebugMouse = true;
            IsInjected = true;

            Task.Factory.StartNew(() =>
            {
                while (IsDebugMouse)
                {
                    Task.Delay(200);

                    var state = new DualShockState();
                    OnReceiveData(ref state);
                }
            });
        }

        public void Stop()
        {
            if (!IsInjected)
            {
                return;
            }

            IsInjected = false;
            IsDebugMouse = false;
            IsDebugKeyboard = false;

            _mouseRemapper.ShowCursorAndToolbar(true);

            _keyboard.UnHook();
            _mouse.UnHook();

            _keyboard.KeyboardPressed -= KeyboardOnKeyboardPressed;
            _mouse.MouseEvent -= MouseOnMouseEvent;

            Interceptor.StopInjection();
            PID = -1;
            RemotePlayProcess = null;
        }
        
        public void OnReceiveData(ref DualShockState state)
        {
            if (!IsInjected)
            {
                return;
            }

            if (CurrentState == null)
            {
                CurrentState = new DualShockState { Battery = 255 };
            }

            _mouseRemapper.OnReceiveData(ref state);
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

            return IsDebugKeyboard || IsDebugMouse;
        }

        public void SetValue(object inputObject, string propertyName, object propertyVal)
        {
            //find out the type
            Type type = inputObject.GetType();

            //get the property information based on the type
            PropertyInfo propertyInfo = type.GetProperty(propertyName);

            //find the property type
            Type propertyType = propertyInfo.PropertyType;

            //Convert.ChangeType does not handle conversion to nullable types
            //if the property type is nullable, we need to get the underlying type of the property
            var targetType = IsNullableType(propertyInfo.PropertyType) ? Nullable.GetUnderlyingType(propertyInfo.PropertyType) : propertyInfo.PropertyType;

            //Returns an System.Object with the specified System.Type and whose value is
            //equivalent to the specified object.
            propertyVal = Convert.ChangeType(propertyVal, targetType);

            //Set the value of the property
            propertyInfo.SetValue(inputObject, propertyVal, null);
        }

        public object GetValue(object src, string propName)
        {
            return src.GetType().GetProperty(propName).GetValue(src, null);
        }

        public bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}