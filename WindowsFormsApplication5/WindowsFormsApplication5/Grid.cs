using System.Drawing;

namespace WindowsFormsApplication5
{
    internal class Grid
    {
        #region Private Fields

        private System.Windows.Forms.DataGridView grid;

        #endregion Private Fields

        #region Public Constructors

        //Metodo grid da chiamare per creare una griglia all'interno del form
        public Grid(int x, int y, float client_height, float client_width)
        {
            grid = new System.Windows.Forms.DataGridView();
            grid.ColumnCount = (int)client_width / 100;
            grid.RowCount = (int)client_height / 50;
            grid.Left = x;
            grid.Top = y;
            grid.Width = (int)client_width;
            grid.Height = (int)(client_height / 3);
        }

        #endregion Public Constructors
        #region Public Methods

        //Metodo insert_grid utilizzato per inserire nei posti giusti i blocchi grazie alle coordinate della griglia
        public void insert_grid(Bitmap Texture, InputManager iManager)
        {
            for (int i = 0; i < grid.ColumnCount; i++)
            {
                for (int k = 0; k < grid.RowCount; k++)
                {
                    Sprite block = new Sprite(Texture, (grid.Width / grid.ColumnCount) * i + 2, (grid.Height / grid.RowCount) * k + 2, grid.Width / grid.ColumnCount, grid.Height / grid.RowCount, Sprite.SpriteType.block);
                    iManager.inGameSprites.Add(block);
                }
            }
        }

        public void redraw_block(Sprite s, int new_width, int new_height, Bitmap risorsa, float nuova_x, float nuova_y)
        {
            s.redraw(s, (grid.Width / grid.ColumnCount), (grid.Height / grid.RowCount), risorsa, nuova_x, nuova_y);
        }

        public void redraw_grid(Grid grid, float client_height, float client_width)
        {
            grid.grid.Width = (int)client_width;
            grid.grid.Height = (int)(client_height / 3);
        }

        #endregion Public Methods
    }
}