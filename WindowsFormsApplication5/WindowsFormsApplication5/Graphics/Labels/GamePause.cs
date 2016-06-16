using System.Windows.Forms;
using System.Drawing;

namespace BlockBreaker
{
    internal class GamePause : Panel
    {

        MyFonts fontTitle;
        MyFonts fontParagraph;
        Label Title;
        Label Paragraph;
        Label Esc;
        #region Methods
        #endregion Methods
        #region Constructors

        public GamePause(int Left, int Top, int Width, int Height)
        {

            this.Left = Left;
            this.Top = Top;
            this.Width = Width;
            this.Height = Height;
            this.BackColor = Color.Black;
            setLabels();
            setFont();
            setText();

        }

        public void setLabels()
        {
            Esc = new Label();
            Title = new Label();
            Paragraph = new Label();
        }

        public void setFont()
        {
            fontParagraph = new MyFonts(MyFonts.FontType.paragraph);
            fontTitle = new MyFonts(MyFonts.FontType.Title);
            Title.Font = new Font(fontTitle.type.Families[0], 30, FontStyle.Regular);
            Paragraph.Font = new Font(fontParagraph.type.Families[0], 14, FontStyle.Regular);
            Esc.Font = new Font(fontParagraph.type.Families[0], 14, FontStyle.Regular);

        }

        public void setText()
        {

            Title.UseCompatibleTextRendering = true;
            Title.Top = 40;
            Title.Text = "Game Pause";
            Title.Height = 60;
            Title.Width = this.Width;

            Title.TextAlign = ContentAlignment.MiddleCenter;
            Title.ForeColor = Color.White;


            Paragraph.Height = 300;
            Paragraph.Top = 100;

            Paragraph.Text = "Press:" +
                "\n.\n.\n." +
                "\nSpace To resume the game" +
                "\n.\n." +
                "\nEsc To GameOver";

            Paragraph.UseCompatibleTextRendering = true;
            Paragraph.Width = this.Width;
            Paragraph.TextAlign = ContentAlignment.MiddleCenter;
            Paragraph.ForeColor = Color.White;

            Esc.UseCompatibleTextRendering = true;
            Esc.Height = 40;
            Esc.Top = this.Height - Esc.Height * 2;
            Esc.Width = 200;
            Esc.TextAlign = ContentAlignment.MiddleCenter;
            Esc.Left = 10;
            Esc.Text = "Space -> Back To Game";
            Esc.ForeColor = Color.White;

            this.Controls.Add(Esc);
            this.Controls.Add(Title);
            this.Controls.Add(Paragraph);
            /*controller.BackgroundImage = Properties.Resources.Instructions;
            controller.BackgroundImageLayout = ImageLayout.Stretch;
            controller.Visible = false;
            return controller;*/
            this.Visible = false;

        }
        #endregion Constructors
    }
}
