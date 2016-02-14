using AtmoSerialize;
using AtmoSerialize.Extensions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Tests {
    [TestClass]
    public class QuaternionTest {
        [TestMethod]
        public void TestFromEuler() {
            Assert.AreEqual(new Quaternion(Vector.UnitX, 90f.ToRadians()), Quaternion.FromEuler(new Vector(90f, 0, 0)));
        }

        [TestMethod]
        public void TestIdentity() {
            var q = new Quaternion(Vector.UnitX, 45f.ToRadians());
            Assert.AreEqual(q, Quaternion.Identity * q);
            Assert.AreEqual(Vector.UnitZ, Quaternion.Identity * Vector.UnitZ);
        }

        [TestMethod]
        public void TestComposition() {
            var q = new Quaternion(Vector.UnitY, 22.67434f.ToRadians());
            Assert.AreEqual(q * q.Inverse, Quaternion.Identity);
        }
    }
}
