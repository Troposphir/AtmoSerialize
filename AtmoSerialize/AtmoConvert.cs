using System;
using System.IO;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace AtmoSerialize {
    public static class AtmoConvert {
        public static void Serialize(Stream stream, Map map, bool compress) {
            var error = ValidateSerializeParameters(stream, map, compress);
            if (error != null) {
                throw error;
            }

            WriteHeader(stream, map, compress);
            if (compress) {
                stream = new DeflaterOutputStream(stream);
            }
            var writer = new BinaryWriter(stream);
            map.SerializeContents(writer);
            writer.Flush();
        }

        public static async Task SerializeAsync(Stream stream, Map map, bool compress) {
            var error = ValidateSerializeParameters(stream, map, compress);
            if (error != null) {
                throw error;
            }

            await Task.Run(() => Serialize(stream, map, compress));
        }

        public static Map Deserialize(Stream stream) {
            var error = ValidateDeserializeParameters(stream);
            if (error != null) {
                throw error;
            }

            return Deserializer.ReadMap(stream);
        }

        public static async Task<Map> DeserializeAsync(Stream stream) {
            var error = ValidateDeserializeParameters(stream);
            if (error != null) {
                throw error;
            }

            return await Task.Run(() => Deserialize(stream));
        }

        private static void WriteHeader(Stream stream, Map map, bool compress) {
            var writer = new BinaryWriter(stream);
            writer.Write(Deserializer.HeaderFormat);
            writer.Write(map.Version.ToString());
            writer.Write(compress);
            writer.Flush();
        }

        private static Exception ValidateDeserializeParameters(Stream stream) {
            if (stream == null) {
                return new ArgumentException($"{nameof(stream)} must not be null.");
            }
            if (!stream.CanRead) {
                return new ArgumentException($"{nameof(stream)} must be writable.");
            }
            return null;
        }

        private static Exception ValidateSerializeParameters(Stream stream, Map map, bool compress) {
            if (stream == null) {
                return new ArgumentException($"{nameof(stream)} must not be null.");
            }
            if (!stream.CanWrite) {
                return new ArgumentException($"{nameof(stream)} must be writable.");
            }
            if (map == null) {
                return new ArgumentException($"{nameof(map)} must not be null.");
            }
            return null;
        }
    }
}
