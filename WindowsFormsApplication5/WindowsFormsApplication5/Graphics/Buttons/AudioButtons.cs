using BlockBreaker.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    internal sealed class AudioButtons : Button
    {
        #region Private Fields

        private readonly MyFonts fonts;
        private Bitmap _buttonBackground;
        private bool _state;

        #endregion Private Fields

        #region Public Constructors

        public AudioButtons(Size s)
        {
            _state = true;
            if (s.Height > 0 && s.Width > 0)
            {
                fonts = new MyFonts(MyFonts.FontType.Paragraph);
                UseCompatibleTextRendering = true;
                Size = s;
                Font = new Font(fonts.Type.Families[0], 12, FontStyle.Regular);
                _buttonBackground = new Bitmap(Resources.Audio, Size);
                BackgroundImage = _buttonBackground;
                BackgroundImageLayout = ImageLayout.Stretch;
                BackColor = Color.Transparent;
                MouseHover += MouseHoverButton;
                MouseLeave += MouseLeaveButton;
                FlatStyle = FlatStyle.Flat;
            }
            else
                MessageBox.Show("invalid button size");
        }

        #endregion Public Constructors

        #region Public Methods

        public void ChangeState()
        {
            _state = !_state;
            _buttonBackground.Dispose();
            if (_state)
                _buttonBackground = new Bitmap(Resources.Audio, Size);
            else
                _buttonBackground = new Bitmap(Resources.AudioOff, Size);
            BackgroundImage = _buttonBackground;
            BackgroundImageLayout = ImageLayout.Stretch;
        }

        #endregion Public Methods

        #region Private Methods

        private void MouseHoverButton(object sender, EventArgs e)
        {
            FlatStyle = FlatStyle.Standard;
        }

        private void MouseLeaveButton(object sender, EventArgs e)
        {
            FlatStyle = FlatStyle.Flat;
        }

        #endregion Private Methods
    }
}
