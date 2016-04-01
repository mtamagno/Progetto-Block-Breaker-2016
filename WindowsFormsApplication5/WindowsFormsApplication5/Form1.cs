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
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private bool AllowInput; /* se l utente inizia a mandare input troppo early potrebbe non verificarsi il corretto funzionamento del programma quindi ci serve una booleana per dare i permessi*/
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
        private Sprite s;

        private void loadContent()
        {
            s = new Sprite(Properties.Resources.ball,100,100,100,100);
            spriteBatch = new SpriteBatch(this.ClientSize, this.CreateGraphics());
            Thread game = new Thread(gameLoop);
            game.Start();
        }

        private void gameLoop()
        {
            gameTime.Start();
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
            spriteBatch.Draw(s);
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
