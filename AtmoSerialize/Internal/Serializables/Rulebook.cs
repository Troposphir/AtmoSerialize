using System.Collections.Generic;

namespace AtmoSerialize.Internal.Serializables {
    internal class Rulebook : IAtmoSerializable {
        private const string PropertySeparator = "=";

        public Dictionary<string, string> Rules; 

        public void Serialize(AtmoWriter writer) {
            writer.Write((byte) Rules.Count);
            foreach (var kvp in Rules) {
                writer.Write(string.Concat(kvp.Key, PropertySeparator, kvp.Value));
            }
        }

        public void Deserialize(AtmoReader reader) {
            var count = reader.ReadByte();
            Rules = new Dictionary<string, string>(count);
            for (var i = 0; i < count; i++) {
                var ruleParts = reader.ReadString().Split(PropertySeparator.ToCharArray(), 2);
                Rules[ruleParts[0]] = ruleParts[1];
            }
        }
    }
}