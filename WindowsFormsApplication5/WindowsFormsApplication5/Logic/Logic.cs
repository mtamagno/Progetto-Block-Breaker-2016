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

        public bool AllowInput;
        public float deltaTime;
        public Stopwatch gameTime = new Stopwatch();
        public InputManager iManager = new InputManager();
        public List<Keys> KeysHeld = new List<Keys>();
        public List<Keys> KeysPressed = new List<Keys>();
        public Point MousePoint;
        public int previous_score = 0;
        public int righe_griglia = 25;
        public int score = 0;
        public bool shouldStop = false;
        public SpriteBatch spriteBatch;
        public int vita_rimanente = 3;
        public HighScore highscore = new HighScore();
        public HighScoreCollection highscorecollection = new HighScoreCollection();

        #endregion Public Fields

        #region Private Fields

        private CheckLife checkLife = new CheckLife();
        private int activeBlock;
        private Form1 ThisForm;
        private FPS_checker Fps;

        #endregion Private Fields

        #region Public Constructors

        public Logic(Form1 form)
        {
            this.ThisForm = form;
            this.vita_rimanente = 3;
            this.Fps = new FPS_checker(this.gameTime);
        }

        #endregion Public Constructors

        #region Public Methods

        public void gameLoop()
        {
            gameTime.Start();
            /*Gioco in esecuzione*/
            spriteBatch = new SpriteBatch(ThisForm.ClientSize, ThisForm.CreateGraphics());
            while (shouldStop == false)
            {
                ThisForm.ball.canCollide = true;
                vita_rimanente = checkLife.check(ThisForm, vita_rimanente);
                if (vita_rimanente <= 0)
                {
                    shouldStop = true;
                    ThisForm.backgroundMusic.Stop();
                    return;
                }
                if (gameTime.ElapsedMilliseconds % 1000 != 0)
                {
                    checkscore();
                    checkActiveBlock();
                }
                Fps.checkfps();
                input();
                Fps.logic(this.ThisForm, this.iManager);
                render();
            }
        }

        public void resize(int li, int hi, int l, int h)
        {
            ThisForm.grid.redraw_grid(ThisForm.grid, ThisForm.ClientRectangle.Height, ThisForm.ClientRectangle.Width);
            foreach (Sprite s in iManager.inGameSprites)
            {
                if (s.GetType().Name == "Ball")
                {
                    s.redraw(s, (int)(Math.Abs(10 * l / li)), (10 * h / hi), Properties.Resources.ball, s.X * l / li, s.Y * h / hi);
                }
                else if (s.GetType().Name == "Paddle")
                    s.redraw(s, (int)(Math.Abs(128 * l / li)), (24 * h / hi), Properties.Resources.New_Piskel, s.X * l / li, s.Y * h / hi);
                else if (s.GetType().Name == "View")
                    s.redraw(s, l, h, Properties.Resources.Background, 0, 0);
                else if (s.GetType().Name == "Block")
                    ThisForm.grid.redraw_block((Block)s, (100 * l / li), (50 * (h / hi)), s.Texture, s.X * l / li, s.Y * h / hi);
                else if (s.GetType().Name == "Life")
                    s.redraw(s, (20 * l / li), (20 * h / hi), Properties.Resources.vita, s.X * l / li, s.Y * h / hi);
            }
            ThisForm.racchetta.Y = h * 9 / 10;
            spriteBatch.cntxt.MaximumBuffer = new Size(ThisForm.ClientSize.Width + 1, ThisForm.ClientSize.Height + 1);
            spriteBatch.bfgfx = spriteBatch.cntxt.Allocate(ThisForm.CreateGraphics(), new Rectangle(Point.Empty, ThisForm.ClientSize));
            spriteBatch.Gfx = ThisForm.CreateGraphics();
        }

        #endregion Public Methods

        #region Private Methods

        private void checkActiveBlock()
        {
            if (activeBlock == 0)
            {
                ThisForm.grid.insert_grid(Properties.Resources.Block_4, this.iManager);
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
                    if (myBlock.block_life == 0)
                    {
                        score += myBlock.block_life;
                        myBlock.block_life = -1;
                    }
                    if (myBlock.block_life > 0)
                    {
                        activeBlock++;
                    }
                }
            }
            if (previous_score < score)
                Console.WriteLine(score);
        }

        private void input()
        {
            AllowInput = false;
            try
            {
                ThisForm.Invoke(new MethodInvoker(delegate
                {
                    if (Container.ActiveForm != null)
                        Container.ActiveForm.Text = "fps: " + Fps.fps.ToString() + "ups:" + Fps.ups.ToString();
                }));
            }
            catch
            {
            }
            /*controllo i tasti che sono stati premuti e svuoto i buffer*/
            iManager.update(MousePoint, KeysPressed.ToArray(), KeysHeld.ToArray(), gameTime, deltaTime);
            KeysPressed.Clear();
            KeysHeld.Clear();
            AllowInput = true;
        }

        private void render()
        {
            spriteBatch.Begin();
            foreach (Sprite s in iManager.inGameSprites)
                if (s.torender == true)
                    spriteBatch.Draw(s);
            spriteBatch.End();
        }

        #endregion Private Methods
    }
}