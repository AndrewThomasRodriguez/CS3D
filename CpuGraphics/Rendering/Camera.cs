using System;
using CS3D.CustomDataTypes;
using CS3D.Graphics.FundamentalGraphics;

namespace CS3D.Rendering
{
    //Holds data about the camera and the screen array to render to
    class Camera
    {
        private Position3D pos;
        private DepthTexture screen;

        private float calculateFov;
        private float fov;
        private float farClippingPlane;

        public DepthTexture ScreenDepthTexture { get { return screen; } }
        public Position3D CameraPosition { get { return pos; } }
        public float Fov { get { return fov; } set {
                fov = value;
                calculateFov = (float)(screen.Width / Math.Tan(MathTools.DegreesToRadians(FovHalf)));
            } }
        public float FovHalf { get { return fov/2.0f; } }
        public float CalculatedFov { get { return calculateFov; } }
        public float FarClippingPlane { get { return farClippingPlane; } }

        public Camera(Position3D cameraPosition, int width, int height, float farClippingPlane, float fovInDegrees)
        {
            screen = new DepthTexture(width, height);

            pos = cameraPosition;
            Fov = fovInDegrees;
            this.farClippingPlane = farClippingPlane;
        }

        public void Clear()
        {
            ScreenDepthTexture.ClearTexture();
        }
    }
}
