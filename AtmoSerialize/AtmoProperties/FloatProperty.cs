using System.Globalization;

namespace AtmoSerialize {
    public class FloatProperty : IAtmoProperty {
        public float Value { get; set; }

        public FloatProperty(string value) {
            Value = float.Parse(value);
        }

        public string ToRaw() => Value.ToString(CultureInfo.InvariantCulture);
    }
}