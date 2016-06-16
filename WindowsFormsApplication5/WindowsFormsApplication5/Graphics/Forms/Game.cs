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

        private Skin _skin;
        public Playground Background;
        public Ball Ball;
        public Thread GameThread;
        public Grid Grid;
        public Logic Logic;
        public Paddle Racchetta;
        public Label Score;
        public Life[] Vita = new Life[3];
        private GamePause _gamePause;
        private Label _gameTitle;
        private bool _ballpointer;
        public float BallX;
        public float BallY;

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
            for (int i = 0; i < Logic.VitaRimanente; i++)
            {
                Vita[i] = new Life(this.ClientRectangle.Width - (float)1 / 50 * this.ClientRectangle.Width
                        - (float)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))) * (i + 1) - 10 * (i + 1),
                    (float)1 / 50 * this.ClientRectangle.Height + (float)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))),
                    (int)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))),
                    (int)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))));
                Logic.IManager.inGameSprites.Add(Vita[i]);
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
                _gamePause.Width = this.Width;
                _gamePause.Height = this.Height;
                Logic.Resize(li, hi, l, h);
                Racchetta.Y = (float)this.Background.Height * 9 / 10 + this.Background.Y;
                Score.Top = this.ClientRectangle.Height - 40;
                Score.Left = this.ClientRectangle.Width / 2 - this.Score.Width / 2;
                _gameTitle.Left = this.ClientRectangle.Width / 2 - this._gameTitle.Width / 2;
                _gamePause.ResetText();
                _gamePause.setText();
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
        /// <param name="sender"></param>
        /// <param name="e"></param>
        protected void OnLoad(object sender, EventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            try
            {
                LoadContent();
            }
            // Gestiamo un raro caso in cui crashava il gioco che viene gestito da OnLoad
            catch
            {
            base.OnLoad(e);
            LoadContent();
        }
        }

        /// <summary>
        /// Funzione che gestisce l'evento della pressione dei tasti durante l'esecuzione del gioco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (!Logic.AllowInput) return;
            if (e.KeyChar == (char)Keys.Space)
            {
                ThrowBall();
                Logic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
            }
            if (e.KeyChar == (char)Keys.Enter && _gamePause.Visible == false)
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
                    Logic.VitaRimanente = 0;
                }
                else if (dialogResult == DialogResult.No)
                {
                    ThrowBall();
                }

            }
        }

        public void ThrowBall()
        {
            if (_gamePause.Visible == false)
            {
                _ballpointer = false;
                Ball.FollowPointer = false;
                Ball.CanFall = true;
                Ball.VelocityTotLimit = 3000;
                Racchetta.FollowPointer = true;
            }
            if (_gamePause.Visible == true)
            {
                _gamePause.Visible = false;
                if (_ballpointer == true)
                {
                    Ball.FollowPointer = true;
                    Racchetta.FollowPointer = true;
                }
            }
        }

        public void Pause()
        {
            if (Ball.FollowPointer == true)
                _ballpointer = true;
            Ball.FollowPointer = false;
            Ball.CanFall = false;
            Ball.PreviousX = Ball.X;
            Ball.PreviousY = Ball.Y;
            Ball.PreviousVelocity.X = Ball.Velocity.X;
            Ball.PreviousVelocity.Y = Ball.Velocity.Y;
            Ball.PreviousVelocityTot = Ball.VelocityTot;
            Racchetta.FollowPointer = false;
            _gamePause.Visible = true;
            
        }

        /// <summary>
        /// Funzione che permette il reset del titolo del gioco per poterlo scalare
        /// </summary>
        private void GameTitleset()
        {
            _gameTitle = new Label();
            _gameTitle.Top = 20;
            _gameTitle.Width = this.ClientRectangle.Width / 3 * 2;
            _gameTitle.TextAlign = ContentAlignment.MiddleCenter;
            _gameTitle.Left = this.ClientRectangle.Width / 2 - this._gameTitle.Width / 2;
            _gameTitle.Text = "BlockBreaker";
            _gameTitle.BackColor = Color.Black;
            _gameTitle.ForeColor = Color.White;
            _gameTitle.Font = new Font("Arial", 15);
            this.Controls.Add(_gameTitle);
        }

        /// <summary>
        /// Funzione che permette di inizializzare la griglia
        /// </summary>
        private void init_grid()
        {
            Grid = new Grid((int)this.Background.X, (int)this.Background.Y, this.Background.Height, this.Background.Width, Properties.Resources.Block_4, Logic);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Funzione che permette di caricare il contenuto all'avvio
        /// </summary>
        private void LoadContent()
        {
            Starter();
        }

        /// <summary>
        /// Funzione che permette
        /// </summary>
        private void ScoreSet()
        {
            Score = new Label();
            Score.Left = this.ClientRectangle.Width / 2 - this.Score.Width / 2;
            Score.Top = this.ClientRectangle.Height - 40;
            Score.Width = this.ClientRectangle.Width / 8;

            Score.TextAlign = ContentAlignment.MiddleCenter;
            Score.Text = "Score: 0";
            Score.BackColor = Color.Black;
            Score.ForeColor = Color.Transparent;
            Score.Font = new Font("Arial", 15);
            this.Controls.Add(Score);
        }

        /// <summary>
        /// Funzione Starter invocata all'avvio di questo form per inizializzarlo
        /// </summary>
        private void Starter()
        {
            // Inizializza la logica
            Logic = new Logic(this);

            //Inizializzo la skin

            _skin = new Skin(this.ClientRectangle.X,
            this.ClientRectangle.Y,
            this.ClientRectangle.Width,
            this.ClientRectangle.Height,
            Logic);
            _skin.X = 0;
            _skin.Y = 0;
            _ballpointer = true;

            // Inizializza la variabile della visione del menù pausa a falso in caso sia vera
            _gamePause = new GamePause(0, 0, this.Width, this.Height);
            _gamePause.Visible = false;
            this.Controls.Add(_gamePause);

            // Inizializza il background
            Background = new Playground(this.ClientRectangle.X,
                this.ClientRectangle.Y,
                this.ClientRectangle.Width / 30 * 29,
                this.ClientRectangle.Height / 5 * 4,
                Logic);
            Background.X = this.ClientRectangle.Width / 2 - this.Background.Width / 2;
            Background.Y = this.ClientRectangle.Height / 2 - this.Background.Height / 2;

            // Inizializza griglia
            init_grid();

            // Inizializza racchetta
            if (this.Visible)
                Racchetta = new Paddle(Logic.MousePoint.X - this.Location.X,
                    (float)this.Background.Height * 9 / 10 + this.Background.Y,
                    (int)(Math.Abs((float)1 / 8 * this.ParentForm.ClientRectangle.Width)),
                    (int)(Math.Abs((float)1 / 15 * this.ParentForm.ClientRectangle.Height)),
                    Logic);

            // Inizializza pallina
            Ball = new Ball(300,
                Racchetta.Y - 10,
                (int)(Math.Abs((float)1 / 50 * Math.Min(this.ParentForm.ClientRectangle.Width, this.ParentForm.ClientRectangle.Height))),
                (int)(Math.Abs((float)1 / 50 * Math.Min(this.ParentForm.ClientRectangle.Width, this.ParentForm.ClientRectangle.Height))),
                Logic);

            // Inizializza le vite
            life_init();

            // inizializzo il titolo del gioco
            GameTitleset();

            //inizializzo il label dello score
            ScoreSet();

            // Inizializza il thread del gioco
            GameThread = new Thread(Logic.GameLoop);
            GameThread.Start();

            // Aspetta il Garbage Collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion Methods
    }
}