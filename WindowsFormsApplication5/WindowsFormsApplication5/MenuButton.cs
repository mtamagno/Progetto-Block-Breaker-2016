using System.Drawing;
using System.Windows.Forms;
using System;
using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication5
{
    public class MenuButton : Button
    {
        #region Fields

        private Bitmap buttonBackground;
        private PrivateFontCollection font;

        #endregion Fields

        #region Constructors

        public MenuButton(Size s)
        {

            FontSet();
            this.UseCompatibleTextRendering = true;
            this.Size = s;
            this.Font = new Font(font.Families[0], 12, FontStyle.Regular);
            this.buttonBackground = new Bitmap(Properties.Resources.BlueRoundedButton, this.Size);
            this.BackgroundImage = buttonBackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;
            this.BackColor = Color.Transparent;
            this.MouseHover += MouseHoverButton;
            this.MouseLeave += MouseLeaveButton;
            this.FlatStyle = FlatStyle.Flat;
           
        }

        private void FontSet()
        {
            this.font = new PrivateFontCollection();
            int fontlenght = Properties.Resources.small_font.Length;
            byte[] fontdata = Properties.Resources.small_font;
            System.IntPtr data = Marshal.AllocCoTaskMem(fontlenght);
            Marshal.Copy(fontdata, 0, data, fontlenght);
            font.AddMemoryFont(data, fontlenght);
            Marshal.FreeCoTaskMem(data);
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