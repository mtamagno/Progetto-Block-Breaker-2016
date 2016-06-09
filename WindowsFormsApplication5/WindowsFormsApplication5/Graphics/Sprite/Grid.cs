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
            this.grid = new System.Windows.Forms.DataGridView();
            this.grid.ColumnCount = 18;
            this.grid.RowCount = 8;
            this.grid.Left = x;
            this.grid.Top = y;
            this.grid.Width = (int)client_width;
            this.grid.Height = (int)(client_height / 3);
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
                    Block block = new Block((grid.Width / grid.ColumnCount) * i + grid.Left + 5, (grid.Height / grid.RowCount) * k + grid.Top + 3, grid.Width / grid.ColumnCount, grid.Height / grid.RowCount);
                    iManager.inGameSprites.Add(block);
                }
            }
        }

        //Metodo utilizzato per scalare i blocchi al variare della dimensione della finestra di gioco
        public void redraw_block(Block s, int newWidth, int new_height, float nuova_x, float nuova_y)
        {
            s.textureSwitcher();
            s.redraw(s, (grid.Width / grid.ColumnCount), (grid.Height / grid.RowCount), s.texture, nuova_x, nuova_y);
        }

        //Metodo utilizzato per scalare la griglia al variare della dimensione della finestra di gioco
        public void redraw_grid(Grid grid, float client_height, float client_width)
        {
            grid.grid.Width = (int)client_width;
            grid.grid.Height = (int)(client_height / 3);
        }

        #endregion Methods
    }
}