using System;
using AtmoSerialize.Extensions;

namespace AtmoSerialize {
    /// <summary>
    /// Represents rotations as quaternions. All angles in radians.
    /// </summary>
    public struct Quaternion: IEquatable<Quaternion> {
        public float X => _v.X;
        public float Y => _v.Y;
        public float Z => _v.Z;
        public readonly float W;
        private readonly Vector _v;

        private Quaternion(Vector axis, float angle, bool raw) {
            if (raw) {
                _v = axis;
                W = angle;
            } else {
                var half = angle / 2;
                _v = axis * (float)Math.Sin(half);
                W = (float)Math.Cos(half);
            }
        }
        
        /// <param name="axis">The axis to rotate around</param>
        /// <param name="angle">Angle in radians</param>
        public Quaternion(Vector axis, float angle) : this(axis, angle, false) {}

        /// <summary>
        /// A quaternion such as `q * q.Inverse == Quaternion.Identity`.
        /// </summary>
        public Quaternion Inverse => new Quaternion(-_v, W, true);

        /// <summary>
        /// Composes both quaternions into a equivalent resulting one.
        /// Order of operators matters!
        /// </summary>
        /// <param name="lhs">The rotation to add</param>
        /// <param name="rhs">The original rotation</param>
        /// <returns>rhs rotated by lhs</returns>
        public static Quaternion operator *(Quaternion lhs, Quaternion rhs) => new Quaternion(
            lhs._v * rhs.W + rhs._v * lhs.W + Vector.Cross(lhs._v, rhs._v),
            lhs.W * rhs.W - Vector.Dot(lhs._v, rhs._v),
            true
        );

        /// <summary>
        /// Rotates the given vector by the quaternion's value.
        /// </summary>
        /// <param name="lhs">Rotation</param>
        /// <param name="rhs">CellPosition to be rotated</param>
        /// <returns>The rotated vector</returns>
        public static Vector operator *(Quaternion lhs, Vector rhs) {
            var vcv = Vector.Cross(lhs._v, rhs);
            return rhs + 2 * vcv * lhs.W + Vector.Cross(lhs._v, vcv) * 2;
        }

        /// <summary>
        /// Returns a quaternion that rotates X degrees around the X axis,
        /// Y degrees around the Y axis and Z degrees around the Z axis,
        /// given the vector of radian angles.
        /// </summary>
        /// <param name="euler">Vector containing the amount to rotate on each axis in radians</param>
        /// <returns>A quaternion rotating on X, Y then Z</returns>
        public static Quaternion FromEuler(Vector euler)
            => FromEuler(euler.X.ToRadians(), euler.Y.ToRadians(), euler.Z.ToRadians());

        /// <summary>
        /// Returns a quaternion that rotates X degrees around the X axis,
        /// Y degrees around the Y axis and Z degrees around the Z axis.
        /// </summary>
        /// <param name="x">Amount to rotate around X axis</param>
        /// <param name="y">Amount to rotate around Y axis</param>
        /// <param name="z">Amount to rotate around Z axis</param>
        /// <returns>A quaternion rotating on X, Y, then Z</returns>
        public static Quaternion FromEuler(float x, float y, float z) {
            var rx = new Quaternion(Vector.UnitX, x);
            var ry = new Quaternion(Vector.UnitY, y);
            var rz = new Quaternion(Vector.UnitZ, z);
            return rx * ry * rz;
        }

        /// <summary>
        /// A quaternion that for some `q`, the equality 
        /// `Quaternion.Identity * q == q` holds true.
        /// </summary>
        public static Quaternion Identity => new Quaternion(Vector.UnitX, 0);

        public override string ToString() => $"({_v}, {W})";

        public bool Equals(Quaternion other) {
            return Math.Abs(W - other.W) < Constants.MathEpsilon
                && _v.Equals(other._v);
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            return obj is Quaternion && Equals((Quaternion) obj);
        }

        public override int GetHashCode() {
            unchecked {
                return (W.GetHashCode() * 397) ^ _v.GetHashCode();
            }
        }

        public static bool operator ==(Quaternion left, Quaternion right) {
            return left.Equals(right);
        }

        public static bool operator !=(Quaternion left, Quaternion right) {
            return !left.Equals(right);
        }
    }
}
