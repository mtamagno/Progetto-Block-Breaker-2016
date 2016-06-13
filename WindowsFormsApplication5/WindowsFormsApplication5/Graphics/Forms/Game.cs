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

        public void life_init()
        {
            for (int i = 0; i < Logic.vita_rimanente; i++)
            {
                vita[i] = new Life(this.ClientRectangle.Width - (float)1 / 50 * this.ClientRectangle.Width
                        - (float)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))) * (i + 1) - 10 * (i + 1),
                    this.ClientRectangle.Height - (float)1 / 50 * this.ClientRectangle.Height
                        - (float)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))) - 5,
                    (int)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))),
                    (int)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))));
                Logic.iManager.inGameSprites.Add(vita[i]);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void on_resize(int li, int hi, int l, int h)
        {
            // Richiama logic.resize
            Logic.resize(li, hi, l, h);
            racchetta.Y = this.background.Height * 9 / 10 + this.background.Y;
            score.Top = this.ClientRectangle.Height - 40;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // Chiudo Game
            base.OnClosing(e);
            System.Environment.Exit(0);
            this.Close();
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);
            loadContent();
        }

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

        private void gameTitleset()
        {
            gameTitle = new Label();
            gameTitle.Left = this.ClientRectangle.Width / 2 - this.gameTitle.Width;
            gameTitle.Top = 20;
            gameTitle.Width = this.ClientRectangle.Width;
            gameTitle.TextAlign = ContentAlignment.MiddleCenter;
            gameTitle.Text = "BlockBreaker";
            gameTitle.BackColor = Color.Black;
            gameTitle.ForeColor = Color.White;
            gameTitle.Font = new Font("Arial", 15);
            this.Controls.Add(gameTitle);
        }

        private void init_grid()
        {
            grid = new Grid((int)this.background.X, (int)this.background.Y, this.background.Height, this.background.Width, Properties.Resources.Block_4, Logic);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void loadContent()
        {
            starter();
        }

        private void scoreSet()
        {
            score = new Label();
            score.Left = this.ClientRectangle.Width/2 - this.score.Width/2;
            score.Top = this.ClientRectangle.Height - 40;
            score.Width = this.ClientRectangle.Width / 8;

            score.TextAlign = ContentAlignment.MiddleCenter;
            score.Text = "Score: 0";
            score.BackColor = Color.Black;
            score.ForeColor = Color.Transparent;
            score.Font = new Font("Arial", 15);
            this.Controls.Add(score);
        }

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

        private void Game_Load(object sender, EventArgs e)
        {

        }
    }
}