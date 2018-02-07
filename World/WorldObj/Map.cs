using CS3D.CpuGraphics;
using CS3D.CustomDataTypes;
using CS3D.Rendering;
using System.Collections.Generic;

namespace CS3D.World.WorldObj
{
    class Map : IDrawable
    {
        private ModelAnimationManager graphic;

        private List<BoundingBox> platforms = new List<BoundingBox>();
        private List<BoundingBox> oneWayPlatforms = new List<BoundingBox>();

        public BoundingBox PlatformCollision(BoundingBox checkAgainst)
        {
            return CollisionCheck(platforms, checkAgainst);
        }
        public BoundingBox OneWayPlatformCollision(BoundingBox checkAgainst)
        {
            return CollisionCheck(oneWayPlatforms, checkAgainst);
        }
        private BoundingBox CollisionCheck(List<BoundingBox> platforms, BoundingBox checkAgainst)
        {
            foreach (BoundingBox i in platforms)
                if (i.Intercepts(checkAgainst)) return i;

            return null;
        }

        public Map( string mapFileObj, string textureFile )
        {
            graphic = new ModelAnimationManager(new Graphics.FundamentalGraphics.DepthTexture(textureFile), false, true, 1, new Position3D(0, 0, 0, 0, 0, 0));
        }

        //polling events
        public void Render(Camera cam)
        {
            graphic.Render(cam);
        }
    }
}
