using CS3D.CustomDataTypes;
using CS3D.Graphics.FundamentalGraphics;
using CS3D.Rendering;
using System;
using System.Collections.Generic;
using System.IO;

namespace CS3D.CpuGraphics
{
    class ModelAnimationManager
    {
        private Dictionary<int, ModelAnimation> Animations = new Dictionary<int, ModelAnimation>();

        private bool orthographic = false;
        private bool backfaceCulling = true;
        private float scale = 1.0f;
        Position3D modelPosition;
        private DepthTexture texture;
        private int animationId = 0;

        public bool Orthographic { get { return orthographic; } set { orthographic = value; } }
        public bool BackfaceCulling { get { return backfaceCulling; } set { backfaceCulling = value; } }
        public float Scale { get { return scale; } set { scale = value; } }
        public int AnimationId { get { return animationId; } set
            {
                if (!Animations.ContainsKey(value))
                    throw new ArgumentOutOfRangeException("AnimationId key does not exists");

                animationId = value;
            } }
        public int AnimationCount { get { return Animations.Count; } }
        public Position3D Position { get { return modelPosition; } set { modelPosition = value; } }
        public DepthTexture Texture { get { return texture; } set { texture = value; } }

        public ModelAnimationManager(DepthTexture texture, bool orthographic, bool backfaceCulling, float scale, Position3D position)
        {
            Texture = texture;
            Orthographic = orthographic;
            BackfaceCulling = backfaceCulling;
            Scale = scale;
            Position = position;
        }

        /// <summary>
        /// Loads a series of .obj files from a folder.
        /// Objs must be triangulated and contain only uvs, vertices and facese.
        /// </summary>
        /// <param name="folderLocationAndName">Location + the start of the file name without the ".obj" at the end.</param>
        /// <param name="animationId">The key to store the animation as.</param>
        /// <param name="playbackSpeed">Playback speed in milliseconds perframe.</param>
        public void AddAnimationFolder(string folderLocationAndName, int animationId, int playbackSpeed)
        {
            int i = 0;
            string tmpFilename;
            ModelAnimation tmpAnimation = new ModelAnimation(playbackSpeed);

            while (File.Exists(tmpFilename = folderLocationAndName + i++.ToString() + GlobalConstants.fileFormat3D))
                tmpAnimation.AddFrameFromObj(tmpFilename);

            Animations.Add(animationId, tmpAnimation);
        }


        //Be sure to set AnimationId before use of Play/PlayLoop/PlayPingPong/Render.
        //Must be called each frame to be updated.
        //Returns true once you hit the end of the animation.
        public bool PlayLoop()
        {
            return Animations[animationId].AnimationPlayTickLoop();
        }

        public bool PlayPingPong()
        {
            return Animations[animationId].AnimationPlayPingPongTick();
        }

        //intended use is to render after you use a play
        public void Render(Camera cam)
        {
            Animations[animationId].Render(cam, Orthographic, BackfaceCulling, Scale, Texture, Position);
        }
    }
}
