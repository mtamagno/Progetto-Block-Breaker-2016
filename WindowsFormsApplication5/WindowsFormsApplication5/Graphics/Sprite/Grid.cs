using System;
using System.Drawing;

namespace BlockBreaker
{
    public class Grid
    {
        #region Fields

        private readonly System.Windows.Forms.DataGridView _grid;

        #endregion Fields

        #region Constructors

        //Metodo grid da chiamare per creare una griglia all'interno del form
        public Grid(int x, int y, float clientHeight, float clientWidth, Bitmap texture, Logic logic)
        {
            if (texture == null) throw new ArgumentNullException(nameof(texture));
            if (logic == null) throw new ArgumentNullException(nameof(logic));
            if (x <= 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y <= 0) throw new ArgumentOutOfRangeException(nameof(y));
            if (clientHeight <= 0) throw new ArgumentOutOfRangeException(nameof(clientHeight));
            if (clientWidth <= 0) throw new ArgumentOutOfRangeException(nameof(clientWidth));

            this._grid = new System.Windows.Forms.DataGridView();
            this._grid.ColumnCount = 20;
            this._grid.RowCount = 8;
            this._grid.Left = x;
            this._grid.Top = y;
            this._grid.Width = (int)clientWidth;
            this._grid.Height = (int)(clientHeight / 3);
            this.insert_grid(texture, logic.IManager);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Metodo insert_grid utilizzato per inserire nei posti giusti i blocchi grazie alle coordinate della griglia
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="iManager"></param>
        public void insert_grid(Bitmap texture, InputManager iManager)
        {
            if (texture == null) throw new ArgumentNullException(nameof(texture));
            if (iManager == null) throw new ArgumentNullException(nameof(iManager));
            for (int i = 0; i < _grid.ColumnCount; i++)
            {
                for (int k = 0; k < _grid.RowCount; k++)
                {
                    Block block = new Block((_grid.Width / _grid.ColumnCount) * i + _grid.Left + 3, (_grid.Height / _grid.RowCount) * k + _grid.Top + 3, _grid.Width / _grid.ColumnCount, _grid.Height / _grid.RowCount);
                    iManager.inGameSprites.Add(block);
                }
            }
        }

        /// <summary>
        /// Metodo utilizzato per scalare i blocchi al variare della dimensione della finestra di gioco
        /// </summary>
        /// <param name="s"></param>
        /// <param name="newWidth"></param>
        /// <param name="newHeight"></param>
        /// <param name="nuovaX"></param>
        /// <param name="nuovaY"></param>
        public void redraw_block(Block s, int newWidth, int newHeight, float nuovaX, float nuovaY)
        {
            s.TextureSwitcher();
            s.Redraw(s, (_grid.Width / _grid.ColumnCount), (_grid.Height / _grid.RowCount), s.texture, nuovaX, nuovaY);
        }

        /// <summary>
        /// Metodo utilizzato per scalare la griglia al variare della dimensione della finestra di gioco
        /// </summary>
        /// <param name="grid"></param>
        /// <param name="clientHeight"></param>
        /// <param name="clientWidth"></param>
        public void redraw_grid(Grid grid, float clientHeight, float clientWidth)
        {
            if (grid == null) throw new ArgumentNullException(nameof(grid));
            if (clientHeight <= 0) throw new ArgumentOutOfRangeException(nameof(clientHeight));
            if (clientWidth <= 0) throw new ArgumentOutOfRangeException(nameof(clientWidth));
            grid._grid.Width = (int)clientWidth;
            grid._grid.Height = (int)(clientHeight / 3);
        }

        #endregion Methods
    }
}