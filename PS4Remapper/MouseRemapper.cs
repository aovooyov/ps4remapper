using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;
using PS4Remapper.Classes;
using PS4Remapper.Hooks;
using PS4Remapper.Hooks.EventArgs;
using PS4Remapper.Hooks.States;
using PS4Remapper.Types;
using PS4RemotePlayInterceptor;

namespace PS4Remapper
{
    public class MouseRemapper
    {
        private readonly Remapper _remapper;

        private const int MOUSE_SENSITIVITY_DIVISOR = 100000;

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

        public double MouseSensitivity { get; set; }
        public double MouseDecayRate { get; set; }
        public double MouseDecayThreshold { get; set; }
        public double MouseAnalogDeadzone { get; set; }
        public double MouseMakeupSpeed { get; set; }
        public Stick MouseMovementAnalog { get; set; }
        public bool MouseInvertXAxis { get; set; }
        public bool MouseInvertYAxis { get; set; }
        public int LeftMouseMapping { get; set; }
        public int RightMouseMapping { get; set; }
        public int MiddleMouseMapping { get; set; }

        private byte RX;
        private byte RY;
        //private double sensitivity = 0.9;
        //private int height;
        //private int width;

        public MouseRemapper(Remapper remapper)
        {
            _remapper = remapper;

            IsCursorShowing = true;

            MouseSensitivity = 1;
            MouseDecayRate = 1.2;
            MouseDecayThreshold = 0.1;
            MouseAnalogDeadzone = 14.25;
            MouseMakeupSpeed = 500;

            MouseMovementAnalog = Stick.Right;
            MouseInvertXAxis = false;
            MouseInvertYAxis = false;
            LeftMouseMapping = 11; // R2
            RightMouseMapping = 10; // L2
            MiddleMouseMapping = 9; // L1
        }

        #region VERSION 1
        public void OnReceiveData1(ref DualShockState state)
        {
            if (!_remapper.CheckFocusedWindow())
                return;

            _remapper.CurrentState.RX = RX;
            _remapper.CurrentState.RY = RY;

            state = _remapper.CurrentState;
        }

        public void OnMouseEvent1(object sender, MouseHookEventArgs e)
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

            switch (e.MouseState)
            {
                case MouseState.LeftButtonDown:
                    LeftMouseDown = true;
                    e.Handled = focusedWindow;
                    break;
                case MouseState.LeftButtonUp:
                    LeftMouseDown = false;
                    e.Handled = focusedWindow;
                    break;
                case MouseState.RightButtonDown:
                    RightMouseDown = true;
                    e.Handled = focusedWindow;
                    break;
                case MouseState.RightButtonUp:
                    RightMouseDown = false;
                    e.Handled = focusedWindow;
                    break;
                case MouseState.Move:

                    if (MouseReleaseTimer != null)
                    {
                        MouseReleaseTimer.Stop();
                        MouseReleaseTimer = null;
                    }

                    var x = e.MouseData.Point.X;
                    var y = e.MouseData.Point.Y;

                    if (x <= 0)
                    {
                        x = 0;
                    }

                    if (y <= 0)
                    {
                        y = 0;
                    }

                    var screen = Screen.FromHandle(_remapper.RemotePlayProcess.MainWindowHandle);
                    var width = screen.WorkingArea.Width;
                    var height = screen.WorkingArea.Height;

                    if (x >= width)
                    {
                        MouseToCenter();
                        break;
                    }

                    if (y >= height)
                    {
                        MouseToCenter();
                        break;
                    }

                    Debug.WriteLine($"{x}x{y}", "Coors");

                    var rx = (byte)Math.Ceiling(x / (double)width * 255);
                    var ry = (byte)Math.Ceiling(y / (double)height * 255);

                    RX = rx;
                    RY = ry;

                    //Debug.WriteLine($"{x} {y} {rx} {ry}");
                    OnMouseAxisChanged?.Invoke(rx, ry);

                    if (MouseReleaseTimer == null)
                    {
                        MouseReleaseTimer = new System.Timers.Timer(200);
                        MouseReleaseTimer.Start();
                        MouseReleaseTimer.Elapsed += (s, args) =>
                        {
                            MouseToCenter();
                        };
                    }
                    e.Handled = focusedWindow;
                    break;
                case MouseState.Wheel:
                    break;
                case MouseState.MiddleButtonDown:
                    break;
                case MouseState.MiddleButtonUp:
                    break;
                default:
                    e.Handled = focusedWindow;
                    break;
            }
        }

        private void MouseToCenter()
        {
            RX = 128;
            RY = 128;

            var screen = Screen.FromHandle(_remapper.RemotePlayProcess.MainWindowHandle);
            var width = screen.WorkingArea.Width;
            var height = screen.WorkingArea.Height;

            var cx = width / 2;
            var cy = height / 2;

            OnMouseAxisChanged?.Invoke(RX, RY);
            CursorHook.SetCursorPosition(cx, cy);
        }
        #endregion


        #region VERSION 2

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

            // Mouse Input
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
                //    //if (MouseReleaseTimer == null)
                //    //{
                //    //    MouseReleaseTimer = new System.Timers.Timer(MOUSE_RELEASE_TIME);
                //    //    MouseReleaseTimer.Start();
                //    //    MouseReleaseTimer.Elapsed += (s, e) =>
                //    //    {
                //    //        // Recenter cursor
                //    //        CursorHook.SetCursorPosition(MOUSE_CENTER_X, MOUSE_CENTER_Y);

                //    //        // Reset cursor overflow
                //    //        CursorOverflowX = 0;
                //    //        CursorOverflowY = 0;

                //    //        // Stop release timer
                //    //        MouseReleaseTimer.Stop();
                //    //        MouseReleaseTimer = null;
                //    //    };

                //    //}
                //}
            }

            const double min = 0;
            const double max = 255;
            string analogProperty = MouseMovementAnalog == Stick.Left ? "L" : "R";

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

            state = _remapper.CurrentState;
            // Invoke callback
            OnMouseAxisChanged?.Invoke(scaledX, scaledY);
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

                #region Adjust cursor position
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

        #endregion

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