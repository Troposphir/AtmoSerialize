using System.Collections.Generic;

namespace AtmoSerialize.Internal.Serializables {
    internal class MapBody : IAtmoSerializable {
        public ICollection<string> References;
        public ICollection<MapItem> Content;

        public void Serialize(AtmoWriter writer) {
            writer.Write(References.Count);
            foreach (var reference in References) {
                writer.Write(reference);
            }

            foreach (var mapItem in Content) {
                writer.Write(mapItem);
            }
        }

        public void Deserialize(AtmoReader reader) {
            References = reader.ReadCollection(reader.ReadString);
            reader.Solver = new ReferenceSolver(References);

            Content = reader.ReadToEnd<MapItem>();
        }
    }
}
