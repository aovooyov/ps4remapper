using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PS4Remapper.FormsApp
{
    public partial class MainForm : Form
    {
        private Remapper _remapper;

        public MainForm()
        {
            InitializeComponent();

            _remapper = new Remapper();
            _remapper.Mouse.OnMouseAxisChanged += OnMouseAxisChanged;
        }

        private void buttonInject_Click(object sender, EventArgs e)
        {

        }

        public void SetAxisDisplay(PointF point)
        {
            BeginInvoke(new Action(() =>
            {
                axisDisplay.Value = point;
                axisDisplay.Invalidate();
            }));
        }

        private void OnMouseAxisChanged(byte x, byte y)
        {
            float fx = (2 * (x / 255f)) - 1;
            float fy = (2 * (y / 255f)) - 1;
            SetAxisDisplay(new PointF(fx, -fy));
        }
    }
}
