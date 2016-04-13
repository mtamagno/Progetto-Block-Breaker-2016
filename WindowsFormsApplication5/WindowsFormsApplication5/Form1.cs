using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form1 : Form
    {

        public int score = 0; //Punteggio


        public Form1()
        {
            InitializeComponent();
            return;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }
        /* se l utente inizia a mandare input troppo early potrebbe non verificarsi il corretto funzionamento del 
        programma quindi ci serve una booleana per dare i permessi*/
        private bool AllowInput; /* Variabile booleana per decidere quando l utente puo' immettere degli input*/
        private Stopwatch gameTime = new Stopwatch(); /* Stopwatch e' una classe che ha un set di metodi utili a misurare il tempo trascorso*/
        private int fps; /*fps totali*/
        private int fpsCounter;
        private int interval = 1000 / 150;
        private int uCounter;
        private int Ups; /* update per second " fps reali, dopo l utilizzo di un fps limiter" */
        private int previousSecond;
        private long uTime;
        private long fpsTime;
        private List<Keys> KeysPressed = new List<Keys>();
        private List<Keys> KeysHeld = new List<Keys>();
        private SpriteBatch spriteBatch;
        private InputManager iManager = new InputManager();
        public Point MousePoint;
        private float deltaTime;
        private long LastTime;
        private Sprite ball;
        private Sprite racchetta;
        private Sprite background;
        public float ball_x = 10;
        public float ball_y = 10;
        public int Lunghezza_Client_inziale = 0;
        public int Altezza_Client_iniziale = 0;
        public float lunghezza = 0;
        public float altezza = 0;
        
        
        private void loadContent()
        {
            Lunghezza_Client_inziale = this.ClientRectangle.Width;
            Altezza_Client_iniziale = this.ClientRectangle.Height;    
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.Bounds = Screen.PrimaryScreen.Bounds;
            background = new Sprite(Properties.Resources.Background, 0, 0, this.ClientRectangle.Width, this.ClientRectangle.Height, Sprite.SpriteType.view);
            iManager.inGameSprites.Add(background);
            racchetta = new Sprite(Properties.Resources.New_Piskel,MousePoint.X -this.Location.X , 300, 128, 24, Sprite.SpriteType.player);
            iManager.inGameSprites.Add(racchetta);
            ball = new Sprite(Properties.Resources.ball, 300, 288, 10, 10, Sprite.SpriteType.ball);
            iManager.inGameSprites.Add(ball);
            ball.canFall = false;
            ball.followPointer = true;
            Thread game = new Thread(gameLoop);
            ball.velocity.X = 50;
            game.Start();
        }
        private void gameLoop()
        {
            gameTime.Start();
            /*Gioco in esecuzione*/
            spriteBatch = new SpriteBatch(this.ClientSize, this.CreateGraphics());
            while (this.Created)
            {
                checkfps();
                deltaTime = gameTime.ElapsedMilliseconds - LastTime;
                LastTime = gameTime.ElapsedMilliseconds;
                input();
                logic();
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
                    this.Text = "fps: " + fps.ToString() + "Ups:" + Ups.ToString();
                    MousePoint = this.PointToClient(Cursor.Position);
                }));
            }

            catch
            {

            }
            /*controllo i tasti che sono stati premuti e svuoto i buffer*/
            iManager.update(MousePoint,KeysPressed.ToArray(),KeysHeld.ToArray(),gameTime,deltaTime);
            KeysPressed.Clear();
            KeysHeld.Clear();
            AllowInput = true;
        }

        private void output()
        {

        }

        private void logic()
        {
            if(gameTime.ElapsedMilliseconds - uTime > interval)
            {
                ball.Update(iManager);
                racchetta.Update(iManager);
                background.Update(iManager);
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

        private void render()
        {      
            spriteBatch.Begin();
            spriteBatch.Draw(background);
            spriteBatch.Draw(ball);
            spriteBatch.Draw(racchetta);
            spriteBatch.End();
        }

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

        /*gestisce le eccezioni alla chiusura del programma*/
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            System.Environment.Exit(0);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            loadContent();
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {
            base.OnKeyPress(e);
            if (AllowInput)
            {
                KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
            }
        }

        protected override void OnKeyDown(KeyEventArgs e)
        {
            base.OnKeyDown(e);
            if (AllowInput)
            {
                KeysHeld.Add(e.KeyCode);
            }
        }


        private void on_resize(object sender, EventArgs e)
        {
            ball.redraw(ball, 10 * this.ClientRectangle.Width / Lunghezza_Client_inziale, 10 * this.ClientRectangle.Height / Altezza_Client_iniziale, Properties.Resources.ball, this.ClientRectangle.Width / Lunghezza_Client_inziale, this.ClientRectangle.Height / Altezza_Client_iniziale);
            racchetta.redraw(racchetta, 150 * this.ClientRectangle.Width / Lunghezza_Client_inziale, 25 * this.ClientRectangle.Height / Altezza_Client_iniziale, Properties.Resources.New_Piskel, this.ClientRectangle.Width / Lunghezza_Client_inziale, this.ClientRectangle.Height / Altezza_Client_iniziale);
            background.redraw(background, this.ClientRectangle.Width, this.ClientRectangle.Height, Properties.Resources.Background, 0, 0);
            racchetta.Y = this.ClientRectangle.Height * 9 / 10;
            spriteBatch.cntxt.MaximumBuffer = new Size(ClientSize.Width + 1, ClientSize.Height + 1);
            spriteBatch.bfgfx = spriteBatch.cntxt.Allocate(this.CreateGraphics(), new Rectangle(Point.Empty, ClientSize));
            spriteBatch.Gfx = this.CreateGraphics();
        }

        private void Form1_ResizeBegin(object sender, EventArgs e)
        {
            lunghezza = this.ClientRectangle.Width;
            altezza = this.ClientRectangle.Height;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)Keys.Space)
            {
                ball.canFall = true;
                ball.followPointer = false;
            }
        }
    }



}
