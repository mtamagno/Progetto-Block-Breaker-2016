using System.Drawing.Text;
using System.Runtime.InteropServices;

namespace WindowsFormsApplication5
{
    class MyFonts 
    {
        public PrivateFontCollection type;       
        public enum FontType { Title, paragraph, };

        public MyFonts(FontType thisFontType)
        {
            this.type = new PrivateFontCollection();
            FontSet(thisFontType);
        }

        private void FontSet(FontType fontType)
        {
          


            if(fontType == MyFonts.FontType.paragraph)
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
            int fontlenght = Properties.Resources.SegoeKeycaps.Length;
            byte[] fontdata = Properties.Resources.SegoeKeycaps;
            System.IntPtr data = Marshal.AllocCoTaskMem(fontlenght);
            Marshal.Copy(fontdata, 0, data, fontlenght);
            type.AddMemoryFont(data, fontlenght);
            Marshal.FreeCoTaskMem(data);
        }


        private void Paragraph()
        {
            int secondfontlenght = Properties.Resources.Linds.Length;
            byte[] secondfontdata = Properties.Resources.Linds;
            System.IntPtr secondata = Marshal.AllocCoTaskMem(secondfontlenght);
            Marshal.Copy(secondfontdata, 0, secondata, secondfontlenght);
            type.AddMemoryFont(secondata, secondfontlenght);
            Marshal.FreeCoTaskMem(secondata);           
        }

    }
}
