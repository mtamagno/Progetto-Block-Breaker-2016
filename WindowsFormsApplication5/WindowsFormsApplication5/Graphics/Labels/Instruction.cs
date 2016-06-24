using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public class Instructions : Panel
    {
        #region Private Fields

        private readonly MyFonts _fontParagraph;
        private readonly MyFonts _fontTitle;

        #endregion Private Fields

        #region Public Constructors
        /// <summary>
        /// Costruttore, prende in ingresso grandezza e posizione e va a costruire il panel riferito allo stato gamePause,
        /// in oltre scrive dentro i label i relativi testi.
        /// </summary>
        /// <returns></returns>
        /// 

        public Instructions(int Left, int Top, int Width, int Height)
        {
            this.Left = Left;
            this.Top = Top;
            this.Width = Width;
            this.Height = Height;
            BackColor = Color.Black;
            _fontParagraph = new MyFonts(MyFonts.FontType.Paragraph);
            _fontTitle = new MyFonts(MyFonts.FontType.Title);
            var Esc = new Label();
            var Title = new Label();
            var Paragraph = new Label();
            Title.UseCompatibleTextRendering = true;
            Title.Top = 30;
            Title.Text = "Help";
            Title.Height = 60;
            Title.Width = this.Width;
            Title.Font = new Font(_fontTitle.Type.Families[0], 30, FontStyle.Regular);
            Title.TextAlign = ContentAlignment.MiddleCenter;
            Title.ForeColor = Color.White;
            Paragraph.Height = 300;
            Paragraph.Top = 100;
            Paragraph.Text = "Ingame Instructions:" +
                "\n.\n." +
                "\nClick on the button start to start the game" +
                "\n." +
                "\nMove your cursor to move the Racket" +
                "\n." +
                "\nPress space to throw the ball" +
                "\n." +
                "\nPress enter to pause the game";
            Paragraph.UseCompatibleTextRendering = true;
            Paragraph.Width = this.Width;
            Paragraph.Font = new Font(_fontParagraph.Type.Families[0], 14, FontStyle.Regular);
            Paragraph.TextAlign = ContentAlignment.MiddleCenter;
            Paragraph.ForeColor = Color.White;
            Esc.UseCompatibleTextRendering = true;
            Esc.Font = new Font(_fontParagraph.Type.Families[0], 14, FontStyle.Regular);
            Esc.Height = 40;
            Esc.Top = this.Height - Esc.Height * 2;
            Esc.Width = 200;
            Esc.TextAlign = ContentAlignment.MiddleCenter;
            Esc.Left = 10;
            Esc.Text = "Esc -> Back To Menu";
            Esc.ForeColor = Color.White;
            Controls.Add(Esc);
            Controls.Add(Title);
            Controls.Add(Paragraph);
            Visible = false;
        }

        #endregion Public Constructors
    }
}
