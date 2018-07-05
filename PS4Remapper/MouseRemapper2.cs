using PS4Remapper.Hooks;
using PS4Remapper.Hooks.EventArgs;
using PS4Remapper.Hooks.States;
using PS4RemotePlayInterceptor;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using Point = System.Drawing.Point;
using Timer = System.Timers.Timer;

namespace PS4Remapper
{
    public class MouseRemapper2
    {
        private readonly Remapper _remapper;

        public delegate void OnMouseAxisChangedDelegate(short x, short y);
        public OnMouseAxisChangedDelegate OnMouseAxisChanged { get; set; }
        
        private Point centered = new Point(Screen.PrimaryScreen.Bounds.Width / 2, Screen.PrimaryScreen.Bounds.Height / 2);
        private byte iMax = byte.MaxValue;
        private byte iMin = byte.MinValue;

        private static Stopwatch stopwatch = Stopwatch.StartNew();
        private static Timer MouseReleaseTimer { get; set; }
        private Thread tMouseMovement;

        public MouseRemapper2(Remapper remapper)
        {
            _remapper = remapper;
        }

        public double Mouse_Sensitivity_X = 1000.21299982071f;
        public double Mouse_Sensitivity_Y = 1000.21299982071f;
        public double Mouse_FinalMod = 100;
        public bool Mouse_Is_RightStick = true;
        public int Mouse_TickRate = 16;
        public bool Mouse_Invert_X = false;
        public bool Mouse_Invert_Y = false;
        public int DeadZoneSize = 0;
        //public bool Proccessed = false;

        public int LeftMouseMapping = 11; // R2
        public int RightMouseMapping = (int)Keys.E;//10; // L2
        public int MiddleMouseMapping = 9; // L1
        public bool LeftMouseDown { get; private set; }
        public bool RightMouseDown { get; private set; }
        public bool MiddleMouseDown { get; private set; }
        
        public void OnReceiveData(ref DualShockState state)
        {
            if (!_remapper.CheckFocusedWindow())
            {
                Stop();
                return;
            }

            Start();
            var checkState = new DualShockState();

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

            state = _remapper.CurrentState;
        }

        private void SetAxis(short x, short y)
        {
#if (DEBUG)
            //Debug.Print($"Mouse (X, Y) = ({x}, {y})");
#endif

            if (_remapper.CurrentState != null)
            {
                var rx = (x / 255f) + (255f / 2);
                var ry = (y / 255f) + (255f / 2);

                _remapper.CurrentState.RX = (byte)rx;
                _remapper.CurrentState.RY = (byte)ry;
            }
            
            OnMouseAxisChanged?.Invoke(x, y);
        }

        public void OnMouseEvent(object sender, MouseHookEventArgs e)
        {
            if (!_remapper.CheckFocusedWindow())
            {
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
            else if(e.MouseState == MouseState.Move)
            {
                //if (MouseReleaseTimer != null)
                //{
                //    MouseReleaseTimer.Stop();
                //    MouseReleaseTimer = null;
                //}

                var rawX = e.MouseData.Point.X;
                var rawY = e.MouseData.Point.Y;

                //Debug.Print($"e.MouseData.Point = {{X={e.MouseData.Point.X},Y={e.MouseData.Point.Y}}}");

                //MouseMovement_DeadZoning(rawX, rawY);
                //e.Handled = true;

                //if (MouseReleaseTimer == null)
                //{
                //    MouseReleaseTimer = new System.Timers.Timer(200);
                //    MouseReleaseTimer.Start();
                //    MouseReleaseTimer.Elapsed += (s, args) =>
                //    {
                //        if(_remapper.CurrentState != null)
                //        {
                //            Cursor.Position = centered;
                //            MouseMovement_DeadZoning(centered.X, centered.Y);
                //        }

                //        if (MouseReleaseTimer != null)
                //        {
                //            MouseReleaseTimer.Stop();
                //            MouseReleaseTimer = null;
                //        }
                //    };
                //}
            }
        }

        public void Start()
        {
            if (tMouseMovement != null)
            {
                return;
            }

            Cursor.Clip = new System.Drawing.Rectangle(0, 0, 1000, 1000); //Screen.FromHandle(_remapper.RemotePlayProcess.MainWindowHandle).Bounds;
            centered = new Point(Cursor.Clip.Width / 2, Cursor.Clip.Height / 2);

            CursorHook.ShowSystemCursor(false);

            tMouseMovement = new Thread(MouseMovementInput);
            tMouseMovement.SetApartmentState(ApartmentState.STA);
            tMouseMovement.IsBackground = true;
            tMouseMovement.Start();
        }

        public void Stop()
        {
            Cursor.Clip = new System.Drawing.Rectangle();
            CursorHook.ShowSystemCursor(true);

            if (tMouseMovement != null)
            {
                tMouseMovement.Abort();
                tMouseMovement = null;
            }

            if (MouseReleaseTimer != null)
            {
                MouseReleaseTimer.Stop();
                MouseReleaseTimer = null;
            }
        }

        public void MouseMovementInput()
        {
            while (true)
            {
                MouseMovement();//Cursor.Position.X, Cursor.Position.Y
                Thread.Sleep(Mouse_TickRate);
            }
        }

        private void MouseMovement()//int x, int y
        {
            //Debug.Print($"Cursor.Position   = {Cursor.Position.ToString()}");

            var mouseX = Cursor.Position.X;
            var mouseY = Cursor.Position.Y;
            Cursor.Position = centered;
            //CursorHook.SetCursorPosition(centered);

            var timeSinceLastPoll = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Restart();

            var changeX = Mouse_Invert_X ? centered.X - mouseX : mouseX - centered.X;
            var changeY = !Mouse_Invert_Y ? mouseY - centered.Y : centered.Y - mouseY;

            double sensitivityScaleX = Mouse_Sensitivity_X / 1000.0;
            double sensitivityScaleY = Mouse_Sensitivity_Y / 1000.0;

            double velocityX = changeX * sensitivityScaleX / timeSinceLastPoll;
            double velocityY = changeY * sensitivityScaleY / timeSinceLastPoll;

            short joyX = 0, joyY = 0;

            if (velocityX != 0 || velocityY != 0)
            {
                var mouseVectorLengthToReachMaxStickPosition = 5d;
                var mouseVector = new Vector(velocityX, velocityY);
                var percentMouseMagnitude = mouseVector.Length / mouseVectorLengthToReachMaxStickPosition;
                percentMouseMagnitude = Math.Min(percentMouseMagnitude, 1.0);

                mouseVector.Normalize();

                var remainingStickMagnitude = short.MaxValue - DeadZoneSize;
                var targetMagnitude = DeadZoneSize + remainingStickMagnitude * percentMouseMagnitude;
                mouseVector *= targetMagnitude;

                joyX = Convert.ToInt16(mouseVector.X);
                joyY = Convert.ToInt16(mouseVector.Y);
            }

            SetAxis(joyX, joyY);
        }
    }
}
