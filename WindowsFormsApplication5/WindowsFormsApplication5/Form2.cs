using System;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form2 : Form
    {
        #region Public Fields

        public int altezza_client;
        public int altezza_client_iniziale;
        public int lunghezza_client;
        public int lunghezza_client_iniziale;
        private object sender;
        private EventArgs e;

        #endregion Public Fields
        #region Private Fields

        private bool button_start = false;
        private Form1 Game = new Form1();
        private Panel GamePanels = new Panel();
        private Form3 Start = new Form3();
        private int primavolta = 0;
        private Thread game;
        #endregion Private Fields

        #region Public Constructors
        public Form2()
        {
            InitializeComponent();
            return;
        }

        #endregion Public Constructors
        #region Public Methods

        public void gameLoop()
        {
            if (button_start)
            {
                Start.Hide();
                GamePanels.Controls.Add(Game);
                game = new Thread(Game.gameLoop);
                Game.Show();
                Game.Focus();
                Game.BringToFront();
                Thread game_alive = new Thread(gameover_check);
                game_alive.Start();
                game.Start();
                // Start.Visible = false;
                /*      Game.Enabled = true;
                      Game.Visible = true;*/
            }
            else {
                if(primavolta == 1)
                {
                                  //qui dovremmo fare ciò che facciamo in form load ma se lo facciamo dice che non abbiamo i permessi ad esempio per fare
                                    //this.controls.clear();... non so cosa farci lol
                                    Game.Invalidate();
                                    //Game.Close();
                                    Game = new Form1();
                                    Start = new Form3();

                                    GamePanels.Controls.Remove(Game);
                                    Game.Hide();
                                    /*Start.Controls.Clear();
                                    Start.Focus();
                                    Start.Show();
                                    Start.Focus();
                                    Start.BringToFront();*/
                    //game.Abort();
                    callonload();
                    primavolta = 0;
                }
                Start.Show();

                Start.BringToFront();
            }
            primavolta = 1;

        }

        public void gameover_check()
        {
            while (this.Created)
            if (Game.shouldStop==true && button_start == true)
            {
                button_start = false;
                gameLoop();
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
            callonload();
        }

        private void callonload()
        {

            lunghezza_client = this.ClientRectangle.Width;
            altezza_client = this.ClientRectangle.Height;
            if(!GamePanels.Created)
            this.Controls.Add(GamePanels);
            Game.TopLevel = false;
            Start.TopLevel = false;
            GamePanels.Top = 0;
            GamePanels.Left = 0;

            GamePanels.Width = 1000;
            GamePanels.Height = 500;
            //GamePanels.Visible = true;
            /*           StartPanel.Top = 0;
                       StartPanel.Left = 0;

                       StartPanel.Width = 1000;
                       StartPanel.Height = 500;
                       StartPanel.Visible = true;*/
            if(!Start.Created)
            GamePanels.Controls.Add(Start);
            //GamePanels.Controls.Add(Game);
            this.Dock = DockStyle.Fill;
            Game.AutoScaleMode = AutoScaleMode.Inherit;
            if (!GamePanels.Created)
                GamePanels.Dock = DockStyle.Fill;
            GamePanels.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            if (!Game.Created)
                Game.Width = GamePanels.Width;
            if (!Game.Created)
                Game.Height = GamePanels.Height;
            Game.Left = 0;
            Game.Top = 0;
            if (!Game.Created)
                Game.FormBorderStyle = FormBorderStyle.None;
            Game.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            Game.AutoScaleMode = AutoScaleMode.Inherit;
            if (!Start.Created)
                Start.Width = GamePanels.Width;
            if (!Start.Created)
                Start.Height = GamePanels.Height;
            Start.Left = 0;
            Start.Top = 0;
            if (!Start.Created)
                Start.FormBorderStyle = FormBorderStyle.None;
            Start.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            Start.AutoScaleMode = AutoScaleMode.Inherit;
            gameLoop();

            Start.start.Click += new EventHandler(this.start_Click);
            primavolta = 0;
            return;
        }

        private void Form2_ResizeBegin(object sender, EventArgs e)
        {
            lunghezza_client_iniziale = this.ClientRectangle.Width;
            altezza_client_iniziale = this.ClientRectangle.Height;
        }

        private void Form2_ResizeEnd(object sender, EventArgs e)
        {
            if (Game.Enabled == true)
            {
       
                GamePanels.Height = this.Height;
                GamePanels.Width = this.Width;
                Game.Height = this.Height;
                Game.Width = this.Width;
                GamePanels.Top = 0;
                GamePanels.Left = 0;
                Game.Top = 0;
                Game.Left = 0;
                Game.on_resize(lunghezza_client_iniziale, altezza_client_iniziale, lunghezza_client, altezza_client);
            }
        }

        #endregion Private Methods
    }
}