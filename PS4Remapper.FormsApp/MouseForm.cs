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

        private void OnMouseAxisChanged(short x, short y)
        {
            var debug = new StringBuilder();
            debug.AppendLine($"x: {x} y: {y}");

            float fx = (x / 255f) + (255f / 2);
            float fy = (y / 255f) + (255f / 2);

            debug.AppendLine($"fx: {fx} fy: {fy}");
            debug.AppendLine($"cx: {Remapper.Instance.Mouse.Center.X} cy: {Remapper.Instance.Mouse.Center.Y}");
            debug.AppendLine($"sx: {Remapper.Instance.Mouse.Clip.X} sy: {Remapper.Instance.Mouse.Clip.Y}");

            try
            {
                BeginInvoke(new Action(() =>
                {
                    //labelMouse.Text = debug.ToString();

                    float px = (2 * (fx / 255f)) - 1;
                    float py = (2 * (fy / 255f)) - 1;
                    PointF point = new PointF(px, -py);

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
    }
}
