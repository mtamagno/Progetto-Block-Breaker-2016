using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace BlockBreaker
{
    public partial class Container : Form
    {
        #region Fields

        private bool again;
        private int altezza_client;
        private int altezza_client_iniziale;
        private Game Game;
        private GameOver GameOver;
        private Panel GamePanels;
        private HighScore HighScore;
        private int lunghezza_client;
        private int lunghezza_client_iniziale;
        private Menu menu;
        private AudioButtons Audio_button;
        private Music Music;
        private TextBox textBox = new TextBox();
        private bool AudioOnOff;

        #endregion Fields

        #region Constructors

        public Container()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Funzione per la chiusura di container
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerFormClosing(object sender, FormClosingEventArgs e)
        {
            this.Invoke(new MethodInvoker(delegate
            {
            //se l utente preme x dalla schermata di gioco devo fermare il thread del gioco
            if (Game != null)
                {
                    Game.Logic.shouldStop = true;

                    while (Game.gameThread.IsAlive) {
                        Game.Logic.shouldStop = true;
                    }
                    while (Game.IsAccessible)
                    {

                    }
                }
            //pulisco tutto
           // DisposeAll();
            base.OnClosing(e);
            System.Environment.Exit(0);
            }));

            
        }

        /// <summary>
        /// Funzione Per il Caricamento di Container
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Container2_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new System.Drawing.Size(700, 450);

            AudioOnOff = true;
            this.Music = new Music();

            // Imposta again a false per dire che il gioco e' stato avviato per la prima volta
            this.again = false;

            // Inizializza il pannello che conterra' i form dell'applicazione
            this.initializeGamePanel();
            this.GamePanels.AutoSizeMode = AutoSizeMode;

            // Inizializza il menu come primo form che apparira' nel gioco
            this.initializeMenu();

            //Salvo le dimensioni del client prima dei ridimensionamenti per poter calcolare la proporzione
            lunghezza_client_iniziale = this.ClientRectangle.Width;
            altezza_client_iniziale = this.ClientRectangle.Height;
        }

        private void ButtonAudioSet(Form form)
        {
            if (Audio_button != null)
                Audio_button.Dispose();
            Size s = new Size(30, 30);
            Audio_button = new AudioButtons(s);
            Audio_button.Visible = true;
            Audio_button.Left = 50;
            Audio_button.Top = this.Height - this.Audio_button.Height - 45;
            this.Controls.Add(Audio_button);
            Audio_button.BringToFront();
            this.Audio_button.MouseClick += Audio;
            Audio_button.TabStop = false;
            Audio_button.BackColor = Color.Transparent;

        }

        private void Audio(object sender, EventArgs e)
        {
            AudioOnOff = !AudioOnOff;

            if (!AudioOnOff)
                this.Music.backgroundMusic.Stop();
            else
                this.Music.backgroundMusic.Play();

            Audio_button.ChangeState();
            this.ProcessTabKey(true);

        }

        /// <summary>
        /// Funzione necessaria per cambiare form dopo la fine della partita
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContinueToMenu(object sender, EventArgs e)
        {

            if (GameOver.textBox.Text != "Insert Name..." && !string.IsNullOrEmpty(GameOver.textBox.Text) && !string.IsNullOrWhiteSpace(GameOver.textBox.Text))
            {
            // Salva prima lo score, poi l'HighScore nell'xml
                this.HighScore.Name = GameOver.textBox.Text;
            HighScoreSaver highScoreSaver = new HighScoreSaver();
            highScoreSaver.ModifyOrCreateXML(HighScore);

            // Imposta che il giocatore ha gia finito una partita
            this.again = true;

            // Pulisce tutto
            this.DisposeAll();

            // Inizializza il gamePanel
            this.initializeGamePanel();

            // Inizializza il menu
            this.initializeMenu();

            // Svuota il garbage collector per liberare memoria
            GC.Collect();

            // Aspetta che il garbage collecor finisca
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
        }

            else
            {
                MessageBox.Show("Inserisci un NickName");
            }
        }

        private void DisposeAll()
        {
            try
            {
                this.Invoke(new MethodInvoker(delegate
                {
                //rimuovo il gamePanel dal container
                this.Controls.Remove(GamePanels);

                //Pulisco il gamepanel
                this.GamePanels.Dispose();
                    this.GamePanels.Controls.Clear();

                // Imposta il gamePanel a null
                this.GamePanels = null;

                //pulisco il container
                this.Controls.Clear();

                //pulisco il gioco
                if (this.Game != null)
                    {
                        this.Game.Controls.Clear();
                        this.Game.Close();
                        this.Game.Dispose();
                        this.Game = null;
                    }

                //pulisco il menu
                if (this.menu != null)
                    {
                        this.menu.start.Dispose();
                        this.menu.cleaner();
                        this.menu.Controls.Clear();
                        this.menu.Close();
                        this.menu.Dispose();
                        this.menu = null;
                    }

                //pulisco il GameOver
                if (this.GameOver != null)
                    {
                        this.GameOver.Continue.Dispose();
                        this.GameOver.cleaner();
                        this.GameOver.Controls.Clear();
                        this.GameOver.Close();
                        this.GameOver.Dispose();
                        this.GameOver = null;
                    }

                //Pulisco il garbage collector
                GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.WaitForFullGCComplete();
                }));
            }
            catch (InvalidOperationException)
            {
                this.Close();
            }
        }

        private void initializeForm(Form form)
        {
            // Imposta il topLevel del form a false
            form.TopLevel = false;
            form.AutoScaleMode = AutoScaleMode.Inherit;

            // Imposta altezza e larghezza del form
            form.Width = GamePanels.Width;
            form.Height = GamePanels.Height;

            // Imposta la posizione del form
            form.Left = 0;
            form.Top = 0;

            //rimuovo il bordo del form
            form.FormBorderStyle = FormBorderStyle.None;

            // Imposta il form per occupare tutto lo spazio disponibile
            form.Dock = DockStyle.Fill;
            form.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            form.AutoScaleMode = AutoScaleMode.Inherit;

            //aggiungo e mostro il form a schermo
            this.Invoke(new MethodInvoker(delegate
            {
                GamePanels.Controls.Add(form);
                form.Show();
                form.Focus();
                form.BringToFront();
            }));
            ButtonAudioSet(form);
            form.Focus();
        }



        /// <summary>
        /// Funzione necessaria ad inizializzare il form del gioco
        /// </summary>
        private void initializeGame()
        {   
            //assegno al gioco un nuovo gioco
            Game = new Game();

            // Inizializza il gioco
            initializeForm(Game);

            //faccio partire la musica di gioco
            Music.Game();

            //assegno un evento alla chiusura del Game
            Game.VisibleChanged += new EventHandler(onGameover);
        }

        /// <summary>
        /// Funzione necessaria a inizializzare il form del gameover
        /// </summary>
        private void initializeGameOver()
        {
            //assegno al GameOver un nuovo GameOver
            GameOver = new GameOver();

            this.Text = "BlockBreaker - Gameover";

            // Inizializza il GameOver
            initializeForm(GameOver);

            //Assegno un testo al pulsante del form GameOver
            GameOver.Continue.Text = "Continue";

            //Assegno un evento al pusalnte del GameOver
            GameOver.Continue.Click += new EventHandler(this.ContinueToMenu);

            //Faccio partire la musica del GameOver
            Music.GameOver();
        }

        private void onGameover(object sender, EventArgs e)
        {
                //se l'utente ha finito il gioco salvo l HighScore ottenuto
                this.HighScore = this.Game.Logic.highScore;

                //ri Imposta il Game.Logic a false
                Game.Logic.shouldStop = false;

                //pulisco tutto
                DisposeAll();

                // Inizializza di nuovo il gamePanel
                initializeGamePanel();

                // Inizializza il GameOver
                initializeGameOver();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

        private void initializeGamePanel()
        {
            //assegno al pannello un nuovo pannello
            GamePanels = new Panel();

            // Imposta altezza e lunghezza del pannello
            GamePanels.Width = this.Width;
            GamePanels.Height = this.Height;

            // Imposta il metodo Dock per far riempire al pannello tutto lo spazio del form
            this.Dock = DockStyle.Fill;

            // Imposta la posizione iniziale del pannello
            GamePanels.Top = 0;
            GamePanels.Left = 0;

            // Imposta il Dock del pannello per far riempire ai Form figli del pannello tutto lo spazio del pannello
            GamePanels.Dock = DockStyle.Fill;
            GamePanels.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;

            //mostro il pannello
            GamePanels.Show();

            try
            {
            //aggiungo il pannello al form
            this.Invoke(new MethodInvoker(delegate
            {
                this.Controls.Add(GamePanels);
            }));
        }
            catch(InvalidOperationException)
            {
                Thread.Sleep(1000);
                this.Close();
            }
        }

        /// <summary>
        /// Funzione necessaria a inizializzare il form del menu
        /// </summary>
        private void initializeMenu()
        {
            // Assegna al menu un nuovo menu
            this.menu = new Menu();
            this.Text = "BlockBreaker - Menu";
            // Inizializza il menu
            initializeForm(menu);

            // Controlla se e' la prima partita dell utente
            if (again)
                this.menu.start.Text = "Play Again";
            else
                this.menu.start.Text = "Play";

            // Assegna al pulsante del menu un evento
            this.menu.start.Click += new EventHandler(this.StartGame);
            this.menu.start.Text = "Play";

            // Fa partire la musica del menu
            this.Music.Menu();
        }

        // Funzione chiamata quando viene ridimensionata la finestra
        private void OnSizeChange(object sender, EventArgs e)
        {
            if (Game != null)
                Game.Logic.waitResize = true;

            // Imposta i nuovi valori del gamePanel
            GamePanels.Height = this.ClientRectangle.Height;
            GamePanels.Width = this.ClientRectangle.Width;
            GamePanels.Top = 0;
            GamePanels.Left = 0;

            // Salva le dimensioni del client
            lunghezza_client = this.ClientRectangle.Width;
            altezza_client = this.ClientRectangle.Height;

            if (Game != null)
            {
                this.initializeForm(Game);
                this.Game.on_resize(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);
                this.Game.ball.totalVelocityReset(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);

            }
            if (this.menu != null)
            {
                this.initializeForm(this.menu);
                this.menu.on_resize(this.Width, this.Height);
                this.menu.start.Click += new EventHandler(this.StartGame);
            }
            if (this.GameOver != null)
            {
                this.initializeForm(GameOver);
                this.GameOver.on_resize(this.Width, this.Height);
            }
            if (this.Game != null)
                Game.Logic.waitResize = false;

            // Aggiorna le nuove dimensioni iniziali del client
            lunghezza_client_iniziale = this.ClientRectangle.Width;
            altezza_client_iniziale = this.ClientRectangle.Height;
        }

        /// <summary>
        /// Funzione necessaria a far partire il gioco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void StartGame(object sender, EventArgs e)
        {
            //Pulisco tutto
            DisposeAll();

            //Inizializzo il GamePanel
            initializeGamePanel();

            //Inizializzo il Gioco
            initializeGame();

            //Svuoto il Garbage collector per liberare memoria
            GC.Collect();

            //Aspetto che il garbage collecor abbia finito di svuotare
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
        }

        #endregion Methods
    }
}