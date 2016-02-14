using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AtmoSerialize.Internal {
    internal class AtmoWriter : IDisposable {
        private readonly BinaryWriter _writer;

        public ReferenceSolver Solver { get; set; }

        public AtmoWriter(Stream output, Encoding encoding = null) {
            _writer = encoding == null 
                ? new BinaryWriter(output)
                : new BinaryWriter(output, encoding);
        }

        public void Write<T>(ICollection<T> collection) where T: IAtmoSerializable {
            _writer.Write(collection.Count);
            Write((IEnumerable<T>) collection);
        }

        public void Write<T>(ICollection<T> collection, Action<T> writer) {
            _writer.Write(collection.Count);
            Write((IEnumerable<T>)collection, writer);
        }

        public void Write<T>(IEnumerable<T> enumerable) where T : IAtmoSerializable {
            Write(enumerable, Write);
        }

        public void Write<T>(IEnumerable<T> enumerable, Action<T> writer) {
            foreach (var item in enumerable) {
                writer(item);
            }
        }

        public void Write<T>(T value) where T : IAtmoSerializable {
            value.Serialize(this);
        }

        public void Write(int value) => _writer.Write(value);
        public void Write(uint value) => _writer.Write(value);
        public void Write(long value) => _writer.Write(value);
        public void Write(char value) => _writer.Write(value);
        public void Write(bool value) => _writer.Write(value);
        public void Write(byte value) => _writer.Write(value);
        public void Write(sbyte value) => _writer.Write(value);
        public void Write(ulong value) => _writer.Write(value);
        public void Write(float value) => _writer.Write(value);
        public void Write(short value) => _writer.Write(value);
        public void Write(ushort value) => _writer.Write(value);
        public void Write(double value) => _writer.Write(value);
        public void Write(string value) => _writer.Write(value);
        public void Write(byte[] value) => _writer.Write(value);
        public void Write(char[] value) => _writer.Write(value);
        public void Write(decimal value) => _writer.Write(value);
        public void Write(byte[] value, int start, int length) => _writer.Write(value);
        public void Write(char[] value, int start, int length) => _writer.Write(value);

        public void Dispose() {
            _writer.Dispose();
        }
    }
}