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
            grid.Left = 0;
            grid.Top = 0;
            grid.Width = Form1.ActiveForm.Width;
            grid.Height = Form1.ActiveForm.Height / 3;
            grid.RowCount = row;
            grid.ColumnCount = column;
        }

        public void insert_grid(Bitmap Texture, InputManager iManager)
        {
            for (int i = 0; i < grid.Columns.Count; i++)
            {
                for (int k = 0; k < grid.Rows.Count; k++)
                {
                    Sprite block = new Sprite(Properties.Resources.Block, grid.Rows[k].Cells[i].ContentBounds.X, grid.Rows[k].Cells[i].ContentBounds.Y, 100, 50, Sprite.SpriteType.block);
                    grid.Rows[k].Cells[i].Value = block;
                    iManager.inGameSprites.Add(block);
                }
            }
        }
    }
}
