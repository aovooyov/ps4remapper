using System.Windows.Forms;

namespace PS4Remapper.Classes
{
    public class MapAction
    {
        public string Name { get; set; }
        public Keys Key { get; set; }
        public string Property { get; set; }
        public object Value { get; set; }

        public object NegativeValue
        {
            get
            {
                switch (Value)
                {
                    case bool _:
                        return false;
                }
                return Value;
            }
        }

        public MapAction()
        {

        }

        public MapAction(string name, Keys key, string property, object value)
        {
            Name = name;
            Key = key;
            Property = property;
            Value = value;
        }

        public MapAction Clone()
        {
            return new MapAction(Name, Key, Property, Value);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}