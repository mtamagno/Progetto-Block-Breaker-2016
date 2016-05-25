using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    internal class FPSChecker
    {
        //variabili per fps
        public int fps;

        private int fpsCounter;
        private long fpsTime;
        public long lastTime;

        //variabile per limitare gli fps
        public int interval = 1000 / 70;

        //vairbili per ups
        public int ups_tmp;

        public int ups;
        public long upsTime;
        public Stopwatch gameTime;
        public int previousSecond;
        public float deltaTime;

        /// <summary>
        /// Funzione che avvia il timer
        /// </summary>
        /// <param name="timer"></param>
        public FPSChecker(Stopwatch timer)
        {
            this.gameTime = timer;
        }


        /// <summary>
        /// Funzione per il check degli fps, li conta per poi resettare il totale e restituire il risultato ogni secondo
        /// </summary>
        /// <param name="controller"></param>
        public void checkfps(Game controller)
        {
            if (gameTime.ElapsedMilliseconds - fpsTime > 1000)
            {
                fpsTime = gameTime.ElapsedMilliseconds;
                fps = fpsCounter;
                fpsCounter = 0;
                fpsWriter(controller);
            }
            else
            {
                fpsCounter++;
            }
            deltaTime = gameTime.ElapsedMilliseconds - lastTime;
            lastTime = gameTime.ElapsedMilliseconds;
        }

        /// <summary>
        /// Funzione che scrive sul form il numero degli fps e degli ups
        /// </summary>
        /// <param name="controller"></param>
        public void fpsWriter(Game controller)
        {
            try
            {
                controller.Invoke(new MethodInvoker(delegate
                {
                    if (controller.ParentForm != null)
                        controller.ParentForm.Text = "fps: " + this.fps.ToString() + "ups:" + this.ups.ToString();
                }));
            }
            catch
            {
                // Gestisce l'errore in caso si arrivi qui ma non si possa invocare il form attivo perchè è stato chiuso
            }

        }
    }
}