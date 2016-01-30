using System;
using System.IO;
using System.Threading.Tasks;
using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace AtmoSerialize {
    /// <summary>
    /// Provides serialization methods to write and read maps.
    /// All methods are non-greedy, this means they expect the stream to be on
    /// a position such as it immediately starts with the map data, and will not
    /// consume any extra bytes from it, assuming a well-formed map.
    /// 
    /// These methods will not close the stream after reading or writing.
    /// </summary>
    public static class AtmoConvert {
        /// <summary>
        /// Writes the contents of the map to the given stream.
        /// </summary>
        /// <param name="stream">The destination stream.</param>
        /// <param name="map">The map to be serialized.</param>
        /// <param name="compress">If true, will compress the map data.</param>
        public static void Serialize(Stream stream, Map map, bool compress) {
            var error = ValidateSerializeParameters(stream, map, compress);
            if (error != null) {
                throw error;
            }

            WriteHeader(stream, map, compress);
            if (compress) {
                stream = new DeflaterOutputStream(stream, new Deflater(Deflater.BEST_COMPRESSION));
            }
            var writer = new BinaryWriter(stream);
            map.SerializeContents(writer);
            writer.Flush();
            if (compress) {
                ((DeflaterOutputStream)stream).Finish();
            }
        }

        /// <summary>
        /// Asynchronously writes the contents of the map to the given stream.
        /// </summary>
        /// <param name="stream">The destination stream.</param>
        /// <param name="map">The map to be serialized.</param>
        /// <param name="compress">If true, will compress the map data.</param>
        /// <returns>A task that is completed once writing finishes.</returns>
        public static async Task SerializeAsync(Stream stream, Map map, bool compress) {
            var error = ValidateSerializeParameters(stream, map, compress);
            if (error != null) {
                throw error;
            }

            await Task.Run(() => Serialize(stream, map, compress));
        }

        /// <summary>
        /// Reads a <see cref="Map"/> instance from a stream.
        /// </summary>
        /// <param name="stream">A stream, positioned at the exact start of the header.</param>
        /// <returns>The object representation of the Map.</returns>
        public static Map Deserialize(Stream stream) {
            var error = ValidateDeserializeParameters(stream);
            if (error != null) {
                throw error;
            }

            return Deserializer.ReadMap(stream);
        }

        /// <summary>
        /// Asynchronously reads a <see cref="Map"/> instance from a stream.
        /// </summary>
        /// <param name="stream">A stream, positioned at the exact start of the header.</param>
        /// <returns>A task which resolves to the object representation of the Map.</returns>
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
