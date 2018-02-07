using CS3D.dataTypes;
using CS3D.Graphics.FundamentalGraphics;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace CS3D.StaticFunctions
{
    static class KeyIO
    {
        //DrawToScreen helpers
        private static Bitmap bitmap = new Bitmap(GlobalConstants.SCREEN_WIDTH, GlobalConstants.SCREEN_HEIGHT, GlobalConstants.PIXEL_FORMAT);
        private static Rectangle rectScreen = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

        public static unsafe void DrawToScreen(DepthTexture screenArray, FormMain drawTo)
        {
            //simple error checking to keep program from crashing if array is oversized
            if (screenArray.Width != bitmap.Width || screenArray.Height != bitmap.Height)
                throw new System.ArgumentException("Parameter array is not the same size as the screen", "original");

            //lock bits to draw screen buffer
            System.Drawing.Imaging.BitmapData bmpData = bitmap.LockBits(rectScreen, System.Drawing.Imaging.ImageLockMode.WriteOnly, bitmap.PixelFormat);

            //Pixel array to bitmap
            UInt32* pixel = (UInt32*)bmpData.Scan0.ToPointer();
            int tempW = bitmap.Width;
            int tempH = bitmap.Height;
            //for (int x = 0; x < bitmap.Width; x++)
            Parallel.For(0, tempH, y =>
            {
                int tmpY = y * tempW;
                for (int x = 0; x < tempW; x++)
                {
                    BasePixel tmpPx = screenArray.GetPixel(x, y);
                    pixel[tmpY + x] = 0xFF000000 | (((UInt32)tmpPx.R) << 16 | (((UInt32)tmpPx.G) << 8)) | tmpPx.B;
                }
            });

            //unlock bits then draw
            bitmap.UnlockBits(bmpData);
            drawTo.DrawImage(bitmap);
        }
    }
}
