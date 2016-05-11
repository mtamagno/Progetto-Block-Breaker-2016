using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {
        #region Public Fields

        public float ball_x = 10;
        public float ball_y = 10;
        public int colonne_griglia = 10;
        public Point MousePoint;
        public int righe_griglia = 25;
        public int score = 0;

        #endregion Public Fields

        //Punteggio
        #region Private Fields

        private bool AllowInput;

        private View background;

        private ball ball;

        private float deltaTime;

        private int fps;

        private int fpsCounter;

        private long fpsTime;

        private Stopwatch gameTime = new Stopwatch();

        private Grid grid;

        private InputManager iManager = new InputManager();

        private int interval = 1000 / 500;

        private List<Keys> KeysHeld = new List<Keys>();

        private List<Keys> KeysPressed = new List<Keys>();

        private long LastTime;

        private int previousSecond;

        private paddle racchetta;

        private SpriteBatch spriteBatch;

        private int uCounter;

        private int Ups;

        private long uTime;

        private life[] vita = new life[3];

        public int vita_rimanente = 3;

        private Panel Game_pause;

        public volatile bool shouldStop = false;

        #endregion Private Fields

        #region Public Constructors

        public Form1()
        {
            InitializeComponent();
            return;
        }

        #endregion Public Constructors
        #region Public Methods

        public void on_resize(int l, int h, int li, int hi/*object sender, EventArgs e*/)
        {
            grid.redraw_grid(grid, this.ClientRectangle.Height, this.ClientRectangle.Width);
            foreach (Sprite s in iManager.inGameSprites)
            {
                

                if (s.GetType().ToString().ToLower() == "windowsformsapplication5.ball")
                {

                    s.redraw(s, (int)(Math.Abs(10 * (float)Form2.ActiveForm.ClientRectangle.Width / li)), (int)(Math.Abs(10 * (float)Form2.ActiveForm.ClientRectangle.Height / hi)), Properties.Resources.ball, s.X * Form2.ActiveForm.ClientRectangle.Width / l, s.Y * Form2.ActiveForm.ClientRectangle.Height / h);
                }
                else if (s.GetType().ToString().ToLower() == "windowsformsapplication5.paddle")
                    s.redraw(s, (int)(Math.Abs(128 * (float)Form2.ActiveForm.ClientRectangle.Width / li)), (int)(Math.Abs(24 * (float)Form2.ActiveForm.ClientRectangle.Height / hi)), Properties.Resources.New_Piskel, s.X * Form2.ActiveForm.ClientRectangle.Width / l, s.Y * Form2.ActiveForm.ClientRectangle.Height / h);
                else if (s.GetType().ToString().ToLower() == "windowsformsapplication5.view")
                    s.redraw(s, (Math.Abs(Form2.ActiveForm.ClientRectangle.Width)), Math.Abs(Form2.ActiveForm.ClientRectangle.Height), Properties.Resources.Background, 0, 0);
                else if (s.GetType().ToString().ToLower() == "windowsformsapplication5.block")
                    grid.redraw_block((Block)s, (int)(100 * (float)Form2.ActiveForm.ClientRectangle.Width / li), (int)(50 * (float)(Form2.ActiveForm.ClientRectangle.Height / hi)), Properties.Resources.Block, s.X * Form2.ActiveForm.ClientRectangle.Width / l, s.Y * Form2.ActiveForm.ClientRectangle.Height / h);
                else if (s.GetType().ToString().ToLower() == "windowsformsapplication5.life")
                    s.redraw(s, (int)(Math.Abs(20 * (float)Form2.ActiveForm.ClientRectangle.Width / li)), (int)(Math.Abs(20 * (float)Form2.ActiveForm.ClientRectangle.Height / hi)), Properties.Resources.vita, s.X * Form2.ActiveForm.ClientRectangle.Width / l, s.Y * Form2.ActiveForm.ClientRectangle.Height / h);
            }
            racchetta.Y = Form2.ActiveForm.ClientRectangle.Height * 9 / 10;
            spriteBatch.cntxt.MaximumBuffer = new Size(ClientSize.Width + 1, ClientSize.Height + 1);
            spriteBatch.bfgfx = spriteBatch.cntxt.Allocate(this.CreateGraphics(), new Rectangle(Point.Empty, ClientSize));
            spriteBatch.Gfx = this.CreateGraphics();
        }

        #endregion Public Methods
        #region Protected Methods

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            System.Environment.Exit(0);
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (AllowInput)
            {
                KeysHeld.Add(e.KeyCode);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);
            if (AllowInput)
            {
                KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            loadContent();
        }

        #endregion Protected Methods

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
                fpsCounter++;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                ball.followPointer = false;
                racchetta.followPointer = true;
                gamepause.Visible = false;
                ball.canFall = true;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                racchetta.followPointer = false;
                ball.followPointer = false;
                ball.canFall = false;
                gamepause.Visible = true;
            }
            if (e.KeyChar == (char)Keys.Escape)
                    this.Close();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        /* se l utente inizia a mandare input troppo early potrebbe non verificarsi il corretto funzionamento del
        programma quindi ci serve una booleana per dare i permessi*/
        /* Variabile booleana per decidere quando l utente puo' immettere degli input*/
        /* Stopwatch e' una classe che ha un set di metodi utili a misurare il tempo trascorso*/
        /*fps totali*/
        /* update per second " fps reali, dopo l utilizzo di un fps limiter" */

        private void gameLoop()
        {
            gameTime.Start();
            /*Gioco in esecuzione*/
            spriteBatch = new SpriteBatch(this.ClientSize, this.CreateGraphics());
            while (shouldStop == false)
            {
                ball.canCollide = true;
                if (background.bottom_collide == 1)
                {
                    ball.canFall = false;
                    ball.Y = racchetta.Y;
                    ball.followPointer = true;
                    ball.velocity.Y = 0;
                    vita_rimanente--;
                    background.bottom_collide = 0;
                    for (int i = vita_rimanente; i < 3; i++)
                    {
                        if (vita_rimanente > 0)
                            vita[i].torender = false;
                        else
                        {
                            shouldStop = true;
                            return;
                        }
                    }
                }
                checkfps();
                deltaTime = gameTime.ElapsedMilliseconds - LastTime;
                LastTime = gameTime.ElapsedMilliseconds;
                input();
                logic();
                Thread.Sleep(10);
                render();
            }
        }

        private void input()
        {
            AllowInput = false;
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    if (Form2.ActiveForm != null)
                        Form2.ActiveForm.Text = "fps: " + fps.ToString() + "Ups:" + Ups.ToString();
                    MousePoint = this.PointToClient(Cursor.Position);
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

        private void loadContent()
        {
            starter();
        }
        private void starter()
        {
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.Bounds = Screen.PrimaryScreen.Bounds;
            //inizializzo il background
            gamepause.Visible = false;
            background = new View(Properties.Resources.Background, this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height);
            iManager.inGameSprites.Add(background);
            //inizializzo griglia
            grid = new Grid(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Height, this.ClientRectangle.Width);
            grid.insert_grid(Properties.Resources.Block, iManager);
            //inizializzo racchetta
            racchetta = new paddle(Properties.Resources.New_Piskel, MousePoint.X - this.Location.X, Form2.ActiveForm.ClientRectangle.Height * 9 / 10, 128, 24);
            iManager.inGameSprites.Add(racchetta);
            //inizializzo pallina
            ball = new ball(Properties.Resources.ball, 300, racchetta.Y - 10, 10, 10);
            iManager.inGameSprites.Add(ball);
            ball.canFall = false;
            ball.followPointer = true;
            for (int i = 0; i < 3; i++)
            {
                vita[i] = new life(Properties.Resources.vita, this.ClientRectangle.Width - 20 - 30 * (i + 1), this.ClientRectangle.Height - 50, 20, 20);
                iManager.inGameSprites.Add(vita[i]);
            }
            Thread game = new Thread(gameLoop);
            ball.velocity.X = 50;
            game.Start();
        }


        private void logic()
        {
            if (gameTime.ElapsedMilliseconds - uTime > interval)
            {
                ball.Update(iManager);
                racchetta.Update(iManager);
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

        public void GameOver(int life)
        {
            vita_rimanente = life;
        }
        #endregion Private Methods

        /*gestisce le eccezioni alla chiusura del programma*/
    }
}