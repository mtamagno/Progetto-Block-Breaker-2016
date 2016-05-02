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
            Game.AutoScaleMode = AutoScaleMode.Inherit;
            GamePanels.Dock = DockStyle.Fill;
            GamePanels.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right; ;
            //GamePanels.Bounds = this.Bounds;
            Game.Width = GamePanels.Width;
            Game.Height = GamePanels.Height;
            Game.Show();
            Game.Left = 0;
            Game.Top = 0;
            Game.FormBorderStyle = FormBorderStyle.None;
            Game.Anchor = AnchorStyles.Top & AnchorStyles.Bottom & AnchorStyles.Left & AnchorStyles.Right;
            Game.AutoScaleMode = AutoScaleMode.Inherit;
            //Game.Dock = DockStyle.Fill;
            //Game.Bounds = this.Bounds;

            return;
        }


        private void Form2_FormClosing(object sender, FormClosingEventArgs e)
        {
            Game.Close();
        }

        private void Form2_ResizeEnd(object sender, EventArgs e)
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
        }
    }
}
