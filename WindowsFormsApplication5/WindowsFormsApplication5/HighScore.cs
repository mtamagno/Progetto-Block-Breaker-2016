using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace WindowsFormsApplication5
{
    public class HighScore
    {
        public int Score { get; set; }

        public string Name { get; set; }
    }

    public class HighScoreCollection : List<HighScore>
    {
        public void SaveToXml(HighScore CurrentHighScore)
        {
            if (File.Exists("HighScores.xml"))
            {
                XDocument xDocument = XDocument.Load("HighScores.xml");
                IEnumerable<XElement> rows;
                XElement root = xDocument.Element("HighScores");
                rows = root.Descendants("HighScore");
                XElement lastrow = rows.Last();
                lastrow.AddAfterSelf(
                new XElement("HighScore",
                new XElement("Name", CurrentHighScore.Name),
                new XElement("Score", CurrentHighScore.Score)));

                // Crea la lista giusta e salvo
                var ciao = xDocument.Descendants("HighScore").OrderByDescending(e => (int.Parse(e.Element("Score").Value)));
                root.ReplaceAll(ciao);
                xDocument.Save("HighScores.xml");

                // Da creare un pulsante "Apri HighScores"
            }
            else
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                using (XmlWriter writer = XmlWriter.Create("HighScores.xml", settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("HighScores");
                    writer.WriteStartElement("HighScore");

                    // Crea un nuovo nodo
                    writer.WriteElementString("Name", CurrentHighScore.Name);
                    writer.WriteElementString("Score", CurrentHighScore.Score.ToString());
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                }
            }
        }
    }
}