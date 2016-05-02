using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public partial class Form2 : Form
    {

        Form3 Start = new Form3();
        Form1 Game = new Form1();
        Panel GamePanels = new Panel();

        public Form2()
        {
            InitializeComponent();
            return;
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            this.Controls.Add(GamePanels);
            Game.TopLevel = false;
            Game.AutoScroll = true;
            GamePanels.Top = 0;
            GamePanels.Left = 0;

            GamePanels.Width = 1000;
            GamePanels.Height = 500;
            GamePanels.Visible = true;
            GamePanels.Controls.Add(Game);
            this.Dock = DockStyle.Fill;
            GamePanels.Dock = DockStyle.Fill;
            //GamePanels.Bounds = this.Bounds;
            Game.Width = GamePanels.Width;
            Game.Height = GamePanels.Height;
            Game.Show();
            Game.Left = 0;
            Game.Top = 0;
            Game.FormBorderStyle = FormBorderStyle.None;
            Game.Dock = DockStyle.Fill;
            //Game.Bounds = this.Bounds;
            return;
        }


        private void loadContent()
        {



        }

        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Game.Close();
        }
    }
}
