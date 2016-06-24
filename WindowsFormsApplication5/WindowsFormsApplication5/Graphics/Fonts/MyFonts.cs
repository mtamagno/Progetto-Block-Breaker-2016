using System.Drawing.Text;

namespace BlockBreaker
{
    internal class MyFonts
    {
        #region Public Fields

        public PrivateFontCollection Type;

        #endregion Public Fields

        #region Public Constructors

        /// <summary>
        /// Costruttore, prende in ingresso il tipo del font scelto
        /// in modo tale da sapere come impostare il tipo di font
        /// </summary>
        /// <returns></returns>
        ///
        public MyFonts(FontType thisFontType)
        {
            FontSet(thisFontType);
        }

        #endregion Public Constructors

        #region Public Enums

        /// <summary>
        /// Tipo enum che contiene i tipi di font disponibili
        /// </summary>
        /// <returns></returns>
        ///
        public enum FontType
        {
            Title,
            Paragraph
        }

        #endregion Public Enums

        /// <summary>
        /// Funzione utilizzata per andare a impostare un font personalizzato
        /// </summary>
        /// <returns></returns>
        ///

        #region Private Methods

        private void FontSet(FontType fontType)
        {
            Type = new PrivateFontCollection();
            if (fontType == FontType.Paragraph)
            {
                Paragraph();
            }
            if (fontType == FontType.Title)
            {
                Title();
            }
        }

        /// <summary>
        /// Funzione che imposta il font che verra' utilizzato nei paragrafi
        /// </summary>
        /// <returns></returns>
        ///
        private void Paragraph()
        {
            PrivateFontCollection Type;
            Type = new PrivateFontCollection();
            Type.AddFontFile("FOnts/Linds.ttf");
            this.Type = Type;
        }

        /// <summary>
        /// Funzione che imposta il font che verra' utilizzato nei titoli
        /// </summary>
        /// <returns></returns>
        ///
        private void Title()
        {
            PrivateFontCollection Type;
            Type = new PrivateFontCollection();
            Type.AddFontFile("FOnts/SegoeKeycaps.TTF");
            this.Type = Type;
        }

        #endregion Private Methods
    }
}
