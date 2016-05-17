using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;

namespace WindowsFormsApplication5
{
    public class HighScore
    {
        public int Score
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }
    }

    public class HighScoreCollection : List<HighScore>
    {
        public void SaveToXml(HighScore CurrentHighScore)
        {
            XmlDocument Hiscore = new XmlDocument();
            try
            {
                Hiscore.Load("Hiscore.xml");

                //XmlNode root = doc.DocumentElement;

                ////Create a new node.
                //XmlElement elem = doc.CreateElement("price");
                //elem.InnerText = "19.95";

                ////Add the node to the document.
                //root.AppendChild(elem)
                //    writer.WriteStartElement("HighScore:\t");
                //writer.WriteValue(CurrentHighScore.Score + "\n");
                //writer.WriteString("Name:\t");
                //writer.WriteString(CurrentHighScore.Name + "\n");
                //writer.Flush();
                //writer.Close();
                Hiscore.Save(Console.Out);

            }
            catch
            {
                using (XmlWriter writer = XmlWriter.Create("Hiscore.xml"))
                {
                    writer.WriteStartDocument(true);
                    writer.WriteStartElement("HighScore:\t");
                    writer.WriteValue(CurrentHighScore.Score + "\n");
                    writer.WriteString("Name:\t");
                    writer.WriteString(CurrentHighScore.Name + "\n");
                    writer.Flush();
                    writer.Close();
                    Console.Out.Write(Hiscore);
                }
            }
        }
    }
}