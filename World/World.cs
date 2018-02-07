using CS3D.World.WorldObj;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CS3D.World
{
    class World
    {
        List<Map> Maps = new List<Map>();
        List<IDrawable> toDraw = new List<IDrawable>();
        List<BaseWorldObj> dynamicObjs = new List<BaseWorldObj>();
    }
}
