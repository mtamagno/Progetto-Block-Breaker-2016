﻿using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Container : Form
    {
        #region Public Fields

        public int altezza_client;
        public int altezza_client_iniziale;
        public int lunghezza_client;
        public int lunghezza_client_iniziale;

        #endregion Public Fields

        #region Private Fields

        private Game game = new Game();
        private Start start = new Start();
        private GameOver Gameover = new GameOver();
        private Panel GamePanels = new Panel();
        private HighScore highscore;
        public TextBox textBox = new TextBox();
        private Button Salva = new Button();
        private int primavolta = 0;
        private bool gameover = false;
        private bool restart_required = false;
        private bool button_start = false;

        #endregion Private Fields

        #region Public Constructors

        public Container()
        {
            InitializeComponent();
            return;
        }

        #endregion Public Constructors

        #region Public Methods

        public void gameLoop()
        {
            if (button_start == true)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                    start.Hide();
                    start.Dispose();
                    Game_starter();
                    GamePanels.Controls.Add(game);
                    game.Show();
                    game.Focus();
                    game.BringToFront();
                    Thread game_alive = new Thread(gameover_check);
                    game_alive.Start();
                }));
            }
            else {
                if (primavolta == 1 && restart_required == true)
                {
                    restart_required = false;
                    starter();
                }
                this.Invoke(new MethodInvoker(delegate
                {
                   /*amePanels.Controls.Remove(Gameover);
                    Gameover.Dispose();*/
                    start.Show();
                    start.Focus();
                    start.BringToFront();
                }));
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            primavolta = 1;
        }

        public void gameover_check()
        {
            while (game.Visible)
            {
                if (game.logic.shouldStop == true && button_start == true)
                {
                    this.highscore = this.game.logic.highscore;
                    game.logic.shouldStop = false;
                    button_start = false;
                    restart_required = true;
                    gameover = true;
                    starter();
                    gameover_call();
                    //gameLoop();
                    return;

                    //termino il thread
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(2000);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected void start_Click(object sender, EventArgs e)
        {
            button_start = true;
            gameLoop();
        }

        #endregion Protected Methods

        #region Private Methods

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            game.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            starter();
        }

        private void Form2_ResizeBegin(object sender, EventArgs e)
        {
            lunghezza_client_iniziale = this.ClientRectangle.Width;
            altezza_client_iniziale = this.ClientRectangle.Height;
        }

        private void Form2_ResizeEnd(object sender, EventArgs e)
        {
            GamePanels.Height = this.ClientRectangle.Height;
            GamePanels.Width = this.ClientRectangle.Width;
            GamePanels.Top = 0;
            GamePanels.Left = 0;
            game.Width = GamePanels.Width;
            game.Height = GamePanels.Height;
            lunghezza_client = this.ClientRectangle.Width;
            altezza_client = this.ClientRectangle.Height;
            game.Top = 0;
            game.Left = 0;
            start.Width = GamePanels.Width;
            start.Height = GamePanels.Height;
            start.Top = 0;
            start.Left = 0;

            if (game.Visible == true)
            {
                game.on_resize(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);
                game.ball.totalVelocityReset(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);
            }
            if (start.Visible == true)
            {
                start.on_resize(this.Width, this.Height);
            }
        }

        /// <summary>
        /// Funzione che esegue le istruzioni a seconda che sia la prima volta che si apre il form o meno
        /// </summary>
        private void starter()
        {
            
            lunghezza_client = this.ClientRectangle.Width;
            altezza_client = this.ClientRectangle.Height;
            this.Invoke(new MethodInvoker(delegate
            {
                if (primavolta == 1)
                {
                    this.game.Dispose();
                    this.start.Dispose();
                    this.Gameover.Dispose();
                    this.GamePanels.Controls.Clear();
                    this.Controls.Clear();
                    this.game = new Game();
                    this.start = new Start();
                    this.Gameover = new GameOver();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    this.start.writer("Replay");
                }
                else
                {
                    //altrimenti scrivo start
                    start.writer("Start");
                }

                //chiamo la funzione standardStarter()
                if(gameover == false)
                    standardStarter();
                if (gameover == true)
                {
                    this.Controls.Add(textBox);
                    GameOverStarter();
                    textBox.Top = ClientRectangle.Height / 7 * 6 - textBox.Height / 2;
                    textBox.Left = ClientRectangle.Width / 2 - textBox.Width / 2;
                    Gameover.Continue.Click += Salva_Click;
                }
            }));
            return;
        }

        private void Salva_Click(object sender, EventArgs e)
        {
            HighScoreCollection functioncaller = new HighScoreCollection();
            this.highscore.Name = textBox.Text;
            functioncaller.SaveToXml(highscore);
            this.Invoke(new MethodInvoker(delegate
            {
                this.start.Controls.Remove(Salva);
                this.Controls.Remove(textBox);
            }));
            gameLoop();
            return;
        }

        private void standardStarter()
        {
            //direttive da eseguire in ogni caso
            this.Controls.Add(GamePanels);
            start.TopLevel = false;

            //imposto i gamepanels e aggiungo start
            GamePanels.Top = 0;
            GamePanels.Left = 0;
            GamePanels.Width = this.Width;
            GamePanels.Height = this.Height;
            GamePanels.Visible = true;
            GamePanels.Controls.Remove(start);
            this.Dock = DockStyle.Fill;

            //imposto start a seconda di come è impostato gamepanels
            menu_starter();

            if (primavolta == 0)
                gameLoop();

            primavolta = 0;
            gameLoop();
            start.start.Click += new EventHandler(this.start_Click);
            GC.Collect();
            GC.WaitForPendingFinalizers();
            return;
        }

        private void GameOverStarter()
        {
           //direttive da eseguire in ogni caso
            this.Controls.Add(GamePanels);

            //perchè falso se invece lo stiamo aggiungendo per vederlo?
            Gameover.TopLevel = false;
            GamePanels.Controls.Add(Gameover);

            //impost Gameover a seconda di come e' il gamepanels
            Gameover.Width = GamePanels.Width;
            Gameover.Height = GamePanels.Height;
            Gameover.Left = 0;
            Gameover.Top = 0;
            Gameover.FormBorderStyle = FormBorderStyle.None;
            Gameover.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;

            // a cosa serve autoscale?
            Gameover.AutoScaleMode = AutoScaleMode.Inherit;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            return;
        }

        private void Game_starter()
        {
            this.Controls.Add(GamePanels);

            //A  cosa servono di nuovo?
            game.TopLevel = false;

            game.AutoScaleMode = AutoScaleMode.Inherit;
            GamePanels.Dock = DockStyle.Fill;
            GamePanels.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;

            //imposto game a seconda di come è impostato gamepanels
            game.Width = GamePanels.Width;
            game.Height = GamePanels.Height;
            game.Left = 0;
            game.Top = 0;
            game.FormBorderStyle = FormBorderStyle.None;
            game.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            game.AutoScaleMode = AutoScaleMode.Inherit;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return;
        }

        private void menu_starter()
        {
            GamePanels.Controls.Add(start);
            start.TopLevel = false;
            GamePanels.Controls.Add(start);

            //imposto start a seconda di come è impostato gamepanels
            start.Width = GamePanels.Width;
            start.Height = GamePanels.Height;
            start.Left = 0;
            start.Top = 0;
            start.FormBorderStyle = FormBorderStyle.None;
            start.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            start.AutoScaleMode = AutoScaleMode.Inherit;
            start.start.Click += new EventHandler(this.start_Click);

            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
        
        private void gameover_call()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                start.Hide();
                start.Dispose();
                Gameover.Show();
                Gameover.Focus();
                Gameover.BringToFront();
                gameover = false;
            }));

        }

        #endregion Private Methods
    }
}