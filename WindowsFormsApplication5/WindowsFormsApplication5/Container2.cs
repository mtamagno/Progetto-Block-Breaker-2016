using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication5
{
    public partial class Container2 : Form
    {

        #region Public Fields

        public int altezza_client;
        public int altezza_client_iniziale;
        public int lunghezza_client;
        public int lunghezza_client_iniziale;

        #endregion Public Fields

        private Game game;
        private Start menu;
        private GameOver gameover;
        private Panel gamePanels;
        private HighScore highscore;
        public TextBox textBox = new TextBox();
        public bool again;
        Thread gameAlive;
        private Music music;

        public Container2()
        {
            InitializeComponent();
        }

        private void Container2_Load(object sender, EventArgs e)
        {
            music = new Music();
            //setto again a false per dire che il gioco e' stato avviato per la prima volta
            again = false;
            //inizializzo il pannello che conterra' i form dell'applicazione
            initializeGamePanel();
            //inizializzo il menu come primo form che apparira' nel gioco
            initializeMenu();
        }

        private void initializeGamePanel()
        {
            //assegno al pannello un nuovo pannello
            gamePanels = new Panel();
            //setto altezza e lunghezza del pannello
            gamePanels.Width = this.Width;
            gamePanels.Height = this.Height;
            //setto il metodo Dock per far riempire al pannello tutto lo spazio del form 
            this.Dock = DockStyle.Fill;
            //setto la posizione iniziale del pannello
            gamePanels.Top = 0;
            gamePanels.Left = 0;
            //setto il Dock del pannello per far riempire ai Form figli del pannello tutto lo spazio del pannello
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
            //inizializzo il gioco
            initializeForm(game);
            //inizializzo il thread per controllare lo stato del gioco
            initializeThread(gameAlive);
            //faccio partire la musica di gioco
            music.Game();
        }

        private void initializeMenu()
        {
            //assegno al menu un nuovo menu
            menu = new Start();
            //inizializzo il menu
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

        private void initializeGameOver()
        {
            //assegno al gameover un nuovo gameover
            gameover = new GameOver();
            //inizializzo il gameover
            initializeForm(gameover);
            //Assegno un testo al pulsante del form gameover
            gameover.Continue.Text = "Continue";
            //Assegno un evento al pusalnte del gameover
            gameover.Continue.Click += new EventHandler(this.ContinueToMenu);
            //Faccio partire la musica del gameover
            music.GameOver();
        }

        private void initializeForm(Form form)
        {
            //setto il topLevel del form a false
            form.TopLevel = false;
            form.AutoScaleMode = AutoScaleMode.Inherit;
            //setto altezza e larghezza del form
            form.Width = gamePanels.Width;
            form.Height = gamePanels.Height;
            //setto la posizione del form
            form.Left = 0;
            form.Top = 0;
            //rimuovo il bordo del form
            form.FormBorderStyle = FormBorderStyle.None;
            //setto il form per occupare tutto lo spazio disponibile
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
            //inizializzo il thread
            gameAlive.Start();
        }

        public void gameoverCheck()
        {
            //controllo se il game e' ancora attivo
            while (game != null)
            {
                //se il game e' ancora attivo controllo se l utente ha finito il gioco
                if (game.logic.shouldStop == true)
                {
                    //se l'utente ha finito il gioco salvo l highscore ottenuto
                    this.highscore = this.game.logic.highscore;
                    //risetto il game.logic a false
                    game.logic.shouldStop = false;
                    //pulisco tutto
                    DisposeAll();
                    //inizializzo di nuovo il gamePanel
                    initializeGamePanel();
                    //inizializzo il GameOver
                    initializeGameOver();
                    return;
                }
               /* GC.Collect();
                GC.WaitForPendingFinalizers();*/
                //eseguo questo controllo ogni 2 secondi
                Thread.Sleep(2000);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                GC.WaitForFullGCComplete();
            }
        }

        public void StartGame(object sender, EventArgs e)
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

        public void ContinueToMenu(object sender, EventArgs e)
        {
            //setto che il giocatore ha gia finito una partita
            again = true;
            //pulisco tutto
            DisposeAll();
            //inizializzo il gamePanel
            initializeGamePanel();
            //inizializzo il Menu
            initializeMenu();
            //Svuoto il garbage collector per liberare memoria
            GC.Collect();
            //Aspetto che il garbage collecor abbia finito di svuotare
            GC.WaitForPendingFinalizers();
            GC.WaitForFullGCComplete();
        }

        public void DisposeAll()
        {
            this.Invoke(new MethodInvoker(delegate
            {
                //rimuovo il gamePanel dal container
                this.Controls.Remove(gamePanels);   
                //Pulisco il gamepanel             
                gamePanels.Dispose();
                gamePanels.Controls.Clear();
                //setto il gamePanel a null
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

        private void Container2_FormClosing(object sender, FormClosingEventArgs e)
        {
            //se l utente preme x dalla schermata di gioco devo fermare il thread del gioco
            if(game != null)
            game.logic.vita_rimanente = 0;
            //pulisco tutto
            DisposeAll();
            base.OnClosing(e);
            System.Environment.Exit(0);
        }

        // FUnzione chiamata quando viene ridimensionata la finestra
        private void OnResizeEnd(object sender, EventArgs e)
        {
            //Setto i nuovi valori del gamePanel
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
            if(gameover != null)
            {
                initializeForm(gameover);
                gameover.on_resize(this.Width, this.Height);
            }
        }

        //Funzione chiamata quando si sta ridimensionando la finestra
        private void Container2_ResizeBegin(object sender, EventArgs e)
        {
            //Salvo le dimensioni del client prima del ridimensionamento per poter calcolare la proporzione
            lunghezza_client_iniziale = this.ClientRectangle.Width;
            altezza_client_iniziale = this.ClientRectangle.Height;
        }
    }
}
