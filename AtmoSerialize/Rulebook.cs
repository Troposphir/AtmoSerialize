using System.Collections.Generic;
using System.IO;

namespace AtmoSerialize {
    public class Rulebook : IAtmoSerializable {
        public const string PropertySeparator = "=";

        public IDictionary<string, string> Properties { get; }

        public Rulebook() {
            Properties = new Dictionary<string, string>();
        }

        void IAtmoSerializable.Serialize(BinaryWriter writer, Map map) {
            writer.Write("Rulebook");
            writer.Write((byte) Properties.Count);
            foreach (var kvp in Properties) {
                writer.Write(kvp.Key + PropertySeparator + kvp.Value);
            }
        }
    }
}