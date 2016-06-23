using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BlockBreaker
{
    internal class FpsChecker
    {
        #region Public Fields

        //variabile per limitare gli fps
        public int Interval = 1000 / 85;
        public int PreviousSecond;
        public int Ups;
        public long UpsTime;

        //vairbili per ups
        public int UpsTmp;

        #endregion Public Fields

        #region Private Fields

        private readonly Stopwatch _gameTime;

        //variabili per fps
        private int _fps;
        private int _fpsCounter;
        private long _fpsTime;

        #endregion Private Fields

        #region Public Constructors

        /// <summary>
        ///     Funzione che avvia il timer
        /// </summary>
        /// <param name="timer"></param>
        public FpsChecker(Stopwatch timer)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));
            _gameTime = timer;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Funzione per il check degli fps, li conta per poi resettare il totale e restituire il risultato ogni secondo
        /// </summary>
        /// <param name="controller"></param>
        public void Checkfps(Game controller)
        {
            if (controller == null) throw new ArgumentNullException(nameof(controller));
            if (_gameTime.ElapsedMilliseconds - _fpsTime > 1000)
            {
                _fpsTime = _gameTime.ElapsedMilliseconds;
                _fps = _fpsCounter;
                _fpsCounter = 0;
                FpsWriter(controller);
            }
            else
            {
                _fpsCounter++;
            }
        }

        /// <summary>
        ///     Funzione che scrive sul form il numero degli fps e degli ups
        /// </summary>
        /// <param name="controller"></param>
        public void FpsWriter(Game controller)
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
                // Gestisce l'errore in caso si arrivi qui ma non si possa invocare il form attivo perchè è stato chiuso
            }
        }

        #endregion Public Methods
    }
}
