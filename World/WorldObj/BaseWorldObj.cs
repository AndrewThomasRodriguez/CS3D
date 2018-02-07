using CS3D.CpuGraphics;
using CS3D.CustomDataTypes;
using CS3D.Rendering;
using CS3D.World.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3D.World.WorldObj
{
    abstract class BaseWorldObj : IDrawable
    {
        private ModelAnimationManager graphics;
        private Position3D graphicsPosistion;

        private BoundingBox location;
        private uint objectId;
        private int objectTag;

        public uint ObjectId { get { return objectId; } }
        public int ObjectTag { get { return objectTag; } }

        //properties
        float hspeed;
        float vspeed;


        //tools
        static private uint idGenerator = 0;
        private uint IdGenerator { get { return idGenerator++; } }

        //polling events
        public virtual void Step() {}
        public virtual void Attack() {}
        public virtual void Dead() {}
        public virtual void Physics() {}
        public virtual void Render(Camera cam) {}
    }

}
