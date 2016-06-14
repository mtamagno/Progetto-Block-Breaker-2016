using System;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication5
{
    public class Instructions : Panel
    {
        #region Methods
        MyFonts fontTitle;
        MyFonts fontParagraph;
        Label Title;
        Label Paragraph;
        Label Esc;

        public Instructions(int Left, int Top, int Width, int Height)
        {

            this.Left = Left;
            this.Top = Top;
            this.Width = Width;
            this.Height = Height;
            this.BackColor = Color.Black;

            fontParagraph = new MyFonts(MyFonts.FontType.paragraph);
            fontTitle = new MyFonts(MyFonts.FontType.Title);
            Esc = new Label();
            Title = new Label();
            Paragraph = new Label();
            Title.UseCompatibleTextRendering = true;
            Title.Top = 30;
            Title.Text = "Help";
            Title.Height = 60;
            Title.Width = this.Width;
            Title.Font = new Font(fontTitle.type.Families[0], 30, FontStyle.Regular);
            Title.TextAlign = ContentAlignment.MiddleCenter;
            Title.ForeColor = Color.White;

            
            Paragraph.Height = 300;
            Paragraph.Top = 100;

            Paragraph.Text = "Ingame Instructions:" +
                "\n.\n." +
                "\nClick on the button start to start the game"+
                "\n."+
                "\nMove your cursor to move the paddle" +
                "\n." +
                "\nPress space to throw the ball" +
                "\n." +
                "\nPress enter to pause the game";

            Paragraph.UseCompatibleTextRendering = true;
            Paragraph.Width = this.Width;
            Paragraph.Font = new Font(fontParagraph.type.Families[0], 14, FontStyle.Regular);
            Paragraph.TextAlign = ContentAlignment.MiddleCenter;
            Paragraph.ForeColor = Color.White;

            Esc.UseCompatibleTextRendering = true;
            Esc.Font = new Font(fontParagraph.type.Families[0], 14, FontStyle.Regular);
            Esc.Height = 40;
            Esc.Top = this.Height - Esc.Height * 8/7;
            Esc.Width = 200;
            Esc.TextAlign = ContentAlignment.MiddleCenter;
            Esc.Left = 10;
            Esc.Text = "Esc -> Back To Menu";
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

        #endregion Methods
    }
}