using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    class GamePause : Panel
    {
        public GamePause(int Left , int Top, int Width, int Height)
        {
            this.Top = Top;
            this.Left = Left;
            this.Width = Width;
            this.Height = Height;
            this.BackgroundImage = Properties.Resources.Gamepause;
        }

    }
}
