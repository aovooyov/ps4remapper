using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PS4Remapper
{
    public class Joystick
    {
        // Properties

        // Physical axis positions
        public double X { get; set; }
        public double Y { get; set; }
        // Virtual axis positions, with all modifiers applied (like deadzone, sensitivity, etc.)
        public double ComputedX { get => ComputeX(); }
        public double ComputedY { get => ComputeY(); }
        // Joystick modifiers, which influence the computed axis positions 
        public double DeadZone { get; set; }
        public double Saturation { get; set; }
        public double Sensitivity { get; set; }
        public double Range { get; set; }
        public bool InvertX { get; set; }
        public bool InvertY { get; set; }
        // Other properties
        public double Distance
        {
            get => CoerceValue(Math.Sqrt((ComputedX * ComputedX) + (ComputedY * ComputedY)), 0d, 1d);
        }
        public double Direction { get => ComputeDirection(); }


        // Methods

        private static double CoerceValue(double value, double minValue, double maxValue)
        {
            return (value < minValue) ? minValue : ((value > maxValue) ? maxValue : value);
        }


        protected virtual double ComputeX()
        {
            double value = X;
            value = CalculateDeadZoneAndSaturation(value, DeadZone, Saturation);
            value = CalculateSensitivity(value, Sensitivity);
            value = CalculateRange(value, Range);
            if (InvertX) value = -value;
            return CoerceValue(value, -1d, 1d);
        }


        protected virtual double ComputeY()
        {
            double value = Y;
            value = CalculateDeadZoneAndSaturation(value, DeadZone, Saturation);
            value = CalculateSensitivity(value, Sensitivity);
            value = CalculateRange(value, Range);
            if (InvertY) value = -value;
            return CoerceValue(value, -1d, 1d);
        }


        /// <sumary>Gets the joystick's direction (from 0 to 1).</summary>
        private double ComputeDirection()
        {
            double x = ComputedX;
            double y = ComputedY;
            if (x != 0d && y != 0d)
            {
                double angle = Math.Atan2(x, y) / (Math.PI * 2d);
                if (angle < 0d) angle += 1d;
                return CoerceValue(angle, 0d, 1d);
            }
            return 0d;
        }


        private double CalculateDeadZoneAndSaturation(double value, double deadZone, double saturation)
        {
            deadZone = CoerceValue(deadZone, 0.0d, 1.0d);
            saturation = CoerceValue(saturation, 0.0d, 1.0d);

            if ((deadZone > 0) | (saturation < 1))
            {
                double distance = CoerceValue(Math.Sqrt((X * X) + (Y * Y)), 0.0d, 1.0d);
                double directionalDeadZone = Math.Abs(deadZone * (value / distance));
                double directionalSaturation = 1 - Math.Abs((1 - saturation) * (value / distance));

                double edgeSpace = (1 - directionalSaturation) + directionalDeadZone;
                double multiplier = 1 / (1 - edgeSpace);
                if (multiplier != 0)
                {
                    if (value > 0)
                    {
                        value = (value - directionalDeadZone) * multiplier;
                        value = CoerceValue(value, 0, 1);
                    }
                    else
                    {
                        value = -((Math.Abs(value) - directionalDeadZone) * multiplier);
                        value = CoerceValue(value, -1, 0);
                    }
                }
                else
                {
                    if (value > 0)
                        value = CoerceValue(value, directionalDeadZone, directionalSaturation);
                    else
                        value = CoerceValue(value, -directionalSaturation, -directionalDeadZone);
                }
                value = CoerceValue(value, -1, 1);
            }

            return value;
        }


        private double CalculateSensitivity(double value, double sensitivity)
        {
            value = CoerceValue(value, -1d, 1d);

            if (sensitivity != 0)
            {
                double axisLevel = value;
                axisLevel = axisLevel + ((axisLevel - Math.Sin(axisLevel * (Math.PI / 2))) * (sensitivity * 2));
                if ((value < 0) & (axisLevel > 0))
                    axisLevel = 0;
                if ((value > 0) & (axisLevel < 0))
                    axisLevel = 0;
                value = CoerceValue(axisLevel, -1d, 1d);
            }

            return value;
        }


        private double CalculateRange(double value, double range)
        {
            value = CoerceValue(value, -1.0d, 1.0d);
            range = CoerceValue(range, 0.0d, 1.0d);
            if (range < 1)
            {
                double distance = CoerceValue(Math.Sqrt((X * X) + (Y * Y)), 0d, 1d);
                double directionalRange = 1 - Math.Abs((1 - range) * (value / distance));
                value *= CoerceValue(directionalRange, 0d, 1d);
            }
            return value;
        }
    }
}
