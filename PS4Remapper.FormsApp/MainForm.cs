﻿using System;
using System.Drawing;
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

        private void buttonDebugKeyboard_Click(object sender, EventArgs e)
        {
            if (Remapper.Instance.IsInjected)
            {
                Remapper.Instance.Stop();
            }
            else
            {
                Remapper.Instance.DebugKeyboard();
            }

            buttonDebugKeyboard.Text = Remapper.Instance.IsInjected ? "Stop Debug Keyboard" : "Debug Keyboard";
        }
        
        private void buttonDebugMouse_Click(object sender, EventArgs e)
        {
            //if (Remapper.Instance.IsInjected)
            //{
            //    Remapper.Instance.Stop();
            //}
            //else
            //{
            //    Remapper.Instance.DebugMouse();
            //}

            //buttonDebugMouse.Text = Remapper.Instance.IsInjected ? "Stop Debug Mouse" : "Debug Mouse";

            var form = new MouseForm();
            form.Show();
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
    }
}
