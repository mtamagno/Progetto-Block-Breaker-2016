using BlockBreaker.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public sealed class MenuButton : Button
    {
        #region Private Fields

        private readonly MyFonts _fonts;

        #endregion Private Fields

        #region Public Constructors
        /// <summary>
        /// Costruttore, prende in ingresso la dimensione e crea il pulsante personalizzato "bottone"
        /// </summary>
        /// <returns></returns>
        /// 
        public MenuButton(Size s)
        {
            if (s.Height > 0 && s.Width > 0)
            {
                _fonts = new MyFonts(MyFonts.FontType.Paragraph);
                UseCompatibleTextRendering = true;
                Size = s;
                Font = new Font(_fonts.Type.Families[0], 12, FontStyle.Regular);
                var buttonBackground = new Bitmap(Resources.BlueRoundedButton, Size);
                BackgroundImage = buttonBackground;
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

        #region Private Methods

        /// <summary>
        /// Evento che si verifica al passaggio del mouse sopra il bottone
        /// </summary>
        /// <returns></returns>
        /// 
        private void MouseHoverButton(object sender, EventArgs e)
        {
            FlatStyle = FlatStyle.Popup;
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
