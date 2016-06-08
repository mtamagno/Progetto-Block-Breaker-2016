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
        private int lunghezza_client;
        private int lunghezza_client_iniziale;

        private TextBox textBox = new TextBox();
        private Game game;
        private Thread gameAlive;
        private GameOver gameover;
        private Panel gamePanels;
        private HighScore highScore;
        private Start menu;
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
            this.MinimumSize = new System.Drawing.Size(400,400);
            music = new Music();

            // Imposta again a false per dire che il gioco e' stato avviato per la prima volta
            again = false;

            // Inizializza il pannello che conterra' i form dell'applicazione
            initializeGamePanel();
            gamePanels.AutoSizeMode = AutoSizeMode;

            // Inizializza il menu come primo form che apparira' nel gioco
            initializeMenu();
        }

        private void initializeGamePanel()
        {
            //assegno al pannello un nuovo pannello
            gamePanels = new Panel();

            // Imposta altezza e lunghezza del pannello
            gamePanels.Width = this.Width;
            gamePanels.Height = this.Height;

            // Imposta il metodo Dock per far riempire al pannello tutto lo spazio del form
            this.Dock = DockStyle.Fill;

            // Imposta la posizione iniziale del pannello
            gamePanels.Top = 0;
            gamePanels.Left = 0;

            // Imposta il Dock del pannello per far riempire ai Form figli del pannello tutto lo spazio del pannello
            gamePanels.Dock = DockStyle.Fill;
            gamePanels.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;

            //mostro il pannello
            gamePanels.Show();

            //aggiungo il pannello al form
            this.Invoke(new MethodInvoker(delegate
            {
                this.Controls.Add(gamePanels);
            }));
        }

        private void initializeGame()
        {   //assegno al gioco un nuovo gioco
            game = new Game();

            // Inizializza il gioco
            initializeForm(game);

            // Inizializza il thread per controllare lo stato del gioco
            initializeThread(gameAlive);

            //faccio partire la musica di gioco
            music.Game();
        }

        private void initializeGameOver()
        {
            //assegno al gameover un nuovo gameover
            gameover = new GameOver();

            // Inizializza il gameover
            initializeForm(gameover);

            //Assegno un testo al pulsante del form gameover
            gameover.Continue.Text = "Continue";

            //Assegno un evento al pusalnte del gameover
            gameover.Continue.Click += new EventHandler(this.ContinueToMenu);

            //Faccio partire la musica del gameover
            music.GameOver();
        }

        private void initializeMenu()
        {
            //assegno al menu un nuovo menu
            menu = new Start();

            // Inizializza il menu
            initializeForm(menu);

            //Controllo se e' la prima partita dell utente
            if (again)
                menu.start.Text = "Play Again";
            else
                menu.start.Text = "Play";

            //Assegno al pulsante del menu un evento
            menu.start.Click += new EventHandler(this.StartGame);
            
            //Faccio partire la musica del menu
            music.Menu();
        }

        private void initializeForm(Form form)
        {
            // Imposta il topLevel del form a false
            form.TopLevel = false;
            form.AutoScaleMode = AutoScaleMode.Inherit;

            // Imposta altezza e larghezza del form
            form.Width = gamePanels.Width;
            form.Height = gamePanels.Height;

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
                gamePanels.Controls.Add(form);
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
            if(game != null)
            game.logic.waitResize = true;
            // Imposta i nuovi valori del gamePanel
            gamePanels.Height = this.ClientRectangle.Height;
            gamePanels.Width = this.ClientRectangle.Width;
            gamePanels.Top = 0;
            gamePanels.Left = 0;

            //Salvo le dimensioni del client
            lunghezza_client = this.ClientRectangle.Width;
            altezza_client = this.ClientRectangle.Height;

            if (game != null)
            {
                initializeForm(game);
                game.on_resize(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);
                game.ball.totalVelocityReset(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);
            }
            if (menu != null)
            {
                initializeForm(menu);
                menu.on_resize(this.Width, this.Height);
            }
            if (gameover != null)
            {
                initializeForm(gameover);
                gameover.on_resize(this.Width, this.Height);
            }
            if(game!=null)
            game.logic.waitResize = false;
        }

        private void DisposeAll()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                //rimuovo il gamePanel dal container
                this.Controls.Remove(gamePanels);

                //Pulisco il gamepanel
                gamePanels.Dispose();
                gamePanels.Controls.Clear();

                // Imposta il gamePanel a null
                gamePanels = null;

                //pulisco il container
                this.Controls.Clear();

                //pulisco il gioco
                if (game != null)
                {
                    game.Controls.Clear();
                    game.Close();
                    game.Dispose();
                    game = null;
                }

                //pulisco il menu
                if (menu != null)
                {
                    menu.start.Dispose();
                    menu.cleaner();
                    menu.Controls.Clear();
                    menu.Close();
                    menu.Dispose();
                    menu = null;
                }

                //pulisco il gameover
                if (gameover != null)
                {
                    gameover.Continue.Dispose();
                    gameover.cleaner();
                    gameover.Controls.Clear();
                    gameover.Close();
                    gameover.Dispose();
                    gameover = null;
                }

                //pulisco il Thread
                if (gameAlive != null)
                    gameAlive = null;

                //Pulisco il garbage collector
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCComplete();
            }));
        }

        private void ContinueToMenu(object sender, EventArgs e)
        {
            // Salva prima lo score, poi l'highScore nell'xml
            this.highScore.Name = gameover.highScore.Name;
            HighScoreSaver highScoreSaver = new HighScoreSaver();
            highScoreSaver.ModifyOrCreateXML(highScore);

            // Imposta che il giocatore ha gia finito una partita
            again = true;

            // Pulisce tutto
            DisposeAll();

            // Inizializza il gamePanel
            initializeGamePanel();

            // Inizializza il Menu
            initializeMenu();

            // Svuota il garbage collector per liberare memoria
            GC.Collect();

            // Aspetta che il garbage collecor finisca
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
        }


        private void gameoverCheck()
        {
            //controllo se il game e' ancora attivo
            while (game != null)
            {
                //se il game e' ancora attivo controllo se l utente ha finito il gioco
                if (game.logic.shouldStop == true && game.logic.waitResize == false)
                {
                    //se l'utente ha finito il gioco salvo l highScore ottenuto
                    this.highScore = this.game.logic.highScore;

                    //ri Imposta il game.logic a false
                    game.logic.shouldStop = false;

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
            if (game != null)
                game.logic.vita_rimanente = 0;

            //pulisco tutto
            DisposeAll();
            base.OnClosing(e);
            System.Environment.Exit(0);
        }

        #endregion Methods

     
    }
}