using CS3D.StaticFunctions.SystemDependent;
using System;

namespace CS3D.StaticFunctions
{
    static class TimerTools
    {
        //Timing tools
        public static Int64 TimerSet(Int64 ms)
        {
            return MasterTimer.TimeInMS() + ms;
        }

        public static bool TimerPassed(Int64 timer)
        {
            return timer < MasterTimer.TimeInMS();
        }

        //Frame Time Tools, FPS and FrameDeltaTime
        public const Int64 oneSecondMS = 1000;
        private static Int64 frameTimerStart = MasterTimer.TimeInMS();
        private static int frameTimerDelta = 0;

        private static int fpsDisplay = 0;
        private static int fpsCount = 0;
        private static Int64 fpsTimer = TimerSet(oneSecondMS);

        public static int FPS { get { return fpsDisplay; } }
        public static int FrameDeltaTime { get { return frameTimerDelta; } }

        /// <summary>
        /// Call each frame to update fps and delta time
        /// </summary>
        public static void FrameTickAll()
        {
            FpsTick();
            DeltaTimeTick();
        }

        public static void FpsTick()
        {
            fpsCount++;
            if (TimerPassed(fpsTimer))
            {
                fpsDisplay = fpsCount;
                fpsCount = 0;
                fpsTimer = TimerSet(oneSecondMS);
            }
        }

        public static void DeltaTimeTick()
        {
            frameTimerDelta = (int)(MasterTimer.TimeInMS() - frameTimerStart);
            frameTimerStart = MasterTimer.TimeInMS();
        }
    }
}
