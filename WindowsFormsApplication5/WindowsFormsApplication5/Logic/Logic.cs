using BlockBreaker.Properties;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public class Logic
    {
        #region Public Constructors

        public Logic(Game form)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));

            //inizializzo il controller
            _myGame = form;

            //inizializzo le variabili
            Init();
        }

        #endregion Public Constructors

        #region Public Fields

        public SpriteBatch MySpriteBatch;
        public Stopwatch MyGameTime;
        public InputManager MyIManager;
        public HighScore MyHighScore;
        public List<Keys> KeysHeld;
        public List<Keys> KeysPressed;
        public Point MousePoint;
        public int PreviousScore;
        public int Score;
        public int VitaRimanente;
        public bool AllowInput;
        public bool ShouldStop;
        public volatile bool WaitResize;

        #endregion Public Fields

        #region Private Fields

        private readonly LifeChecker _myLifeChecker = new LifeChecker();
        private int _activeBlocks;
        private readonly Game _myGame;
        private FpsChecker _myFpsChecker;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        /// Loop che costituisce il gioco vero e proprio
        /// </summary>
        public void GameLoop()
        {
            // Elimino i comandi premuti prima dell'inizio del gioco
            Input();

            // Inizializza il timer del gioco
            MyGameTime.Start();

            // Crea il buffer
            // Finchè non si deve fermare continua ad eseguire
            while (ShouldStop == false && VitaRimanente > 0)
            {
                if (_myGame.WindowState == FormWindowState.Minimized)
                {
                    _myGame.Pause();
                }
                if (WaitResize == false)
                {
                    if (_myGame.WindowState != FormWindowState.Minimized)
                    {
                        // La pallina deve collidere di nuovo se era stata disabilitata la sua collisione
                        _myGame.MyBall.CanCollide = true;

                        // Controlla le vite che rimangono al giocatore
                        VitaRimanente = _myLifeChecker.Check(_myGame, VitaRimanente);

                        // Altrimenti controlla che sia passato un secondo dall'ultimo check di punteggio e blocchi attivi, e in caso chiama la funzione
                        if (MyGameTime.ElapsedMilliseconds % 1000 != 0)
                        {
                            Checkscore();
                            CheckActiveBlock();
                        }

                        // Controlla gli fps contandoli e vede se è il caso di stamparli
                        _myFpsChecker.Checkfps(_myGame);
                        Updater(_myGame, MyIManager, _myFpsChecker);
                        Render();
                    }
                }
            }

            // Se non ne rimangono segnala con la variabile shouldStop che si deve visualizzare la schermata GameOver
            if (VitaRimanente <= 0)
            {
                foreach (var s in MyIManager.InGameSprites)
                {
                    s.ToRender = false;
                }
                Gameover();
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            foreach (var s in MyIManager.InGameSprites)
            {
                s.ToRender = false;
            }
        }

        public void Gameover()
        {
            // Salva lo score
            MyHighScore.Score = Score;

            //comunico al gioco che le vite sono finite
            _myGame.LifeEnd();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
        }

        //funzione per ridimensionare gli elementi
        public void Resize(int li, int hi, int l, int h)
        {
            if (VitaRimanente > 0 && h > 0 && hi > 0 && l > 0 && li > 0)
            {
                //controllo tutti gli sprite che sono in gioco
                foreach (var s in MyIManager.InGameSprites)
                {
                    //ridimensiono la pallina
                    if (s.GetType().Name == "MyBall")
                    {
                        var myBall = (Ball)s;
                        if (myBall.X > 1000 && myBall.Y < 0)
                            s.X = myBall.PreviousX;
                        if (myBall.Y == 0)
                            s.Y = myBall.PreviousY;
                        s.Redraw(s,
                                (int)Math.Abs((float)1 / 50 * Math.Min(l, h)),
                                (int)Math.Abs((float)1 / 50 * Math.Min(l, h)),
                                Resources.Ball,
                                s.X * l / li,
                                s.Y * h / hi);
                    }

                    //ridimensiono la racchetta
                    else if (s.GetType().Name == "Racket")
                    {
                        s.Redraw(s, (int)Math.Abs((float)1 / 8 * l),
                            (int)Math.Abs((float)1 / 15 * h),
                            Resources.New_Piskel,
                            s.X * l / li,
                            s.Y * h / hi);
                    }

                    //ridimensiono lo sfondo
                    else if (s.GetType().Name == "Playground")
                    {
                        s.Redraw(s,
                                l / 30 * 29,
                                h / 5 * 4,
                                Resources.Schermo_800_600_GBA,
                            0,
                            0);
                        s.X = _myGame.ClientRectangle.Width / 2 - s.Width / 2;
                        s.Y = _myGame.ClientRectangle.Height / 2 - s.Height / 2;
                    }

                    // Ridimensiono la vita
                    else if (s.GetType().Name == "Life")
                    {
                        s.Redraw(s,
                                (int)Math.Abs((float)1 / 25 * Math.Min(l, h)),
                                (int)Math.Abs((float)1 / 25 * Math.Min(l, h)),
                                Resources.Life, s.X * l / li, s.Y * h / hi);
                    }

                    // Ridimensiono la skin
                    else if (s.GetType().Name == "Skin")
                    {
                        s.Redraw(s,
                            l,
                            h,
                                Resources.Skin,
                            0,
                            0);
                    }
                }

                // Ridimensiono la griglia
                _myGame.MyBlockGrid.redraw_grid(_myGame.MyBlockGrid, _myGame.MyPlayground.Height,
                    _myGame.MyPlayground.Width);

                //Per ogni sprite in iManager.inGameSprites, ridimensiono lo sprite
                foreach (var s in MyIManager.InGameSprites)
                {
                    //ridimensiono i blocchi di gioco
                    if (hi > 0 && li > 0)
                    {
                        if (s.GetType().Name == "Block")
                        {
                            _myGame.MyBlockGrid.redraw_block((Block)s,
                                100 * l / li,
                                50 * (h / hi),
                                s.X * l / li,
                                s.Y * h / hi);
                        }
                    }
                }

                // Sposto la racchetta all'altezza giusta
                _myGame.MyRacket.Y = h * 9 / 10;

                // Ridimensiono la superfice di disegno
                MySpriteBatch.Cntxt.MaximumBuffer = new Size(_myGame.ClientSize.Width + 1,
                    _myGame.ClientSize.Height + 1);
                MySpriteBatch.Bfgfx = MySpriteBatch.Cntxt.Allocate(_myGame.CreateGraphics(),
                    new Rectangle(Point.Empty, _myGame.ClientSize));
                MySpriteBatch.Gfx = _myGame.CreateGraphics();

                //Uso il garbage collector per pulire
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }

        #endregion Public Methods

        #region Private Methods

        /// <summary>
        ///     Funzione per l'inizializzazione del gioco
        /// </summary>
        private void Init()
        {
            VitaRimanente = 3;
            MyGameTime = new Stopwatch();
            _myFpsChecker = new FpsChecker(MyGameTime);
            MyIManager = new InputManager();
            MyHighScore = new HighScore();
            KeysHeld = new List<Keys>();
            KeysPressed = new List<Keys>();
            ShouldStop = false;
            VitaRimanente = 3;
            Score = 0;
            PreviousScore = 0;
            MySpriteBatch = new SpriteBatch(_myGame.ClientSize, _myGame.CreateGraphics());
        }

        /// <summary>
        ///     Funzione per il controllo di quanti blocchi sono rimasti in gioco
        /// </summary>
        private void CheckActiveBlock()
        {
            if (_activeBlocks == 0)
            {
                _myGame.MyBlockGrid.insert_grid(Resources.Block_4, MyIManager);
            }
        }

        /// <summary>
        ///     Funzione per il check dello score dell'utente
        /// </summary>
        private void Checkscore()
        {
            PreviousScore = Score;
            _activeBlocks = 0;
            foreach (var s in MyIManager.InGameSprites)
            {
                if (s.GetType().Name == "Block")
                {
                    var myBlock = (Block)s;
                    if (myBlock.BlockLife == 0)
                    {
                        Score += myBlock.InitialLife;
                        myBlock.BlockLife = -1;
                    }
                    if (myBlock.BlockLife > 0)
                    {
                        _activeBlocks++;
                    }
                }
            }
            if (PreviousScore < Score)
            {
                _myGame.Invoke(new MethodInvoker(delegate { _myGame.MyScore.Text = "Score: " + Score; }));
            }
        }

        /// <summary>
        ///     Funzione che svuota il buffer creato quando il Thread Game non è ancora partito ma si è spinto qualcosa
        /// </summary>
        private void Input()
        {
            AllowInput = false;

            // Controlla i tasti che sono stati premuti e svuoto i buffer
            MyIManager.update(MousePoint, KeysPressed.ToArray(), KeysHeld.ToArray());
            KeysPressed.Clear();
            KeysHeld.Clear();
            AllowInput = true;
        }

        /// <summary>
        ///     Funzione render che disegna nella posizione giusta e aggiorna il buffer
        /// </summary>
        private void Render()
        {
            MySpriteBatch.Clear();
            foreach (var s in MyIManager.InGameSprites)
            {
                if (s.ToRender)
                {
                    if (s.GetType().Name == "MyBall")
                    {
                        var myBall = (Ball)s;
                        if (myBall.X > 1000 && myBall.Y < 0)
                            s.X = myBall.PreviousX;
                        if (myBall.Y == 0)
                            s.Y = myBall.PreviousY;
                    }
                    MySpriteBatch.Draw(s);
                }
            }
            MySpriteBatch.End();
        }

        /// <summary>
        ///     Funzione che calcola la logica e gli ups (Updates per second , cioè aggiornamento delle posizioni e calcolo di
        ///     eventuali hit)
        /// </summary>
        private void Updater(Game thisForm, InputManager iManager, FpsChecker fpsChecker)
        {
            if (VitaRimanente <= 0) return;
            if (MyGameTime.ElapsedMilliseconds - fpsChecker.UpsTime <= fpsChecker.Interval) return;
            thisForm.MyBall.Update(iManager, thisForm.ParentForm);
            thisForm.MyRacket.Update(iManager, thisForm.ParentForm);
            if (MyGameTime.Elapsed.Seconds != fpsChecker.PreviousSecond)
            {
                fpsChecker.PreviousSecond = MyGameTime.Elapsed.Seconds;
                fpsChecker.Ups = fpsChecker.UpsTmp;
                fpsChecker.UpsTmp = 0;
            }
            fpsChecker.UpsTime = MyGameTime.ElapsedMilliseconds;
            fpsChecker.UpsTmp++;
        }
    }

    #endregion Private Methods
}
