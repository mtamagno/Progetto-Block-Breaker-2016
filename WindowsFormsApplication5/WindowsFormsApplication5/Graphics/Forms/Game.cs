using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Game : Form
    {
        #region Fields

        public Skin skin;
        public View background;
        public Ball ball;
        public Thread gameThread;
        public Grid grid;
        public Logic Logic;
        public Paddle racchetta;
        public Label score;
        public Life[] vita = new Life[3];
        private GamePause gamePause;
        private Label gameTitle;

        #endregion Fields

        #region Constructors

        public Game()
        {
            // Inizializza i componenti
            InitializeComponent();
            return;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Funzione che permette l'inizializzazione delle vite e del loro disegno iniziale
        /// </summary>
        public void life_init()
        {
            for (int i = 0; i < Logic.vita_rimanente; i++)
            {
                vita[i] = new Life(this.ClientRectangle.Width - (float)1 / 50 * this.ClientRectangle.Width
                            - (float)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))) * (i + 1) - 10 * (i + 1),
                    (float)1 / 50 * this.ClientRectangle.Height + (float)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))),
                    (int)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))),
                    (int)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))));
                Logic.iManager.inGameSprites.Add(vita[i]);
            }

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Funzione che viene chiamata al resize della finestra e che chiama le funzioni che cambiano i vari size dei componenti passandogli le nuove e le vecchie misure
        /// </summary>
        /// <param name="li">initial width</param>
        /// <param name="hi">initial heigth</param>
        /// <param name="l">new width</param>
        /// <param name="h">new heigth</param>
        public void on_resize(int li, int hi, int l, int h)
        {
            // Richiama logic.resize
            Logic.resize(li, hi, l, h);
            racchetta.Y = this.background.Height * 9 / 10 + this.background.Y;
            score.Top = this.ClientRectangle.Height - 40;
            score.Left = this.ClientRectangle.Width / 2 - this.score.Width / 2;
            gameTitle.Left = this.ClientRectangle.Width / 2 - this.gameTitle.Width / 2;

        }

        /// <summary>
        /// Funzione chiamata 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {
            try
            {
                // Prova a chiudere il form, se non ci riesce allora lo fa nel modo più lento ma sicuro
                this.Close();
            }
            catch
            {
                // Verifico che IsAlive non sia verificato
                while (this.gameThread.IsAlive) { }

                // Libera la memoria e chiudo il form
                Thread.Sleep(1000);
                this.Dispose();
                base.OnClosing(e);
                System.Environment.Exit(0);
                this.Close();
            }
        }

        /// <summary>
        /// Funzione che permette il caricamento iniziale del gioco
        /// </summary>
        /// <param name="e"></param>
        protected void OnLoad(object sender, EventArgs e)
        {
            try
            {
                loadContent();
            }
            // Gestiamo un raro caso in cui crashava il gioco che viene gestito da OnLoad
            catch
            {
                base.OnLoad(e);
                loadContent();
            }
        }

        /// <summary>
        /// Funzione che gestisce l'evento della pressione dei tasti durante l'esecuzione del gioco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Logic.AllowInput)
            {
                if (e.KeyChar == (char)Keys.Space)
                {
                    ball.followPointer = false;
                    ball.canFall = true;
                    racchetta.followPointer = true;
                    gamePause.Visible = false;
                    Logic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
                }
                if (e.KeyChar == (char)Keys.Enter && gamePause.Visible == false)
                {
                    ball.followPointer = false;
                    ball.canFall = false;
                    racchetta.followPointer = false;
                    gamePause.Visible = true;
                    Logic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
                }
                if (e.KeyChar == (char)Keys.Escape)
                {
                    this.Logic.vita_rimanente = 0;
                    this.Close();
                }
            }
        }

        /// <summary>
        /// Funzione che permette il reset del titolo del gioco per poterlo scalare
        /// </summary>
        private void gameTitleset()
        {
            gameTitle = new Label();
            gameTitle.Top = 20;
            gameTitle.Width = this.ClientRectangle.Width / 3 * 2;
            gameTitle.TextAlign = ContentAlignment.MiddleCenter;
            gameTitle.Left = this.ClientRectangle.Width / 2 - this.gameTitle.Width / 2;
            gameTitle.Text = "BlockBreaker";
            gameTitle.BackColor = Color.Black;
            gameTitle.ForeColor = Color.White;
            gameTitle.Font = new Font("Arial", 15);
            this.Controls.Add(gameTitle);
        }

        /// <summary>
        /// Funzione che permette di inizializzare la griglia
        /// </summary>
        private void init_grid()
        {
            grid = new Grid((int)this.background.X, (int)this.background.Y, this.background.Height, this.background.Width, Properties.Resources.Block_4, Logic);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Funzione che permette di caricare il contenuto all'avvio
        /// </summary>
        private void loadContent()
        {
            starter();
        }

        /// <summary>
        /// Funzione che permette
        /// </summary>
        private void scoreSet()
        {
            score = new Label();
            score.Left = this.ClientRectangle.Width / 2 - this.score.Width / 2;
            score.Top = this.ClientRectangle.Height - 40;
            score.Width = this.ClientRectangle.Width / 8;

            score.TextAlign = ContentAlignment.MiddleCenter;
            score.Text = "Score: 0";
            score.BackColor = Color.Black;
            score.ForeColor = Color.Transparent;
            score.Font = new Font("Arial", 15);
            this.Controls.Add(score);
        }

        /// <summary>
        /// Funzione starter invocata all'avvio di questo form per inizializzarlo
        /// </summary>
        private void starter()
        {
            // Inizializza la logica
            Logic = new Logic(this);

            //Inizializzo la skin

            skin = new Skin(this.ClientRectangle.X,
            this.ClientRectangle.Y,
            this.ClientRectangle.Width,
            this.ClientRectangle.Height,
            Logic);
            skin.X = 0;
            skin.Y = 0;

            // Inizializza la variabile della visione del menù pausa a falso in caso sia vera
            gamePause = new GamePause(0, 0, 1000, 500);
            gamePause.Visible = false;
            this.Controls.Add(gamePause);

            // Inizializza il background
            background = new View(this.ClientRectangle.X,
                this.ClientRectangle.Y,
                this.ClientRectangle.Width / 30 * 29,
                this.ClientRectangle.Height / 5 * 4,
                Logic);
            background.X = this.ClientRectangle.Width / 2 - this.background.Width / 2;
            background.Y = this.ClientRectangle.Height / 2 - this.background.Height / 2;

            // Inizializza griglia
            init_grid();

            // Inizializza racchetta
            if (this.Visible)
                racchetta = new Paddle(Logic.MousePoint.X - this.Location.X,
                    this.background.Height * 9 / 10 + this.background.Y,
                    (int)(Math.Abs((float)1 / 8 * this.ParentForm.ClientRectangle.Width)),
                    (int)(Math.Abs((float)1 / 15 * this.ParentForm.ClientRectangle.Height)),
                    Logic);

            // Inizializza pallina
            ball = new Ball(300,
                racchetta.Y - 10,
                (int)(Math.Abs((float)1 / 50 * Math.Min(this.ParentForm.ClientRectangle.Width, this.ParentForm.ClientRectangle.Height))),
                (int)(Math.Abs((float)1 / 50 * Math.Min(this.ParentForm.ClientRectangle.Width, this.ParentForm.ClientRectangle.Height))),
                Logic);

            // Inizializza le vite
            life_init();

            // inizializzo il titolo del gioco
            gameTitleset();

            //inizializzo il label dello score
            scoreSet();

            // Inizializza il thread del gioco
            gameThread = new Thread(Logic.gameLoop);
            gameThread.Start();

            // Aspetta il Garbage Collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion Methods
    }
}