using System.Diagnostics;

namespace WindowsFormsApplication5
{
    internal class FPS_checker
    {
        //variabili per fps
        public int fps;

        private int fps_tmp;
        private long fpsTime;
        public long LastTime;

        //variabile per limitare gli fps
        public int interval = 1000 / 70;

        //vairbili per ups
        public int ups_tmp;

        public int ups;
        public long upsTime;
        public Stopwatch gameTime;
        public int previousSecond;
        public float deltaTime;

        //
        public FPS_checker(Stopwatch timer)
        {
            this.gameTime = timer;
        }

        //funzione per il check degli fps(Frames per Second, le volte in cui si renderizza la grafica in un solo secondo), li conta per poi resettare il totale e restituire il risultato ogni secondo
        public void checkfps()
        {
            if (gameTime.ElapsedMilliseconds - fpsTime > 1000)
            {
                fpsTime = gameTime.ElapsedMilliseconds;
                fps = fps_tmp;
                fps_tmp = 0;
            }
            else
            {
                fps_tmp++;
            }
            deltaTime = gameTime.ElapsedMilliseconds - LastTime;
            LastTime = gameTime.ElapsedMilliseconds;
        }

        //funzione che calcola la logica e gli ups (Updates per second , cioè aggiornamento delle posizioni e calcolo di eventuali hit)
        public void logic(Form1 ThisForm, InputManager iManager)
        {
            if (gameTime.ElapsedMilliseconds - upsTime > interval)
            {
                ThisForm.ball.Update(iManager, ThisForm.ParentForm);
                ThisForm.racchetta.Update(iManager, ThisForm.ParentForm);
                if (gameTime.Elapsed.Seconds != previousSecond)
                {
                    previousSecond = gameTime.Elapsed.Seconds;
                    ups = ups_tmp;
                    ups_tmp = 0;
                }
                upsTime = gameTime.ElapsedMilliseconds;
                ups_tmp++;
            }
        }
    }
}