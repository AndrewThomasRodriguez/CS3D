namespace CS3D.dataTypes
{
    class BasePixel
    {
        private byte r, g, b, a;
        private float depth;
        private float maxDistance;

        public byte R { get { return r; } set { r = value; } }
        public byte G { get { return g; } set { g = value; } }
        public byte B { get { return b; } set { b = value; } }
        public byte A { get { return a; } set { a = value; } }
        public float Depth { get { return depth; } set { depth = value; } }

        public BasePixel(byte r, byte g, byte b, byte a, float maxDistance)
        {
            this.r = r;
            this.g = g;
            this.b = b;
            this.a = a;

            this.maxDistance = maxDistance;
            depth = this.maxDistance;
        }

        public BasePixel(float maxDistance)
        {
            this.maxDistance = maxDistance;
            depth = this.maxDistance;
            r = 0;
            g = 0;
            b = 0;
            a = 0;
        }

        public void PixelCopy(BasePixel pixelToCopy)
        {
            r = pixelToCopy.R;
            g = pixelToCopy.G;
            b = pixelToCopy.B;
            a = pixelToCopy.A;
            depth = pixelToCopy.Depth;
        }

        //copies a pixel if passes a depth test and alpha test
        public void PixelSetDepthTest(BasePixel pixelToCopy)
        {
            PixelSetDepthTest(pixelToCopy, pixelToCopy.depth);
        }

        // returns if it was successful
        public bool PixelSetDepthTest(BasePixel pixelToCopy, float depth)
        {
            if (depth < this.depth && pixelToCopy.A != 0)
            {
                pixelToCopy.depth = depth;
                PixelCopy(pixelToCopy);
                return true;
            }
            return false;
        }

        //set pixels back to default values. used to clear the screen after drawing
        public void ClearPixel()
        {
            r = 0;
            g = 0;
            b = 0;
            a = byte.MaxValue;
            depth = maxDistance;
        }


    }
}
