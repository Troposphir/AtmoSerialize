using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AtmoSerialize.Internal;
using AtmoSerialize.Internal.Serializables;

namespace AtmoSerialize {
    public static class AtmoConvert {
        public static void Serialize(Stream stream, Map map, bool compress = true) {
            var oldMap = new Internal.Map(5, compress) {
                Rulebook = new Internal.Serializables.Rulebook {
                    Rules = StringifyProperties(map.Rulebook)
                }
            };
            oldMap.Items.AddRange(map.Where(i => i != null).Select(i => new MapItem {
                ResourceName = i.ResourceName,
                Position = ToFloatvector(i.Position),
                Rotation = ToByteVector(i.Rotation),
                Scale = i.Scale,
                Properties = StringifyProperties(i.Properties),
                Rulebook = null
            }));
            InternalConverter.Serialize(stream, oldMap);
        }

        public static async Task SerializeAsync(Stream stream, Map map, bool compress = true) {
            await Task.Run(() => Serialize(stream, map, compress));
        }

        public static Map Deserialize(Stream stream) {
            var oldMap = InternalConverter.Deserialize(stream);

            var map = new Map {
                Rulebook = new Rulebook(PropertizeStrings(oldMap.Rulebook.Rules))
            };
            foreach (var mapItem in oldMap.Items) {
                map.Add(new Item(mapItem.ResourceName) {
                    Position = MakeVector(mapItem.Position),
                    Rotation = MakeVector(mapItem.Rotation),
                    Scale = mapItem.Scale,
                    Properties = PropertizeStrings(mapItem.Properties)
                });
            }

            return map;
        }

        public static async Task<Map> DeserializeAsync(Stream stream) {
            return await Task.Run(() => Deserialize(stream));
        }

        private static Dictionary<string, IAtmoProperty> PropertizeStrings(Dictionary<string, string> properties) {
            var converted = new Dictionary<string, IAtmoProperty>(properties.Count);
            foreach (var kvp in properties) {
                switch (kvp.Key) {
                    case "infoContent":
                        converted[kvp.Key] = new TextProperty(kvp.Value);
                        break;
                    case "detectPlayers":
                        converted[kvp.Key] = new PlayerTargetProperty(kvp.Value);
                        break;
                    case "countType":
                        converted[kvp.Key] = new CountTypeProperty(kvp.Value);
                        break;
                    default:
                        var lowerKey = kvp.Key.ToLowerInvariant();
                        var lowerValue = kvp.Value.ToLowerInvariant();
                        if (new[] {"false", "true"}.Contains(lowerValue)) {
                            converted[kvp.Key] = new BooleanProperty(kvp.Value);
                        } else if (lowerKey.EndsWith("trigger")) {
                            converted[kvp.Key] = new TriggerProperty(kvp.Value);
                        } else if (Regex.IsMatch(lowerValue, @"^-?\d+$")) {
                            converted[kvp.Key] = new IntegerProperty(kvp.Value);
                        } else if (Regex.IsMatch(lowerValue, @"^-?\d*\.\d*$")) {
                            converted[kvp.Key] = new FloatProperty(kvp.Value);
                        } else {
                            converted[kvp.Key] = new GenericProperty(kvp.Value);
                        }
                        break;
                }
            }
            return converted;
        }

        private static Dictionary<string, string> StringifyProperties(IDictionary<string, IAtmoProperty> properties) {
            return properties.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToRaw());
        }

        private static ByteVector ToByteVector(Vector vector) => new ByteVector {
            X = (byte)(vector.X / 2),
            Y = (byte)(vector.Y / 2),
            Z = (byte)(vector.Z / 2)
        };
        
        private static FloatVector ToFloatvector(Vector vector) => new FloatVector {
            X = vector.X,
            Y = vector.Y,
            Z = vector.Z
        };

        private static Vector MakeVector(FloatVector vector) => new Vector(vector.X, vector.Y, vector.Z);
        private static Vector MakeVector(ByteVector vector) => new Vector(vector.X, vector.Y, vector.Z) * 2;
    }
}
