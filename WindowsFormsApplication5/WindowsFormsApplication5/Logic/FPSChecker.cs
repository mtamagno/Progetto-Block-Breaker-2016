using System;
using System.Diagnostics;
using System.Windows.Forms;

namespace BlockBreaker
{
    internal class FpsChecker
    {
        #region Constructors

        /// <summary>
        ///     Funzione che avvia il timer
        /// </summary>
        /// <param name="timer"></param>
        public FpsChecker(Stopwatch timer)
        {
            if (timer == null) throw new ArgumentNullException(nameof(timer));
            _gameTime = timer;
        }

        #endregion Constructors

        #region Fields

        //variabili per fps
        private int _fps;

        private readonly Stopwatch _gameTime;

        //variabile per limitare gli fps
        public int Interval = 1000/85;

        public int PreviousSecond;
        public int Ups;

        //vairbili per ups
        public int UpsTmp;

        public long UpsTime;
        private int _fpsCounter;
        private long _fpsTime;

        #endregion Fields

        #region Methods

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
            try
            {
                controller?.Invoke(new MethodInvoker(delegate
                {
                    if (controller.ParentForm != null)
                        controller.ParentForm.Text = "BlockBreaker - Game      " + "    fps: " + _fps + "ups:" + Ups;
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