namespace AtmoSerialize.Internal.Serializables {
    internal class MapHeader : IAtmoSerializable {
        public string Format;
        public string Version;
        public bool Compressed;

        public void Serialize(AtmoWriter writer) {
            writer.Write(Format);
            writer.Write(Version);
            writer.Write(Compressed);
        }

        public void Deserialize(AtmoReader reader) {
            Format = reader.ReadString();
            Version = reader.ReadString();
            Compressed = reader.ReadBoolean();
        }
    }
}
