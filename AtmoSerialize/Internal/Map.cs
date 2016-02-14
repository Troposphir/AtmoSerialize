using System.Collections.Generic;
using AtmoSerialize.Internal.Serializables;

namespace AtmoSerialize.Internal {
    internal class Map {
        public int Version { get; }
        public bool Compressed { get; }
        public Serializables.Rulebook Rulebook;
        public List<MapItem> Items { get; }

        public Map(int version, bool compressed = true) {
            Compressed = compressed;
            Version = version;
            Items = new List<MapItem>();
        }
    }
}