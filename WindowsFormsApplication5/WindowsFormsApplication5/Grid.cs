using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace WindowsFormsApplication5
{
    class Grid
    {
        System.Windows.Forms.TableLayoutPanel grid;

        public Grid(int x, int y,int row,int column)
        {
            grid = new System.Windows.Forms.TableLayoutPanel();
            grid.Left = x;
            grid.Top = y;
            grid.ColumnCount = 25;
            grid.RowCount = 10;
            grid.Width = Form1.ActiveForm.Width;
            grid.Height = Form1.ActiveForm.Height / 3;
            grid.RowCount = row;
            grid.ColumnCount = column;
        }   

        public void insert_grid(Sprite s,int row, int column)
        {
      



        }
    }
}
