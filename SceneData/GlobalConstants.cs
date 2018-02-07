using System.Drawing.Imaging;

namespace CS3D
{
    static class GlobalConstants
    {
        //screen format
        public const int SCREEN_WIDTH = 1280;
        public const int SCREEN_HEIGHT = 720;

        public const PixelFormat PIXEL_FORMAT = PixelFormat.Format32bppArgb;

        //camera
        public const float FAR_CLIPPING_PLANE = float.MaxValue;
        public const float CAMERA_FOV = 60.0f;

        //files
        public const string fileFormat3D = ".obj";

        //
        enum Direction
        {
            UP,
            DOWN,
            LEFT,
            RIGHT
        };
    }
}
