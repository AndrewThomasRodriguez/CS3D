using CS3D.Graphics.FundamentalGraphics;
using CS3D.Rendering;
using CS3D.CustomDataTypes;
using System;

namespace CS3D.DataTypes.FundamentalGraphics
{
    struct TriangleData
    {
        public Vector3 Vertex1;
        public Vector2 Uv1;

        public Vector3 Vertex2;
        public Vector2 Uv2;

        public Vector3 Vertex3;
        public Vector2 Uv3;
    }

    class BaseTriangle
    {
        TriangleData tData;

        public BaseTriangle(TriangleData tData)
        {
            this.tData = tData;
        }

        public void Render(Camera cam, bool orthographic, bool backfaceCulling, float scale, DepthTexture texture, Position3D modelPosition)
        {
            //make a copy, don't touch original points to keep accuracy
            TriangleData tDataTmp = tData;

            //move the vertices in local space
            ScaleVertices(ref tDataTmp, scale);
            RotateVertices(ref tDataTmp, modelPosition);
            TranslateVerticesLocal(ref tDataTmp, modelPosition);

            //move the vertices in world space
            TranslateVerticesWorld(ref tDataTmp, cam.CameraPosition);
            RotateVertices(ref tDataTmp, cam.CameraPosition);

            //culling/drawing/perspective
            if (InZRange(tDataTmp, cam.FarClippingPlane))
            {
                if (!orthographic)
                    PerspectiveVertices(ref tDataTmp, cam.ScreenDepthTexture, cam.CalculatedFov);

                if (backfaceCulling && NotFacingCamera(tDataTmp))
                    return;
                
                DrawTriangle(tDataTmp, texture, cam.ScreenDepthTexture);
            }
        }

        //triangle render tools
        private void RotateVertices(ref TriangleData tData, Position3D angle)
        {
            tData.Vertex1.Rotate(angle);
            tData.Vertex2.Rotate(angle);
            tData.Vertex3.Rotate(angle);
        }

        private void TranslateVerticesWorld(ref TriangleData tData, Position3D delta)
        {
            tData.Vertex1.TranslateWorld(delta);
            tData.Vertex2.TranslateWorld(delta);
            tData.Vertex3.TranslateWorld(delta);
        }

        private void TranslateVerticesLocal(ref TriangleData tData, Position3D delta)
        {
            tData.Vertex1.TranslateLocal(delta);
            tData.Vertex2.TranslateLocal(delta);
            tData.Vertex3.TranslateLocal(delta);
        }

        private void ScaleVertices(ref TriangleData tData, float scale)
        {
            if (scale != 1.0f)
            {
                tData.Vertex1.Scale(scale);
                tData.Vertex2.Scale(scale);
                tData.Vertex3.Scale(scale);
            }
        }

        private void PerspectiveVertices(ref TriangleData tData, DepthTexture screenTexture, float calculatedFov)
        {
            if (tData.Vertex1.z > 0)
                tData.Vertex1.Perspective(calculatedFov, screenTexture.HalfWidth, screenTexture.HalfHeight);

            if (tData.Vertex2.z > 0)
                tData.Vertex2.Perspective(calculatedFov, screenTexture.HalfWidth, screenTexture.HalfHeight);

            if (tData.Vertex3.z > 0)
                tData.Vertex3.Perspective(calculatedFov, screenTexture.HalfWidth, screenTexture.HalfHeight);
        }

        private bool InZRange(TriangleData tData, float farClippingPlane)
        {
            return !(MaxZ(tData) < 0 || MinZ(tData) > farClippingPlane);
        }

        //Check if its in the screenTexture area. Left/right/... are the bounding box of the triangle limited to screen space.
        //Returns False if not in screen screenTexture area.
        private bool OnScreen(TriangleData tData, DepthTexture screenTexture, ref int left, ref int right, ref int top, ref int bottom)
        {
            left = (int)Left(tData);
            if (left > screenTexture.Width)
                return false;

            right = (int)Right(tData);
            if (right < 0)
                return false;

            top = (int)Top(tData);
            if ( top > screenTexture.Height)
                return false;

            bottom = (int)Bottom(tData);
            if ( bottom < 0)
                return false;

            left = Math.Max(0, left);
            right = Math.Min(screenTexture.Width, right);
            top = Math.Max(0, top);
            bottom = Math.Min(screenTexture.Height, bottom);

            return true;
        }

        private bool NotFacingCamera(TriangleData tData)
        {
            return (Area(tData) > -1);
        }

        private float MaxZ(TriangleData tData)
        {
            return MathTools.Max3(tData.Vertex1.z, tData.Vertex2.z, tData.Vertex3.z);
        }

        private float MinZ(TriangleData tData)
        {
            return MathTools.Min3(tData.Vertex1.z, tData.Vertex2.z, tData.Vertex3.z);
        }

        private int Area(TriangleData tData)
        {
            int x1 = (int)tData.Vertex1.x;
            int y1 = (int)tData.Vertex1.y;

            int x2 = (int)tData.Vertex2.x;
            int y2 = (int)tData.Vertex2.y;

            int x3 = (int)tData.Vertex3.x;
            int y3 = (int)tData.Vertex3.y;

            return x1 * (y2 - y3) + x2 * (y3 - y1) + x3 * (y1 - y2);
        }

        private float Left(TriangleData tData)
        {
            return MathTools.Min3(tData.Vertex1.x, tData.Vertex2.x, tData.Vertex3.x);
        }

        private float Right(TriangleData tData)
        {
            return MathTools.Max3(tData.Vertex1.x, tData.Vertex2.x, tData.Vertex3.x);
        }

        private float Top(TriangleData tData)
        {
            return MathTools.Min3(tData.Vertex1.y, tData.Vertex2.y, tData.Vertex3.y);
        }

        private float Bottom(TriangleData tData)
        {
            return MathTools.Max3(tData.Vertex1.y, tData.Vertex2.y, tData.Vertex3.y);
        }

        private void DrawTriangle(TriangleData tData, DepthTexture modelTexture, DepthTexture screenTexture)
        {
            //set array boundes
            int left = 0;
            int right = 0;
            int top = 0;
            int bottom = 0;
            if (!OnScreen(tData, screenTexture, ref left, ref right, ref top, ref bottom))
                return; //cull out triangles not in screen space

            //setup values for the draw loop
            int x1 = (int)tData.Vertex1.x;
            int y1 = (int)tData.Vertex1.y;
            float z1 = tData.Vertex1.z;

            int x2 = (int)tData.Vertex2.x;
            int y2 = (int)tData.Vertex2.y;
            float z2 = tData.Vertex2.z;

            int x3 = (int)tData.Vertex3.x;
            int y3 = (int)tData.Vertex3.y;
            float z3 = tData.Vertex3.z;

            //precomputed values
            int totalArea = Math.Abs(Area(tData));
            if (totalArea < 1) return; //filter subpixel triangles

            //precompute area pt1
            int y2my3 = y2 - y3;
            int y3my1 = y3 - y1;
            int y1my2 = y1 - y2;

            //computing uvs
            float uvX1 = tData.Uv1.u * modelTexture.Width;
            float uvX2 = tData.Uv2.u * modelTexture.Width;
            float uvX3 = tData.Uv3.u * modelTexture.Width;

            float uvY1 = tData.Uv1.v * modelTexture.Height;
            float uvY2 = tData.Uv2.v * modelTexture.Height;
            float uvY3 = tData.Uv3.v * modelTexture.Height;

            //draw loop
            for (int y = top; y < bottom; y++)
            {
                //precompute area pt2
                int apre = x2 * (y3 - y) + x3 * (y - y2);
                int bpre = x1 * (y - y3) + x3 * (y1 - y);
                int cpre = x1 * (y2 - y) + x2 * (y - y1);

                bool inTriangle = false; //right side exit scanline optimization

                for (int x = left; x < right; x++)
                {
                    //find out what points are in the triangle with the precompute area
                    int ba = Math.Abs(x * y2my3 + apre);
                    int bb = Math.Abs(x * y3my1 + bpre);
                    int bc = Math.Abs(x * y1my2 + cpre);

                    if (ba + bb + bc <= totalArea)
                    {
                        inTriangle = true;

                        //interpolate pixel amounts
                        float a = ba / (float)totalArea;
                        float b = bb / (float)totalArea;
                        float c = 1 - (a + b);

                        //get the uv/depth
                        int u = (int)(uvX1 * a + uvX2 * b + uvX3 * c);
                        int v = (int)(uvY1 * a + uvY2 * b + uvY3 * c);
                        float depth = z1 * a + z2 * b + z3 * c;

                        //set the pixel if passes depth test
                        screenTexture.SetPixelDepthTest(x, y, modelTexture.GetPixel(u, v), depth);
                    }
                    else
                        if (inTriangle) break; //end scanline if exiting right side of the triangle
                }
            }
        }
    }
}
