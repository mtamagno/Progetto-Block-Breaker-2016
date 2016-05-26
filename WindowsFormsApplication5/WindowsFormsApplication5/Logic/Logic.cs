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
        public Stopwatch gameTime = new Stopwatch();
        public InputManager iManager = new InputManager();
        public HighScore highscore = new HighScore();
        public HighScoreCollection highscorecollection = new HighScoreCollection(); public List<Keys> KeysHeld = new List<Keys>();
        public List<Keys> KeysPressed = new List<Keys>();
        public Point MousePoint;
        public float deltaTime;
        public int previous_score = 0;
        public int righe_griglia = 25;
        public int score = 0;
        public int vita_rimanente = 3;
        public bool AllowInput;
        public bool shouldStop = false;

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
            this.controller = form;
            this.vita_rimanente = 3;
            this.fpsChecker = new FPSChecker(this.gameTime);
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
            spriteBatch = new SpriteBatch(controller.ClientSize, controller.CreateGraphics());

            // Finchè non si deve fermare continua ad eseguire
            while (shouldStop == false)
            {
                // La pallina deve collidere di nuovo se era stata disabilitata la sua collisione
                controller.ball.canCollide = true;

                // Controlla le vite che rimangono al giocatore
                vita_rimanente = checkLife.check(controller, vita_rimanente);

                // Se non ne rimangono segnala con la variabile shouldStop che si deve visualizzare la schermata GameOver
                if (vita_rimanente <= 0)
                {
                    shouldStop = true;

                    // Salva lo score
                    this.highscore.Score = score;

                    // Ferma la musica e termina il thread
                    controller.backgroundMusic.Stop();
                    return;
                }

                // Altrimenti controlla che sia passato un secondo dall'ultimo check di punteggio e blocchi attivi, e in caso chiama la funzione
                if (gameTime.ElapsedMilliseconds % 1000 != 0)
                {
                    checkscore();
                    checkActiveBlock();
                }

                // Controlla gli fps contandoli e vede se è il caso di stamparli
                fpsChecker.checkfps(controller);
                
                //
                this.updater(this.controller, this.iManager, fpsChecker);
                render();
            }
        }

        public void resize(int li, int hi, int l, int h)
        {
            controller.grid.redraw_grid(controller.grid, controller.ClientRectangle.Height, controller.ClientRectangle.Width);
            foreach (Sprite s in iManager.inGameSprites)
            {
                if (s.GetType().Name == "Ball")
                {
                    s.redraw(s, (int)(Math.Abs(10 * l / li)), (10 * h / hi), Properties.Resources.ball, s.X * l / li, s.Y * h / hi);
                }
                else if (s.GetType().Name == "Paddle")
                {
                    s.redraw(s, (int)(Math.Abs(128 * l / li)), (24 * h / hi), Properties.Resources.New_Piskel, s.X * l / li, s.Y * h / hi);
                }
                else if (s.GetType().Name == "View")
                {
                    s.redraw(s, l, h, Properties.Resources.Background, 0, 0);
                }
                else if (s.GetType().Name == "Block")
                {
                    controller.grid.redraw_block((Block)s, (100 * l / li), (50 * (h / hi)), s.X * l / li, s.Y * h / hi);
                }
                else if (s.GetType().Name == "Life")
                {
                    s.redraw(s, (int)(Math.Abs(20 * l / li)), (int)(Math.Abs(20 * h / hi)), Properties.Resources.vita, s.X * l / li, s.Y * h / hi);
                }
            }
            controller.racchetta.Y = h * 9 / 10;
            spriteBatch.cntxt.MaximumBuffer = new Size(controller.ClientSize.Width + 1, controller.ClientSize.Height + 1);
            spriteBatch.bfgfx = spriteBatch.cntxt.Allocate(controller.CreateGraphics(), new Rectangle(Point.Empty, controller.ClientSize));
            spriteBatch.Gfx = controller.CreateGraphics();
        }

        #endregion Public Methods

        #region Private Methods

        private void checkActiveBlock()
        {
            if (activeBlock == 0)
            {
                controller.grid.insert_grid(Properties.Resources.Block_4, this.iManager);
            }
        }

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
                Console.WriteLine(score);
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
                if (s.torender == true)
                    spriteBatch.Draw(s);
            spriteBatch.End();
        }

        /// <summary>
        /// Funzione che calcola la logica e gli ups (Updates per second , cioè aggiornamento delle posizioni e calcolo di eventuali hit)
        /// </summary>
        private void updater(Game ThisForm, InputManager iManager, FPSChecker fpsChecker)
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

        #endregion Private Methods
    }