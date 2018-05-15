using System;
using System.Reflection;
using System.Text;
using PS4Remapper.Hooks.EventArgs;

namespace PS4Remapper.Classes
{
    public class MouseStroke
    {
        public MouseHookEventArgs RawData { get; set; }
        public DateTime Timestamp { get; set; }
        public bool DidMoved { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public double VelocityX { get; set; }
        public double VelocityY { get; set; }

        private PropertyInfo[] _PropertyInfos = null;
        public override string ToString()
        {
            if (_PropertyInfos == null)
                _PropertyInfos = GetType().GetProperties();

            var sb = new StringBuilder();

            foreach (var info in _PropertyInfos)
            {
                var value = info.GetValue(this, null) ?? "(null)";
                sb.AppendLine(info.Name + ": " + value.ToString());
            }

            return sb.ToString();
        }
    }
}