namespace AtmoSerialize.Internal {
    internal interface IAtmoSerializable {
        void Serialize(AtmoWriter writer);
        void Deserialize(AtmoReader reader);
    }
}
