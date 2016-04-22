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

        //ok adesso va e gli elementi sono dentro alla griglia ma bisogna trovare il modo per 
        //approssimare lo scarto che rimane o ridimensionare la griglia 
        //o le immagini dei blocchi
        public Grid(int x, int y,float client_height,float client_width)
        {
            grid = new System.Windows.Forms.DataGridView();
            grid.ColumnCount = (int)client_width / 100;
            grid.RowCount = (int)client_height / 50;
            grid.Left = x;
            grid.Top = y;
            grid.Width = (int)client_width;
            grid.Height = (int)(client_height / 3);
        }

        public void insert_grid(Bitmap Texture, InputManager iManager)
        {
            for (int i = 0; i < grid.ColumnCount; i++)
            {
                for (int k = 0; k < grid.RowCount; k++)
                {
                    Sprite block = new Sprite(Texture, (grid.Width/grid.ColumnCount) * i + 2, (grid.Height/grid.RowCount) * k + 2, grid.Width / grid.ColumnCount, grid.Height / grid.RowCount, Sprite.SpriteType.block);
                    grid.Rows[k].Cells[i].Value = block;
                    iManager.inGameSprites.Add(block);
                }
            }
        }

        public void redraw_grid(Grid grid, float client_height, float client_width)
        {
            grid.grid.Width = (int)client_width;
            grid.grid.Height= (int)(client_height / 3);
        }
    }
}
