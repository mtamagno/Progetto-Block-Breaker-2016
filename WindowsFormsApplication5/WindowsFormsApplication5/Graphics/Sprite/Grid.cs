using System.Drawing;

namespace BlockBreaker
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
            this.grid.ColumnCount = 20;
            this.grid.RowCount = 8;
            this.grid.Left = x;
            this.grid.Top = y;
            this.grid.Width = (int)client_width;
            this.grid.Height = (int)(client_height / 3);
            this.insert_grid(Texture, logic.iManager);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Metodo insert_grid utilizzato per inserire nei posti giusti i blocchi grazie alle coordinate della griglia
        /// </summary>
        /// <param name="Texture"></param>
        /// <param name="iManager"></param>
        public void insert_grid(Bitmap Texture, InputManager iManager)
        {
            for (int i = 0; i < grid.ColumnCount; i++)
            {
                for (int k = 0; k < grid.RowCount; k++)
                {
                    Block block = new Block((grid.Width / grid.ColumnCount) * i + grid.Left + 3, (grid.Height / grid.RowCount) * k + grid.Top + 3, grid.Width / grid.ColumnCount, grid.Height / grid.RowCount);
                    iManager.inGameSprites.Add(block);
                }
            }
        }

        /// <summary>
        /// Metodo utilizzato per scalare i blocchi al variare della dimensione della finestra di gioco
        /// </summary>
        /// <param name="s"></param>
        /// <param name="newWidth"></param>
        /// <param name="new_height"></param>
        /// <param name="nuova_x"></param>
        /// <param name="nuova_y"></param>
        public void redraw_block(Block s, int newWidth, int new_height, float nuova_x, float nuova_y)
        {
            s.textureSwitcher();
            s.Redraw(s, (grid.Width / grid.ColumnCount), (grid.Height / grid.RowCount), s.texture, nuova_x, nuova_y);
        }

        /// <summary>
        /// Metodo utilizzato per scalare la griglia al variare della dimensione della finestra di gioco
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="client_height"></param>
        /// <param name="client_width"></param>
        public void redraw_grid(Grid grid, float client_height, float client_width)
        {
            grid.grid.Width = (int)client_width;
            grid.grid.Height = (int)(client_height / 3);
        }

        #endregion Methods
    }
}