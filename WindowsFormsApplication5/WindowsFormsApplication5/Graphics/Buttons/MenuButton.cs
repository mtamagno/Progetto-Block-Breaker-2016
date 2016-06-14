using System.Drawing;
using System.Windows.Forms;
using System;

namespace WindowsFormsApplication5
{
    public class MenuButton : Button
    {
        #region Fields

        private Bitmap buttonBackground;
        MyFonts fonts;
        #endregion Fields

        #region Constructors

        public MenuButton(Size s)
        {
            if (s.Height > 0 && s.Width > 0)
            {
                fonts = new MyFonts(MyFonts.FontType.paragraph);
                this.UseCompatibleTextRendering = true;
                this.Size = s;
                this.Font = new Font(fonts.type.Families[0], 12, FontStyle.Regular);
                this.buttonBackground = new Bitmap(Properties.Resources.BlueRoundedButton, this.Size);
                this.BackgroundImage = buttonBackground;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                this.BackColor = Color.Transparent;
                this.MouseHover += MouseHoverButton;
                this.MouseLeave += MouseLeaveButton;
                this.FlatStyle = FlatStyle.Flat;
            }
        }


        private void MouseHoverButton(object sender, EventArgs e)
        {
            this.FlatStyle = FlatStyle.Standard;
        }

        private void MouseLeaveButton(object sender, EventArgs e)
        {
            
            this.FlatStyle = FlatStyle.Flat;
        }

        #endregion Constructors
    }
}