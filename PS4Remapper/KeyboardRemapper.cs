using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using PS4Remapper.Classes;
using PS4Remapper.Hooks;
using PS4Remapper.Hooks.EventArgs;
using PS4Remapper.Hooks.States;
using PS4RemotePlayInterceptor;

namespace PS4Remapper
{
    public class KeyboardRemapper
    {
        private readonly Remapper _remapper;

        private Dictionary<Keys, bool> _pressed;
        private Dictionary<Keys, MapAction> _keys;

        public List<MapAction> Map { get; private set; }

        public delegate void OnKeyChangedDelegate(string name);
        public OnKeyChangedDelegate OnKeyChanged { get; set; }

        public KeyboardRemapper(Remapper remapper)
        {
            _remapper = remapper;
            _pressed = new Dictionary<Keys, bool>();
            _keys = new Dictionary<Keys, MapAction>();

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

            CreateActions();
        }

        public void OnKeyPressed(object sender, KeyboardHookEventArgs e)
        {
            // https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx

            if (!_remapper.CheckFocusedWindow())
            {
                return;
            }

            int vk = e.KeyboardData.VirtualCode;
            Keys key = (Keys)vk;

            Debug.WriteLine(key);

            // Key down
            if (e.KeyboardState == KeyboardState.KeyDown)
            {
                if (!_pressed.ContainsKey(key))
                {
                    _pressed.Add(key, true);
                    ExecuteActionsByKey(_pressed.Keys.ToList());
                }

                e.Handled = true;
            }
            // Key up
            else if (e.KeyboardState == KeyboardState.KeyUp)
            {
                if (_pressed.ContainsKey(key))
                {
                    _pressed.Remove(key);
                    ExecuteActionsByKey(_pressed.Keys.ToList());
                }

                e.Handled = true;
            }

            // Reset state
            if (!IsKeyDown())
            {
                _remapper.CurrentState = null;
            }
        }

        public void CreateActions()
        {
            var dict = new Dictionary<Keys, MapAction>();

            Action<MapAction> addItem = item =>
            {
                try
                {
                    dict.Add(item.Key, item);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            foreach (MapAction item in Map)
            {
                if (item.Key == Keys.None) continue;
                addItem(item);
            }

            _keys = dict;
        }

        public bool IsKeyDown()
        {
            return _pressed.Count > 0;
        }

        public bool IsKeyInUse(Keys key)
        {
            return _keys.ContainsKey(key);
        }

        public void ExecuteActionsByKey(List<Keys> keys)
        {
            foreach (var key in keys)
            {
                if (key == Keys.None) continue;

                try
                {
                    var action = _keys[key];
                    ExecuteRemapAction(action);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                }
            }
        }

        private void ExecuteRemapAction(MapAction action)
        {
            if (_remapper.CurrentState == null)
                _remapper.CurrentState = new DualShockState();

            // Try to set property using Reflection
            bool didSetProperty = false;
            try
            {
                SetValue(_remapper.CurrentState, action.Property, action.Value);
                didSetProperty = true;
                OnKeyChanged?.Invoke(action.Name);
            }
            catch (Exception ex) { Debug.WriteLine(ex.StackTrace); }

            if (didSetProperty)
            {
                _remapper.CurrentState.Battery = 255;
                _remapper.CurrentState.IsCharging = true;
            }
        }

        public static void SetValue(object inputObject, string propertyName, object propertyVal)
        {
            //find out the type
            Type type = inputObject.GetType();

            //get the property information based on the type
            System.Reflection.PropertyInfo propertyInfo = type.GetProperty(propertyName);

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

        public static bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Nullable<>));
        }
    }
}