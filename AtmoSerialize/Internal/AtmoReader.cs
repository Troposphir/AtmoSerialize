using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace AtmoSerialize.Internal {
    internal class AtmoReader : IDisposable {
        private readonly BinaryReader _reader;

        public ReferenceSolver Solver { get; set; }

        public AtmoReader(Stream input, Encoding encoding = null) {
            _reader = encoding == null
                ? new BinaryReader(input)
                : new BinaryReader(input, encoding);
        }

        public T Read<T>() where T : IAtmoSerializable, new() {
            var target = new T();
            target.Deserialize(this);
            return target;
        }

        public ICollection<T> ReadCollection<T>() where T : IAtmoSerializable, new() {
            return ReadCollection(Read<T>);
        }

        public ICollection<T> ReadCollection<T>(Func<T> readerFunction) {
            var length = ReadInt32();
            var target = new List<T>(length);
            for (var i = 0; i < length; i++) {
                target.Add(readerFunction());
            }
            return target;
        }

        public ICollection<T> ReadToEnd<T>() where T : IAtmoSerializable, new() {
            return ReadToEnd(Read<T>);
        }
        
        public ICollection<T> ReadToEnd<T>(Func<T> readerFunction) {
            var result = new List<T>();
            try {
                while (true) {
                    result.Add(readerFunction());
                }
            } catch (EndOfStreamException) { }
            return result;
        }

        public int Read() => _reader.Read();
        public int Read(byte[] buffer, int start, int length) => _reader.Read(buffer, start, length);
        public int Read(char[] buffer, int start, int length) => _reader.Read(buffer, start, length);
        public bool ReadBoolean() => _reader.ReadBoolean();
        public byte ReadByte() => _reader.ReadByte();
        public byte[] ReadBytes(int count) => _reader.ReadBytes(count);
        public char ReadChar() => _reader.ReadChar();
        public char[] ReadChars(int count) => _reader.ReadChars(count);
        public decimal ReadDecimal() => _reader.ReadDecimal();
        public double ReadDouble() => _reader.ReadDouble();
        public short ReadInt16() => _reader.ReadInt16();
        public int ReadInt32() => _reader.ReadInt32();
        public long ReadInt64() => _reader.ReadInt64();
        public sbyte ReadSByte() => _reader.ReadSByte();
        public float ReadSingle() => _reader.ReadSingle();
        public string ReadString() => _reader.ReadString();
        public ushort ReadUInt16() => _reader.ReadUInt16();
        public uint ReadUInt32() => _reader.ReadUInt32();
        public ulong ReadUInt64() => _reader.ReadUInt64();

        public void Dispose() {
            _reader.Dispose();
        }
    }
}