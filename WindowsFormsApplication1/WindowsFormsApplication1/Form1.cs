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

        Graphics drawings;

        public Form1()
        {
            InitializeComponent();
            drawings = pictureBox1.CreateGraphics();
        }

        //public class Ball
        //{
        //    private void DrawRectangle()
        //    {
        //        System.Drawing.Pen myPen = new System.Drawing.Pen(System.Drawing.Color.Red);
        //        System.Drawing.Graphics formGraphics;
        //        formGraphics = Form1.pictureBox1.CreateGraphics();
        //        formGraphics.DrawRectangle(myPen, new Rectangle(0, 0, 200, 300));
        //        myPen.Dispose();
        //        formGraphics.Dispose();
        //    }
        //}

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            SolidBrush myBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
            Graphics formGraphics;
            drawings.FillRectangle(myBrush, new Rectangle(0, 0, 50, 50));
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            
        }

        //private void FillRectangleInt(PaintEventArgs ciao)
        //{

        //    // Create solid brush.
        //    SolidBrush blueBrush = new SolidBrush(Color.Blue);

        //    // Create location and size of rectangle.
        //    int x = 0;
        //    int y = 0;
        //    int width = 200;
        //    int height = 200;

        //    // Fill rectangle to screen.
        //    ciao.Graphics.FillRectangle(blueBrush, x, y, width, height);
        //}
        
    }
}
