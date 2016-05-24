using System;
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

        private bool gameover = false;
        private bool button_start = false;
        private Form1 Game = new Form1();
        private Panel GamePanels = new Panel();
        private int primavolta = 0;
        private bool restart_required = false;
        private Form3 Start = new Form3();
        private HighScore highscore;
        public TextBox textBox = new TextBox();
        private Button Salva = new Button();
        private GameOver Gameover = new GameOver();

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
                    Start.Hide();
                    Start.Dispose();
                    Game_starter();
                    GamePanels.Controls.Add(Game);
                    Game.Show();
                    Game.Focus();
                    Game.BringToFront();
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
                    Start.Show();
                    Start.Focus();
                    Start.BringToFront();
                }));
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
            primavolta = 1;
        }

        public void gameover_check()
        {
            while (Game.Visible)
            {
                if (Game.logic.shouldStop == true && button_start == true)
                {
                    this.highscore = this.Game.logic.highscore;
                    Game.logic.shouldStop = false;
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
            Game.Close();
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
            Game.Width = GamePanels.Width;
            Game.Height = GamePanels.Height;
            lunghezza_client = this.ClientRectangle.Width;
            altezza_client = this.ClientRectangle.Height;
            Game.Top = 0;
            Game.Left = 0;
            Start.Width = GamePanels.Width;
            Start.Height = GamePanels.Height;
            Start.Top = 0;
            Start.Left = 0;

            if (Game.Visible == true)
            {
                Game.on_resize(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);
                Game.ball.totalVelocityReset(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);
            }
            if (Start.Visible == true)
            {
                Start.on_resize(this.Width, this.Height);
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
                    this.Game.Dispose();
                    this.Start.Dispose();
                    this.Gameover.Dispose();
                    this.GamePanels.Controls.Clear();
                    this.Controls.Clear();
                    this.Game = new Form1();
                    this.Start = new Form3();
                    this.Gameover = new GameOver();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();

                    this.Start.writer("Replay");
                }
                else
                {
                    //altrimenti scrivo start
                    Start.writer("Start");
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
                this.Start.Controls.Remove(Salva);
                this.Controls.Remove(textBox);
            }));
            gameLoop();
            return;
        }

        private void standardStarter()
        {
            //direttive da eseguire in ogni caso
            this.Controls.Add(GamePanels);
            Start.TopLevel = false;

            //imposto i gamepanels e aggiungo start
            GamePanels.Top = 0;
            GamePanels.Left = 0;
            GamePanels.Width = this.Width;
            GamePanels.Height = this.Height;
            GamePanels.Visible = true;
            GamePanels.Controls.Remove(Start);
            this.Dock = DockStyle.Fill;

            //imposto start a seconda di come è impostato gamepanels
            menu_starter();

            if (primavolta == 0)
                gameLoop();

            primavolta = 0;
            gameLoop();
            Start.start.Click += new EventHandler(this.start_Click);
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
            Game.TopLevel = false;

            Game.AutoScaleMode = AutoScaleMode.Inherit;
            GamePanels.Dock = DockStyle.Fill;
            GamePanels.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;

            //imposto game a seconda di come è impostato gamepanels
            Game.Width = GamePanels.Width;
            Game.Height = GamePanels.Height;
            Game.Left = 0;
            Game.Top = 0;
            Game.FormBorderStyle = FormBorderStyle.None;
            Game.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            Game.AutoScaleMode = AutoScaleMode.Inherit;

            GC.Collect();
            GC.WaitForPendingFinalizers();

            return;
        }

        private void menu_starter()
        {
            GamePanels.Controls.Add(Start);
            Start.TopLevel = false;
            GamePanels.Controls.Add(Start);

            //imposto start a seconda di come è impostato gamepanels
            Start.Width = GamePanels.Width;
            Start.Height = GamePanels.Height;
            Start.Left = 0;
            Start.Top = 0;
            Start.FormBorderStyle = FormBorderStyle.None;
            Start.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            Start.AutoScaleMode = AutoScaleMode.Inherit;
            Start.start.Click += new EventHandler(this.start_Click);

            GC.Collect();
            GC.WaitForPendingFinalizers();

        }
        
        private void gameover_call()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                Start.Hide();
                Start.Dispose();
                Gameover.Show();
                Gameover.Focus();
                Gameover.BringToFront();
                gameover = false;
            }));

        }

        #endregion Private Methods
    }
}