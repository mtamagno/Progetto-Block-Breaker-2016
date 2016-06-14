﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.Xml;
using System.IO;


namespace WindowsFormsApplication5
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
                            Highscore_counter++;
                            break;

                        case XmlNodeType.EndElement:
                            Console.WriteLine("</" + Reader.Name + ">");
                            break;
                    }
                    if (Highscore_counter == 2)
                    {
                        Paragraph.Text += "\n\n";
                        Highscore_counter = 0;
                    }

                }

                Console.ReadLine();

                this.Left = Left;
                this.Top = Top;
                this.Width = Width;
                this.Height = Height;
                this.BackColor = Color.Black;

                fontParagraph = new MyFonts(MyFonts.FontType.paragraph);
                fontTitle = new MyFonts(MyFonts.FontType.Title);
                Title.UseCompatibleTextRendering = true;
                Title.Top = 30;
                Title.Text = "Highscores";
                Title.Height = 60;
                Title.Width = this.Width;
                Title.Font = new Font(fontTitle.type.Families[0], 30, FontStyle.Regular);
                Title.TextAlign = ContentAlignment.MiddleCenter;
                Title.ForeColor = Color.White;

                Paragraph.Height = 300;
                Paragraph.Top = 100;

                Paragraph.UseCompatibleTextRendering = true;
                Paragraph.Width = 200;
                Paragraph.Left = this.Width / 2 - Paragraph.Width / 2;
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
        }
    }
}