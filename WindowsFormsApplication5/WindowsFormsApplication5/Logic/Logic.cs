using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using BlockBreaker.Properties;

namespace BlockBreaker
{
    public class Logic
    {
        #region Public Constructors

        public Logic(Game form)
        {
            if (form == null) throw new ArgumentNullException(nameof(form));
            //inizializzo il controller
            _controller = form;

            //inizializzo le variabili
            Init();
        }

        #endregion Public Constructors

        #region Public Fields

        public SpriteBatch SpriteBatch;
        public Stopwatch GameTime;
        public InputManager IManager;
        public HighScore HighScore;
        public List<Keys> KeysHeld;
        public List<Keys> KeysPressed;
        public Point MousePoint;
        public float DeltaTime;
        public int PreviousScore;
        public int RigheGriglia;
        public int Score;
        public int VitaRimanente;
        public bool AllowInput;
        public bool ShouldStop;
        public volatile bool WaitResize;

        #endregion Public Fields

        #region Private Fields

        private readonly CheckLife _checkLife = new CheckLife();
        private int _activeBlock;
        private readonly Game _controller;
        private FpsChecker _fpsChecker;

        #endregion Private Fields

        #region Public Methods

        /// <summary>
        ///     Loop che costituisce il gioco vero e proprio
        /// </summary>
        public void GameLoop()
        {
            // Elimino i comandi premuti prima dell'inizio del gioco
            Input();

            // Inizializza il timer del gioco
            GameTime.Start();

            // Crea il buffer

            // Finchè non si deve fermare continua ad eseguire
            while (ShouldStop == false && VitaRimanente > 0)
            {
                if (_controller.WindowState == FormWindowState.Minimized)
                {
                    _controller.Pause();
                }

                if (WaitResize == false)
                {
                    if (_controller.WindowState != FormWindowState.Minimized)
                    {
                        // La pallina deve collidere di nuovo se era stata disabilitata la sua collisione
                        _controller.Ball.CanCollide = true;

                        // Controlla le vite che rimangono al giocatore
                        VitaRimanente = _checkLife.Check(_controller, VitaRimanente);

                        // Altrimenti controlla che sia passato un secondo dall'ultimo check di punteggio e blocchi attivi, e in caso chiama la funzione
                        if (GameTime.ElapsedMilliseconds%1000 != 0)
                        {
                            Checkscore();
                            CheckActiveBlock();
                        }

                        // Controlla gli fps contandoli e vede se è il caso di stamparli

                        _fpsChecker.Checkfps(_controller);

                        Updater(_controller, IManager, _fpsChecker);
                        Render();
                    }
                }
            }

            // Se non ne rimangono segnala con la variabile shouldStop che si deve visualizzare la schermata GameOver
            if (VitaRimanente <= 0)
            {
                foreach (var s in IManager.InGameSprites)
                {
                    s.ToRender = false;
                }
                Gameover();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
            foreach (var s in IManager.InGameSprites)
            {
                s.ToRender = false;
            }
        }

        public void Gameover()
        {
            // Salva lo score
            HighScore.Score = Score;
            //comunico al gioco che le vite sono finite
            _controller.lifeEnd();
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
                foreach (var s in IManager.InGameSprites)
                {
                    //ridimensiono la pallina
                    if (s.GetType().Name == "Ball")
                    {
                        var myBall = (Ball) s;
                        if (myBall.X > 1000 && myBall.Y < 0)
                            s.X = myBall.PreviousX;
                        if (myBall.Y == 0)
                            s.Y = myBall.PreviousY;


                        s.Redraw(s,
                            (int) Math.Abs((float) 1/50*Math.Min(l, h)),
                            (int) Math.Abs((float) 1/50*Math.Min(l, h)),
                            Resources.Ball,
                            s.X*l/li,
                            s.Y*h/hi);
                    }

                    //ridimensiono la racchetta
                    else if (s.GetType().Name == "Paddle")
                    {
                        s.Redraw(s, (int) Math.Abs((float) 1/8*l),
                            (int) Math.Abs((float) 1/15*h),
                            Resources.New_Piskel,
                            s.X*l/li,
                            s.Y*h/hi);
                    }

                    //ridimensiono lo sfondo
                    else if (s.GetType().Name == "Playground")
                    {
                        s.Redraw(s,
                            l/30*29,
                            h/5*4,
                            Resources.Schermo_800_600_GBA,
                            0,
                            0);
                        s.X = _controller.ClientRectangle.Width/2 - s.Width/2;
                        s.Y = _controller.ClientRectangle.Height/2 - s.Height/2;
                    }

                    // Ridimensiono la vita
                    else if (s.GetType().Name == "Life")
                    {
                        s.Redraw(s,
                            (int) Math.Abs((float) 1/25*Math.Min(l, h)),
                            (int) Math.Abs((float) 1/25*Math.Min(l, h)),
                            Resources.Life, s.X*l/li, s.Y*h/hi);
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
                _controller.Grid.redraw_grid(_controller.Grid, _controller.Background.Height,
                    _controller.Background.Width);

                //Per ogni sprite in iManager.inGameSprites, ridimensiono lo sprite
                foreach (var s in IManager.InGameSprites)
                {
                    //ridimensiono i blocchi di gioco
                    if (hi > 0 && li > 0)
                    {
                        if (s.GetType().Name == "Block")
                        {
                            _controller.Grid.redraw_block((Block) s,
                                100*l/li,
                                50*(h/hi),
                                s.X*l/li,
                                s.Y*h/hi);
                        }
                    }
                }

                // Sposto la racchetta all'altezza giusta
                _controller.Racchetta.Y = h*9/10;

                // Ridimensiono la superfice di disegno
                SpriteBatch.Cntxt.MaximumBuffer = new Size(_controller.ClientSize.Width + 1,
                    _controller.ClientSize.Height + 1);
                SpriteBatch.Bfgfx = SpriteBatch.Cntxt.Allocate(_controller.CreateGraphics(),
                    new Rectangle(Point.Empty, _controller.ClientSize));
                SpriteBatch.Gfx = _controller.CreateGraphics();

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
            GameTime = new Stopwatch();
            _fpsChecker = new FpsChecker(GameTime);
            IManager = new InputManager();
            HighScore = new HighScore();
            KeysHeld = new List<Keys>();
            KeysPressed = new List<Keys>();
            ShouldStop = false;
            VitaRimanente = 3;
            Score = 0;
            RigheGriglia = 25;
            PreviousScore = 0;
            SpriteBatch = new SpriteBatch(_controller.ClientSize, _controller.CreateGraphics());
        }

        /// <summary>
        ///     Funzione per il controllo di quanti blocchi sono rimasti in gioco
        /// </summary>
        private void CheckActiveBlock()
        {
            if (_activeBlock == 0)
            {
                _controller.Grid.insert_grid(Resources.Block_4, IManager);
            }
        }

        /// <summary>
        ///     Funzione per il check dello score dell'utente
        /// </summary>
        private void Checkscore()
        {
            PreviousScore = Score;
            _activeBlock = 0;
            foreach (var s in IManager.InGameSprites)
            {
                if (s.GetType().Name == "Block")
                {
                    var myBlock = (Block) s;
                    if (myBlock.BlockLife == 0)
                    {
                        Score += myBlock.InitialLife;
                        myBlock.BlockLife = -1;
                    }
                    if (myBlock.BlockLife > 0)
                    {
                        _activeBlock++;
                    }
                }
            }
            if (PreviousScore < Score)
            {
                _controller.Invoke(new MethodInvoker(delegate { _controller.Score.Text = "Score: " + Score; }));
            }
        }

        /// <summary>
        ///     Funzione che svuota il buffer creato quando il Thread Game non è ancora partito ma si è spinto qualcosa
        /// </summary>
        private void Input()
        {
            AllowInput = false;

            // Controlla i tasti che sono stati premuti e svuoto i buffer
            IManager.update(MousePoint, KeysPressed.ToArray(), KeysHeld.ToArray(), GameTime, DeltaTime);
            KeysPressed.Clear();
            KeysHeld.Clear();
            AllowInput = true;
        }

        /// <summary>
        ///     Funzione render che disegna nella posizione giusta e aggiorna il buffer
        /// </summary>
        private void Render()
        {
            SpriteBatch.Clear();
            foreach (var s in IManager.InGameSprites)
            {
                if (s.ToRender)
                {
                    if (s.GetType().Name == "Ball")
                    {
                        var myBall = (Ball) s;
                        if (myBall.X > 1000 && myBall.Y < 0)
                            s.X = myBall.PreviousX;
                        if (myBall.Y == 0)
                            s.Y = myBall.PreviousY;
                    }
                    SpriteBatch.Draw(s);
                }
            }
            SpriteBatch.End();
        }

        /// <summary>
        ///     Funzione che calcola la logica e gli ups (Updates per second , cioè aggiornamento delle posizioni e calcolo di
        ///     eventuali hit)
        /// </summary>
        private void Updater(Game thisForm, InputManager iManager, FpsChecker fpsChecker)
        {
            if (VitaRimanente <= 0) return;
            if (GameTime.ElapsedMilliseconds - fpsChecker.UpsTime <= fpsChecker.Interval) return;
            thisForm.Ball.Update(iManager, thisForm.ParentForm);
            thisForm.Racchetta.Update(iManager, thisForm.ParentForm);
            if (GameTime.Elapsed.Seconds != fpsChecker.PreviousSecond)
            {
                fpsChecker.PreviousSecond = GameTime.Elapsed.Seconds;
                fpsChecker.Ups = fpsChecker.UpsTmp;
                fpsChecker.UpsTmp = 0;
            }
            fpsChecker.UpsTime = GameTime.ElapsedMilliseconds;
            fpsChecker.UpsTmp++;
        }
    }

    #endregion Private Methods
}