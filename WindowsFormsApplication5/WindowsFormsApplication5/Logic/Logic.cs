using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public class Logic
    {
        #region Public Fields

        public bool AllowInput;
        public float ball_x = 10;
        public float ball_y = 10;
        public int colonne_griglia = 10;
        public float deltaTime;
        public int fps;
        public int fpsCounter;
        public long fpsTime;
        public Stopwatch gameTime = new Stopwatch();
        public InputManager iManager = new InputManager();
        public int interval = 1000 / 70;
        public List<Keys> KeysHeld = new List<Keys>();
        public List<Keys> KeysPressed = new List<Keys>();
        public long LastTime;
        public Point MousePoint;
        public int previous_scoure = 0;
        public int previousSecond;
        public int righe_griglia = 25;
        public int score = 0;
        public bool shouldStop = false;
        public SpriteBatch spriteBatch;
        public int uCounter;
        public int Ups;
        public long uTime;
        public int vita_rimanente = 3;

        #endregion Public Fields



        #region Private Fields

        private int activeBlock;
        private Form1 ThisForm;

        #endregion Private Fields

        #region Public Constructors

        public Logic(Form1 form)
        {
            ThisForm = form;
            this.vita_rimanente = 3;
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
                if (ThisForm.background.bottom_collide == 1)
                {
                    ThisForm.ball.canFall = false;
                    ThisForm.ball.Y = ThisForm.racchetta.Y;
                    ThisForm.ball.followPointer = true;
                    ThisForm.ball.velocity_tot = 0;
                    ThisForm.ball.velocity.X = 0;
                    ThisForm.ball.velocity.Y = 0;
                    vita_rimanente--;
                    ThisForm.background.bottom_collide = 0;
                    for (int i = vita_rimanente; i < 3; i++)
                    {
                        if (vita_rimanente > 0)
                            ThisForm.vita[i].torender = false;
                        else
                        {
                            shouldStop = true;
                            return;
                        }
                    }
                }
                if (gameTime.ElapsedMilliseconds % 1000 != 0) {
                    checkscore();
                    checkActiveBlock();
                   }
                checkfps();
                deltaTime = gameTime.ElapsedMilliseconds - LastTime;
                LastTime = gameTime.ElapsedMilliseconds;
                input();
                logic();
                Thread.Sleep(9);
                render();
            }
        }

        public void resize(int l, int h, int li, int hi/*object sender, EventArgs e*/)
        {
            ThisForm.grid.redraw_grid(ThisForm.grid, ThisForm.ClientRectangle.Height, ThisForm.ClientRectangle.Width);
            foreach (Sprite s in iManager.inGameSprites)
            {
                if (s.GetType().Name == "Ball")
                {
                    s.redraw(s, (int)(Math.Abs(10 * (float)Form2.ActiveForm.ClientRectangle.Width / li)), (int)(Math.Abs(10 * (float)Form2.ActiveForm.ClientRectangle.Height / hi)), Properties.Resources.ball, s.X * Form2.ActiveForm.ClientRectangle.Width / l, s.Y * Form2.ActiveForm.ClientRectangle.Height / h);
                }
                else if (s.GetType().Name == "Paddle")
                    s.redraw(s, (int)(Math.Abs(128 * (float)Form2.ActiveForm.ClientRectangle.Width / li)), (int)(Math.Abs(24 * (float)Form2.ActiveForm.ClientRectangle.Height / hi)), Properties.Resources.New_Piskel, s.X * Form2.ActiveForm.ClientRectangle.Width / l, s.Y * Form2.ActiveForm.ClientRectangle.Height / h);
                else if (s.GetType().Name == "View")
                    s.redraw(s, (Math.Abs(Form2.ActiveForm.ClientRectangle.Width)), Math.Abs(Form2.ActiveForm.ClientRectangle.Height), Properties.Resources.Background, 0, 0);
                else if (s.GetType().Name == "Block")
                    ThisForm.grid.redraw_block((Block)s, (int)(100 * (float)Form2.ActiveForm.ClientRectangle.Width / li), (int)(50 * (float)(Form2.ActiveForm.ClientRectangle.Height / hi)), s.Texture, s.X * Form2.ActiveForm.ClientRectangle.Width / l, s.Y * Form2.ActiveForm.ClientRectangle.Height / h);
                else if (s.GetType().Name == "Life")
                    s.redraw(s, (int)(Math.Abs(20 * (float)Form2.ActiveForm.ClientRectangle.Width / li)), (int)(Math.Abs(20 * (float)Form2.ActiveForm.ClientRectangle.Height / hi)), Properties.Resources.vita, s.X * Form2.ActiveForm.ClientRectangle.Width / l, s.Y * Form2.ActiveForm.ClientRectangle.Height / h);
            }
            ThisForm.racchetta.Y = Form2.ActiveForm.ClientRectangle.Height * 9 / 10;
            spriteBatch.cntxt.MaximumBuffer = new Size(ThisForm.ClientSize.Width + 1, ThisForm.ClientSize.Height + 1);
            spriteBatch.bfgfx = spriteBatch.cntxt.Allocate(ThisForm.CreateGraphics(), new Rectangle(Point.Empty, ThisForm.ClientSize));
            spriteBatch.Gfx = ThisForm.CreateGraphics();
        }

        #endregion Public Methods

        #region Private Methods

        private void checkfps()
        {
            if (gameTime.ElapsedMilliseconds - fpsTime > 1000)
            {
                fpsTime = gameTime.ElapsedMilliseconds;
                fps = fpsCounter;
                fpsCounter = 0;
            }
            else
            {
                fpsCounter++;
            }
        }

        private void checkActiveBlock()
        {
            if (activeBlock == 0)
            {
                ThisForm.grid.insert_grid(Properties.Resources.Block,this.iManager);
            }
        }

        private void checkscore()
        {
            previous_scoure = score;
            activeBlock = 0;
            foreach (Sprite s in iManager.inGameSprites)
            {
                if (s.GetType().Name == "Block")
                {
                    Block myBlock = (Block)s;
                    if (myBlock.remaining_bounces == 0)
                    {
                        score += myBlock.block_life;
                        myBlock.remaining_bounces = -1;
                    }
                    if(myBlock.remaining_bounces > 0)
                    {
                        activeBlock++;
                    }
                }

            }
            if (previous_scoure < score)
                Console.WriteLine(score);
        }

        private void input()
        {
            AllowInput = false;
            try
            {
                ThisForm.Invoke(new MethodInvoker(delegate
                {
                    if (Form2.ActiveForm != null)
                        Form2.ActiveForm.Text = "fps: " + fps.ToString() + "Ups:" + Ups.ToString();
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

        private void logic()
        {
            if (gameTime.ElapsedMilliseconds - uTime > interval)
            {
                ThisForm.ball.Update(iManager, ThisForm.ParentForm);
                ThisForm.racchetta.Update(iManager, ThisForm.ParentForm);
                if (gameTime.Elapsed.Seconds != previousSecond)
                {
                    previousSecond = gameTime.Elapsed.Seconds;
                    Ups = uCounter;
                    uCounter = 0;
                }
                uTime = gameTime.ElapsedMilliseconds;
                uCounter++;
            }
        }

        private void output()
        {
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