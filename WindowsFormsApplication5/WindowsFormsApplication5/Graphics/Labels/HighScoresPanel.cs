using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace BlockBreaker
{
    internal sealed class HighScoresPanel : Panel
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
        public HighScoresPanel(int left, int top, int width, int height)
        {
            var esc = new Label();
            var title = new Label();
            var paragraph = new Label();
            if (File.Exists("HighScores.xml"))
            {
                var Reader = new XmlTextReader("HighScores.xml");
                var highscoreCounter = 0;
                for (var i = 0; Reader.Read() && i < 100; i++)
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
            Left = left;
            Top = top;
            Width = width;
            Height = height;
            BackColor = Color.Black;
            _fontParagraph = new MyFonts(MyFonts.FontType.Paragraph);
            _fontTitle = new MyFonts(MyFonts.FontType.Title);
            title.UseCompatibleTextRendering = true;
            title.Top = 30;
            title.Text = "Highscores";
            title.Height = 60;
            title.Width = Width;
            title.Font = new Font(_fontTitle.Type.Families[0], 30, FontStyle.Regular);
            title.TextAlign = ContentAlignment.MiddleCenter;
            title.ForeColor = Color.White;
            paragraph.Height = 300;
            paragraph.Top = 100;
            paragraph.UseCompatibleTextRendering = true;
            paragraph.Width = Width;
            paragraph.Font = new Font(_fontParagraph.Type.Families[0], 14, FontStyle.Regular);
            paragraph.TextAlign = ContentAlignment.MiddleCenter;
            paragraph.ForeColor = Color.White;
            esc.UseCompatibleTextRendering = true;
            esc.Font = new Font(_fontParagraph.Type.Families[0], 14, FontStyle.Regular);
            esc.Height = 40;
            esc.Top = Height - esc.Height * 2;
            esc.Width = 200;
            esc.TextAlign = ContentAlignment.MiddleCenter;
            esc.Left = 10;
            esc.Text = "Esc -> Back To Menu";
            esc.ForeColor = Color.White;
            Controls.Add(esc);
            Controls.Add(title);
            Controls.Add(paragraph);
            Visible = false;
        }

        #endregion Public Constructors
    }
}
