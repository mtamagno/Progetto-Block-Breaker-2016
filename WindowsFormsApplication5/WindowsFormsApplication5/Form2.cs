using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication5
{
    public partial class Form2 : Form
    {

        Form3 Start = new Form3();
        Form1 Game = new Form1();
        Panel GamePanels = new Panel();
      //  Panel StartPanel = new Panel();
        public int lunghezza_client_iniziale;
        public int altezza_client_iniziale;
        public int lunghezza_client;
        public int altezza_client;
        bool button_start = false;

        public Form2()
        {
            InitializeComponent();
            return;
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
          //  GamePanels.Controls.Add(Game);
            this.Dock = DockStyle.Fill;
            Game.AutoScaleMode = AutoScaleMode.Inherit;
            GamePanels.Dock = DockStyle.Fill;
            GamePanels.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right; ;


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

        public void gameLoop()
        {
                if (button_start)
                {

                Start.Enabled = false;


                Game.Enabled = true;
                GamePanels.Controls.Remove(Start);
                GamePanels.Controls.Add(Game);
     
                Game.Activate();
                Start.Close();
                Game.Show();

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

        protected void start_Click(object sender, EventArgs e)
        {
            //this.Hide();
            Console.WriteLine("ciao");
            button_start = true;
            gameLoop();

        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Game.Close();
        }

        private void Form2_ResizeEnd(object sender, EventArgs e)
        {
            if (Game.Enabled == true)
            {
                Console.WriteLine(Game.Left);
                Console.WriteLine(GamePanels.Left);
                Console.WriteLine(Game.Height);
                Console.WriteLine(GamePanels.Height);
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

        private void Form2_ResizeBegin(object sender, EventArgs e)
        {
            lunghezza_client_iniziale = this.ClientRectangle.Width;
            altezza_client_iniziale = this.ClientRectangle.Height;
        }
    }
}
