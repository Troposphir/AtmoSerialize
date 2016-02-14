using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using AtmoSerialize.Internal.Serializables;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace AtmoSerialize.Internal {
    internal static class InternalConverter {
        private const string FormatIdentifier = "AtmoMap";

        public static Map Deserialize(Stream stream, Encoding encoding = null) {
            var reader = new AtmoReader(stream, encoding);
            var header = reader.Read<MapHeader>();
            int version;
            if (header.Format != FormatIdentifier
                || !int.TryParse(header.Version, out version)) {
                throw new InvalidDataException("Unknown map format");
            }
            if (header.Compressed) {
                reader = new AtmoReader(new InflaterInputStream(stream));
            }

            var body = reader.Read<MapBody>();
            var map = new Map(version) {
                Rulebook = body.Content
                    .Select(i => i.Rulebook)
                    .First(r => r != null)
            };
            map.Items.AddRange(body.Content.Where(i => i.Rulebook == null));
            return map;
        }

        public static void Serialize(Stream stream, Map map, Encoding encoding = null) {
            var writer = new AtmoWriter(stream, encoding);
            writer.Write(new MapHeader {
                Format = FormatIdentifier,
                Version = map.Version.ToString(),
                Compressed = map.Compressed,
            });
            if (map.Compressed) {
                stream = new DeflaterOutputStream(stream, new Deflater(Deflater.BEST_COMPRESSION));
            }
            writer = new AtmoWriter(stream, encoding) {
                Solver = new ReferenceSolver()
            };
            writer.Write(writer.Solver);
            writer.Write(new MapItem {
                ResourceName = "Rulebook",
                Rulebook = map.Rulebook
            });
            writer.Write((IEnumerable<MapItem>) map.Items);
            
            if (map.Compressed) {
                ((DeflaterOutputStream) stream).Finish();
            }
        }
    }
}
