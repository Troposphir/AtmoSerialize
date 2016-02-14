using System.Threading;

namespace AtmoSerialize {
    public class BooleanProperty : IAtmoProperty {
        public bool Value { get; set; }

        public BooleanProperty(string raw) {
            Value = bool.Parse(raw);
        }

        public string ToRaw() {
            return Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(Value.ToString());
        }
    }
}
