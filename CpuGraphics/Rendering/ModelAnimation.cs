using CS3D.DataTypes.FundamentalGraphics;
using CS3D.Graphics.FundamentalGraphics;
using CS3D.StaticFunctions;
using CS3D.CustomDataTypes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using CS3D.StaticFunctions.SystemDependent;

namespace CS3D.Rendering
{
    //Holds a single Animation. Can hold a single model if needed, just load only one frame.
    //Only one texture perAnimation but UVs may change each frame. You may use a large texture and animate UVs
    class ModelAnimation
    {
        private List<BaseTriangle[]> frames = new List<BaseTriangle[]>();

        private int currentFrame;
        private Int64 frameTimer = 0;
        private Int64 frameTimeMs = 1;
        private bool pingPong = false;

        public Int64 FrameTime
        {
            get { return frameTimeMs; }
            set
            {
                if (value < 1)
                    throw new ArgumentOutOfRangeException("time must be greater than one ms");

                frameTimeMs = value;
            }
        }
        public bool LastFrame { get { return currentFrame == AnimationLength - 1; } }
        public bool FirstFrame { get { return currentFrame == 0; } }
        public bool AtAnEnd { get { return LastFrame || FirstFrame; } }
        public int AnimationLength { get { return frames.Count; } }

        public ModelAnimation(Int64 frameTimeMs)
        {
            FrameTime = frameTimeMs;
        }

        public ModelAnimation()
        {
        }

        public void Render(Camera cam, bool orthographic, bool backfaceCulling, float scale, DepthTexture texture, Position3D modelPosition)
        {
            //multi threaded
            Parallel.ForEach(frames[currentFrame], triangle =>
                {
                    triangle.Render(cam, orthographic, backfaceCulling, scale, texture, modelPosition);
                });
        }

        //Animation methods return true when on the start/end of the animation.
        public bool AnimationPlayTick()
        {
            if (TimerTools.TimerPassed(frameTimer))
            {
                frameTimer = TimerTools.TimerSet(frameTimeMs);
                IncrementFrame();
            }

            return LastFrame;
        }

        public bool AnimationPlayTickLoop()
        {
            if (LastFrame)
                AnimationSetToStart();

            return AnimationPlayTick();
        }

        public bool AnimationPlayReverseTick()
        {
            if (TimerTools.TimerPassed(frameTimer))
            {
                frameTimer = TimerTools.TimerSet(frameTimeMs);
                DecrementFrame();
            }

            return FirstFrame;
        }

        public bool AnimationPlayReverseTickLoop()
        {
            if (FirstFrame)
                AnimationSetToEnd();

            return AnimationPlayReverseTick();
        }

        //Animation reverses play direction once reaching and end in a loop.
        public bool AnimationPlayPingPongTick()
        {
            if (TimerTools.TimerPassed(frameTimer))
            {
                frameTimer = TimerTools.TimerSet(frameTimeMs);
                PingPong();
            }

            return AtAnEnd;
        }

        //Moves one frame, loops automatically
        public void PingPong()
        {
            if (pingPong)
                IncrementFrame();
            else
                DecrementFrame();

            if (AtAnEnd)
                pingPong = !pingPong;
        }

        public void IncrementFrame()
        {
            if (currentFrame + 1 < AnimationLength)
                currentFrame++;
        }

        public void DecrementFrame()
        {
            if (currentFrame - 1 >= 0)
                currentFrame--;
        }

        public void AnimationSetToStart()
        {
            frameTimer = TimerTools.TimerSet(frameTimeMs);
            currentFrame = 0;
        }

        public void AnimationSetToEnd()
        {
            frameTimer = TimerTools.TimerSet(frameTimeMs);
            currentFrame = AnimationLength - 1;
        }

        public void AddFrameFromObj(string fileName)
        {
            frames.Add(ObjFileIO.LoadObj(fileName));
        }
    }
}
