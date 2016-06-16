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
        MyFonts fontTitle;
        MyFonts fontParagraph;
        Label Title;
        Label Paragraph;
        Label Esc;
        XmlTextReader Reader;
        int Highscore_counter;

        public HighScoresPanel(int Left, int Top, int Width, int Height)
        {
            Esc = new Label();
            Title = new Label();
            Paragraph = new Label();

            if (File.Exists("HighScores.xml"))
            {
                Reader = new XmlTextReader("HighScores.xml");
                Highscore_counter = 0;
                for (int i = 0; Reader.Read() && i < 100; i++)
            {
                switch (Reader.NodeType)
                {

                    case XmlNodeType.Element:
                        Console.WriteLine("<" + Reader.Name + ">");
                        break;

                    case XmlNodeType.Text:
                        Paragraph.Text += Reader.Value + " - ";
                            Console.WriteLine(Reader.Value);
                            this.Highscore_counter++;
                        break;

                    case XmlNodeType.EndElement:
                        Console.WriteLine("</" + Reader.Name + ">");
                        break;
                }
                if (Highscore_counter == 2)
                {
                        this.Paragraph.Text += "\n\n";
                        this.Highscore_counter = 0;
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
            this.Title.UseCompatibleTextRendering = true;
            this.Title.Top = 30;
            this.Title.Text = "Highscores";
            this.Title.Height = 60;
            this.Title.Width = this.Width;
            this.Title.Font = new Font(fontTitle.type.Families[0], 30, FontStyle.Regular);
            this.Title.TextAlign = ContentAlignment.MiddleCenter;
            this.Title.ForeColor = Color.White;

            this.Paragraph.Height = 300;
            this.Paragraph.Top = 100;

            this.Paragraph.UseCompatibleTextRendering = true;
            this.Paragraph.Width = this.Width;
            this.Paragraph.Font = new Font(fontParagraph.type.Families[0], 14, FontStyle.Regular);
            this.Paragraph.TextAlign = ContentAlignment.MiddleCenter;
            this.Paragraph.ForeColor = Color.White;

                Esc.UseCompatibleTextRendering = true;
                Esc.Font = new Font(fontParagraph.type.Families[0], 14, FontStyle.Regular);
                Esc.Height = 40;
                Esc.Top = this.Height - Esc.Height * 2;
                Esc.Width = 200;
                Esc.TextAlign = ContentAlignment.MiddleCenter;
                Esc.Left = 10;
                Esc.Text = "Esc -> Back To Menu";
                Esc.ForeColor = Color.White;

                this.Controls.Add(Esc);
            this.Controls.Add(Title);
            this.Controls.Add(Paragraph);
            this.Visible = false;
            }
        }
    }

