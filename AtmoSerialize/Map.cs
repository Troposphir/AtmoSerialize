using System.Collections.Generic;
using System.IO;

namespace AtmoSerialize {
    public class Map {
        public int Version { get; }
        public IList<string> Resources { get; }  
        public IList<MapItem> Items { get; }
        public Rulebook Rulebook { get; internal set; }

        public Map(int version = 5) {
            Version = version;
            Resources = new List<string>();
            Items = new List<MapItem>();
        }

        internal void SerializeContents(BinaryWriter writer) {
            writer.Write(Resources.Count);
            foreach (var resource in Resources) {
                writer.Write(resource);
            }
            foreach (var item in Items) {
                ((IAtmoSerializable) item).Serialize(writer, this);
            }
            if (Rulebook != null) {
                ((IAtmoSerializable)Rulebook).Serialize(writer, this);
            }
            writer.Write("EOF");
            writer.Write((byte) 0);
        }
    }

}
