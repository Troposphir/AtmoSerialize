using System.Linq;
using AtmoSerialize;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
    [TestClass]
    public class MapTest {
        [TestMethod]
        public void AtCellTest() {
            var positive = new Item("mi_test_item") {
                Position = new Vector(13.5f, 9.9f, 3.1f)
            };
            var negative = new Item("mi_test_item") {
                Position = new Vector(-3f, 5f, 92f)
            };
            var map = new Map {
                positive,
                negative
            };
            var subject = new Vector(6, 4, 1);
            CollectionAssert.Contains(map.AtCell(subject).ToList(), positive);
            CollectionAssert.DoesNotContain(map.AtCell(subject).ToList(), negative);
        }
    }
}
