using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Container : Form
    {
        #region Fields

        public int altezza_client;
        public int altezza_client_iniziale;
        public int lunghezza_client;
        public int lunghezza_client_iniziale;

        public TextBox textBox = new TextBox();
        private bool button_start = false;
        private bool entra;
        private Game game = new Game();
        private Thread game_alive;
        private bool gameover = false;
        private GameOver Gameover = new GameOver();
        private Panel GamePanels = new Panel();
        private HighScore highscore;
        private Music music;
        private int primavolta = 0;
        private bool restart_required = false;
        private Button Salva = new Button();
        private Start start = new Start();

        #endregion Fields

        #region Constructors

        public Container()
        {
            music = new Music();
            entra = false;
            InitializeComponent();
            return;
        }

        #endregion Constructors

        #region Methods

        public void gameLoop()
        {
            if (button_start == true)
            {
                if (entra == false)
                    this.Invoke(new MethodInvoker(delegate
                    {
                        entra = true;
                        start.Close();
                        start.Hide();
                        start.Dispose();
                        Game_starter();
                        GamePanels.Controls.Remove(start);
                        GamePanels.Controls.Clear();
                        GamePanels.Controls.Add(game);
                        game.Show();
                        game.Focus();
                        game.BringToFront();
                        game_alive = new Thread(gameover_check);
                        game_alive.Start();
                    }));
            }
            else {
                if (primavolta == 1 && restart_required == true)
                {
                    this.music.Dispose_Music();
                    restart_required = false;
                    starter();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }
                this.Invoke(new MethodInvoker(delegate
                {
                    Gameover.Close();
                    Gameover.Dispose();
                    GamePanels.Controls.Remove(Gameover);
                    GamePanels.Controls.Clear();
                    menu_starter();
                    Gameover.Dispose();
                    start.Show();
                    start.Focus();
                    start.BringToFront();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }));
            }
            primavolta = 1;
        }

        public void gameover_check()
        {
            while (game.Visible)
            {
                if (game.logic.shouldStop == true && button_start == true)
                {
                    this.highscore = this.game.logic.highScore;
                    game.logic.shouldStop = false;
                    button_start = false;
                    restart_required = true;
                    gameover = true;
                    starter();
                    gameover_call();
                    entra = false;

                    //gameLoop();
                    return;

                    //termino il thread
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();
                Thread.Sleep(2000);
            }
        }

        protected void start_Click(object sender, EventArgs e)
        {
            button_start = true;
            gameLoop();
        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            game.Close();
            this.Dispose();
            base.OnClosing(e);
            System.Environment.Exit(0);
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

        private void Game_starter()
        {
            this.music.Dispose_Music();

            //A  cosa servono di nuovo?
            music.Game();
            this.Controls.Clear();
            this.Controls.Remove(GamePanels);
            this.Controls.Add(GamePanels);
            game.TopLevel = false;
            game.AutoScaleMode = AutoScaleMode.Inherit;
            GamePanels.Dock = DockStyle.Fill;
            GamePanels.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;

            // Imposta game a seconda di come è impostato gamepanels
            game.Width = GamePanels.Width;
            game.Height = GamePanels.Height;
            game.Left = 0;
            game.Top = 0;
            game.FormBorderStyle = FormBorderStyle.None;
            game.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            game.AutoScaleMode = AutoScaleMode.Inherit;
            return;
        }

        private void gameover_call()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                game.Close();
                game.Dispose();
                GamePanels.Controls.Remove(game);
                start.Close();
                start.Hide();
                start.Dispose();
                Gameover.Show();
                Gameover.Focus();
                Gameover.BringToFront();
                gameover = false;
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }));
        }

        private void GameOverStarter()
        {
            //direttive da eseguire in ogni caso
            this.music.Dispose_Music();
            music.GameOver();
            this.Controls.Clear();
            this.Controls.Remove(GamePanels);
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
            return;
        }

        private void menu_starter()
        {
            music.Menu();
            GamePanels.Controls.Add(start);
            start.TopLevel = false;

            // Imposta start a seconda di come è impostato gamepanels
            this.Controls.Clear();
            this.Controls.Remove(GamePanels);
            this.Controls.Add(GamePanels);
            start.Width = GamePanels.Width;
            start.Height = GamePanels.Height;
            start.Left = 0;
            start.Top = 0;
            start.FormBorderStyle = FormBorderStyle.None;
            start.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            start.AutoScaleMode = AutoScaleMode.Inherit;
            start.start.Click += new EventHandler(this.start_Click);
        }

        private void Salva_Click(object sender, EventArgs e)
        {
            HighScoreSaver functioncaller = new HighScoreSaver();

            this.highscore.Name = textBox.Text;
            functioncaller.ModifyOrCreateXML(highscore);
            this.Invoke(new MethodInvoker(delegate
            {
                this.start.Controls.Remove(Salva);
                this.Controls.Remove(textBox);
            }));
            this.Controls.Clear();
            this.GamePanels.Controls.Clear();
            gameLoop();
            return;
        }

        private void standardStarter()
        {
            //direttive da eseguire in ogni caso
            start.TopLevel = false;

            // Imposta i gamepanels e aggiungo start
            GamePanels.Top = 0;
            GamePanels.Left = 0;
            GamePanels.Width = this.Width;
            GamePanels.Height = this.Height;
            GamePanels.Visible = true;
            GamePanels.Controls.Remove(start);
            this.Dock = DockStyle.Fill;

            // Imposta start a seconda di come è impostato gamepanels
            menu_starter();

            if (primavolta == 0)
                gameLoop();

            primavolta = 0;
            start.start.Click += new EventHandler(this.start_Click);
            return;
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
                    this.Controls.Remove(GamePanels);
                    this.Controls.Clear();
                    this.game = new Game();
                    this.start = new Start();
                    this.Gameover = new GameOver();
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    this.start.writer("Replay");
                }
                else
                {
                        //altrimenti scrivo start
                        start.writer("Start");
                }

                    //chiamo la funzione standardStarter()
                    if (gameover == false)
                    standardStarter();
                if (gameover == true)
                {
                    this.Controls.Add(textBox);
                    GameOverStarter();
                    textBox.Top = ClientRectangle.Height / 7 * 6 - textBox.Height / 2;
                    textBox.Left = ClientRectangle.Width / 2 - textBox.Width / 2;
                    Gameover.Continue.Click += Salva_Click;
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                }
            }));

            return;
        }

        #endregion Methods
    }
}