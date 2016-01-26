using System;
using System.Collections.Generic;
using System.IO;
using ICSharpCode.SharpZipLib;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace AtmoSerialize {
    internal static class Deserializer {
        public const string HeaderFormat = "AtmoMap";
        public static Map ReadMap(Stream stream) {
            var header = new HeaderData(stream);
            var reader = header.MapReader;

            var map = new Map(header.Version);
            foreach (var resource in GetResources(reader)) {
                map.Resources.Add(resource);
            }
            LoadItems(reader, map.Version, map);

            return map;
        }

        private static void LoadItems(BinaryReader reader, int version, Map target) {
            try {
                while (true) {
                    var name = reader.ReadString();
                    if (name == "EOF") break;
                    if (name == "Rulebook") {
                        target.Rulebook = ReadRulebook(reader);
                    }

                    var item = LoadSingleItem(reader, ResolveResourceName(target.Resources, name), version);
                    target.Items.Add(item);
                }
            } catch (EndOfStreamException) {
                // read file until the end
            } catch (SharpZipBaseException e) {
                if (!e.Message.Contains("Unexpected EOF")) {
                    throw;
                }
            }
        }

        private static MapItem LoadSingleItem(BinaryReader reader, string name, int version) {
            var position = new Vector(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            var rotation = new Vector(reader.ReadByte(), reader.ReadByte(), reader.ReadByte());
            var scale = version < 2 ? reader.ReadByte() : reader.ReadSingle();
            if (version < 3)
                scale /= 10;

            var item = new MapItem(name, position, rotation, scale);

            var propertyCount = reader.ReadByte();
            for (var i = 0; i < propertyCount; i++) {
                var propertyName = reader.ReadString();
                var value = reader.ReadString();
                item.Properties[propertyName] = value;
            }

            return item;
        }

        private static Rulebook ReadRulebook(BinaryReader reader) {
            var rules = new Rulebook();
            var count = reader.ReadByte();
            for (var i = 0; i < count; i++) {
                var raw = reader.ReadString()
                    .Split(Rulebook.PropertySeparator.ToCharArray(), 2);
                var kvp = new KeyValuePair<string, string>(raw[0], raw[1]);
                rules.Properties.Add(kvp);
            }
            return rules;
        }

        private static IEnumerable<string> GetResources(BinaryReader reader) {
            var resourceCount = reader.ReadInt32();
            for (var i = 0; i < resourceCount; i++) {
                yield return reader.ReadString();
            }
        }

        private static string ResolveResourceName(IList<string> resources, string name) {
            int index;
            if (int.TryParse(name, out index)) {
                name = resources[index];
            }
            return name;
        }

        private class HeaderData {
            public int Version { get; }
            public BinaryReader MapReader { get; }

            public HeaderData(Stream stream) {
                var reader = new BinaryReader(stream);
                if (reader.ReadString() != HeaderFormat) {
                    throw new FormatException("Stream does not contain an AtmoMap at the current position!");
                }
                var version = int.Parse(reader.ReadString());
                var compressed = reader.ReadBoolean();
                if (compressed) {
                    reader = new BinaryReader(new InflaterInputStream(stream));
                }
                Version = version;
                MapReader = reader;
            }
        }
    }
}
