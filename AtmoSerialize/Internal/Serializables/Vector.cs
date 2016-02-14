namespace AtmoSerialize.Internal.Serializables {
    internal class FloatVector : IAtmoSerializable {
        public float X, Y, Z;

        public void Serialize(AtmoWriter writer) {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        public void Deserialize(AtmoReader reader) {
            X = reader.ReadSingle();
            Y = reader.ReadSingle();
            Z = reader.ReadSingle();
        }
    }

    internal class ByteVector : IAtmoSerializable {
        public byte X, Y, Z;

        public void Serialize(AtmoWriter writer) {
            writer.Write(X);
            writer.Write(Y);
            writer.Write(Z);
        }

        public void Deserialize(AtmoReader reader) {
            X = reader.ReadByte();
            Y = reader.ReadByte();
            Z = reader.ReadByte();
        }
    }
}
