namespace AtmoSerialize {
    public class GenericProperty : IAtmoProperty {
        public string Value { get; set; }

        public GenericProperty(string value) {
            Value = value;
        }

        public string ToRaw() => Value;
    }
}