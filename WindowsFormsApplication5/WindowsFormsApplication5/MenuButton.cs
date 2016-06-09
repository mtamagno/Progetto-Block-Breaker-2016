using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public class MenuButton : Button
    {
        #region Fields

        private Bitmap buttonBackground;

        #endregion Fields

        #region Constructors

        public MenuButton(Size s)
        {
            this.Size = s;
            this.buttonBackground = new Bitmap(Properties.Resources.BlueRoundedButton, this.Size);
            this.BackgroundImage = buttonBackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackColor = Color.Black;
        }

        #endregion Constructors
    }
}