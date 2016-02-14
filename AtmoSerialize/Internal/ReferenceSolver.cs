using System.Collections.Generic;
using System.Linq;

namespace AtmoSerialize.Internal {
    internal class ReferenceSolver : IAtmoSerializable {
        private readonly List<string> _references;

        public ReferenceSolver() : this(new List<string>()) {}

        public ReferenceSolver(IEnumerable<string> references) {
            _references = references.ToList();
        }

        public string Solve(string possibleReference) {
            int index;
            if (int.TryParse(possibleReference, out index)
                && index >= 0 && index < _references.Count) {
                return _references[index];
            }
            _references.Add(possibleReference);
            return possibleReference;
        }

        public string Reference(string value) {
            var index = _references.IndexOf(value);
            if (index >= 0) {
                return index.ToString();
            }
            _references.Add(value);
            return value;
        }

        public void Serialize(AtmoWriter writer) {
            writer.Write(_references, writer.Write);
        }

        public void Deserialize(AtmoReader reader) {
            _references.Clear();
            _references.AddRange(reader.ReadCollection(reader.ReadString));
        }
    }
}
