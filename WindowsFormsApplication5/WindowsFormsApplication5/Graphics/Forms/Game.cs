using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BlockBreaker
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
        public bool ballpointer;
        public float ballX;
        public float ballY;

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

        public void lifeEnd()
        {
                this.Invoke(new MethodInvoker(delegate
                {
                    this.Visible = false;
                }));
            
            
        }

        public void on_resize(int li, int hi, int l, int h)
        {
            // Richiama logic.resize
            if (hi > 0 && h > 0 && li > 0 && l > 0)
            {
                gamePause.Width = this.Width;
                gamePause.Height = this.Height;
                Logic.resize(li, hi, l, h);
                racchetta.Y = this.background.Height * 9 / 10 + this.background.Y;
                score.Top = this.ClientRectangle.Height - 40;
                score.Left = this.ClientRectangle.Width / 2 - this.score.Width / 2;
                gameTitle.Left = this.ClientRectangle.Width / 2 - this.gameTitle.Width / 2;
                gamePause.ResetText();
                gamePause.setText();
                Pause();
            }
            else
            {
                Pause();
            }

        }

        /// <summary>
        /// Funzione chiamata 
        /// </summary>
        /// <param name="e"></param>
        protected override void OnClosing(CancelEventArgs e)
        {

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
                    ThrowBall();
                    Logic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
                }
                if (e.KeyChar == (char)Keys.Enter && gamePause.Visible == false)
                {
                    Pause();
                    Logic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
                }
                if (e.KeyChar == (char)Keys.Escape)
                {
                    Pause();
                    DialogResult dialogResult = MessageBox.Show("Do you want to proced to GameOver?", "ALERT", MessageBoxButtons.YesNo);
                    if (dialogResult == DialogResult.Yes)
                    {
                        Logic.vita_rimanente = 0;
                    }
                    else if (dialogResult == DialogResult.No)
                    {
                        ThrowBall();
                    }

                }
                }
            }

        public void ThrowBall()
        {
            if (gamePause.Visible == false)
            {
                ballpointer = false;
                ball.followPointer = false;
                ball.canFall = true;

                racchetta.followPointer = true;
            }
            if (gamePause.Visible == true)
            {
                gamePause.Visible = false;
                if (ballpointer == true)
                {
                    ball.followPointer = true;
                    racchetta.followPointer = true;
                }
            }
        }

        public void Pause()
        {
            if (ball.followPointer == true)
                ballpointer = true;
            ball.followPointer = false;
            ball.canFall = false;
            ball.previousX = ball.X;
            ball.previousY = ball.Y;
            ball.previousVelocity.X = ball.velocity.X;
            ball.previousVelocity.Y = ball.velocity.Y;
            ball.previousVelocityTot = ball.velocityTot;
            racchetta.followPointer = false;
            gamePause.Visible = true;
            
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
            ballpointer = true;

            // Inizializza la variabile della visione del menù pausa a falso in caso sia vera
            gamePause = new GamePause(0, 0, this.Width, this.Height);
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