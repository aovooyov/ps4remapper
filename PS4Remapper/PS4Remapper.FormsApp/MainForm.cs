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
        private readonly Remapper _remapper;
        private bool IsTest = false;

        public MainForm()
        {
            InitializeComponent();

            _remapper = new Remapper();
            _remapper.Mouse.OnMouseAxisChanged += OnMouseAxisChanged;
            _remapper.Keyboard.OnKeyChanged += OnKeyChanged;
        }
        
        private void buttonInject_Click(object sender, EventArgs e)
        {
            if (_remapper.IsInjected)
            {
                _remapper.Stop();
            }
            else
            {
                _remapper.Inject();
            }

            buttonInject.Text = _remapper.IsInjected ? "Stop" : "Inject";
        }

        private void OnMouseAxisChanged(byte x, byte y)
        {
            float fx = (2 * (x / 255f)) - 1;
            float fy = (2 * (y / 255f)) - 1;
            PointF point = new PointF(fx, -fy);

            BeginInvoke(new Action(() =>
            {
                labelMouse.Text = $"Mouse: {x} {y}";

                axisDisplay.Value = point;
                axisDisplay.Invalidate();
            }));
        }

        private void OnKeyChanged(string name)
        {
            BeginInvoke(new Action(() =>
            {
                labelKey.Text = $"Key: {name}";
            }));
        }

        private void buttonTest_Click(object sender, EventArgs e)
        {
            //IsTest = !IsTest;

            //Cursor.Clip = IsTest ? new Rectangle(0, 0, 500, 500) : Screen.PrimaryScreen.Bounds;

            //buttonTest.Text = IsTest ? "Stop" : "Test";
        }
    }
}
