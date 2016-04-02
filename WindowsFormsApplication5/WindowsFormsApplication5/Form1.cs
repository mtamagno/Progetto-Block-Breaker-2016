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
        public int Speed_x = 1; // setto la velocita' nell asse delle x
        public int Speed_y = 1; // setto la velocita' nell asse delle y
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
        private bool AllowInput;
        private Stopwatch gameTime = new Stopwatch();
        private int fps;
        private int fpsCounter;
        private long fpsTime;
        private List<Keys> KeysPressed = new List<Keys>();
        private List<Keys> KeysHeld = new List<Keys>();
        private SpriteBatch spriteBatch;
        private InputManager iManager = new InputManager();
        private Point MousePoint;
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
            Thread game = new Thread(gameLoop);
            game.Start();
        }
        private void gameLoop()
        {
            gameTime.Start();
            /*Gioco in esecuzione*/
            while (this.Created)
            {
                ball = new Sprite(Properties.Resources.ball, ball_x, ball_y, 20, 20);
                racchetta = new Sprite(Properties.Resources.New_Piskel, (float)MousePoint.X-150/2, (float)MousePoint.Y-50/2, 150, 50);
                spriteBatch = new SpriteBatch(this.ClientSize, this.CreateGraphics());
                checkfps();
                deltaTime = gameTime.ElapsedMilliseconds - LastTime;
                LastTime = gameTime.ElapsedMilliseconds;
                input();
                logic();
                render();
                ball_x += Speed_x;
                ball_y += Speed_y;
                if ((ball_x+10) >= ((float)MousePoint.X - 150) && (ball_x-10) <= ((float)MousePoint.X + 150) && ball_y + 10 == ((float)MousePoint.Y - 50))
                    Speed_y = - Speed_y;
            }

        }

        private void input()
        {

            AllowInput = false;
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    this.Text = fps.ToString();
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
