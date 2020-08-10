using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace PS4Remapper.FormsApp
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();

            Remapper.Instance.Mouse.OnMouseAxisChanged += OnMouseAxisChanged;
            Remapper.Instance.Keyboard.OnKeyChanged += OnKeyChanged;

            sensitivity.Value = Convert.ToDecimal(Remapper.Instance.Mouse.MouseSensitivity);
            deadZone.Value = Convert.ToDecimal(Remapper.Instance.Mouse.DeadZoneSize);
            tickRate.Value = Convert.ToDecimal(Remapper.Instance.Mouse.Mouse_TickRate);

            sensitivity.ValueChanged += (sender, args) =>
            {
                Remapper.Instance.Mouse.MouseSensitivity = Convert.ToDouble(sensitivity.Value);
            };

            deadZone.ValueChanged += (sender, args) =>
            {
                Remapper.Instance.Mouse.DeadZoneSize = Convert.ToDouble(deadZone.Value);
            };

            tickRate.ValueChanged += (sender, args) =>
            {
                Remapper.Instance.Mouse.Mouse_TickRate = Convert.ToInt32(tickRate.Value);
            };
        }

        private void buttonInject_Click(object sender, EventArgs e)
        {
            if (Remapper.Instance.IsInjected)
            {
                Remapper.Instance.Stop();
            }
            else
            {
                Remapper.Instance.Inject();
            }

            buttonInject.Text = Remapper.Instance.IsInjected ? "Stop" : "Inject";
        }

        private void buttonDebugMouse_Click(object sender, EventArgs e)
        {
            var form = new MouseForm();
            form.Show();
        }

        private void OnMouseAxisChanged(byte x, byte y)
        {
            var debug = new StringBuilder();
            debug.AppendLine($"x: {x} y: {y}");

            //float fx = (x / 255f) + (255f / 2);
            //float fy = (y / 255f) + (255f / 2);
            float fx = (2 * (x / 255f)) - 1;
            float fy = (2 * (y / 255f)) - 1;
            var point = new PointF(fx, -fy);

            debug.AppendLine($"fx: {point.X} fy: {point.Y}");

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

        private void OnKeyChanged(string name)
        {
            BeginInvoke(new Action(() =>
            {
                labelKey.Text = $"Key: {name}";
            }));
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (Form.ModifierKeys == Keys.None && keyData == Keys.Escape)
            {
                this.Close();
                return true;
            }
            return base.ProcessDialogKey(keyData);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Remapper.Instance.Stop();
            base.OnClosing(e);
        }
    }
}
