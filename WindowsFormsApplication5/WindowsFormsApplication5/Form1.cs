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


        public View background;
        public ball ball;
        public paddle racchetta;
        public life[] vita = new life[3];
        public Logica logic;
        private Grid grid;

        #endregion Public Fields

        //Punteggio
        #region Private Fields


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
            foreach (Sprite s in logic.iManager.inGameSprites)
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
            logic.spriteBatch.cntxt.MaximumBuffer = new Size(ClientSize.Width + 1, ClientSize.Height + 1);
            logic.spriteBatch.bfgfx = logic.spriteBatch.cntxt.Allocate(this.CreateGraphics(), new Rectangle(Point.Empty, ClientSize));
            logic.spriteBatch.Gfx = this.CreateGraphics();
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
            if (logic.AllowInput)
            {
                logic.KeysHeld.Add(e.KeyCode);
            }
        }

        protected override void OnKeyPress(KeyPressEventArgs e) {
            base.OnKeyPress(e);
            if (logic.AllowInput)
            {
                logic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            loadContent();
        }

        #endregion Protected Methods

        #region Private Methods

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space)
            {
                ball.followPointer = false;
                ball.canFall = true;
                racchetta.followPointer = true;
                gamepause.Visible = false;
            }

            if (e.KeyChar == (char)Keys.Enter)
            {
                ball.followPointer = false;
                ball.canFall = false;
                racchetta.followPointer = false;
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

 

        private void loadContent()
        {
            starter();
        }
        private void starter()
        {
            //this.FormBorderStyle = FormBorderStyle.None;
            //this.Bounds = Screen.PrimaryScreen.Bounds;
            //inizializzo il background
            logic = new Logica(this);
            gamepause.Visible = false;
            background = new View(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height);
            logic.iManager.inGameSprites.Add(background);
            //inizializzo griglia
            grid = new Grid(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Height, this.ClientRectangle.Width);
            grid.insert_grid(Properties.Resources.Block, logic.iManager);
            //inizializzo racchetta
            racchetta = new paddle(logic.MousePoint.X - this.Location.X, Form2.ActiveForm.ClientRectangle.Height * 9 / 10, 128, 24);
            logic.iManager.inGameSprites.Add(racchetta);
            //inizializzo pallina
            ball = new ball(300, racchetta.Y - 10, 10, 10);
            logic.iManager.inGameSprites.Add(ball);
            ball.canFall = false;
            ball.followPointer = true;
            for (int i = 0; i < 3; i++)
            {
                vita[i] = new life(this.ClientRectangle.Width - 20 - 30 * (i + 1), this.ClientRectangle.Height - 50, 20, 20);
                logic.iManager.inGameSprites.Add(vita[i]);
            }
            Thread game = new Thread(logic.gameLoop);
            game.Start();
        }




       /* public void GameOver(int life)
        {
            vita_rimanente = life;
        }*/

        #endregion Private Methods

        /*gestisce le eccezioni alla chiusura del programma*/
    }
}