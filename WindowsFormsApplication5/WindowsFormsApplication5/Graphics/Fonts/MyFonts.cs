using System.Drawing.Text;

namespace BlockBreaker
{
    internal class MyFonts
    {
        #region Public Fields

        public PrivateFontCollection Type;

        #endregion Public Fields

        #region Public Constructors

        public MyFonts(FontType thisFontType)
        {
            FontSet(thisFontType);
        }

        #endregion Public Constructors

        #region Public Enums

        public enum FontType
        {
            Title,
            Paragraph
        }

        #endregion Public Enums

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

        private void Paragraph()
        {
            PrivateFontCollection Type;
            Type = new PrivateFontCollection();
            Type.AddFontFile("FOnts/Linds.ttf");
            this.Type = Type;
        }

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
