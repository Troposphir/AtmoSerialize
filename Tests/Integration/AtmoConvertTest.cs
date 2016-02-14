using System.Collections.Generic;
using System.IO;
using System.Linq;
using AtmoSerialize;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests.Integration {
    [TestClass]
    public class AtmoConvertTest {

        [TestMethod]
        public void TestDeserialize() {
            const string testLevelPath = "serialize_test.atmo";

            using (var file = File.OpenRead(testLevelPath)) {
                var map = AtmoConvert.Deserialize(file);
                var firstSticker = map.First(i => i.ResourceName == "mi_info_sticker");
                Assert.AreEqual(5, map.Version);
                Assert.AreEqual(3, map.Rulebook.Get<IntegerProperty>("InitialLives").Value);
                Assert.AreEqual(
                    "text text text bla bla bla",
                    firstSticker.GetProperty<TextProperty>("infoContent").Text
                );
                Assert.AreEqual(
                    Map.WorldToCell(firstSticker.Position),
                    firstSticker.GetProperties<TriggerProperty>().First().Conditions.First().CellPosition
                );
            }
        }

        [TestMethod]
        public void TestSerialize() {
            const string compareFilePath = "compare_simple.atmo";

            var buf = new byte[128];
            using (var stream = new MemoryStream(buf, true)) {
                var map = new Map {
                    Rulebook = new Rulebook(new Dictionary<string, IAtmoProperty> { { "whatever", new GenericProperty("blah") } })
                };
                map.Add(new Item("mi_test_item") {
                    Position = new Vector(2, 3, 4),
                    Rotation = Vector.Zero,
                    Scale = 1,
                    Properties = {
                        {"test", new BooleanProperty("true")}
                    }
                });
                AtmoConvert.Serialize(stream, map, false);
                var compareBuf = File.ReadAllBytes(compareFilePath);
                CollectionAssert.AreEqual(compareBuf, buf);
            }
        }
    }
}
