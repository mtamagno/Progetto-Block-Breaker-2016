using System.Drawing;
using System.Windows.Forms;
using System;

namespace BlockBreaker
{
    public sealed class MenuButton : Button
    {
        #region Fields

        private readonly MyFonts _fonts;
        #endregion Fields

        #region Constructors

        public MenuButton(Size s)
        {
            if (s.Height > 0 && s.Width > 0)
            {
                _fonts = new MyFonts(MyFonts.FontType.paragraph);
                this.UseCompatibleTextRendering = true;
                this.Size = s;
                this.Font = new Font(_fonts.type.Families[0], 12, FontStyle.Regular);
                var buttonBackground = new Bitmap(Properties.Resources.BlueRoundedButton, this.Size);
                this.BackgroundImage = buttonBackground;
                this.BackgroundImageLayout = ImageLayout.Stretch;
                this.BackColor = Color.Transparent;
                this.MouseHover += MouseHoverButton;
                this.MouseLeave += MouseLeaveButton;
                this.FlatStyle = FlatStyle.Flat;
            }
            else
                MessageBox.Show("invalid button size");
        }


        private void MouseHoverButton(object sender, EventArgs e)
        {
            this.FlatStyle = FlatStyle.Popup;
           
        }

        private void MouseLeaveButton(object sender, EventArgs e)
        {
            
            this.FlatStyle = FlatStyle.Flat;
        }

        #endregion Constructors
    }
}