using System.Drawing;

namespace WindowsFormsApplication5
{
    public class Grid
    {
        #region Fields

        private System.Windows.Forms.DataGridView grid;

        #endregion Fields

        #region Constructors

        //Metodo grid da chiamare per creare una griglia all'interno del form
        public Grid(int x, int y, float client_height, float client_width, Bitmap Texture, Logic logic)
        {
            grid = new System.Windows.Forms.DataGridView();
            grid.ColumnCount = 18;
            grid.RowCount = 8;
            grid.Left = x;
            grid.Top = y;
            grid.Width = (int)client_width;
            grid.Height = (int)(client_height / 3);
            this.insert_grid(Texture, logic.iManager);
        }

        #endregion Constructors

        #region Methods

        //Metodo insert_grid utilizzato per inserire nei posti giusti i blocchi grazie alle coordinate della griglia
        public void insert_grid(Bitmap Texture, InputManager iManager)
        {
            for (int i = 0; i < grid.ColumnCount; i++)
            {
                for (int k = 0; k < grid.RowCount; k++)
                {
                    Block block = new Block((grid.Width / grid.ColumnCount) * i + 6, (grid.Height / grid.RowCount) * k + 2, grid.Width / grid.ColumnCount, grid.Height / grid.RowCount);
                    iManager.inGameSprites.Add(block);
                }
            }
        }

        public void redraw_block(Block s, int newWidth, int new_height, float nuova_x, float nuova_y)
        {
            s.textureSwitcher();
            s.redraw(s, (grid.Width / grid.ColumnCount), (grid.Height / grid.RowCount), s.texture, nuova_x, nuova_y);
        }

        public void redraw_grid(Grid grid, float client_height, float client_width)
        {
            grid.grid.Width = (int)client_width;
            grid.grid.Height = (int)(client_height / 3);
        }

        #endregion Methods
    }
}