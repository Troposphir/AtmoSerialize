using System.IO;

namespace AtmoSerialize {
    interface IAtmoSerializable {
        void Serialize(BinaryWriter writer, Map map);
    }
}
