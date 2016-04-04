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
        private int interval = 1000/63;
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
        public float ball_x = 10;
        public float ball_y = 10;
        //public float racchetta_x = MousePosition.X;
        //public float racchetta_y = MousePosition.Y;

        private void loadContent()
        {
            ball = new Sprite(Properties.Resources.ball, ball_x, ball_y, 20, 20, Sprite.SpriteType.ball);
            iManager.inGameSprites.Add(ball);
            racchetta = new Sprite(Properties.Resources.New_Piskel, 150, 300, 150, 50, Sprite.SpriteType.player);
            iManager.inGameSprites.Add(ball);
            Thread game = new Thread(gameLoop);
            game.Start();
        }
        private void gameLoop()
        {
            gameTime.Start();
            /*Gioco in esecuzione*/
            while (this.Created)
            {
                spriteBatch = new SpriteBatch(this.ClientSize, this.CreateGraphics());
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
                //ball.isCollidingWith(racchetta);
                //racchetta.isCollidingWith(ball);
                //racchetta.isTouchingTop(ball);
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


     
    }



}
