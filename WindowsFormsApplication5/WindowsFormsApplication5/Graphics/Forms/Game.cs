﻿using System;
using System.ComponentModel;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Game : Form
    {
        #region Public Fields

        public SoundPlayer backgroundMusic;
        public View background;
        public Ball ball;
        public Grid grid;
        public Logic logic;
        public Paddle racchetta;
        public Life[] vita = new Life[3];

        #endregion Public Fields

        #region Public Constructors

        public Game()
        {
            InitializeComponent();
            return;
        }

        #endregion Public Constructors

        #region Public Methods

        public void on_resize(int li, int hi, int l, int h)
        {
            logic.resize(li, hi, l, h);
        }

        #endregion Public Methods

        #region Protected Methods

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            System.Environment.Exit(0);
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
                    logic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
                }
                if (e.KeyChar == (char)Keys.Escape)
                    this.Close();
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            loadContent();
        }

        #endregion Protected Methods

        #region Private Methods

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

            //inizializzo il suono
            backgroundMusic = new SoundPlayer(Properties.Resources.Background_Music);
            backgroundMusic.PlayLooping();

            //inizializzo il background
            background = new View(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height, logic);

            //inizializzo griglia
            init_grid();

            //inizializzo racchetta
            racchetta = new Paddle(logic.MousePoint.X - this.Location.X, WindowsFormsApplication5.Container.ActiveForm.ClientRectangle.Height * 9 / 10, 128, 24, logic);

            //inizializzo pallina
            ball = new Ball(300, racchetta.Y - 10, 10, 10, logic);

            //inizializzo le vite
            life_init();

            //inizializzo il thread del gioco
            Thread gameThread = new Thread(logic.gameLoop);
            gameThread.Start();
        }

        private void init_grid()
        {
            grid = new Grid(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Height, this.ClientRectangle.Width, Properties.Resources.Block_4, logic);
        }

        private void life_init()
        {
            for (int i = 0; i < logic.vita_rimanente; i++)
            {
                vita[i] = new Life(this.ClientRectangle.Width - 20 - 30 * (i + 1), this.ClientRectangle.Height - 50, 20, 20);
                logic.iManager.inGameSprites.Add(vita[i]);
            }
        }

        #endregion Private Methods
    }
}