using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace WindowsFormsApplication5
{
    class FPS_checker
    {
        public int fps;
        private int fpsCounter;
        private long fpsTime;
        public int interval = 1000 / 70;
        public long LastTime;
        public int uCounter;
        public int Ups;
        public long uTime;
        public Stopwatch gameTime;
        public int previousSecond;
        public float deltaTime;

        public FPS_checker(Stopwatch timer)
        {
            this.gameTime = timer;

        }
        public void checkfps()
        {
            if (gameTime.ElapsedMilliseconds - fpsTime > 1000)
            {
                fpsTime = gameTime.ElapsedMilliseconds;
                fps = fpsCounter;
                fpsCounter = 0;
            }
            else
            {
                fpsCounter++;
            }
            deltaTime = gameTime.ElapsedMilliseconds - LastTime;
            LastTime = gameTime.ElapsedMilliseconds;
        }

        public void logic(Form1 ThisForm, InputManager iManager)
        {
            if (gameTime.ElapsedMilliseconds - uTime > interval)
            {
                ThisForm.ball.Update(iManager, ThisForm.ParentForm);
                ThisForm.racchetta.Update(iManager, ThisForm.ParentForm);
                if (gameTime.Elapsed.Seconds != previousSecond)
                {
                    previousSecond = gameTime.Elapsed.Seconds;
                    Ups = uCounter;
                    uCounter = 0;
                }
                uTime = gameTime.ElapsedMilliseconds;
                uCounter++;
            }
        }

    }
}
