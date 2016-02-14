using System;
using System.Globalization;

namespace AtmoSerialize {
    public class EnumProperty<T> : IAtmoProperty where T : struct, IConvertible {
        public T Value { get; set; }

        public EnumProperty(string raw) {
            if (!typeof(T).IsEnum) {
                throw new ArgumentException($"{nameof(T)} must be an enum");
            }
            T output;
            if (!Enum.TryParse(raw, out output)) {
                throw new ArgumentOutOfRangeException(nameof(raw));
            }
        }

        public string ToRaw() => Value.ToString(CultureInfo.InvariantCulture);
    }
}
