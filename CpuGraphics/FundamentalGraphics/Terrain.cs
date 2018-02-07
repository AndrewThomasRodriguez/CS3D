using CS3D.CustomDataTypes;
using CS3D.dataTypes;
using CS3D.Graphics.FundamentalGraphics;
using CS3D.Rendering;
using System;
using System.Threading.Tasks;

namespace CS3D.CpuGraphics
{
    class Terrain
    {
        private struct Pixel
        {
            public byte R, G, B;
        }

        private Pixel[,] diffuseMap;
        private float[,] bumpMap;
        private int h, w;
        private float xzScale;

        public Terrain(DepthTexture diffuseMap, DepthTexture bumpMap, float yScale, float xzScale)
        {
            if (diffuseMap.Height != bumpMap.Height || diffuseMap.Width != bumpMap.Width)
                throw new Exception("diffuseMap and bumpMap must be the same size");

            if (yScale < 1.0f || xzScale < 1.0f)
                throw new Exception("only can scale up or keep a scale of 1");

            //make bare-bones simplified arrays/per-computed values
            this.xzScale = xzScale;
            h = bumpMap.Height;
            w = bumpMap.Width;

            this.bumpMap = new float[bumpMap.Width, bumpMap.Height];
            this.diffuseMap = new Pixel[diffuseMap.Width, diffuseMap.Height];

            Parallel.For(0, bumpMap.Width, x =>
            {
                for (int y = 0; y < bumpMap.Height; y++)
                {
                    this.bumpMap[x, y] = bumpMap.GetPixel(x, y).R * yScale;

                    ref Pixel tmpPix = ref this.diffuseMap[x, y];

                    BasePixel tmpPix2 = diffuseMap.GetPixel(x, y);
                    tmpPix.R = tmpPix2.R;
                    tmpPix.G = tmpPix2.G;
                    tmpPix.B = tmpPix2.B;
                }
            });
        }

        private void DrawVerticalLine(DepthTexture texture, Pixel color, float depth, int startX, int startY, float width)
        {
            //make sure the range is in drawing area
            startY = Math.Max(startY, 0);
            int xEnd = Math.Min(startX + (int)Math.Ceiling(width), texture.Width - 1);

            //draw the rectangle
            for (int xOff = Math.Max(0, startX); xOff < xEnd; xOff++)
                for (int y = startY; y < texture.Height; y++)
                {
                    BasePixel tmpPx = texture.GetPixel(xOff, y);

                    if (tmpPx.Depth < depth) break;

                    tmpPx.R = color.R;
                    tmpPx.G = color.G;
                    tmpPx.B = color.B;
                    tmpPx.Depth = depth;
                }
        }

        private void ScalePoint(ref Vector3 point, float scaleXZ)
        {
            point.x *= scaleXZ;
            point.z *= scaleXZ;
        }

        private float PerspectiveScaleX(float xzScale, float z, float fov)
        {
            return xzScale * (1.0f - (z / (z + fov)));
        }

        public void Render(Camera cam)
        {
            Parallel.For(0, w, x =>
            {
                for (int z = 0; z < h; z++)
                {
                    //get bump map height
                    float y = cam.ScreenDepthTexture.Height - bumpMap[x, z];

                    //scale then center then translate vertex to the camara pt.2
                    Vector3 mainPoint;
                    mainPoint.x = x;
                    mainPoint.y = y;
                    mainPoint.z = z;

                    ScalePoint(ref mainPoint, xzScale);
                    mainPoint.TranslateWorld(cam.CameraPosition);
                    mainPoint.Rotate(cam.CameraPosition);

                    //filter points on distance
                    if (mainPoint.z > 0.0f || mainPoint.z < cam.FarClippingPlane)
                    {
                        mainPoint.Perspective(cam.CalculatedFov, cam.ScreenDepthTexture.HalfWidth, cam.ScreenDepthTexture.HalfWidth);

                        //how long a row should be perspective corrected
                        float overdraw = PerspectiveScaleX(xzScale, mainPoint.z, cam.CalculatedFov);

                        //filter if not in screen space, allows overdraw on left side
                        if (mainPoint.x < -overdraw || mainPoint.x >= cam.ScreenDepthTexture.Width || mainPoint.y >= cam.ScreenDepthTexture.Height)
                            continue;

                        DrawVerticalLine(cam.ScreenDepthTexture, diffuseMap[x, z], mainPoint.z, (int)mainPoint.x, (int)mainPoint.y, overdraw);
                    }
                }
            });
        }

        public float GetHeight(float x, float z)
        {
            return bumpMap[(int)(-x / xzScale), (int)(-z / xzScale)];
        }
    }
}
