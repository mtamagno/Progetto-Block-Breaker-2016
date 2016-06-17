using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.IO;


namespace BlockBreaker
{
    class HighScoresPanel : Panel
    {
        private MyFonts fontTitle;
        private MyFonts fontParagraph;

       

        public HighScoresPanel(int Left, int Top, int Width, int Height)
        {
            var esc = new Label();
            var title = new Label();
            var paragraph = new Label();

            if (File.Exists("HighScores.xml"))
            {
                var Reader = new XmlTextReader("HighScores.xml");
                var highscoreCounter = 0;
                for (int i = 0; Reader.Read() && i < 100; i++)
            {
                switch (Reader.NodeType)
                {

                    case XmlNodeType.Element:
                        Console.WriteLine("<" + Reader.Name + ">");
                        break;

                    case XmlNodeType.Text:
                        paragraph.Text += Reader.Value + " - ";
                            Console.WriteLine(Reader.Value);
                            highscoreCounter++;
                        break;

                    case XmlNodeType.EndElement:
                        Console.WriteLine("</" + Reader.Name + ">");
                        break;
                }
                if (highscoreCounter == 2)
                {
                        paragraph.Text += "\n\n";
                        highscoreCounter = 0;
                }
            }
            }

            Console.ReadLine();

            this.Left = Left;
            this.Top = Top;
            this.Width = Width;
            this.Height = Height;
            this.BackColor = Color.Black;

            this.fontParagraph = new MyFonts(MyFonts.FontType.paragraph);
            this.fontTitle = new MyFonts(MyFonts.FontType.Title);
            title.UseCompatibleTextRendering = true;
            title.Top = 30;
            title.Text = "Highscores";
            title.Height = 60;
            title.Width = this.Width;
            title.Font = new Font(fontTitle.type.Families[0], 30, FontStyle.Regular);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.ForeColor = Color.White;

            paragraph.Height = 300;
            paragraph.Top = 100;

            paragraph.UseCompatibleTextRendering = true;
            paragraph.Width = this.Width;
            paragraph.Font = new Font(fontParagraph.type.Families[0], 14, FontStyle.Regular);
            paragraph.TextAlign = ContentAlignment.MiddleCenter;
            paragraph.ForeColor = Color.White;

                esc.UseCompatibleTextRendering = true;
                esc.Font = new Font(fontParagraph.type.Families[0], 14, FontStyle.Regular);
                esc.Height = 40;
                esc.Top = this.Height - esc.Height * 2;
                esc.Width = 200;
                esc.TextAlign = ContentAlignment.MiddleCenter;
                esc.Left = 10;
                esc.Text = "Esc -> Back To Menu";
                esc.ForeColor = Color.White;

                this.Controls.Add(esc);
            this.Controls.Add(title);
            this.Controls.Add(paragraph);
            this.Visible = false;
            }
        }
    }

