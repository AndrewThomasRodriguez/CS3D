using CS3D.dataTypes;
using System;
using System.Threading.Tasks;
using CS3D.StaticFunctions.SystemDependent;

namespace CS3D.Graphics.FundamentalGraphics
{
    class DepthTexture
    {
        private BasePixel[,] TextureArr;
        private int width;
        private int height;
        private int halfWidth;
        private int halfHeight;

        public int Width { get { return width; } }
        public int Height { get { return height; } }
        public int HalfWidth { get { return halfWidth; } }
        public int HalfHeight { get { return halfHeight; } }

        public DepthTexture(int width, int height, float depth)
        {
            MakeTexture(width, height, depth);
        }

        public DepthTexture(int width, int height)
        {
            MakeTexture(width, height, float.MaxValue);
        }

        public DepthTexture(string filename)
        {
            TextureArr = ImageFileIO.LoadImage(filename);
            UpdateSize();
        }

        private void MakeTexture(int width, int height, float depth)
        {
            TextureArr = new BasePixel[width, height];
            for (int x = 0; x < width; x++)
                for (int y = 0; y < height; y++)
                    TextureArr[x, y] = new BasePixel(depth);

            UpdateSize();
        }

        private void UpdateSize()
        {
            width = TextureArr.GetLength(0);
            height = TextureArr.GetLength(1);

            halfWidth = width / 2;
            halfHeight = height / 2;
        }

        public BasePixel GetPixel(int x, int y)
        {
            return TextureArr[x, y];
        }

        public void SetPixel(int x, int y, BasePixel pix)
        {
            TextureArr[x, y].PixelCopy(pix);
        }

        public bool SetPixelDepthTest(int x, int y, BasePixel pix, float depth)
        {
            return TextureArr[x, y].PixelSetDepthTest(pix, depth);
        }

        public void CopyTexture(DepthTexture text)
        {
            Parallel.For(0, text.Width, x =>
            {
                for (int y = 0; y < text.Height; y++)
                    TextureArr[x, y].PixelCopy(text.GetPixel(x, y));
            });
        }

        //Copies one texture to another with an offset. Over cropping gets warped to the other side of the image.
        public void CopyTextureOffset(DepthTexture text, int xOffset, int yOffset)
        {
            for (int x = 0; x < text.Width; x++)
                for (int y = 0; y < text.Height; y++)
                    TextureArr[(x + xOffset) % (Width - 1),
                               (y + yOffset) % (Height - 1)] = text.GetPixel(x, y);
        }

        public void CopyTextureDepthTest(DepthTexture text)
        {
            Parallel.For(0, text.Width, x =>
            {
                for (int y = 0; y < text.Height; y++)
                    TextureArr[x, y].PixelSetDepthTest(text.GetPixel(x, y));
            });
        }

        public void ClearTexture()
        {
            Parallel.For(0, Width, x =>
            {
                for (int y = 0; y < Height; y++)
                    TextureArr[x, y].ClearPixel();
            });
        }
    }
}
