using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Game : Form
    {
        #region Fields

        public View background;
        public Ball ball;
        public Grid grid;
        public Logic logic;
        public Paddle racchetta;
        public Life[] vita = new Life[3];
        public Thread gameThread;
        private GamePause gamePause;
        #endregion Fields

        #region Constructors

        public Game()
        {
            InitializeComponent();
            return;
        }

        #endregion Constructors

        #region Methods

        public void on_resize(int li, int hi, int l, int h)
        {
            // Richiama logic.resize
            logic.resize(li, hi, l, h);
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
            if (logic.AllowInput)
            {
                if (e.KeyChar == (char)Keys.Space)
                {
                    ball.followPointer = false;
                    ball.canFall = true;
                    racchetta.followPointer = true;
                    gamePause.Visible = false;
                    logic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
                }
                if (e.KeyChar == (char)Keys.Enter && gamePause.Visible == false)
                {
                    ball.followPointer = false;
                    ball.canFall = false;
                    racchetta.followPointer = false;
                    gamePause.Visible = true;
                    logic.KeysPressed.Add((Keys)e.KeyChar.ToString().ToUpper().ToCharArray()[0]);
                }
                if (e.KeyChar == (char)Keys.Escape)
                {
                    this.logic.vita_rimanente = 0;
                    this.Close();
                }
            }
        }

        private void init_grid()
        {
            grid = new Grid(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Height, this.ClientRectangle.Width, Properties.Resources.Block_4, logic);
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        public void life_init()
        {
            for (int i = 0; i < logic.vita_rimanente; i++)
            {
                vita[i] = new Life(this.ClientRectangle.Width - (float)1 / 50 * this.ClientRectangle.Width 
                        - (float)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width,this.ClientRectangle.Height))) * (i + 1), 
                    this.ClientRectangle.Height - (float)1/50 * this.ClientRectangle.Height 
                        - (float)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))),
                    (int)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))),
                    (int)(Math.Abs((float)1 / 25 * Math.Min(this.ClientRectangle.Width, this.ClientRectangle.Height))));
                logic.iManager.inGameSprites.Add(vita[i]);
            }
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        private void loadContent()
        {
            starter();
        }

        private void starter()
        {
            // Inizializza la logica
            logic = new Logic(this);

            // Inizializza la variabile della visione del menù pausa a falso in caso sia vera
            gamePause = new GamePause(0, 0, 1000, 500);
            gamePause.Visible = false;
            this.Controls.Add(gamePause);

            // Inizializza il background
            background = new View(this.ClientRectangle.X, this.ClientRectangle.Y, this.ClientRectangle.Width, this.ClientRectangle.Height, logic);

            // Inizializza griglia
            init_grid();

            // Inizializza racchetta
            if (this.Visible)
                racchetta = new Paddle(logic.MousePoint.X - this.Location.X,
                    this.ParentForm.ClientRectangle.Height * 9 / 10,
                    (int)(Math.Abs((float)1 / 8 * this.ParentForm.ClientRectangle.Width)),
                    (int)(Math.Abs((float)1 / 22 * this.ParentForm.ClientRectangle.Height)),
                    logic);

            // Inizializza pallina
            ball = new Ball(300,
                racchetta.Y - 10,
                (int)(Math.Abs((float)1 / 50 * Math.Min(this.ParentForm.ClientRectangle.Width, this.ParentForm.ClientRectangle.Height))),
                (int)(Math.Abs((float)1 / 50 * Math.Min(this.ParentForm.ClientRectangle.Width, this.ParentForm.ClientRectangle.Height))),
                logic);

            // Inizializza le vite
            life_init();

            // Inizializza il thread del gioco
            gameThread = new Thread(logic.gameLoop);
            gameThread.Start();

            // Aspetta il Garbage Collector
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion Methods
    }
}