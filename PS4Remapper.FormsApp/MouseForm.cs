using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace PS4Remapper.FormsApp
{
    public partial class MouseForm : Form
    {
        public MouseForm()
        {
            InitializeComponent();

            Remapper.Instance.Mouse.OnMouseAxisChanged += OnMouseAxisChanged;

            sensitivity.Value = Convert.ToDecimal(Remapper.Instance.Mouse.MouseSensitivity);
            decayRate.Value = Convert.ToDecimal(Remapper.Instance.Mouse.MouseDecayRate);
            decayThreshold.Value = Convert.ToDecimal(Remapper.Instance.Mouse.MouseDecayThreshold);
            analogDeadzone.Value = Convert.ToDecimal(Remapper.Instance.Mouse.MouseAnalogDeadzone);
            makeupSpeed.Value = Convert.ToDecimal(Remapper.Instance.Mouse.MouseMakeupSpeed);

            sensitivity.ValueChanged += (sender, args) =>
            {
                Remapper.Instance.Mouse.MouseSensitivity = Convert.ToDouble(sensitivity.Value);
            };

            decayRate.ValueChanged += (sender, args) =>
            {
                Remapper.Instance.Mouse.MouseDecayRate = Convert.ToDouble(decayRate.Value);
            };

            decayThreshold.ValueChanged += (sender, args) =>
            {
                Remapper.Instance.Mouse.MouseDecayThreshold = Convert.ToDouble(decayThreshold.Value);
            };

            analogDeadzone.ValueChanged += (sender, args) =>
            {
                Remapper.Instance.Mouse.MouseAnalogDeadzone = Convert.ToDouble(analogDeadzone.Value);
            };

            makeupSpeed.ValueChanged += (sender, args) =>
            {
                Remapper.Instance.Mouse.MouseMakeupSpeed = Convert.ToDouble(makeupSpeed.Value);
            };

            Remapper.Instance.Mouse.ShowCursorAndToolbar(true);
            Remapper.Instance.DebugMouse(Process.GetCurrentProcess());

            Debug.WriteLine($"Process ID {Remapper.Instance.RemotePlayProcess.Id}");
            Debug.WriteLine($"Process Name {Remapper.Instance.RemotePlayProcess.ProcessName}");
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Remapper.Instance.Stop();
            base.OnClosing(e);
        }

        private void OnMouseAxisChanged(byte x, byte y)
        {
            float fx = (2 * (x / 255f)) - 1;
            float fy = (2 * (y / 255f)) - 1;
            PointF point = new PointF(fx, -fy);

            BeginInvoke(new Action(() =>
            {
                axisDisplay.Value = point;
                axisDisplay.Invalidate();
            }));
        }


    }
}
