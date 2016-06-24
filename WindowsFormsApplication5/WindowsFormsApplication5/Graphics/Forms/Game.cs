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

        public Playground MyPlayground;
        public Ball MyBall;
        public Thread MyGameThread;
        public Grid MyBlockGrid;
        public Logic MyGameLogic; 
        public Racket MyRacket;
        public Label MyScore;
        public Life[] MyLife;

        #endregion Public Fields

        #region Private Fields

        private bool _myBallpointer;
        private GamePause _myGamePause;
        private Label _myGameTitle;
        private Skin _mySkin;

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
        /// Funzione che permette l'inizializzazione delle vite e del loro disegno iniziale
        /// </summary>
        private void life_init()
        {

            for (var i = 0; i < MyGameLogic.VitaRimanente; i++)
            {
                var lifeX = ClientRectangle.Width - (float)1 / 50 * ClientRectangle.Width -
                Math.Abs((float)1 / 25 * Math.Min(ClientRectangle.Width, ClientRectangle.Height)) * (i + 1) - 10 * (i + 1);
                var lifeY = (float)1 / 50 * ClientRectangle.Height +
                    Math.Abs((float)1 / 25 * Math.Min(ClientRectangle.Width, ClientRectangle.Height));
                var lifeWidth = (int)Math.Abs((float)1 / 25 * Math.Min(ClientRectangle.Width, ClientRectangle.Height));
                var lifeHeigth = lifeWidth;
                MyLife[i] = 
                    new Life(lifeX, lifeY,lifeWidth, lifeHeigth);
                MyGameLogic.MyIManager.InGameSprites.Add(MyLife[i]);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Funzione che rende il form invisibile perchè non si hanno piu vite a disposizione
        /// </summary>
        public void LifeEnd()
        {
            Invoke(new MethodInvoker(delegate { this.Visible = false; }));
        }

        /// <summary>
        /// Funzione utilizzata durante i resize
        /// </summary>
        /// <param name="li"></param>
        /// <param name="hi"></param>
        /// <param name="l"></param>
        /// <param name="h"></param>
        public void on_resize(int li, int hi, int l, int h)
        {
            // Richiama logic.resize
            if (hi > 0 && h > 0 && li > 0 && l > 0)
            {
                _myGamePause.Width = Width;
                _myGamePause.Height = Height;
                MyGameLogic.Resize(li, hi, l, h);
                MyRacket.Y = (float)MyPlayground.Height * 9 / 10 + MyPlayground.Y;
                MyScore.Top = ClientRectangle.Height - 40;
                MyScore.Left = ClientRectangle.Width / 2 - MyScore.Width / 2;
                _myGameTitle.Left = ClientRectangle.Width / 2 - _myGameTitle.Width / 2;
                _myGamePause.ResetText();
                _myGamePause.SetText();
            }
            else
            {
                Pause();
            }
        }

        /// <summary>
        /// Funzione utilizzata per mettere in pausa il gioco
        /// </summary>
        public void Pause()
        {
            if (MyBall.FollowPointer)
                _myBallpointer = true;
            MyBall.FollowPointer = false;
            MyBall.CanFall = false;
            MyBall.PreviousX = MyBall.X;
            MyBall.PreviousY = MyBall.Y;
            MyBall.PreviousVelocityTot = MyBall.VelocityTot;
            if (MyBall.Velocity.X != 0)
            {
                MyBall.PreviousVelocity.X = MyBall.Velocity.X;
            }
            if (MyBall.Velocity.Y != 0)
            {
                MyBall.PreviousVelocity.Y = MyBall.Velocity.Y;
            }
            MyRacket.FollowPointer = false;
            _myGamePause.Visible = true;
        }

        /// <summary>
        /// Funzione utilizzata per lanciare la pallina
        /// </summary>
        private void ThrowBall()
        {
            if (_myGamePause.Visible == false)
            {
                _myBallpointer = false;
                MyBall.FollowPointer = false;
                MyBall.CanFall = true;
                MyBall.VelocityTotLimit = 3000;
                MyRacket.FollowPointer = true;
            }
            if (_myGamePause.Visible)
            {
                _myGamePause.Visible = false;
                if (_myBallpointer)
                {
                    MyBall.FollowPointer = true;
                    MyRacket.FollowPointer = true;
                }
            }
        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        /// Funzione che permette il caricamento iniziale del gioco
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
        /// Funzione che gestisce l'evento della pressione dei tasti durante l'esecuzione del gioco
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (sender == null) throw new ArgumentNullException(nameof(sender));
            if (!MyGameLogic.AllowInput) return;
            if (e.KeyChar == (char)Keys.Space)
            {
                ThrowBall();
                MyGameLogic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
            }
            if (e.KeyChar == (char)Keys.Enter && _myGamePause.Visible == false)
            {
                Pause();
                MyGameLogic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
            }
            if (e.KeyChar == (char)Keys.Escape)
            {
                Pause();
                var dialogResult = MessageBox.Show("Do you want to proced to GameOver?", "ALERT",
                    MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)
                {
                    MyGameLogic.VitaRimanente = 0;
                }
                else if (dialogResult == DialogResult.No)
                {
                    ThrowBall();
                }
            }
        }

        /// <summary>
        /// Funzione che permette il reset del titolo del gioco per poterlo scalare
        /// </summary>
        private void GameTitleset()
        {
            _myGameTitle = new Label();
            _myGameTitle.Top = 20;
            _myGameTitle.Width = ClientRectangle.Width / 3 * 2;
            _myGameTitle.TextAlign = ContentAlignment.MiddleCenter;
            _myGameTitle.Left = ClientRectangle.Width / 2 - _myGameTitle.Width / 2;
            _myGameTitle.Text = "BlockBreaker";
            _myGameTitle.BackColor = Color.Black;
            _myGameTitle.ForeColor = Color.White;
            _myGameTitle.Font = new Font("Arial", 15);
            Controls.Add(_myGameTitle);
        }

        /// <summary>
        /// Funzione che permette di inizializzare la griglia
        /// </summary>
        private void init_grid()
        {
            MyBlockGrid = new Grid((int)MyPlayground.X, (int)MyPlayground.Y, MyPlayground.Height, MyPlayground.Width,
                Resources.Block_4, MyGameLogic);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }


        /// <summary>
        /// Funzione che permette
        /// </summary>
        private void ScoreSet()
        {   
            MyScore = new Label();
            MyScore.Left = ClientRectangle.Width / 2 - MyScore.Width / 2;
            MyScore.Top = ClientRectangle.Height - 40;
            MyScore.Width = ClientRectangle.Width / 8;
            MyScore.TextAlign = ContentAlignment.MiddleCenter;
            MyScore.Text = "MyScore: 0";
            MyScore.BackColor = Color.Black;
            MyScore.ForeColor = Color.Transparent;
            MyScore.Font = new Font("Arial", 15);
            Controls.Add(MyScore);
        }

        /// <summary>
        /// Funzione Starter invocata all'avvio di questo form per inizializzarlo
        /// </summary>
        private void Starter()
        {
            MyLife = new Life[3];
            // Inizializza la logica
            MyGameLogic = new Logic(this);

            //Inizializzo la skin
            _mySkin = new Skin(ClientRectangle.X,
                ClientRectangle.Y,
                ClientRectangle.Width,
                ClientRectangle.Height,
            MyGameLogic);
            _mySkin.X = 0;
            _mySkin.Y = 0;
            _myBallpointer = true;

            // Inizializza la variabile della visione del menù pausa a falso in caso sia vera
            _myGamePause = new GamePause(0, 0, Width, Height);
            _myGamePause.Visible = false;
            Controls.Add(_myGamePause);

            // Inizializza il background
            MyPlayground = new Playground(ClientRectangle.X,
                ClientRectangle.Y,
                ClientRectangle.Width / 30 * 29,
                ClientRectangle.Height / 5 * 4,
                MyGameLogic);
            MyPlayground.X = ClientRectangle.Width / 2 - MyPlayground.Width / 2;
            MyPlayground.Y = ClientRectangle.Height / 2 - MyPlayground.Height / 2;

            // Inizializza griglia
            init_grid();

            // Inizializza racchetta
            if (Visible)
                MyRacket = new Racket(Cursor.Position.X - this.Location.X,
                    (float)MyPlayground.Height * 9 / 10 + MyPlayground.Y,
                    (int)Math.Abs((float)1 / 8 * ParentForm.ClientRectangle.Width),
                    (int)Math.Abs((float)1 / 15 * ParentForm.ClientRectangle.Height),
                    MyGameLogic);

            // Inizializza pallina
            MyBall = new Ball(300,
                MyRacket.Y - 10,
                (int)
                    Math.Abs((float)1 / 50 * Math.Min(ParentForm.ClientRectangle.Width, ParentForm.ClientRectangle.Height)),
                (int)
                    Math.Abs((float)1 / 50 * Math.Min(ParentForm.ClientRectangle.Width, ParentForm.ClientRectangle.Height)),
                MyGameLogic);

            // Inizializza le vite
            life_init();

            // inizializzo il titolo del gioco
            GameTitleset();

            //inizializzo il label dello score
            ScoreSet();

            // Inizializza il thread del gioco
            MyGameThread = new Thread(MyGameLogic.GameLoop);
            MyGameThread.IsBackground = true;
            MyGameThread.Start();


            // Aspetta il Garbage Collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion Private Methods
    }
}
