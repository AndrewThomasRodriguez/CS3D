namespace CS3D.CustomDataTypes
{
    struct Vector2
    {
        public float u, v;
    }

    class BoundingBox
    {
        private float x;
        private float y;
        private float w;
        private float h;

        public float X { get { return x; } set { x = value; } }
        public float Y { get { return y; } set { y = value; } }
        public float W { get { return w; } set { w = value; } }
        public float H { get { return h; } set { h = value; } }

        public float Left { get { return x; } set { X = value; } }
        public float Top { get { return y; } set { Y = value; } }
        public float Right { get { return X + W; } set { X = value - W; } }
        public float Bottom { get { return Y + H; } set { Y = value - H; } }

        public float HalfW { get { return W / 2; } }
        public float HalfH { get { return H / 2; } }

        public float CenterX { get { return X + HalfW; } }
        public float CenterY { get { return Y + HalfH; } }

        public BoundingBox(float x, float y, float w, float h)
        {
            X = x;
            Y = y;
            W = w;
            H = h;
        }

        public void LeftOf(BoundingBox source)
        {
            Right = source.Left;
        }
        public void RightOf(BoundingBox source)
        {
            Left = source.Right;
        }
        public void TopOf(BoundingBox source)
        {
            Bottom = source.Top;
        }
        public void BottomOf(BoundingBox source)
        {
            Top = source.Bottom;
        }

        public bool Intercepts(BoundingBox source)
        {
            return !(x >= source.Right || Right <= source.X || Y >= source.Bottom || Bottom <= source.Y);
        }
    }
}
