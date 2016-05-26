using System.Diagnostics;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    internal class FPSChecker
    {
        #region Fields

        public float deltaTime;

        //variabili per fps
        public int fps;

        public Stopwatch gameTime;

        //variabile per limitare gli fps
        public int interval = 1000 / 70;

        public long lastTime;
        public int previousSecond;
        public int ups;

        //vairbili per ups
        public int ups_tmp;

        public long upsTime;
        private int fpsCounter;
        private long fpsTime;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Funzione che avvia il timer
        /// </summary>
        /// <param name="timer"></param>
        public FPSChecker(Stopwatch timer)
        {
            this.gameTime = timer;
        }

        #endregion Constructors

        #region Methods

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

        #endregion Methods
    }
}