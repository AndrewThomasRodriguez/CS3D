using CS3D.dataTypes;
using System.Drawing;

namespace CS3D.StaticFunctions.SystemDependent
{
    static class ImageFileIO
    {
        public static BasePixel[,] LoadImage(string filename)
        {
            Bitmap tmpImg = (Bitmap)Image.FromFile(filename);
            BasePixel[,] returnArray = new BasePixel[tmpImg.Width, tmpImg.Height];

            for (int x = 0; x < tmpImg.Width; x++)
                for (int y = 0; y < tmpImg.Height; y++)
                {
                    Color tmpColor = tmpImg.GetPixel(x, y);

                    returnArray[x, y] = new BasePixel(
                        tmpColor.R,
                        tmpColor.G,
                        tmpColor.B,
                        tmpColor.A,
                        0);
                }

            return returnArray;
        }
    }
}
