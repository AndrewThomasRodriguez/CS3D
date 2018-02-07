using CS3D.CpuGraphics;
using CS3D.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3D.World.WorldObj
{
    interface IDrawable
    {
        void Render(Camera cam);
    }
}
