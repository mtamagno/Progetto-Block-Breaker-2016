using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication5
{
    internal class GamePause : Panel
    {
        #region Constructors

        public GamePause(int Left, int Top, int Width, int Height)
        {
            this.Top = Top;
            this.Left = Left;
            this.Width = Width;
            this.Height = Height;
            this.BackgroundImage = new Bitmap(Properties.Resources.Gamepause, this.Size);
        }

        #endregion Constructors
    }
}