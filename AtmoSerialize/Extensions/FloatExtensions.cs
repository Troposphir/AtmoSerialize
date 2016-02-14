using System;

namespace AtmoSerialize.Extensions {
    public static class FloatExtensions {
        public static float ToRadians(this float degrees) => (float) Math.PI / 180 * degrees;
        public static float ToDegrees(this float radians) => radians * 180 / (float) Math.PI;
    }
}
