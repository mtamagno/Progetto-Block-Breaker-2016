using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Container : Form
    {
        #region Fields

        private bool again;
        private int altezza_client;
        private int altezza_client_iniziale;
        private Game Game;
        private Thread GameAlive;
        private GameOver GameOver;
        private Panel GamePanels;
        private HighScore HighScore;
        private int lunghezza_client;
        private int lunghezza_client_iniziale;

        private Menu menu;
        private Music Music;
        private TextBox textBox = new TextBox();

        #endregion Fields

        #region Constructors

        public Container()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void Container2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //se l utente preme x dalla schermata di gioco devo fermare il thread del gioco
            if (Game != null)
                Game.Logic.vita_rimanente = 0;

            //pulisco tutto
            DisposeAll();
            base.OnClosing(e);
            System.Environment.Exit(0);
        }

        private void Container2_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new System.Drawing.Size(700, 450);
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

        private void ContinueToMenu(object sender, EventArgs e)
        {
            // Salva prima lo score, poi l'HighScore nell'xml
            this.HighScore.Name = GameOver.highScore.Name;
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

        private void DisposeAll()
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

                //pulisco il Thread
                if (this.GameAlive != null)
                    this.GameAlive = null;

                //Pulisco il garbage collector
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCComplete();
            }));
        }

        private void gameoverCheck()
        {
            //controllo se il Game e' ancora attivo
            while (Game != null)
            {
                //se il Game e' ancora attivo controllo se l utente ha finito il gioco
                if (Game.Logic.shouldStop == true && Game.Logic.waitResize == false)
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
                    return;
                }
                GC.Collect();
                GC.WaitForPendingFinalizers();

                //eseguo questo controllo ogni 2 secondi
                Thread.Sleep(2000);
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
        }

        private void initializeGame()
        {   //assegno al gioco un nuovo gioco
            Game = new Game();

            // Inizializza il gioco
            initializeForm(Game);

            // Inizializza il thread per controllare lo stato del gioco
            initializeThread(GameAlive);

            //faccio partire la musica di gioco
            Music.Game();
        }

        private void initializeGameOver()
        {
            //assegno al GameOver un nuovo GameOver
            GameOver = new GameOver();

            // Inizializza il GameOver
            initializeForm(GameOver);

            //Assegno un testo al pulsante del form GameOver
            GameOver.Continue.Text = "Continue";

            //Assegno un evento al pusalnte del GameOver
            GameOver.Continue.Click += new EventHandler(this.ContinueToMenu);

            //Faccio partire la musica del GameOver
            Music.GameOver();
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

            //aggiungo il pannello al form
            this.Invoke(new MethodInvoker(delegate
            {
                this.Controls.Add(GamePanels);
            }));
        }

        private void initializeMenu()
        {
            // Assegna al menu un nuovo menu
            this.menu = new Menu();

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

        private void initializeThread(Thread thread)
        {   // Assegna al thread una funzione
            GameAlive = new Thread(gameoverCheck);

            // Inizializza il thread
            GameAlive.Start();
        }

        // Funzione chiamata quando viene ridimensionata la finestra
        private void OnResizeEnd(object sender, EventArgs e)
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