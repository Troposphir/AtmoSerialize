using System;

namespace AtmoSerialize {
    public struct Vector: IEquatable<Vector> {
        public readonly float X;
        public readonly float Y;
        public readonly float Z;

        public float SquareLength => X * X + Y * Y + Z * Z;
        public float Length => (float)Math.Sqrt(SquareLength);

        public Vector Normalized {
            get {
                var len = Length;
                return len > 0
                           ? this / len
                           : this;
            }
        }

        public Vector(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }

        public static float Distance(Vector a, Vector b) => (a - b).Length;
        public static float Dot(Vector a, Vector b) => a.X * b.X + a.Y * b.Y + a.Z * b.Z;
        public static Vector Cross(Vector a, Vector b) => new Vector(
            a.Y * b.Z - a.Z * b.Y,
            a.Z * b.X - a.X * b.Z,
            a.X * b.Y - a.Y * b.X
        );

        public static Vector Floor(Vector v) => new Vector(
            (float)Math.Floor(v.X),
            (float)Math.Floor(v.Y),
            (float)Math.Floor(v.Z)
        );
        public static Vector Ceiling(Vector v) => new Vector(
            (float)Math.Ceiling(v.X),
            (float)Math.Ceiling(v.Y),
            (float)Math.Ceiling(v.Z)
        );
        public static Vector Scale(Vector a, Vector b) => new Vector(
            a.X * b.X,
            a.Y * b.Y,
            a.Z * b.Z
        );

        public static float Angle(Vector a, Vector b) {
            var an = a.Normalized;
            var bn = b.Normalized;
            if (Math.Abs(an.SquareLength) < Constants.MathEpsilon
                || Math.Abs(bn.SquareLength) < Constants.MathEpsilon) {
                return 0f;
            }
            return (float) Math.Acos(Dot(a.Normalized, b.Normalized));
        }

        public static Vector operator +(Vector lhs, Vector rhs) => new Vector(
            lhs.X + rhs.X,
            lhs.Y + rhs.Y,
            lhs.Z + rhs.Z
        );

        public static Vector operator -(Vector lhs, Vector rhs) => new Vector(
            lhs.X + rhs.X,
            lhs.Y + rhs.Y,
            lhs.Z + rhs.Z
        );

        public static Vector operator -(Vector vec) => new Vector(
            -vec.X,
            -vec.Y,
            -vec.Z
        );

        public static Vector operator /(Vector lhs, float rhs) => lhs * (1 / rhs);
        public static Vector operator *(float lhs, Vector rhs) => rhs * lhs;
        public static Vector operator *(Vector lhs, float rhs) => new Vector(
            lhs.X * rhs,
            lhs.Y * rhs,
            lhs.Z * rhs
        );

        public static Vector Zero => new Vector(0, 0, 0);
        public static Vector One => new Vector(1, 1, 1);
        public static Vector UnitX => new Vector(1, 0, 0);
        public static Vector UnitY => new Vector(0, 1, 0);
        public static Vector UnitZ => new Vector(0, 0, 1);

        public override string ToString() => $"({X}, {Y}, {Z})";

        public bool Equals(Vector other) {
            return Math.Abs(X - other.X) < Constants.MathEpsilon
                && Math.Abs(Y - other.Y) < Constants.MathEpsilon
                && Math.Abs(Z - other.Z) < Constants.MathEpsilon;
        }

        public override bool Equals(object obj) {
            if (ReferenceEquals(null, obj)) {
                return false;
            }
            return obj is Vector && Equals((Vector) obj);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 397) ^ Y.GetHashCode();
                hashCode = (hashCode * 397) ^ Z.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Vector left, Vector right) {
            return left.Equals(right);
        }

        public static bool operator !=(Vector left, Vector right) {
            return !left.Equals(right);
        }
    }
}
