using System.Collections.Generic;
using System.Linq;

namespace AtmoSerialize {
    public class Item {
        public string ResourceName { get; }
        public Vector Position { get; set; } = Vector.Zero;
        public Vector Rotation { get; set; }
        public Quaternion RotationQuaternion => Quaternion.FromEuler(Rotation);
        public float Scale { get; set; } = 0;
        public Dictionary<string, IAtmoProperty> Properties;

        public Item(string resourceName) {
            ResourceName = resourceName;
            Properties = new Dictionary<string, IAtmoProperty>();
        }

        public T GetProperty<T>(string key) where T : IAtmoProperty
            => (T) Properties[key];

        public IEnumerable<T> GetProperties<T>() where T : IAtmoProperty
            => Properties.Values.OfType<T>();
    }
}