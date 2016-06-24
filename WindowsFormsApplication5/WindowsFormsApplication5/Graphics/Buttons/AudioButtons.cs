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
        /// <summary>
        /// Costruttore, prende in ingresso la dimensione e crea il pulsante personalizzato "bottone"
        /// </summary>
        /// <returns></returns>
        /// 
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

        /// <summary>
        /// Funzione che cambia la grafica del bottone nel caso in cui l audio sia attivo o meno
        /// </summary>
        /// <returns></returns>
        /// 
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
        /// <summary>
        /// Evento che si verifica al passaggio del mouse sopra il bottone
        /// </summary>
        /// <returns></returns>
        /// 
        private void MouseHoverButton(object sender, EventArgs e)
        {
            FlatStyle = FlatStyle.Standard;
        }
        /// <summary>
        /// Evento che si verifica quando si sposta il cursore del mouse dall'interno del bottone all'esterno
        /// </summary>
        /// <returns></returns>
        /// 

        private void MouseLeaveButton(object sender, EventArgs e)
        {
            FlatStyle = FlatStyle.Flat;
        }

        #endregion Private Methods
    }
}
