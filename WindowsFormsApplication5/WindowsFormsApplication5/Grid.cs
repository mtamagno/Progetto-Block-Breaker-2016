using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using System.Drawing;


namespace WindowsFormsApplication5
{
    class Grid
    {
        System.Windows.Forms.DataGridView grid;

        public Grid(int x, int y,int row,int column)
        {
            grid = new System.Windows.Forms.DataGridView();
            grid.Left = x;
            grid.Top = y;
            grid.Width = Form1.ActiveForm.Width;
            grid.Height = Form1.ActiveForm.Height / 3;
            grid.RowCount = row;
            grid.ColumnCount = column;
        }   

        public void insert_grid(Bitmap Texture,int row, int column, InputManager iManager)
        {
                         
        }
    }
}
