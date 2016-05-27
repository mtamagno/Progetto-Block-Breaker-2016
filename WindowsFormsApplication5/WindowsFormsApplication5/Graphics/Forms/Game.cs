﻿using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Game : Form
    {
        #region Fields

        public View background;
        public Ball ball;
        public Grid grid;
        public Logic logic;
        public Paddle racchetta;
        public Life[] vita = new Life[3];
        public Thread gameThread;

        #endregion Fields

        #region Constructors

        public Game()
        {
            InitializeComponent();
            return;
        }

        #endregion Constructors

        #region Methods

        public void on_resize(int li, int hi, int l, int h)
        {
            logic.resize(li, hi, l, h);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            this.Close();
            gameThread = null;
            base.OnClosing(e);
            System.Environment.Exit(0);
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            loadContent();
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (logic.AllowInput)
            {
                if (e.KeyChar == (char)Keys.Space)
                {
                    ball.followPointer = false;
                    ball.canFall = true;
                    racchetta.followPointer = true;
                    Gamepause.Visible = false;
                    logic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
                }
                if (e.KeyChar == (char)Keys.Enter && Gamepause.Visible == false)
                {
                    ball.followPointer = false;
                    ball.canFall = false;
                    racchetta.followPointer = false;
                    Gamepause.Visible = true;
                    Gamepause.BorderStyle = BorderStyle.Fixed3D;
                    Gamepause.Left = this.ClientRectangle.Width / 2 - Gamepause.Width / 2;
                    Gamepause.Top = this.ClientRectangle.Height / 2 - Gamepause.Height / 2;
                    logic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
                }
                if (e.KeyChar == (char)Keys.Escape)
                    this.Close();
            }
        }

        private void init_grid()
        {
            grid = new Grid(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Height, this.ClientRectangle.Width, Properties.Resources.Block_4, logic);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void life_init()
        {
            for (int i = 0; i < logic.vita_rimanente; i++)
            {
                vita[i] = new Life(this.ClientRectangle.Width - 20 - 30 * (i + 1), this.ClientRectangle.Height - 50, 20, 20);
                logic.iManager.inGameSprites.Add(vita[i]);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
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
            Gamepause.Visible = false;

            //inizializzo il background
            background = new View(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height, logic);

            //inizializzo griglia
            init_grid();

            //inizializzo racchetta
            if (this.Visible)
                racchetta = new Paddle(logic.MousePoint.X - this.Location.X,
                    this.ParentForm.ClientRectangle.Height * 9 / 10,
                    (int)(Math.Abs((float)1 / 8 * this.ParentForm.ClientRectangle.Width)),
                    (int)(Math.Abs((float)1 / 22 * this.ParentForm.ClientRectangle.Height)),
                    logic);

            //inizializzo pallina
            ball = new Ball(300,
                racchetta.Y - 10,
                (int)(Math.Abs((float)1 / 50 * Math.Min(this.ParentForm.ClientRectangle.Width, this.ParentForm.ClientRectangle.Height))),
                (int)(Math.Abs((float)1 / 50 * Math.Min(this.ParentForm.ClientRectangle.Width, this.ParentForm.ClientRectangle.Height))),
                logic);

            //inizializzo le vite
            life_init();

            //inizializzo il thread del gioco
            gameThread = new Thread(logic.gameLoop);
            gameThread.Start();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion Methods
    }
}