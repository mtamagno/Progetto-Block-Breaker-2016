using System;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication5
{
    public partial class Form2 : Form
    {
        #region Public Fields

        public int altezza_client;
        public int altezza_client_iniziale;
        public int lunghezza_client;
        //  Panel StartPanel = new Panel();
        public int lunghezza_client_iniziale;

        #endregion Public Fields
        #region Private Fields

        private bool button_start = false;
        private Form1 Game = new Form1();
        private Panel GamePanels = new Panel();
        private Form3 Start = new Form3();
        private int sono_entrato = 1;
        private Thread isAlive;
        #endregion Private Fields

        #region Public Constructors

        public Form2()
        {
            InitializeComponent();
            return;
        }

        #endregion Public Constructors
        #region Public Methods

        private void gameLoop()
        {
            if (button_start)
            {
                Start.Enabled = false;
                Start.Close();
                Game.Enabled = true;
                GamePanels.Controls.Remove(Start);
                GamePanels.Controls.Add(Game);
                Game.Activate();

                Game.Show();
                Game.Focus();
                Game.BringToFront();
                
                


                // Start.Visible = false;
                /*      Game.Enabled = true;
                      Game.Visible = true;*/
            }
            else {
                Start.Show();
                //Game.Show();
            }
        }

        private void isalive()
        {
            while (this.Created)
            {
                if (Game.vita_rimanente < 1)
                {
                    ciao();
                    
                    Start.Show();
                }
            }

        }

        public void ciao()
        {
            if (Game.Created && sono_entrato == 1)
            {
                Game.iwanttoclosethis();
                sono_entrato = 0;
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
            lunghezza_client = this.ClientRectangle.Width;
            altezza_client = this.ClientRectangle.Height;
            this.Controls.Add(GamePanels);
            Game.TopLevel = false;
            Game.AutoScroll = true;
            Start.TopLevel = false;
            Start.AutoScroll = true;
            isAlive = new Thread(isalive);
            isAlive.Start();
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

            GamePanels.Controls.Add(Start);
            //GamePanels.Controls.Add(Game);
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
            gameLoop();
            Start.start.Click += new EventHandler(this.start_Click);
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