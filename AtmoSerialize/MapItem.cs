using System.Collections.Generic;
using System.IO;

namespace AtmoSerialize {
    public class MapItem : IAtmoSerializable {
        public string ResourceName { get; }
        public Vector Position { get; }
        public Vector Rotation { get; }
        public float Scale { get;}
        public Dictionary<string, string> Properties { get; }
        public string SingleProperty { get; internal set; }

        public MapItem(string resourceName, Vector position, Vector rotation, float scale) {
            ResourceName = resourceName;
            Position = position;
            Rotation = rotation;
            Scale = scale;
            Properties = new Dictionary<string, string>();
        }

        void IAtmoSerializable.Serialize(BinaryWriter writer, Map map) {
            var index = map.Resources.IndexOf(ResourceName);
            var name = index >= 0
                           ? index.ToString()
                           : ResourceName;
            writer.Write(name);
            writer.Write(Position.X);
            writer.Write(Position.Y);
            writer.Write(Position.Z);
            writer.Write((byte) Rotation.X);
            writer.Write((byte) Rotation.Y);
            writer.Write((byte) Rotation.Z);
            var scale = map.Version < 2
                            ? Scale * 10
                            : Scale;
            if (map.Version < 2) {
                writer.Write((byte) scale);
            } else {
                writer.Write(scale);
            }
            writer.Write((byte) Properties.Count);
            foreach (var kvp in Properties) {
                writer.Write(kvp.Key);
                writer.Write(kvp.Value);
            }
        }
    }
}