using System;
using System.Drawing;
using PS4Remapper.Hooks;
using PS4Remapper.Hooks.EventArgs;
using PS4Remapper.Interceptor;

namespace PS4Remapper
{
    public class Remapper
    {
        private KeyboardHook _keyboard;
        private MouseHook _mouse;

        private MouseRemapper _mouseRemapper;
        
        public MouseRemapper Mouse => _mouseRemapper;

        public Remapper()
        {
            _keyboard = new KeyboardHook();
            _mouse = new MouseHook();
            _mouseRemapper = new MouseRemapper();
        }

        public void Initialize()
        {
            _keyboard.KeyboardPressed += KeyboardOnKeyboardPressed;
            _mouse.MouseEvent += MouseOnMouseEvent;

            Interceptor.Interceptor.EmulateController = true;
            Interceptor.Interceptor.InjectionMode = InjectionMode.Compatibility;

            Interceptor.Interceptor.Callback = new InterceptionDelegate(_mouseRemapper.OnReceiveData);
        }
        
        private void KeyboardOnKeyboardPressed(object sender, KeyboardHookEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void MouseOnMouseEvent(object sender, MouseHookEventArgs e)
        {
            _mouseRemapper.OnMouseEvent(sender, e);
        }
    }
}