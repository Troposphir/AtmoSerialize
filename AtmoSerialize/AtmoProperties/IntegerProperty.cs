using System.Globalization;

namespace AtmoSerialize {
    public class IntegerProperty : IAtmoProperty {
        public int Value { get; set; }

        public IntegerProperty(string value) {
            Value = int.Parse(value);
        }

        public string ToRaw() => Value.ToString(CultureInfo.InvariantCulture);
        
    }
}