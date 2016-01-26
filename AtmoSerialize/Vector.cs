namespace AtmoSerialize {
    public struct Vector {
        public float X;
        public float Y;
        public float Z;

        public Vector(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }
        
        public static Vector Zero => new Vector(0, 0, 0);
    }
}