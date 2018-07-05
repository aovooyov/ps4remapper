using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PS4Remapper.FormsApp
{
    public partial class MouseForm : Form
    {
        public MouseForm()
        {
            InitializeComponent();

            Remapper.Instance.Mouse.OnMouseAxisChanged += OnMouseAxisChanged;

            //sensitivity.Value = Convert.ToDecimal(Remapper.Instance.Mouse.MouseSensitivity);
            //decayRate.Value = Convert.ToDecimal(Remapper.Instance.Mouse.MouseDecayRate);
            //decayThreshold.Value = Convert.ToDecimal(Remapper.Instance.Mouse.MouseDecayThreshold);
            //analogDeadzone.Value = Convert.ToDecimal(Remapper.Instance.Mouse.MouseAnalogDeadzone);
            //makeupSpeed.Value = Convert.ToDecimal(Remapper.Instance.Mouse.MouseMakeupSpeed);

            //sensitivity.ValueChanged += (sender, args) =>
            //{
            //    Remapper.Instance.Mouse.MouseSensitivity = Convert.ToDouble(sensitivity.Value);
            //};

            //decayRate.ValueChanged += (sender, args) =>
            //{
            //    Remapper.Instance.Mouse.MouseDecayRate = Convert.ToDouble(decayRate.Value);
            //};

            //decayThreshold.ValueChanged += (sender, args) =>
            //{
            //    Remapper.Instance.Mouse.MouseDecayThreshold = Convert.ToDouble(decayThreshold.Value);
            //};

            //analogDeadzone.ValueChanged += (sender, args) =>
            //{
            //    Remapper.Instance.Mouse.MouseAnalogDeadzone = Convert.ToDouble(analogDeadzone.Value);
            //};

            //makeupSpeed.ValueChanged += (sender, args) =>
            //{
            //    Remapper.Instance.Mouse.MouseMakeupSpeed = Convert.ToDouble(makeupSpeed.Value);
            //};

            //Remapper.Instance.Mouse.ShowCursorAndToolbar(true);
            //Remapper.Instance.DebugMouse(Process.GetCurrentProcess());

            //Debug.WriteLine($"Process ID {Remapper.Instance.RemotePlayProcess.Id}");
            //Debug.WriteLine($"Process Name {Remapper.Instance.RemotePlayProcess.ProcessName}");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Remapper.Instance.Mouse.OnMouseAxisChanged -= OnMouseAxisChanged;
            Remapper.Instance.Stop();
            base.OnClosing(e);
        }

        private void OnMouseAxisChanged(short x, short y)
        {
            //var rx = (byte)Math.Ceiling(x / (double)short.MaxValue * 255);
            //var ry = (byte)Math.Ceiling(y / (double)short.MaxValue * 255);

            //float fx = (2 * (rx / 255f)) - 1;
            //float fy = (2 * (ry / 255f)) - 1;
            //PointF point = new PointF(fx, -fy);


            //float fx = (2 * (x / 255f)) - 1;
            //float fy = (2 * (y / 255f)) - 1;
            //PointF point = new PointF(fx, -fy);

            //float fx = (2 * (x / short.MaxValue)) - 1;
            //float fy = (2 * (y / short.MaxValue)) - 1;

            var debug = new StringBuilder();
            debug.AppendLine($"x: {x} y: {y}");

            float fx = (x / 255f) + (255f / 2);
            float fy = (y / 255f) + (255f / 2);

            debug.AppendLine($"fx: {fx} fy: {fy}");

            //fx = (2 * (x / 255f)) - 1;
            //fy = (2 * (y / 255f)) - 1;

            debug.AppendLine($"px: {fx} py: {fy}");

            PointF point = new PointF(fx, -fy);
            Debug.Print(debug.ToString());

            try
            {
                BeginInvoke(new Action(() =>
                {
                    labelMouse.Text = debug.ToString();

                    axisDisplay.Value = point;
                    axisDisplay.Invalidate();
                }));
            }
            catch
            {

            }
        }

        private void axisDisplay_DoubleClick(object sender, EventArgs e)
        {
            if(Remapper.Instance.IsDebugMouse)
            {
                Cursor.Clip = new Rectangle();
                Remapper.Instance.Stop();
                return;
            }

            Cursor.Clip = RectangleToClient(Bounds);
            Remapper.Instance.DebugMouse(null);
        }
    }
}
