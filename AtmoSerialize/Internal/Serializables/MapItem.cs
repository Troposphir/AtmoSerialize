using System.Collections.Generic;
using System.IO;

namespace AtmoSerialize.Internal.Serializables {
    internal class MapItem : IAtmoSerializable {
        public string ResourceName;
        public Rulebook Rulebook;
        public FloatVector Position;
        public ByteVector Rotation;
        public float Scale;
        public Dictionary<string, string> Properties;

        public void Serialize(AtmoWriter writer) {
            writer.Write(ResourceName);
            if (Rulebook != null) {
                writer.Write(Rulebook);
                return;
            }

            writer.Write(Position);
            writer.Write(Rotation);
            writer.Write(Scale);

            writer.Write((byte)Properties.Count);
            foreach (var kvp in Properties) {
                writer.Write(writer.Solver?.Reference(kvp.Key));
                writer.Write(kvp.Value);
            }
        }

        public void Deserialize(AtmoReader reader) {
            ResourceName = reader.Solver.Solve(reader.ReadString());
            switch (ResourceName) {
                case "EOF":
                    throw new EndOfStreamException("Found EOF item");
                case "Rulebook":
                    Rulebook = reader.Read<Rulebook>();
                    return;
            }
            Position = reader.Read<FloatVector>();
            Rotation = reader.Read<ByteVector>();
            Scale = reader.ReadSingle();

            var propCount = reader.ReadByte();
            Properties = new Dictionary<string, string>(propCount);
            for (var i = 0; i < propCount; i++) {
                var key = reader.Solver.Solve(reader.ReadString());
                var value = reader.ReadString();
                Properties[key] = value;
            }
        }
    }
}