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
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            Remapper.Instance.Mouse.OnMouseAxisChanged -= OnMouseAxisChanged;
            Remapper.Instance.Stop();
            base.OnClosing(e);
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

            debug.AppendLine($"fx: {point.X} fy: {point.Y}"); try
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
                Remapper.Instance.Stop();
                return;
            }

            Remapper.Instance.DebugMouse(null);
        }

        private void MouseForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                this.Close();
            }
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
    }
}
