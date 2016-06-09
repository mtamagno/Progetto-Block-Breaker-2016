﻿using System;
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
        private int lunghezza_client;
        private int lunghezza_client_iniziale;

        private TextBox textBox = new TextBox();
        private Game Game;
        private Thread gameAlive;
        private GameOver GameOver;
        private Panel GamePanels;
        private HighScore highScore;
        private Start Menu;
        private Music music;

        #endregion Fields

        #region Constructors

        public Container()
        {
            InitializeComponent();
        }

        #endregion Constructors

        #region Methods

        private void Container2_Load(object sender, EventArgs e)
        {
            this.MinimumSize = new System.Drawing.Size(700,450);
            music = new Music();

            // Imposta again a false per dire che il gioco e' stato avviato per la prima volta
            again = false;
            
            // Inizializza il pannello che conterra' i form dell'applicazione
            initializeGamePanel();
            GamePanels.AutoSizeMode = AutoSizeMode;

            // Inizializza il Menu come primo form che apparira' nel gioco
            initializeMenu();
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

        private void initializeGame()
        {   //assegno al gioco un nuovo gioco
            Game = new Game();

            // Inizializza il gioco
            initializeForm(Game);

            // Inizializza il thread per controllare lo stato del gioco
            initializeThread(gameAlive);

            //faccio partire la musica di gioco
            music.Game();
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
            music.GameOver();
        }

        private void initializeMenu()
        {
            //assegno al Menu un nuovo Menu
            Menu = new Start();

            // Inizializza il Menu
            initializeForm(Menu);

            //Controllo se e' la prima partita dell utente
            if (again)
                Menu.start.Text = "Play Again";
            else
                Menu.start.Text = "Play";

            //Assegno al pulsante del Menu un evento
            Menu.start.Click += new EventHandler(this.StartGame);
            
            //Faccio partire la musica del Menu
            music.Menu();
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

        private void initializeThread(Thread thread)
        {   //assegno al thread una funzione
            gameAlive = new Thread(gameoverCheck);

            // Inizializza il thread
            gameAlive.Start();
        }

        //Funzione chiamata quando si sta ridimensionando la finestra
        private void Container2_ResizeBegin(object sender, EventArgs e)
        {
            //Salvo le dimensioni del client prima del ridimensionamento per poter calcolare la proporzione
            lunghezza_client_iniziale = this.ClientRectangle.Width;
            altezza_client_iniziale = this.ClientRectangle.Height;
        }

        // FUnzione chiamata quando viene ridimensionata la finestra
        private void OnResizeEnd(object sender, EventArgs e)
        {
            if(Game != null)
            Game.Logic.waitResize = true;
            // Imposta i nuovi valori del gamePanel
            GamePanels.Height = this.ClientRectangle.Height;
            GamePanels.Width = this.ClientRectangle.Width;
            GamePanels.Top = 0;
            GamePanels.Left = 0;

            //Salvo le dimensioni del client
            lunghezza_client = this.ClientRectangle.Width;
            altezza_client = this.ClientRectangle.Height;

            if (Game != null)
            {
                initializeForm(Game);
                Game.on_resize(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);
                Game.ball.totalVelocityReset(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);
            }
            if (Menu != null)
            {
                initializeForm(Menu);
                Menu.on_resize(this.Width, this.Height);
            }
            if (GameOver != null)
            {
                initializeForm(GameOver);
                GameOver.on_resize(this.Width, this.Height);
            }
            if(Game!=null)
            Game.Logic.waitResize = false;
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

                //pulisco il Menu
                if (this.Menu != null)
                {
                    this.Menu.start.Dispose();
                    this.Menu.cleaner();
                    this.Menu.Controls.Clear();
                    this.Menu.Close();
                    this.Menu.Dispose();
                    this.Menu = null;
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
                if (this.gameAlive != null)
                    this.gameAlive = null;

                //Pulisco il garbage collector
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCComplete();
            }));
        }

        private void ContinueToMenu(object sender, EventArgs e)
        {
            // Salva prima lo score, poi l'highScore nell'xml
            this.highScore.Name = GameOver.highScore.Name;
            HighScoreSaver highScoreSaver = new HighScoreSaver();
            highScoreSaver.ModifyOrCreateXML(highScore);

            // Imposta che il giocatore ha gia finito una partita
            this.again = true;

            // Pulisce tutto
            this.DisposeAll();

            // Inizializza il gamePanel
            this.initializeGamePanel();

            // Inizializza il Menu
            this.initializeMenu();

            // Svuota il garbage collector per liberare memoria
            GC.Collect();

            // Aspetta che il garbage collecor finisca
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
        }


        private void gameoverCheck()
        {
            //controllo se il Game e' ancora attivo
            while (Game != null)
            {
                //se il Game e' ancora attivo controllo se l utente ha finito il gioco
                if (Game.Logic.shouldStop == true && Game.Logic.waitResize == false)
                {
                    //se l'utente ha finito il gioco salvo l highScore ottenuto
                    this.highScore = this.Game.Logic.highScore;

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

        #endregion Methods

     
    }
}