﻿
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;
using PS4RemotePlayInterceptor;
using System.IO;
using PS4Remapper.Classes;
using PS4Remapper.Hooks.States;
using PS4Remapper.Hooks;
using PS4Remapper.Hooks.EventArgs;

namespace PS4Remapper
{
    public enum AnalogStick
    {
        Left,
        Right
    }

    public class MouseRemapper3
    {
        private readonly Remapper _remapper;

        //private const int MOUSE_CENTER_X = 500;
        //private const int MOUSE_CENTER_Y = 500;
        //private const int MOUSE_RELEASE_TIME = 50;
        private const int MOUSE_SENSITIVITY_DIVISOR = 100000;

        // Delegates
        public delegate void OnMouseAxisChangedDelegate(byte x, byte y);
        public OnMouseAxisChangedDelegate OnMouseAxisChanged { get; set; }

        public MouseStroke CurrentMouseStroke { get; private set; }
        public System.Timers.Timer MouseReleaseTimer { get; private set; }
        public bool IsCursorShowing { get; private set; }
        public int CursorOverflowX { get; private set; }
        public int CursorOverflowY { get; private set; }
        public bool LeftMouseDown { get; private set; }
        public bool RightMouseDown { get; private set; }
        public bool MiddleMouseDown { get; private set; }
        public double MouseSpeedX { get; private set; }
        public double MouseSpeedY { get; private set; }

        public bool EnableMouseInput { get; set; }
        public bool DebugCursor { get; set; }
        public double MouseSensitivity { get; set; }
        public double MouseDecayRate { get; set; }
        public double MouseDecayThreshold { get; set; }
        public double MouseAnalogDeadzone { get; set; }
        public double MouseMakeupSpeed { get; set; }
        public AnalogStick MouseMovementAnalog { get; set; }
        public bool MouseInvertXAxis { get; set; }
        public bool MouseInvertYAxis { get; set; }
        public int LeftMouseMapping { get; set; }
        public int RightMouseMapping { get; set; }
        public int MiddleMouseMapping { get; set; }

        public MouseRemapper3(Remapper remapper)
        {
            _remapper = remapper;

            IsCursorShowing = true;
            EnableMouseInput = false;
            DebugCursor = false;
            MouseSensitivity = 1;
            MouseDecayRate = 1.2;
            MouseDecayThreshold = 0.1;
            MouseAnalogDeadzone = 14.25;
            MouseMakeupSpeed = 10;
            MouseMovementAnalog = AnalogStick.Right;
            MouseInvertXAxis = false;
            MouseInvertYAxis = false;
            LeftMouseMapping = 11; // R2
            RightMouseMapping = 10; // L2
            MiddleMouseMapping = 9; // L1
        }

        public void OnReceiveData(ref DualShockState state)
        {
            if (!_remapper.CheckFocusedWindow())
                return;

            var checkState = new DualShockState();

            // Left mouse
            var leftMap = _remapper.Map.ElementAtOrDefault(LeftMouseMapping);
            if (leftMap != null)
            {
                if (LeftMouseDown)
                {
                    _remapper.SetValue(_remapper.CurrentState, leftMap.Property, leftMap.Value);
                }
                else
                {
                    var defaultValue = _remapper.GetValue(checkState, leftMap.Property);
                    _remapper.SetValue(_remapper.CurrentState, leftMap.Property, defaultValue);
                }
            }

            // Right mouse
            var rightMap = _remapper.Map.ElementAtOrDefault(RightMouseMapping);
            if (rightMap != null)
            {
                if (RightMouseDown)
                {
                    _remapper.SetValue(_remapper.CurrentState, rightMap.Property, rightMap.Value);
                }
                else
                {
                    var defaultValue = _remapper.GetValue(checkState, rightMap.Property);
                    _remapper.SetValue(_remapper.CurrentState, rightMap.Property, defaultValue);
                }
            }

            // Middle mouse
            var middleMap = _remapper.Map.ElementAtOrDefault(MiddleMouseMapping);
            if (middleMap != null)
            {
                if (MiddleMouseDown)
                {
                    _remapper.SetValue(_remapper.CurrentState, middleMap.Property, middleMap.Value);
                }
                else
                {
                    var defaultValue = _remapper.GetValue(checkState, middleMap.Property);
                    _remapper.SetValue(_remapper.CurrentState, middleMap.Property, defaultValue);
                }
            }

            // Mouse moved
            if (CurrentMouseStroke != null && CurrentMouseStroke.DidMoved)
            {
                MouseSpeedX = (CurrentMouseStroke.VelocityX * MouseSensitivity) / MOUSE_SENSITIVITY_DIVISOR;
                if (MouseInvertXAxis) MouseSpeedX *= -1;

                MouseSpeedY = (CurrentMouseStroke.VelocityY * MouseSensitivity) / MOUSE_SENSITIVITY_DIVISOR;
                if (MouseInvertYAxis) MouseSpeedY *= -1;

                CurrentMouseStroke.DidMoved = false;

                // Stop release timer
                //if (MouseReleaseTimer != null)
                //{
                //    MouseReleaseTimer.Stop();
                //    MouseReleaseTimer = null;
                //}
            }
            // Mouse idle
            else
            {
                // Start decay
                MouseSpeedX /= MouseDecayRate;
                MouseSpeedY /= MouseDecayRate;

                // Stop decaying joystick if below threshold
                //if (Math.Abs(MouseSpeedX) < MouseDecayThreshold || Math.Abs(MouseSpeedY) < MouseDecayThreshold)
                //{
                //    // Reset mouse speed
                //    if (Math.Abs(MouseSpeedX) < MouseDecayThreshold) MouseSpeedX = 0;
                //    if (Math.Abs(MouseSpeedY) < MouseDecayThreshold) MouseSpeedY = 0;

                //    // Start release timer
                //    if (MouseReleaseTimer == null)
                //    {
                //        MouseReleaseTimer = new System.Timers.Timer(MOUSE_RELEASE_TIME);
                //        MouseReleaseTimer.Start();
                //        MouseReleaseTimer.Elapsed += (s, e) =>
                //        {
                //            // Recenter cursor
                //            RemapperUtility.SetCursorPosition(MOUSE_CENTER_X, MOUSE_CENTER_Y);

                //            // Reset cursor overflow
                //            CursorOverflowX = 0;
                //            CursorOverflowY = 0;

                //            // Stop release timer
                //            MouseReleaseTimer.Stop();
                //            MouseReleaseTimer = null;
                //        };

                //    }
                //}
            }

            const double min = 0;
            const double max = 255;
            string analogProperty = MouseMovementAnalog == AnalogStick.Left ? "L" : "R";

            // Minimum speed
            double positiveSpeed = 128 + MouseAnalogDeadzone;
            double negativeSpeed = 128 - MouseAnalogDeadzone;

            // Base speed
            double baseX = ((MouseSpeedX > 0) ? positiveSpeed : ((MouseSpeedX < 0) ? negativeSpeed : 128));
            double baseY = ((MouseSpeedY > 0) ? positiveSpeed : ((MouseSpeedY < 0) ? negativeSpeed : 128));

            // Makeup speed
            double makeupX = Math.Sign(MouseSpeedX) * MouseMakeupSpeed;
            double makeupY = Math.Sign(MouseSpeedY) * MouseMakeupSpeed;

            // Scale speed to analog values
            double rx = baseX + (makeupX * MouseSpeedX * MouseSpeedX * 127);
            double ry = baseY + (makeupY * MouseSpeedY * MouseSpeedY * 127);

            byte scaledX = (byte)((rx < min) ? min : (rx > max) ? max : rx);
            byte scaledY = (byte)((ry < min) ? min : (ry > max) ? max : ry);

            _remapper.CurrentState.RX = scaledX;
            _remapper.CurrentState.RY = scaledY;

            // Invoke callback
            OnMouseAxisChanged?.Invoke(scaledX, scaledY);
            
            state = _remapper.CurrentState;
        }

        public void OnMouseEvent(object sender, MouseHookEventArgs e)
        {
            bool focusedWindow = _remapper.CheckFocusedWindow();

            // Focused
            if (focusedWindow)
            {
                if (IsCursorShowing)
                {
                    ShowCursorAndToolbar(false);
                }

            }
            // Not focused
            else
            {
                // Show cursor
                if (!IsCursorShowing)
                {
                    ShowCursorAndToolbar(true);
                }

                // Ignore the rest if not focused
                return;
            }
            
            if (e.MouseState == MouseState.LeftButtonDown)
            {
                LeftMouseDown = true;
                e.Handled = true;
            }
            else if (e.MouseState == MouseState.LeftButtonUp)
            {
                LeftMouseDown = false;
                e.Handled = true;
            }
            // Right mouse
            else if (e.MouseState == MouseState.RightButtonDown)
            {
                RightMouseDown = true;
                e.Handled = true;
            }
            else if (e.MouseState == MouseState.RightButtonUp)
            {
                RightMouseDown = false;
                e.Handled = true;
            }
            // Middle mouse
            else if (e.MouseState == MouseState.MiddleButtonDown)
            {
                MiddleMouseDown = true;
                e.Handled = focusedWindow;
            }
            else if (e.MouseState == MouseState.MiddleButtonUp)
            {
                MiddleMouseDown = false;
                e.Handled = focusedWindow;
            }
            // Mouse move
            else if (e.MouseState == MouseState.Move)
            {
                var rawX = e.MouseData.Point.X;
                var rawY = e.MouseData.Point.Y;

                // Ignore if at center
                //if (rawX == MOUSE_CENTER_X && rawY == MOUSE_CENTER_Y)
                //    return;

                #region Store mouse stroke
                var newStroke = new MouseStroke()
                {
                    Timestamp = DateTime.Now,
                    RawData = e,
                    DidMoved = true,
                    X = rawX + CursorOverflowX,
                    Y = rawY + CursorOverflowY
                };

                if (CurrentMouseStroke != null)
                {
                    double deltaTime = (newStroke.Timestamp - CurrentMouseStroke.Timestamp).TotalSeconds;
                    newStroke.VelocityX = (newStroke.X - CurrentMouseStroke.X) / deltaTime;
                    newStroke.VelocityY = (newStroke.Y - CurrentMouseStroke.Y) / deltaTime;
                }

                CurrentMouseStroke = newStroke;
                #endregion

                #region Adjust cursor position;
                var didSetPosition = false;
                var screen = Screen.FromHandle(_remapper.RemotePlayProcess.MainWindowHandle);
                var workingArea = screen.WorkingArea;
                var tmpX = rawX - workingArea.X;
                var tmpY = rawY - workingArea.Y;

                if (tmpX >= workingArea.Width)
                {
                    CursorOverflowX += workingArea.Width;
                    //tmpX = 0;
                    didSetPosition = true;
                }
                else if (tmpX <= 0)
                {
                    CursorOverflowX -= workingArea.Width;
                    //tmpX = workingArea.Width;
                    didSetPosition = true;
                }

                if (tmpY >= workingArea.Height)
                {
                    CursorOverflowY += workingArea.Height;
                    //tmpY = 0;
                    didSetPosition = true;
                }
                else if (tmpY <= 0)
                {
                    CursorOverflowY -= workingArea.Height;
                    //tmpY = workingArea.Height;
                    didSetPosition = true;
                }

                // Block cursor
                if (didSetPosition)
                {
                    //RemapperUtility.SetCursorPosition(tmpX, tmpY);
                    e.Handled = true;
                }
                #endregion
            }
        }

        public void ShowCursorAndToolbar(bool value)
        {
            //if (_remapper.IsDebugMouse)
            //{
            //    return;
            //}

            //if (_remapper.RemotePlayProcess == null)
            //{
            //    return;
            //}

            CursorHook.ShowSystemCursor(value);
            //WindowHook.ShowStreamingToolBar(_remapper.RemotePlayProcess, value);
            IsCursorShowing = value;
        }
    }
}