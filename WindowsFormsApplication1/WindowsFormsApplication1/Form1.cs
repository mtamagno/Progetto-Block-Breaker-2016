using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class Form1 : Form
    {
        enum Position {
            Left, Up, Right, Down, Rendi
        }

        private int _x;
        private int _y;
        private Position _objPosition;
        private Sprite s;
        private SpriteBatch spritebatch;


        public Form1()
        {
            s = new Sprite(Properties.Resources.Pallina, 100, 100, 100,100);
            InitializeComponent();
            _x = 50;
            _y = 50;
            _objPosition = Position.Right;
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.FillRectangle(Brushes.Blue, _x, _y, 100, 100);
        }

        private void timer_tick(object sender, EventArgs e)
        {
            if(_objPosition==Position.Right)
            _x += 10;
            if (_objPosition == Position.Left)
                _x -= 10;
            if (_objPosition == Position.Up)
                _y -= 10;
            if (_objPosition == Position.Down)
                _y += 10;
            if (_objPosition == Position.Rendi)
            {
                spritebatch.Begin();
                spritebatch.Draw(s);
                spritebatch.End();
            }

            Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Left)
            {
                _objPosition = Position.Left;
            }
            if (e.KeyCode == Keys.Right)
            {
                _objPosition = Position.Right;
            }
            if (e.KeyCode == Keys.Up)
            {
                _objPosition = Position.Up;
            }
            if (e.KeyCode == Keys.Down)
            {
                _objPosition = Position.Down;
            }
            if(e.KeyCode == Keys.D)
            {
                _objPosition = Position.Rendi;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}

