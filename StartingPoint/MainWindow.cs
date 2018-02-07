using System;
using System.Drawing;
using System.Windows.Forms;
using CS3D.StaticFunctions;
using CS3D.Rendering;
using CS3D.CustomDataTypes;
using CS3D.Graphics.FundamentalGraphics;
using CS3D.CpuGraphics;

namespace CS3D
{
    public partial class FormMain : Form
    {
        private System.Drawing.Graphics screenMain;

        //testing
        Position3D mopos;
        DepthTexture mot;
        ModelAnimationManager mo;
        Camera cameraTest;
        Terrain testT;
        float xys = 50.0f;
        float hs = 25.0f;


        public FormMain()
        {
            InitializeComponent();

            //make buffer that will be drawn on screen
            screenMain = pictureBoxMain.CreateGraphics();

            //test camera
            cameraTest = new Camera(new Position3D(0, 0, 0, 0, 0, 0), GlobalConstants.SCREEN_WIDTH, GlobalConstants.SCREEN_HEIGHT, GlobalConstants.FAR_CLIPPING_PLANE, GlobalConstants.CAMERA_FOV);

            //test terrain
            testT = new Terrain(new DepthTexture(@"SceneData\bin\bumpMapColor.png"), new DepthTexture(@"SceneData\bin\bumpMapTest.png"), hs, xys);

            //models test
            mopos = new Position3D(0, 0, 0, 0, 180, 0);
            mot = new DepthTexture(@"SceneData\bin\testImage.png");

            mo = new ModelAnimationManager(mot, false, true, 1.7f, mopos);
            mo.AddAnimationFolder(@"SceneData\bin\Run\run", 0, 110);
            mo.AddAnimationFolder(@"SceneData\bin\Walk\walk", 1, 110);
        }

        private void FormMain_Load(object sender, EventArgs e)
        {
        }

        public void DrawImage(Bitmap img)
        {
            screenMain.DrawImage(img, 0, 0);
        }

        //test draw function
        void FormMain_KeyPress(object sender, KeyPressEventArgs e)
        {
            //camera settings terrain
            cameraTest.CameraPosition.X = -(512 * xys) + 5000;
            cameraTest.CameraPosition.Z = -(512 * xys);
            cameraTest.CameraPosition.Y = 0;
            cameraTest.CameraPosition.Angle3 = 270;
            cameraTest.CameraPosition.Angle1 = -15;

            //model starting position
            mopos.X = -(512 * xys) + 4400;
            mopos.Z = -(512 * xys) + 200;
            mopos.Y = 100;

            //fps counter
            Int64 fpsUpdate = TimerTools.TimerSet(TimerTools.oneSecondMS);

            //testing
            while (true)
            {
                cameraTest.Clear();

                //terrain test
                testT.Render(cameraTest);
                cameraTest.CameraPosition.Y = testT.GetHeight(cameraTest.CameraPosition.X, cameraTest.CameraPosition.Z) + 400; //set camera on top of terrain

                //cameraTest.CameraPosition.Angle3 -= 0.20f; //spin camera

                //model test
                mopos.Y = testT.GetHeight(mopos.X, mopos.Z) - 50; //set model on top of terrain
                mo.Position.Angle3 += 0.2f; //spin model
                mo.PlayPingPong();
                mo.Render(cameraTest);

                //drawing
                KeyIO.DrawToScreen(cameraTest.ScreenDepthTexture, this);

                //performance measuring
                if (TimerTools.TimerPassed(fpsUpdate))
                {
                    fpsUpdate = TimerTools.TimerSet(TimerTools.oneSecondMS);
                    this.Text = TimerTools.FPS.ToString() + " " + TimerTools.FrameDeltaTime.ToString();//cameraTest.CameraPosition.Angle3.ToString();
                }

                TimerTools.FrameTickAll();
            }
        }
    }
}
