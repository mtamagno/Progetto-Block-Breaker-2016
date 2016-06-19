using System.Drawing.Text;

namespace BlockBreaker
{
    internal class MyFonts
    {
        public enum FontType
        {
            Title,
            Paragraph
        }

        public PrivateFontCollection Type;

        public MyFonts(FontType thisFontType)
        {
            FontSet(thisFontType);
        }

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


        private void Title()
        {
            PrivateFontCollection Type;
            Type = new PrivateFontCollection();
            Type.AddFontFile("FOnts/SegoeKeycaps.TTF");
            this.Type = Type;
        }


        private void Paragraph()
        {
            PrivateFontCollection Type;
            Type = new PrivateFontCollection();
            Type.AddFontFile("FOnts/Linds.ttf");
            this.Type = Type;
        }
    }
}