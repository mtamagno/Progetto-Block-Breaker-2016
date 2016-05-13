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
            logic.vita_rimanente = 3;
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
            for (int i = 0; i < logic.vita_rimanente; i++)
            {
                vita[i] = new life(this.ClientRectangle.Width - 20 - 30 * (i + 1), this.ClientRectangle.Height - 50, 20, 20);
                logic.iManager.inGameSprites.Add(vita[i]);
            }
            Thread game = new Thread(logic.gameLoop);
            game.Start();
        }
        #endregion Private Methods

    }
}