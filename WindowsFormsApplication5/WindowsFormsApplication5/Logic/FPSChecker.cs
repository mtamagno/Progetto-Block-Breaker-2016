using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BlockBreaker
{
    internal class FpsInit
    {
        #region PubliFields

        public int Limiter = 1000 / 85;
        public int PreviousSecond;
        public int Ups;
        public long UpsTime;
        public int UpsCounter;

        #endregion Public Fields

        #region Private Fields

        private readonly Stopwatch _gameTime;
        private int _fps;
        private int _fpsCounter;
        private long _fpsTime;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        /// Funzione che prende il timer dal Form Game
        /// </summary>
        /// <param name="timer"></param>
        public FpsInit(Stopwatch timer)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));
            _gameTime = timer;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Funzione per il check degli fps, li conta per poi resettare il totale e restituire il risultato ogni secondo
        /// </summary>
        /// <param name="controller"></param>
        public void CheckFps_Ups(Game controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (_gameTime.ElapsedMilliseconds - _fpsTime > 1000)
            {
                _fpsTime = _gameTime.ElapsedMilliseconds;
                _fps = _fpsCounter;
                _fpsCounter = 0;
                Writer(controller);
            }
            else
            {
                _fpsCounter++;
            }
        }

        /// <summary>
        /// Funzione che scrive sul form il numero degli fps e degli ups
        /// </summary>
        /// <param name="controller"></param>
        public void Writer(Game controller)
        {
            if (controller.ParentForm != null)
                try
            {
                controller?.Invoke(new MethodInvoker(delegate
                {                  
                        controller.ParentForm.Text = "BlockBreaker - Game      " + "    fps: " + _fps + "ups:" + Ups;
                }));
            }
            catch
            {
                    // Prende l'eccezione senza fare nulla poichè basta aspettare che il form si chiuda se non si
                    // riesce a scrivere, dato che vuol dire che sta temrinando
            }
        }

        #endregion Public Methods
    }
}
