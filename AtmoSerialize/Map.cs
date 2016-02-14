using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace AtmoSerialize {
    public class Map : ICollection<Item> {
        public int Version { get; }
        public Rulebook Rulebook { get; set; }
        private readonly List<Item> _items;

        public Map(int version = 5) {
            Version = version;
            _items = new List<Item>();
        }

        public IEnumerable<Item> AtCell(Vector cellPosition) => _items
            .Where(i => WorldToCell(i.Position) == cellPosition);

        public static Vector CellToWorld(Vector cellPosition) => cellPosition * Constants.CellSize;
        public static Vector WorldToCell(Vector worldPosition) => Vector.Floor(worldPosition / Constants.CellSize);

        public IEnumerator<Item> GetEnumerator() => _items.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
        public void Add(Item item) => _items.Add(item);
        public void Clear() => _items.Clear();
        public bool Contains(Item item) => _items.Contains(item);
        public void CopyTo(Item[] array, int arrayIndex) => _items.CopyTo(array, arrayIndex);
        public bool Remove(Item item) => _items.Remove(item);
        public int Count => _items.Count;
        public bool IsReadOnly => false;
    }
}
