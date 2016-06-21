using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BlockBreaker
{
    public partial class Container : Form
    {
        #region Private Fields

        private bool _again;
        private int _altezzaClient;
        private int _altezzaClientIniziale;
        private AudioButtons _audioButton;
        private bool _audioOnOff;
        private Game _game;
        private GameOver _gameOver;
        private Panel _gamePanels;
        private HighScore _highScore;
        private int _lunghezzaClient;
        private int _lunghezzaClientIniziale;
        private Menu _menu;
        private Music _music;

        #endregion Private Fields

        #region Public Constructors

        public Container()
        {
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Private Methods

        private void Audio(object sender, EventArgs e)
        {
            _audioOnOff = !_audioOnOff;
            if (!_audioOnOff)
                _music.BackgroundMusic.Stop();
            else
                _music.BackgroundMusic.Play();
            _audioButton.ChangeState();
            ProcessTabKey(true);
        }

        private void ButtonAudioSet()
        {
            if (_audioButton != null)
                _audioButton.Dispose();
            var s = new Size(30, 30);
            _audioButton = new AudioButtons(s);
            _audioButton.Visible = true;
            _audioButton.Left = 50;
            _audioButton.Top = Height - _audioButton.Height - 45;
            Controls.Add(_audioButton);
            _audioButton.BringToFront();
            _audioButton.MouseClick += Audio;
            _audioButton.TabStop = false;
            _audioButton.BackColor = Color.Transparent;
        }

        /// <summary>
        ///     Funzione Per il Caricamento di Container
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Container2_Load(object sender, EventArgs e)
        {
            MinimumSize = new Size(700, 450);
            _audioOnOff = true;
            _music = new Music();

            // Imposta _again a false per dire che il gioco e' stato avviato per la prima volta
            _again = false;

            // Inizializza il pannello che conterra' i form dell'applicazione
            InitializeGamePanel();
            _gamePanels.AutoSizeMode = AutoSizeMode;

            // Inizializza il _menu come primo form che apparira' nel gioco
            InitializeMenu();

            //Salvo le dimensioni del client prima dei ridimensionamenti per poter calcolare la proporzione
            _lunghezzaClientIniziale = ClientRectangle.Width;
            _altezzaClientIniziale = ClientRectangle.Height;
        }

        /// <summary>
        ///     Funzione per la chiusura di container
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContainerFormClosing(object sender, FormClosingEventArgs e)
        {
            Invoke(new MethodInvoker(delegate
            {
                //se l utente preme x dalla schermata di gioco devo fermare il thread del gioco
                if (_game != null)
                {
                    _game.Logic.ShouldStop = true;
                    _game.GameThread.Join();
                    /* while (_game.GameThread.IsAlive)
                    {
                        _game.Logic.ShouldStop = true;
                        _game.GameThread.Priority = ThreadPriority.Highest;
                        _game.GameThread.Join();
                    }*/
                }
                _music.Dispose_Music();

                //pulisco tutto
                DisposeAll();
                OnClosing(e);
                Environment.Exit(0);
            }));
        }

        /// <summary>
        ///     Funzione necessaria per cambiare form dopo la fine della partita
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ContinueToMenu(object sender, EventArgs e)
        {
            if (_gameOver.TextBox.Text != "Insert Name..." && !string.IsNullOrEmpty(_gameOver.TextBox.Text) &&
                !string.IsNullOrWhiteSpace(_gameOver.TextBox.Text))
            {
                // Salva prima lo score, poi l'_highScore nell'xml
                _highScore.Name = _gameOver.TextBox.Text;
                var highScoreSaver = new HighScoreSaver();
                highScoreSaver.ModifyOrCreateXml(_highScore);

                // Imposta che il giocatore ha gia finito una partita
                _again = true;

                // Pulisce tutto
                DisposeAll();

                // Inizializza il gamePanel
                InitializeGamePanel();

                // Inizializza il _menu
                InitializeMenu();

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
                Invoke(new MethodInvoker(delegate
                {
                    //rimuovo il gamePanel dal container
                    Controls.Remove(_gamePanels);

                    //Pulisco il gamepanel
                    _gamePanels.Dispose();
                    _gamePanels.Controls.Clear();

                    // Imposta il gamePanel a null
                    _gamePanels = null;

                    //pulisco il container
                    Controls.Clear();

                    //pulisco il gioco
                    if (_game != null)
                    {
                        _game.Controls.Clear();
                        _game.Close();
                        _game.Dispose();
                        _game = null;
                    }

                    //pulisco il _menu
                    if (_menu != null)
                    {
                        _menu.Start.Dispose();
                        _menu.Cleaner();
                        _menu.Controls.Clear();
                        _menu.Close();
                        _menu.Dispose();
                        _menu = null;
                    }

                    //pulisco il _gameOver
                    if (_gameOver != null)
                    {
                        _gameOver.Continue.Dispose();
                        _gameOver.Cleaner();
                        _gameOver.Controls.Clear();
                        _gameOver.Close();
                        _gameOver.Dispose();
                        _gameOver = null;
                    }

                    //Pulisco il garbage collector
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.WaitForFullGCComplete();
                }));
            }
            catch (InvalidOperationException)
            {
                Close();
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
            Invoke(new MethodInvoker(delegate
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
        ///     Funzione necessaria ad inizializzare il form del gioco
        /// </summary>
        private void InitializeGame()
        {
            //assegno al gioco un nuovo gioco
            _game = new Game();

            // Inizializza il gioco
            InitializeForm(_game);
            _game.Refresh();

            //faccio partire la musica di gioco
            _music.Game();

            //assegno un evento alla chiusura del _game
            _game.VisibleChanged += OnGameover;
        }

        /// <summary>
        ///     Funzione necessaria a inizializzare il form del gameover
        /// </summary>
        private void InitializeGameOver()
        {
            //assegno al _gameOver un nuovo _gameOver
            _gameOver = new GameOver();
            Text = "BlockBreaker - Gameover";

            // Inizializza il _gameOver
            InitializeForm(_gameOver);

            //Assegno un testo al pulsante del form _gameOver
            _gameOver.Continue.Text = "Continue";

            //Assegno un evento al pusalnte del _gameOver
            _gameOver.Continue.Click += ContinueToMenu;

            //Faccio partire la musica del _gameOver
            _music.GameOver();
        }

        private void InitializeGamePanel()
        {
            //assegno al pannello un nuovo pannello
            _gamePanels = new Panel();

            // Imposta altezza e lunghezza del pannello
            _gamePanels.Width = Width;
            _gamePanels.Height = Height;

            // Imposta il metodo Dock per far riempire al pannello tutto lo spazio del form
            Dock = DockStyle.Fill;

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
                Invoke(new MethodInvoker(delegate { Controls.Add(_gamePanels); }));
            }
            catch (InvalidOperationException)
            {
                Thread.Sleep(1000);
                Close();
            }
        }

        /// <summary>
        ///     Funzione necessaria a inizializzare il form del _menu
        /// </summary>
        private void InitializeMenu()
        {
            // Assegna al _menu un nuovo _menu
            _menu = new Menu();
            Text = "BlockBreaker - Menu";

            // Inizializza il _menu
            InitializeForm(_menu);

            // Controlla se e' la prima partita dell utente
            if (_again)
                _menu.Start.Text = "Play Again";
            else
                _menu.Start.Text = "Play";

            // Assegna al pulsante del _menu un evento
            _menu.Start.Click += StartGame;
            _menu.Start.Text = "Play";

            // Fa partire la musica del _menu
            _music.Menu();
        }

        private void OnGameover(object sender, EventArgs e)
        {
            //se l'utente ha finito il gioco salvo l _highScore ottenuto
            _highScore = _game.Logic.HighScore;

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

        // Funzione chiamata quando viene ridimensionata la finestra
        private void OnSizeChange(object sender, EventArgs e)
        {
            if (_game != null)
                _game.Logic.WaitResize = true;

            // Imposta i nuovi valori del gamePanel
            _gamePanels.Height = ClientRectangle.Height;
            _gamePanels.Width = ClientRectangle.Width;
            _gamePanels.Top = 0;
            _gamePanels.Left = 0;

            // Salva le dimensioni del client
            _lunghezzaClient = ClientRectangle.Width;
            _altezzaClient = ClientRectangle.Height;
            if (_game != null)
            {
                InitializeForm(_game);
                _game.on_resize(_lunghezzaClientIniziale, _altezzaClientIniziale, _lunghezzaClient, _altezzaClient);
                _game.Ball.TotalVelocityReset(_lunghezzaClientIniziale, _altezzaClientIniziale, _lunghezzaClient,
                    _altezzaClient);
            }
            if (_menu != null)
            {
                InitializeForm(_menu);
                _menu.on_resize(Width, Height);
                _menu.Start.Click += StartGame;
            }
            if (_gameOver != null)
            {
                InitializeForm(_gameOver);
                _gameOver.on_resize(Width, Height);
            }
            if (_game != null)
                _game.Logic.WaitResize = false;

            // Aggiorna le nuove dimensioni iniziali del client
            _lunghezzaClientIniziale = ClientRectangle.Width;
            _altezzaClientIniziale = ClientRectangle.Height;
        }

        /// <summary>
        ///     Funzione necessaria a far partire il gioco
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

        #endregion Private Methods
    }
}
