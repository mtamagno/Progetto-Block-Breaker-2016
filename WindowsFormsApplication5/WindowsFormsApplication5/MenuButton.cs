using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication5
{
    public class MenuButton : Button
    {
        private Bitmap buttonBackground;
        public MenuButton(Size s)
        {
            this.Size = s;
            this.buttonBackground = new Bitmap(Properties.Resources.BlueRoundedButton, this.Size);
            this.BackgroundImage = buttonBackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackColor = Color.Black;
            this.MouseHover += MouseHoverButton;
            this.MouseLeave += MouseLeaveButton;
            this.FlatStyle = FlatStyle.Flat;
        }

        private void MouseHoverButton(object sender, EventArgs e)
        {
            this.FlatStyle = FlatStyle.Standard;
        }

        private void MouseLeaveButton(object sender, EventArgs e)
        {
            
            this.FlatStyle = FlatStyle.Flat;
        }
    }
}
