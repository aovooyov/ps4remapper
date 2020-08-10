using PS4Remapper.Hooks;
using PS4Remapper.Hooks.EventArgs;
using PS4Remapper.Hooks.States;
using PS4RemotePlayInterceptor;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using Point = System.Drawing.Point;

namespace PS4Remapper
{
    public class MouseRemapper2
    {
        private readonly Remapper _remapper;

        public delegate void OnMouseAxisChangedDelegate(byte x, byte y);
        public OnMouseAxisChangedDelegate OnMouseAxisChanged { get; set; }

        private byte iMax = byte.MaxValue;
        private byte iMin = byte.MinValue;

        private static Stopwatch stopwatch = Stopwatch.StartNew();
        private static System.Timers.Timer MouseReleaseTimer { get; set; }

        public MouseRemapper2(Remapper remapper)
        {
            _remapper = remapper;
        }

        public double MouseSensitivity = 0.1;
        public double Mouse_Sensitivity_X = 1000.21299982071f;
        public double Mouse_Sensitivity_Y = 1000.21299982071f;
        public double Mouse_FinalMod = 100;
        public bool Mouse_Is_RightStick = true;
        public int Mouse_TickRate = 16;
        public bool Mouse_Invert_X = false;
        public bool Mouse_Invert_Y = false;
        public double DeadZoneSize = 10;

        public short Size = 500;
        public Rectangle Clip => Cursor.Clip;
        public Point Position => Cursor.Position;

        private Point _center => new Point(Size / 2, Size / 2);
        public Point Center => _center;

        //public bool Proccessed = false;

        public int LeftMouseMapping = 11; // R2
        public int RightMouseMapping = (int)Keys.LShiftKey;//10; // L2
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

            //if (mouseMoved)
            //{
            //    mouseMoved = false;
            //}
            //else
            //{
            //    CursorHook.SetCursorPosition(Center);
            //    SetAxis(0, 0);
            //}

            //if(mouseMoved)
            //{
            //    mouseMoved = true;
            //    CursorHook.SetCursorPosition(Center);
            //}
            //else
            //{
            //    SetAxis(0, 0);
            //}

            //if (mouseMoved)
            //{
            //    rightX = mouseAccelX;
            //    rightY = mouseAccelY;
            //    Cursor.Position = Center;
            //    mouseMoved = false;
            //}
            //else
            //{
            //    rightX /= 5;
            //    rightY /= 5;

            //    if (Math.Abs(rightX) <= 0.5f && Math.Abs(rightY) <= 0.5f)
            //    {
            //        rightX = 0;
            //        rightY = 0;
            //    }
            //}

            //var rx = Convert.ToByte(Math.Min(Math.Max(128 + rightX * 127, 0), 255));
            //var ry = Convert.ToByte(Math.Min(Math.Max(128 + rightY * 127, 0), 255));

            //_remapper.CurrentState.RX = rx;
            //_remapper.CurrentState.RY = ry;

            state = _remapper.CurrentState;
        }

        private void SetAxis(byte axisX, byte axisY)
        {
#if (DEBUG)
            //Debug.Print($"Mouse (X, Y) = ({x}, {y})");
#endif

            if (_remapper.CurrentState != null)
            {
                //var rx = (axisX / 255f) + (255f / 2);
                //var ry = (axisY / 255f) + (255f / 2);

                _remapper.CurrentState.RX = axisX;
                _remapper.CurrentState.RY = axisY;
            }

            OnMouseAxisChanged?.Invoke(axisX, axisY);
        }

        private MouseHook.POINT lastMouse;
        private double lastMouseTime = 0d;

        private bool mouseMoved = false;
        double mouseAccelX, mouseAccelY, mouseVelX, mouseVelY;
        bool kicked, decayKicked;
        double leftX = 1, leftY = 1, rightX = 1, rightY = 1;

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
            else if (e.MouseState == MouseState.Move)
            {
                var mouseX = e.MouseData.Point.X - 62;
                var mouseY = e.MouseData.Point.Y - 62;

                //Cursor.Position = Center;
                e.Handled = true;
                mouseMoved = true;

                var timeSinceLastPoll = stopwatch.Elapsed.TotalMilliseconds;
                stopwatch.Restart();

                double changeX = Mouse_Invert_X ? Center.X - mouseX : mouseX - Center.X;
                double changeY = Mouse_Invert_Y ? Center.Y - mouseY : mouseY - Center.Y;

                //double deadZone = 0.2;
                //double distance = Math.Sqrt(Math.Pow(changeX, 2) + Math.Pow(changeY, 2));
                //double directionalDeadZone = Math.Abs(deadZone * (changeX / distance));
                //double multiplier = 1 / (1 - directionalDeadZone);
                
                double sensitivityScaleX = MouseSensitivity; //MouseSensitivity;//Mouse_Sensitivity_X / 1000.0;
                double sensitivityScaleY = MouseSensitivity; //MouseSensitivity;//Mouse_Sensitivity_Y / 1000.0;

                float velocityX = Convert.ToSingle(changeX * sensitivityScaleX / timeSinceLastPoll);
                float velocityY = Convert.ToSingle(changeY * sensitivityScaleY / timeSinceLastPoll);

                //double scaleX = (1 / 250d) * velocityX;
                //double scaleY = (1 / 250d) * velocityY;

                var mouseVector = new Vector(velocityX, velocityY);

                double magnitude = Math.Sqrt(Math.Pow(mouseVector.X, 2) + Math.Pow(mouseVector.Y, 2));

                if (magnitude < DeadZoneSize)
                    mouseVector = new Vector(0, 0);
                //else
                //    mouseVector *= ((magnitude - DeadZoneSize) / (1 - DeadZoneSize));

                //var mouseVectorLengthToReachMaxStickPosition = 5d;
                //var percentMouseMagnitude = mouseVector.Length / mouseVectorLengthToReachMaxStickPosition;
                //percentMouseMagnitude = Math.Min(percentMouseMagnitude, 1.0);

                //mouseVector.Normalize();

                //var remainingStickMagnitude = byte.MaxValue - DeadZoneSize;
                //var targetMagnitude = DeadZoneSize + remainingStickMagnitude * percentMouseMagnitude;
                //mouseVector *= targetMagnitude;

                var rx = (mouseVector.X / 255f) + (255f / 2);
                var ry = (mouseVector.Y / 255f) + (255f / 2);

                Debug.WriteLine($"vx, vy: {velocityX} {velocityY}");
                //Debug.WriteLine($"sx, sy: {scaleX} {scaleY}");
                Debug.WriteLine($"vx, vy: {mouseVector.X} {mouseVector.Y}");
                Debug.WriteLine($"rx, ry: {rx} {ry}");
                Debug.WriteLine("---");

                SetAxis((byte)rx, (byte)ry);
                //return;

                //var timeSinceLastPoll = stopwatch.Elapsed.TotalMilliseconds;
                //stopwatch.Restart();

                //double changeX = Mouse_Invert_X ? Center.X - mouseX : mouseX - Center.X;
                //double changeY = Mouse_Invert_Y ? Center.Y - mouseY : mouseY - Center.Y;

                //var d = Math.Sqrt(Math.Pow(changeX, 2) + Math.Pow(changeY, 2));

                //Debug.WriteLine($"{changeX} {changeY}");
                //Debug.WriteLine($"d: {d} {DeadZoneSize}");

                //if (d < DeadZoneSize)
                //{
                //    var dx = DeadZoneSize - changeX;
                //    var dy = DeadZoneSize - changeY;

                //    changeX = changeX > 0 ? changeX + dx : changeX - dx;
                //    changeY = changeY > 0 ? changeY + dy : changeY - dy;

                //    Debug.WriteLine($"d: {changeX} {changeY}");
                //}

                //double sensitivityScaleX = MouseSensitivity;//Mouse_Sensitivity_X / 1000.0;
                //double sensitivityScaleY = MouseSensitivity;//Mouse_Sensitivity_Y / 1000.0;

                //double velocityX = changeX * sensitivityScaleX / timeSinceLastPoll;
                //double velocityY = changeY * sensitivityScaleY / timeSinceLastPoll;

                //Debug.WriteLine("---");

                //if (velocityX != 0 || velocityY != 0)
                //{
                //    mouseMoved = true;
                //    short joyX = 0, joyY = 0;

                //    var mouseVectorLengthToReachMaxStickPosition = 5d;
                //    var mouseVector = new Vector(velocityX, velocityY);

                //    var percentMouseMagnitude = mouseVector.Length / mouseVectorLengthToReachMaxStickPosition;
                //    percentMouseMagnitude = Math.Min(percentMouseMagnitude, 1.0);

                //    mouseVector.Normalize();

                //    var remainingStickMagnitude = short.MaxValue - DeadZoneSize;
                //    var targetMagnitude = DeadZoneSize + remainingStickMagnitude * percentMouseMagnitude;
                //    mouseVector *= targetMagnitude;

                //    joyX = Convert.ToInt16(mouseVector.X);
                //    joyY = Convert.ToInt16(mouseVector.Y);
                //    SetAxis(joyX, joyY);
                //    return;
                //}

                //mouseMoved = false;
                //SetAxis(0, 0);
                //var mouse = e.MouseData.Point;
                //mouseMoved = true;
                //MouseMovement();

                //var curtime = stopwatch.Elapsed.TotalMilliseconds;
                //stopwatch.Restart();

                //var time = curtime - lastMouseTime;
                //var velX = (mouse.X - lastMouse.X) / time;
                //var velY = (mouse.Y - lastMouse.Y) / time;
                //mouseAccelX = (velX - mouseVelX) / time;
                //mouseAccelY = (velY - mouseVelY) / time;

                //mouseVelX = velX;
                //mouseVelY = velY;
                //lastMouseTime = curtime;
                //lastMouse = mouse;
                //mouseMoved = true;

                //if (_remapper.IsDebugMouse)
                //{
                //    //Debug.WriteLine($"ax {mouseAccelX} ay {mouseAccelY}");

                //    rightX = -mouseAccelX;
                //    rightY = mouseAccelY;

                //    Debug.WriteLine($"vx {128 + rightX * 127} vy {128 + rightY * 127}");

                //    var rx = Convert.ToByte(Math.Min(Math.Max(128 + rightX * 127, 0), 255));
                //    var ry = Convert.ToByte(Math.Min(Math.Max(128 + rightY * 127, 0), 255));
                //    Debug.WriteLine($"rx {rx} {ry}");
                //}
            }
        }

        private Thread tMouseMovement;

        public void Start()
        {
            if (tMouseMovement != null)
            {
                return;
            }

            Cursor.Clip = new Rectangle(0, 0, Size, Size);
            CursorHook.SetCursorPosition(Center);
            lastMouse = new MouseHook.POINT
            {
                X = Center.X,
                Y = Center.Y
            };
            
            tMouseMovement = new Thread(MouseMovementInput);
            tMouseMovement.SetApartmentState(ApartmentState.STA);
            tMouseMovement.IsBackground = true;
            tMouseMovement.Start();
            CursorHook.ShowSystemCursor(false);
        }

        public void Stop()
        {
            Cursor.Clip = new Rectangle();
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
                //MouseMovement();
                if (mouseMoved)
                {
                    mouseMoved = false;
                }
                else
                {
                    CursorHook.SetCursorPosition(Center);
                    SetAxis(127, 127);
                }

                Thread.Sleep(Mouse_TickRate);
            }
        }

        private void MouseMovement()
        {
            var mouseX = Cursor.Position.X;
            var mouseY = Cursor.Position.Y;

            Cursor.Position = Center;

            var timeSinceLastPoll = stopwatch.Elapsed.TotalMilliseconds;
            stopwatch.Restart();

            Debug.WriteLine($"x: {mouseX} y: {mouseY}");

            double changeX = Mouse_Invert_X ? Center.X - mouseX : mouseX - Center.X;
            double changeY = Mouse_Invert_Y ? Center.Y - mouseY : mouseY - Center.Y;

            if ((Math.Sqrt(Math.Pow(changeX, 2) + Math.Pow(changeY, 2))) > DeadZoneSize)
            //if (velocityX != 0 || velocityY != 0)
            {
                //double sensitivityScaleX = MouseSensitivity;//Mouse_Sensitivity_X / 1000.0;
                //double sensitivityScaleY = MouseSensitivity;//Mouse_Sensitivity_Y / 1000.0;

                //double velocityX = changeX * sensitivityScaleX / timeSinceLastPoll;
                //double velocityY = changeY * sensitivityScaleY / timeSinceLastPoll;

                //short joyX = 0, joyY = 0;

                //var mouseVectorLengthToReachMaxStickPosition = 5d;
                //var mouseVector = new Vector(velocityX, velocityY);

                //var percentMouseMagnitude = mouseVector.Length / mouseVectorLengthToReachMaxStickPosition;
                //percentMouseMagnitude = Math.Min(percentMouseMagnitude, 1.0);

                //mouseVector.Normalize();

                //var remainingStickMagnitude = short.MaxValue - DeadZoneSize;
                //var targetMagnitude = DeadZoneSize + remainingStickMagnitude * percentMouseMagnitude;
                //mouseVector *= targetMagnitude;

                //joyX = Convert.ToInt16(mouseVector.X);
                //joyY = Convert.ToInt16(mouseVector.Y);
                //SetAxis(joyX, joyY);

                changeX = changeX + (changeX / MouseSensitivity);
                changeY = changeY + (changeY / MouseSensitivity);

                var jox = (short)Math.Ceiling(changeX / (double)Size * short.MaxValue);
                var joy = (short)Math.Ceiling(changeY / (double)Size * short.MaxValue);

                //SetAxis(jox, joy);
                return;
            }

            SetAxis(0, 0);
        }
    }
}
