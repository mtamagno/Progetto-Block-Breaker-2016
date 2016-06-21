using BlockBreaker.Properties;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BlockBreaker
{
    public partial class Game : Form
    {
        #region Public Fields

        public Playground Background;
        public Ball Ball;
        public Thread GameThread;
        public Grid Grid;
        public Logic Logic;
        public Racket Racchetta;
        public Label Score;
        public Life[] Vita;

        #endregion Public Fields

        #region Private Fields

        private bool _ballpointer;
        private GamePause _gamePause;
        private Label _gameTitle;
        private Skin _skin;

        #endregion Private Fields

        #region Public Constructors

        public Game()
        {
            // Inizializza i componenti
            InitializeComponent();
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Funzione che permette l'inizializzazione delle vite e del loro disegno iniziale
        /// </summary>
        private void life_init()
        {
            for (var i = 0; i < Logic.VitaRimanente; i++)
            {
                Vita[i] = new Life(ClientRectangle.Width - (float)1 / 50 * ClientRectangle.Width
                                   -
                                   Math.Abs((float)1 / 25 * Math.Min(ClientRectangle.Width, ClientRectangle.Height)) *
                                   (i + 1) - 10 * (i + 1),
                    (float)1 / 50 * ClientRectangle.Height +
                    Math.Abs((float)1 / 25 * Math.Min(ClientRectangle.Width, ClientRectangle.Height)),
                    (int)Math.Abs((float)1 / 25 * Math.Min(ClientRectangle.Width, ClientRectangle.Height)),
                    (int)Math.Abs((float)1 / 25 * Math.Min(ClientRectangle.Width, ClientRectangle.Height)));
                Logic.IManager.InGameSprites.Add(Vita[i]);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void LifeEnd()
        {
            Invoke(new MethodInvoker(delegate { Visible = false; }));
        }

        public void on_resize(int li, int hi, int l, int h)
        {
            // Richiama logic.resize
            if (hi > 0 && h > 0 && li > 0 && l > 0)
            {
                _gamePause.Width = Width;
                _gamePause.Height = Height;
                Logic.Resize(li, hi, l, h);
                Racchetta.Y = (float)Background.Height * 9 / 10 + Background.Y;
                Score.Top = ClientRectangle.Height - 40;
                Score.Left = ClientRectangle.Width / 2 - Score.Width / 2;
                _gameTitle.Left = ClientRectangle.Width / 2 - _gameTitle.Width / 2;
                _gamePause.ResetText();
                _gamePause.SetText();

                //               Pause();
            }
            else
            {
                Pause();
            }
        }

        public void Pause()
        {
            if (Ball.FollowPointer)
                _ballpointer = true;
            Ball.FollowPointer = false;
            Ball.CanFall = false;
            Ball.PreviousX = Ball.X;
            Ball.PreviousY = Ball.Y;
            Ball.PreviousVelocityTot = Ball.VelocityTot;
            if (Ball.Velocity.X != 0)
            {
                Ball.PreviousVelocity.X = Ball.Velocity.X;
            }
            if (Ball.Velocity.Y != 0)
            {
                Ball.PreviousVelocity.Y = Ball.Velocity.Y;
            }
            Racchetta.FollowPointer = false;
            _gamePause.Visible = true;
        }

        private void ThrowBall()
        {
            if (_gamePause.Visible == false)
            {
                _ballpointer = false;
                Ball.FollowPointer = false;
                Ball.CanFall = true;
                Ball.VelocityTotLimit = 3000;
                Racchetta.FollowPointer = true;
            }
            if (_gamePause.Visible)
            {
                _gamePause.Visible = false;
                if (_ballpointer)
                {
                    Ball.FollowPointer = true;
                    Racchetta.FollowPointer = true;
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        ///     Funzione che permette il caricamento iniziale del gioco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnLoad(object sender, EventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            try
            {
                Starter();
            }

            // Gestiamo un raro caso in cui crashava il gioco che viene gestito da OnLoad
            catch
            {
                base.OnLoad(e);
                Starter();
            }
        }

        #endregion Protected Methods

        #region Private Methods

        /// <summary>
        ///     Funzione che gestisce l'evento della pressione dei tasti durante l'esecuzione del gioco
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
                var dialogResult = MessageBox.Show("Do you want to proced to GameOver?", "ALERT",
                    MessageBoxButtons.YesNo);
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

        /// <summary>
        ///     Funzione che permette il reset del titolo del gioco per poterlo scalare
        /// </summary>
        private void GameTitleset()
        {
            _gameTitle = new Label();
            _gameTitle.Top = 20;
            _gameTitle.Width = ClientRectangle.Width / 3 * 2;
            _gameTitle.TextAlign = ContentAlignment.MiddleCenter;
            _gameTitle.Left = ClientRectangle.Width / 2 - _gameTitle.Width / 2;
            _gameTitle.Text = "BlockBreaker";
            _gameTitle.BackColor = Color.Black;
            _gameTitle.ForeColor = Color.White;
            _gameTitle.Font = new Font("Arial", 15);
            Controls.Add(_gameTitle);
        }

        /// <summary>
        ///     Funzione che permette di inizializzare la griglia
        /// </summary>
        private void init_grid()
        {
            Grid = new Grid((int)Background.X, (int)Background.Y, Background.Height, Background.Width,
                Resources.Block_4, Logic);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        /// <summary>
        ///     Funzione che permette
        /// </summary>
        private void ScoreSet()
        {
            Score = new Label();
            Score.Left = ClientRectangle.Width / 2 - Score.Width / 2;
            Score.Top = ClientRectangle.Height - 40;
            Score.Width = ClientRectangle.Width / 8;
            Score.TextAlign = ContentAlignment.MiddleCenter;
            Score.Text = "Score: 0";
            Score.BackColor = Color.Black;
            Score.ForeColor = Color.Transparent;
            Score.Font = new Font("Arial", 15);
            Controls.Add(Score);
        }

        /// <summary>
        ///     Funzione Starter invocata all'avvio di questo form per inizializzarlo
        /// </summary>
        private void Starter()
        {
            Vita = new Life[3];
            // Inizializza la logica
            Logic = new Logic(this);

            //Inizializzo la skin
            _skin = new Skin(ClientRectangle.X,
                ClientRectangle.Y,
                ClientRectangle.Width,
                ClientRectangle.Height,
            Logic);
            _skin.X = 0;
            _skin.Y = 0;
            _ballpointer = true;

            // Inizializza la variabile della visione del menù pausa a falso in caso sia vera
            _gamePause = new GamePause(0, 0, Width, Height);
            _gamePause.Visible = false;
            Controls.Add(_gamePause);

            // Inizializza il background
            Background = new Playground(ClientRectangle.X,
                ClientRectangle.Y,
                ClientRectangle.Width / 30 * 29,
                ClientRectangle.Height / 5 * 4,
                Logic);
            Background.X = ClientRectangle.Width / 2 - Background.Width / 2;
            Background.Y = ClientRectangle.Height / 2 - Background.Height / 2;

            // Inizializza griglia
            init_grid();

            // Inizializza racchetta
            if (Visible)
                Racchetta = new Racket(Logic.MousePoint.X - Location.X,
                    (float)Background.Height * 9 / 10 + Background.Y,
                    (int)Math.Abs((float)1 / 8 * ParentForm.ClientRectangle.Width),
                    (int)Math.Abs((float)1 / 15 * ParentForm.ClientRectangle.Height),
                    Logic);

            // Inizializza pallina
            Ball = new Ball(300,
                Racchetta.Y - 10,
                (int)
                    Math.Abs((float)1 / 50 * Math.Min(ParentForm.ClientRectangle.Width, ParentForm.ClientRectangle.Height)),
                (int)
                    Math.Abs((float)1 / 50 * Math.Min(ParentForm.ClientRectangle.Width, ParentForm.ClientRectangle.Height)),
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

        #endregion Private Methods
    }
}
