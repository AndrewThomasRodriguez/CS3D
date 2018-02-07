using System;

namespace CS3D.StaticFunctions.SystemDependent
{
    static class MasterTimer
    {
        public static Int64 TimeInMS()
        {
            return DateTime.Now.Ticks / TimeSpan.TicksPerMillisecond;
        }
    }
}
