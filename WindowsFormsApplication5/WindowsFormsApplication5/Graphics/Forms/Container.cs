using System;
using System.Threading;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication5
{
    public partial class Container : Form
    {
        #region Public Fields

        public int altezza_client;
        public int altezza_client_iniziale;
        public int lunghezza_client;
        public int lunghezza_client_iniziale;

        #endregion Public Fields

        #region Private Fields

        private bool button_start = false;
        private Form1 Game = new Form1();
        private Panel GamePanels = new Panel();
        private int primavolta = 0;
        private bool restart_required = false;
        private Form3 Start = new Form3();

        #endregion Private Fields

        #region Public Constructors

        public Container()
        {
            InitializeComponent();
            return;
        }

        #endregion Public Constructors

        #region Public Methods

        public void gameLoop()
        {
            if (button_start == true)
            {
                this.Invoke(new MethodInvoker(delegate
                {
                Start.Hide();
                Start.Dispose();
                GamePanels.Controls.Add(Game);
                Game.Show();
                Game.Focus();
                Game.BringToFront();
                Thread game_alive = new Thread(gameover_check);
                game_alive.Start();
                }));
            }
            else {
                if (primavolta == 1 && restart_required == true)
                {
                    restart_required = false;
                    starter();
                }
                this.Invoke(new MethodInvoker(delegate
                {
                    Start.Show();
                    Start.Focus();
                    Start.BringToFront();
                }));
            }
            primavolta = 1;
        }

        public void gameover_check()
        {
            while (this.Created)
            {
                if (Game.logic.shouldStop == true && button_start == true)
                {
                    Game.logic.shouldStop = false;
                    button_start = false;
                    restart_required = true;
                    gameLoop();
                    return;

                    //termino il thread
                }
                Thread.Sleep(2000);
            }
        }

        #endregion Public Methods

        #region Protected Methods

        protected void start_Click(object sender, EventArgs e)
        {
            button_start = true;
            gameLoop();
        }

        #endregion Protected Methods

        #region Private Methods

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Game.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            starter();
        }

        private void Form2_ResizeBegin(object sender, EventArgs e)
        {
            lunghezza_client_iniziale = this.ClientRectangle.Width;
            altezza_client_iniziale = this.ClientRectangle.Height;
        }

        private void Form2_ResizeEnd(object sender, EventArgs e)
        {
            GamePanels.Height = this.ClientRectangle.Height;
            GamePanels.Width = this.ClientRectangle.Width;
            GamePanels.Top = 0;
            GamePanels.Left = 0;
            Game.Width = GamePanels.Width;
            Game.Height = GamePanels.Height;
            lunghezza_client = this.ClientRectangle.Width;
            altezza_client = this.ClientRectangle.Height;
            Game.Top = 0;
            Game.Left = 0;
            Start.Width = GamePanels.Width;
            Start.Height = GamePanels.Height;
            Start.Top = 0;
            Start.Left = 0;

            if (Game.Visible == true)
            {
                Game.on_resize(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);
            }
            if (Start.Visible == true)
            {
                Start.on_resize(this.Width, this.Height);
            }
        }

        private void starter()
        {
            lunghezza_client = this.ClientRectangle.Width;
            altezza_client = this.ClientRectangle.Height;
            this.Invoke(new MethodInvoker(delegate
            {
                if (primavolta == 1)
                {
                    this.Game.Dispose();
                    this.Start.Dispose();
                    this.GamePanels.Controls.Clear();
                    this.Controls.Clear();
                    this.Game = new Form1();
                    this.Start = new Form3();

                }
                this.Controls.Add(GamePanels);
                    Game.TopLevel = false;
                    Game.AutoScroll = true;
                    Start.TopLevel = false;
                    Start.AutoScroll = true;

                    GamePanels.Top = 0;
                    GamePanels.Left = 0;
                    GamePanels.Width = this.Width;
                    GamePanels.Height = this.Height;
                    GamePanels.Visible = true;
                    GamePanels.Controls.Remove(Start);
                    GamePanels.Controls.Add(Start);
                    this.Dock = DockStyle.Fill;
                    Game.AutoScaleMode = AutoScaleMode.Inherit;
                    GamePanels.Dock = DockStyle.Fill;
                    GamePanels.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;

                    Game.Width = GamePanels.Width;
                    Game.Height = GamePanels.Height;
                    Game.Left = 0;
                    Game.Top = 0;
                    Game.FormBorderStyle = FormBorderStyle.None;
                    Game.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
                    Game.AutoScaleMode = AutoScaleMode.Inherit;

                    Start.Width = GamePanels.Width;
                    Start.Height = GamePanels.Height;
                    Start.Left = 0;
                    Start.Top = 0;
                    Start.FormBorderStyle = FormBorderStyle.None;
                    Start.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
                    Start.AutoScaleMode = AutoScaleMode.Inherit;
                    Start.start.Click += new EventHandler(this.start_Click);
                    if (primavolta == 0)
                        gameLoop();
                
                primavolta = 0;
                GC.Collect();
                
                GC.WaitForPendingFinalizers();

            }));
            return;
        }

        #endregion Private Methods
    }
}