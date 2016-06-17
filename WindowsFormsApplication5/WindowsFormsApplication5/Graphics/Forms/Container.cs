using System;
using System.Threading;
using System.Windows.Forms;
using System.Drawing;

namespace BlockBreaker
{
    public partial class Container : Form
    {
        #region Fields

        private bool _again;
        private int _altezzaClient;
        private int _altezzaClientIniziale;
        private Game _game;
        private GameOver _gameOver;
        private Panel _gamePanels;
        private HighScore _highScore;
        private int _lunghezzaClient;
        private int _lunghezzaClientIniziale;
        private Menu _menu;
        private AudioButtons _audioButton;
        private Music _music;
        private bool _audioOnOff;

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
            if (_game != null)
                {
                    _game.Logic.ShouldStop = true;

                    while (_game.GameThread.IsAlive) {
                        _game.Logic.ShouldStop = true;
                    }
                    while (_game.IsAccessible)
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

            _audioOnOff = true;
            this._music = new Music();

            // Imposta _again a false per dire che il gioco e' stato avviato per la prima volta
            this._again = false;

            // Inizializza il pannello che conterra' i form dell'applicazione
            this.InitializeGamePanel();
            this._gamePanels.AutoSizeMode = AutoSizeMode;

            // Inizializza il _menu come primo form che apparira' nel gioco
            this.InitializeMenu();

            //Salvo le dimensioni del client prima dei ridimensionamenti per poter calcolare la proporzione
            _lunghezzaClientIniziale = this.ClientRectangle.Width;
            _altezzaClientIniziale = this.ClientRectangle.Height;
        }

        private void ButtonAudioSet()
        {
            if (_audioButton != null)
                _audioButton.Dispose();
            Size s = new Size(30, 30);
            _audioButton = new AudioButtons(s);
            _audioButton.Visible = true;
            _audioButton.Left = 50;
            _audioButton.Top = this.Height - this._audioButton.Height - 45;
            this.Controls.Add(_audioButton);
            _audioButton.BringToFront();
            this._audioButton.MouseClick += Audio;
            _audioButton.TabStop = false;
            _audioButton.BackColor = Color.Transparent;

        }

        private void Audio(object sender, EventArgs e)
        {
            _audioOnOff = !_audioOnOff;

            if (!_audioOnOff)
                this._music.BackgroundMusic.Stop();
            else
                this._music.BackgroundMusic.Play();

            _audioButton.ChangeState();
            this.ProcessTabKey(true);

        }

        /// <summary>
        /// Funzione necessaria per cambiare form dopo la fine della partita
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContinueToMenu(object sender, EventArgs e)
        {

            if (_gameOver.TextBox.Text != "Insert Name..." && !string.IsNullOrEmpty(_gameOver.TextBox.Text) && !string.IsNullOrWhiteSpace(_gameOver.TextBox.Text))
            {
            // Salva prima lo score, poi l'_highScore nell'xml
                this._highScore.Name = _gameOver.TextBox.Text;
            HighScoreSaver highScoreSaver = new HighScoreSaver();
            highScoreSaver.ModifyOrCreateXML(_highScore);

            // Imposta che il giocatore ha gia finito una partita
            this._again = true;

            // Pulisce tutto
            this.DisposeAll();

            // Inizializza il gamePanel
            this.InitializeGamePanel();

            // Inizializza il _menu
            this.InitializeMenu();

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
                this.Controls.Remove(_gamePanels);

                //Pulisco il gamepanel
                this._gamePanels.Dispose();
                    this._gamePanels.Controls.Clear();

                // Imposta il gamePanel a null
                this._gamePanels = null;

                //pulisco il container
                this.Controls.Clear();

                //pulisco il gioco
                if (this._game != null)
                    {
                        this._game.Controls.Clear();
                        this._game.Close();
                        this._game.Dispose();
                        this._game = null;
                    }

                //pulisco il _menu
                if (this._menu != null)
                    {
                        this._menu.Start.Dispose();
                        this._menu.Cleaner();
                        this._menu.Controls.Clear();
                        this._menu.Close();
                        this._menu.Dispose();
                        this._menu = null;
                    }

                //pulisco il _gameOver
                if (this._gameOver != null)
                    {
                        this._gameOver.Continue.Dispose();
                        this._gameOver.Cleaner();
                        this._gameOver.Controls.Clear();
                        this._gameOver.Close();
                        this._gameOver.Dispose();
                        this._gameOver = null;
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

        private void InitializeForm(Form form)
        {
            // Imposta il topLevel del form a false
            form.TopLevel = false;
            form.AutoScaleMode = AutoScaleMode.Inherit;

            // Imposta altezza e larghezza del form
            form.Width = _gamePanels.Width;
            form.Height = _gamePanels.Height;

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
                _gamePanels.Controls.Add(form);
                form.Show();
                form.Focus();
                form.BringToFront();
            }));
            ButtonAudioSet();
            if (_audioOnOff == false)
            {
                _audioOnOff = true;
            }
            form.Focus();
        }



        /// <summary>
        /// Funzione necessaria ad inizializzare il form del gioco
        /// </summary>
        private void InitializeGame()
        {   
            //assegno al gioco un nuovo gioco
            _game = new Game();

            // Inizializza il gioco
            InitializeForm(_game);

            //faccio partire la musica di gioco
            _music.Game();

            //assegno un evento alla chiusura del _game
            _game.VisibleChanged += new EventHandler(OnGameover);
        }

        /// <summary>
        /// Funzione necessaria a inizializzare il form del gameover
        /// </summary>
        private void InitializeGameOver()
        {
            //assegno al _gameOver un nuovo _gameOver
            _gameOver = new GameOver();

            this.Text = "BlockBreaker - Gameover";

            // Inizializza il _gameOver
            InitializeForm(_gameOver);

            //Assegno un testo al pulsante del form _gameOver
            _gameOver.Continue.Text = "Continue";

            //Assegno un evento al pusalnte del _gameOver
            _gameOver.Continue.Click += new EventHandler(this.ContinueToMenu);

            //Faccio partire la musica del _gameOver
            _music.GameOver();
        }

        private void OnGameover(object sender, EventArgs e)
        {
                //se l'utente ha finito il gioco salvo l _highScore ottenuto
                this._highScore = this._game.Logic.HighScore;

                //ri Imposta il _game.Logic a false
                _game.Logic.ShouldStop = false;

                //pulisco tutto
                DisposeAll();

                // Inizializza di nuovo il gamePanel
                InitializeGamePanel();

                // Inizializza il _gameOver
                InitializeGameOver();
            
            GC.Collect();
            GC.WaitForPendingFinalizers();

        }

        private void InitializeGamePanel()
        {
            //assegno al pannello un nuovo pannello
            _gamePanels = new Panel();

            // Imposta altezza e lunghezza del pannello
            _gamePanels.Width = this.Width;
            _gamePanels.Height = this.Height;

            // Imposta il metodo Dock per far riempire al pannello tutto lo spazio del form
            this.Dock = DockStyle.Fill;

            // Imposta la posizione iniziale del pannello
            _gamePanels.Top = 0;
            _gamePanels.Left = 0;

            // Imposta il Dock del pannello per far riempire ai Form figli del pannello tutto lo spazio del pannello
            _gamePanels.Dock = DockStyle.Fill;
            _gamePanels.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;

            //mostro il pannello
            _gamePanels.Show();

            try
            {
            //aggiungo il pannello al form
            this.Invoke(new MethodInvoker(delegate
            {
                this.Controls.Add(_gamePanels);
            }));
        }
            catch(InvalidOperationException)
            {
                Thread.Sleep(1000);
                this.Close();
            }
        }

        /// <summary>
        /// Funzione necessaria a inizializzare il form del _menu
        /// </summary>
        private void InitializeMenu()
        {
            // Assegna al _menu un nuovo _menu
            this._menu = new Menu();
            this.Text = "BlockBreaker - Menu";
            // Inizializza il _menu
            InitializeForm(_menu);

            // Controlla se e' la prima partita dell utente
            if (_again)
                this._menu.Start.Text = "Play Again";
            else
                this._menu.Start.Text = "Play";

            // Assegna al pulsante del _menu un evento
            this._menu.Start.Click += new EventHandler(this.StartGame);
            this._menu.Start.Text = "Play";

            // Fa partire la musica del _menu
            this._music.Menu();
        }

        // Funzione chiamata quando viene ridimensionata la finestra
        private void OnSizeChange(object sender, EventArgs e)
        {
            if (_game != null)
                _game.Logic.WaitResize = true;

            // Imposta i nuovi valori del gamePanel
            _gamePanels.Height = this.ClientRectangle.Height;
            _gamePanels.Width = this.ClientRectangle.Width;
            _gamePanels.Top = 0;
            _gamePanels.Left = 0;

            // Salva le dimensioni del client
            _lunghezzaClient = this.ClientRectangle.Width;
            _altezzaClient = this.ClientRectangle.Height;

            if (_game != null)
            {
                this.InitializeForm(_game);
                this._game.on_resize(_lunghezzaClientIniziale, _altezzaClientIniziale, _lunghezzaClient, _altezzaClient);
                this._game.Ball.TotalVelocityReset(_lunghezzaClientIniziale, _altezzaClientIniziale, _lunghezzaClient, _altezzaClient);

            }
            if (this._menu != null)
            {
                this.InitializeForm(this._menu);
                this._menu.on_resize(this.Width, this.Height);
                this._menu.Start.Click += new EventHandler(this.StartGame);
            }
            if (this._gameOver != null)
            {
                this.InitializeForm(_gameOver);
                this._gameOver.on_resize(this.Width, this.Height);
            }
            if (this._game != null)
                _game.Logic.WaitResize = false;

            // Aggiorna le nuove dimensioni iniziali del client
            _lunghezzaClientIniziale = this.ClientRectangle.Width;
            _altezzaClientIniziale = this.ClientRectangle.Height;
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
            InitializeGamePanel();

            //Inizializzo il Gioco
            InitializeGame();

            //Svuoto il Garbage collector per liberare memoria
            GC.Collect();

            //Aspetto che il garbage collecor abbia finito di svuotare
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
        }

        #endregion Methods
    }
}