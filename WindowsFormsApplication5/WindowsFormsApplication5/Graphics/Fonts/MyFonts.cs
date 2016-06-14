using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO;
using System;

namespace WindowsFormsApplication5
{
    class MyFonts
    {   
        public PrivateFontCollection type;
        public enum FontType { Title, paragraph, };

        public MyFonts(FontType thisFontType)
        {
            FontSet(thisFontType);
        }

        private void FontSet(FontType fontType)
        {

            type = new PrivateFontCollection();
            if (fontType == MyFonts.FontType.paragraph)
             {
                 Paragraph();
             }

             if (fontType == MyFonts.FontType.Title)
             {
                 Title();
             }

        }


        private void Title()
        {
            PrivateFontCollection Type;
            Type = new PrivateFontCollection();
            Type.AddFontFile("SegoeKeycaps.TTF");
            type = Type;
        }

    


        private void Paragraph()
        {
            PrivateFontCollection Type;
            Type = new PrivateFontCollection();
            Type.AddFontFile("Linds.ttf");
            type = Type;
        }

    }
}
