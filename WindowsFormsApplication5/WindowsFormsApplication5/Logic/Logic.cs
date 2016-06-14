using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public class Logic
    {
        #region Public Fields

        public SpriteBatch spriteBatch;
        public Stopwatch gameTime;
        public InputManager iManager;
        public HighScore highScore;
        public List<Keys> KeysHeld;
        public List<Keys> KeysPressed;
        public Point MousePoint;
        public float deltaTime;
        public int previous_score;
        public int righe_griglia;
        public int score;
        public int vita_rimanente;
        public bool AllowInput;
        public bool shouldStop;
        public volatile bool waitResize;

        #endregion Public Fields

        #region Private Fields

        private CheckLife checkLife = new CheckLife();
        private int activeBlock;
        private Game controller;
        private FPSChecker fpsChecker;

        #endregion Private Fields

        #region Public Constructors

        public Logic(Game form)
        {
            //inizializzo il controller
            this.controller = form;

            //inizializzo le variabili
            init();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Loop che costituisce il gioco vero e proprio
        /// </summary>
        public void gameLoop()
        {
            // Elimino i comandi premuti prima dell'inizio del gioco
            input();

            // Inizializza il timer del gioco
            gameTime.Start();

            // Crea il buffer

            // Finchè non si deve fermare continua ad eseguire
            while (shouldStop == false && vita_rimanente > 0)
            {
                if (controller.WindowState == FormWindowState.Minimized)
                {
                    controller.Pause();
                }

                if (waitResize == false)
                {

                        if (controller.WindowState != FormWindowState.Minimized)
                    {
                        // La pallina deve collidere di nuovo se era stata disabilitata la sua collisione
                        controller.ball.canCollide = true;

                        // Controlla le vite che rimangono al giocatore
                        vita_rimanente = checkLife.check(controller, vita_rimanente);

                        // Altrimenti controlla che sia passato un secondo dall'ultimo check di punteggio e blocchi attivi, e in caso chiama la funzione
                        if (gameTime.ElapsedMilliseconds % 1000 != 0)
                        {
                            checkscore();
                            checkActiveBlock();
                        }

                        // Controlla gli fps contandoli e vede se è il caso di stamparli

                        fpsChecker.checkfps(controller);

                        this.updater(this.controller, this.iManager, fpsChecker);
                        render();
                    }
                }
            }

            // Se non ne rimangono segnala con la variabile shouldStop che si deve visualizzare la schermata GameOver
            if (vita_rimanente <= 0)
            {
                gameover();
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void gameover()
        {
            // Salva lo score
            this.highScore.Score = score;
            //comunico al gioco che le vite sono finite
            controller.lifeEnd();
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            return;

        }

        //funzione per ridimensionare gli elementi
        public void resize(int li, int hi, int l, int h)
        {
            if (vita_rimanente > 0 && h > 0 && hi > 0 && l > 0 && li > 0)               
            {
                //controllo tutti gli sprite che sono in gioco
                foreach (Sprite s in iManager.inGameSprites)
                {
                    //ridimensiono la pallina
                    if (s.GetType().Name == "Ball")
                    {

                            Ball myBall = (Ball)s;
                            if (myBall.X > 1000 && myBall.Y < 0)
                                s.X = myBall.previousX;
                            if (myBall.Y == 0)
                                s.Y = myBall.previousY;
                        

                        s.redraw(s,
                            (int)(Math.Abs((float)1 / 50 * Math.Min(l, h))),
                            (int)(Math.Abs((float)1 / 50 * Math.Min(l, h))),
                            Properties.Resources.Ball,
                            s.X * l / li,
                            s.Y * h / hi);
                    }

                    //ridimensiono la racchetta
                    else if (s.GetType().Name == "Paddle")
                    {
                        s.redraw(s, (int)(Math.Abs((float)1 / 8 * l)),
                            (int)(Math.Abs((float)1 / 15 * h)),
                            Properties.Resources.New_Piskel,
                            s.X * l / li,
                            s.Y * h / hi);
                    }

                    //ridimensiono lo sfondo
                    else if (s.GetType().Name == "View")
                    {
                        s.redraw(s,
                            l / 30 * 29,
                            h / 5 * 4,
                            Properties.Resources.Schermo_800_600_GBA,
                            0,
                            0);
                        s.X = controller.ClientRectangle.Width / 2 - s.Width / 2;
                        s.Y = controller.ClientRectangle.Height / 2 - s.Height / 2;
                    }

                    //ridimensiono la vita
                    else if (s.GetType().Name == "Life")
                    {
                        s.redraw(s,
                            (int)(Math.Abs((float)1 / 25 * Math.Min(l, h))),
                            (int)(Math.Abs((float)1 / 25 * Math.Min(l, h))),
                            Properties.Resources.Life, s.X * l / li, s.Y * h / hi);
                    }

                    else if (s.GetType().Name == "Skin")
                    {
                        s.redraw(s,
                            l,
                            h,
                            Properties.Resources.Skin,
                            0,
                            0);
                    }


                }

                controller.grid.redraw_grid(controller.grid, controller.background.Height, controller.background.Width);

                foreach (Sprite s in iManager.inGameSprites)
                {
                    //ridimensiono i blocchi di gioco
                    if (hi > 0 && li > 0)
                    {
                        if (s.GetType().Name == "Block")
                        {
                            controller.grid.redraw_block((Block)s,
                                (100 * l / li),
                                (50 * (h / hi)),
                                s.X * l / li,
                                s.Y * h / hi);
                        }
                    }
                }

                controller.racchetta.Y = h * 9 / 10;


                spriteBatch.cntxt.MaximumBuffer = new Size(controller.ClientSize.Width + 1, controller.ClientSize.Height + 1);
                spriteBatch.bfgfx = spriteBatch.cntxt.Allocate(controller.CreateGraphics(), new Rectangle(Point.Empty, controller.ClientSize));
                spriteBatch.Gfx = controller.CreateGraphics();

                //uso il garbage collector per pulire
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.Collect();
            }
        }
        #endregion Public Methods

        #region Private Methods

        //inizializzo le variabili di gioco
        private void init()
        {
            this.vita_rimanente = 3;
            this.gameTime = new Stopwatch();
            this.fpsChecker = new FPSChecker(this.gameTime);
            this.iManager = new InputManager();
            this.highScore = new HighScore();
            this.KeysHeld = new List<Keys>();
            this.KeysPressed = new List<Keys>();
            this.shouldStop = false;
            this.vita_rimanente = 3;
            this.score = 0;
            this.righe_griglia = 25;
            this.previous_score = 0;
            this.spriteBatch = new SpriteBatch(controller.ClientSize, controller.CreateGraphics());
        }

        //controllo quanti blocchi sono rimasti in gioco
        private void checkActiveBlock()
        {
            if (activeBlock == 0)
            {
                controller.grid.insert_grid(Properties.Resources.Block_4, this.iManager);
            }
        }

        //setto lo score dell utente
        private void checkscore()
        {

                previous_score = score;
                activeBlock = 0;
                foreach (Sprite s in iManager.inGameSprites)
                {
                    if (s.GetType().Name == "Block")
                    {
                        Block myBlock = (Block)s;
                        if (myBlock.blockLife == 0)
                        {
                            score += myBlock.initialLife;
                            myBlock.blockLife = -1;
                        }
                        if (myBlock.blockLife > 0)
                        {
                            activeBlock++;
                        }
                    }
                }
                if (previous_score < score)
                {
                    controller.Invoke(new MethodInvoker(delegate
                    {
                        controller.score.Text = "Score: " + score;
                    }));
                }
            
        }
        /// <summary>
        /// Funzione che svuota il buffer creato quando il Thread Game non è ancora partito ma si è spinto qualcosa
        /// </summary>
        private void input()
        {
            AllowInput = false;

            // Controlla i tasti che sono stati premuti e svuoto i buffer
            iManager.update(MousePoint, KeysPressed.ToArray(), KeysHeld.ToArray(), gameTime, deltaTime);
            KeysPressed.Clear();
            KeysHeld.Clear();
            AllowInput = true;
        }

        /// <summary>
        /// Funzione render che disegna nella posizione giusta e aggiorna il buffer
        /// </summary>
        private void render()
        {
                spriteBatch.Begin();
                foreach (Sprite s in iManager.inGameSprites)
                {
                if (s.torender == true)
                {
                    if (s.GetType().Name == "Ball")
                    {
                        Ball myBall = (Ball)s;
                        if (myBall.X > 1000 && myBall.Y < 0)
                            s.X = myBall.previousX;
                        if (myBall.Y == 0)
                            s.Y = myBall.previousY;
                    }
                    spriteBatch.Draw(s);
                }
                }
                spriteBatch.End();           
        }

        /// <summary>
        /// Funzione che calcola la logica e gli ups (Updates per second , cioè aggiornamento delle posizioni e calcolo di eventuali hit)
        /// </summary>
        private void updater(Game ThisForm, InputManager iManager, FPSChecker fpsChecker)
        {
            if (vita_rimanente > 0)
            {
                if (gameTime.ElapsedMilliseconds - fpsChecker.upsTime > fpsChecker.interval)
                {
                    ThisForm.ball.Update(iManager, ThisForm.ParentForm);
                    ThisForm.racchetta.Update(iManager, ThisForm.ParentForm);
                    if (gameTime.Elapsed.Seconds != fpsChecker.previousSecond)
                    {
                        fpsChecker.previousSecond = gameTime.Elapsed.Seconds;
                        fpsChecker.ups = fpsChecker.ups_tmp;
                        fpsChecker.ups_tmp = 0;
                    }
                    fpsChecker.upsTime = gameTime.ElapsedMilliseconds;
                    fpsChecker.ups_tmp++;
                }
            }
        }
    }

    #endregion Private Methods
}