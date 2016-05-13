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
        public Ball ball;
        public Paddle racchetta;
        public Life[] vita = new Life[3];
        public Logic logic;
        public Grid grid;
        #endregion Public Fields



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
            logic.resize( l, h, li, hi);
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
        //Menu di pausa
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Space && gamepause.Visible == true)
            {
                ball.followPointer = false;
                ball.canFall = true;
                racchetta.followPointer = true;
                gamepause.Visible = false;
            }

            if (e.KeyChar == (char)Keys.Enter && gamepause.Visible == false)
            {
                ball.followPointer = false;
                ball.canFall = false;
                racchetta.followPointer = false;
                gamepause.Visible = true;
            }
            if (e.KeyChar == (char)Keys.Escape)
                    this.Close();
        }

        private void loadContent()
        {
            starter();
        }

        private void starter()
        {
            //inizializzo la logica
            logic = new Logic(this);

            //inizializzo la variabile della visione del menù pausa a falso in caso sia vera
            gamepause.Visible = false;

            //inizializzo il background
            background = new View(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height, logic);
           
            //inizializzo griglia
            grid = new Grid(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Height, this.ClientRectangle.Width, Properties.Resources.Block, logic);
            
            //inizializzo racchetta
            racchetta = new Paddle(logic.MousePoint.X - this.Location.X, Form2.ActiveForm.ClientRectangle.Height * 9 / 10, 128, 24, logic);
            
            //inizializzo pallina
            ball = new Ball(300, racchetta.Y - 10, 10, 10, logic);
            //inizializzo le vite
            for (int i = 0; i < logic.vita_rimanente; i++)
            {
                vita[i] = new Life(this.ClientRectangle.Width - 20 - 30 * (i + 1), this.ClientRectangle.Height - 50, 20, 20);
                logic.iManager.inGameSprites.Add(vita[i]);
            }
            //creo e inizializzo il thread del gioco
            Thread game = new Thread(logic.gameLoop);
            game.Start();
        }
        #endregion Private Methods

    }
}