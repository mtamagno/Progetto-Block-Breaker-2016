using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

namespace BlockBreaker
{
    sealed class AudioButtons : Button
    {
        #region Fields

        private Bitmap _buttonBackground;
        private bool _state;

        readonly MyFonts fonts;
        #endregion Fields

        #region Constructors

        public AudioButtons(Size s)
        {
            _state = true;
            if (s.Height > 0 && s.Width > 0)
            {
                fonts = new MyFonts(MyFonts.FontType.paragraph);
                this.UseCompatibleTextRendering = true;
                this.Size = s;
                this.Font = new Font(fonts.type.Families[0], 12, FontStyle.Regular);
                this._buttonBackground = new Bitmap(Properties.Resources.Audio, this.Size);
                this.BackgroundImage = _buttonBackground;
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
            this.FlatStyle = FlatStyle.Standard;
        }

        private void MouseLeaveButton(object sender, EventArgs e)
        {

            this.FlatStyle = FlatStyle.Flat;
        }

        public void ChangeState()
        {
            _state = !_state;
            this._buttonBackground.Dispose();
            if(_state)
            this._buttonBackground = new Bitmap(Properties.Resources.Audio, this.Size);
            else
            this._buttonBackground = new Bitmap(Properties.Resources.AudioOff, this.Size);
            this.BackgroundImage = _buttonBackground;
            this.BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion Constructors
    }
}

