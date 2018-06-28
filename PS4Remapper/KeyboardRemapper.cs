using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
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
        private Dictionary<Keys, MapAction> _actions;
        
        public delegate void OnKeyChangedDelegate(string name);
        public OnKeyChangedDelegate OnKeyChanged { get; set; }

        public KeyboardRemapper(Remapper remapper)
        {
            _remapper = remapper;
            _pressed = new Dictionary<Keys, bool>();
            _actions = new Dictionary<Keys, MapAction>();

            CreateActions();
        }
        
        public void CreateActions()
        {
            var dict = new Dictionary<Keys, MapAction>();

            foreach (MapAction item in _remapper.Map)
            {
                if (item.Key == Keys.None)
                {
                    continue;
                }

                try
                {
                    dict.Add(item.Key, item);
                }
                catch (ArgumentException ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            _actions = dict;
        }
        
        public void OnKeyPressed(object sender, KeyboardHookEventArgs e)
        {
            // https://msdn.microsoft.com/en-us/library/windows/desktop/dd375731(v=vs.85).aspx

            if (!_remapper.CheckFocusedWindow())
            {
                return;
            }

            var key = (Keys)e.KeyboardData.VirtualCode;

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

            if (_remapper.IsDebugKeyboard)
            {
                if (IsKeyDown())
                {
                    Debug.WriteLine(string.Join(",", _pressed.Keys));
                }
            }
        }

        public bool IsKeyDown()
        {
            return _pressed.Count > 0;
        }

        public void ExecuteActionsByKey(List<Keys> pressed)
        {
            var state = new DualShockState();

            foreach (var key in pressed)
            {
                if (key == Keys.None)
                {
                    continue;
                }

                try
                {
                    var action = _actions[key];
                    ExecuteRemapAction(action, state);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.StackTrace);
                }
            }
        }

        private void ExecuteRemapAction(MapAction action, DualShockState state)
        {
            // Try to set property using Reflection
            bool didSetProperty = false;
            try
            {
                _remapper.SetValue(state, action.Property, action.Value);
                didSetProperty = true;
                OnKeyChanged?.Invoke(action.Name);
            }
            catch (Exception ex) { Debug.WriteLine(ex.StackTrace); }

            if (didSetProperty)
            {
                state.Battery = 255;
                state.IsCharging = true;

                _remapper.CurrentState = state;
            }
        }
    }
}