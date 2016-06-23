using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace BlockBreaker
{
    public class HighScore
    {
        #region Public Properties

        public string Name { get; set; }
        public int Score { get; set; }

        #endregion Public Properties
    }

    public class HighScoreSaver
    {
        #region Public Methods

        /// <summary>
        /// Funzione che si occupa di aggiungere gli highScores e di riordinarli ogni volta in un file "HighScores.xml" che
        /// viene creato se non esiste
        /// </summary>
        /// <param name="currentHighScore"></param>
        public void ModifyOrCreateXml(HighScore currentHighScore)
        {
            if (File.Exists("HighScores.xml"))
            {
                var xDocument = XDocument.Load("HighScores.xml");
                IEnumerable<XElement> rows;
                var root = xDocument.Element("HighScores");
                rows = root.Descendants("HighScore");
                var lastrow = rows.Last();
                lastrow.AddAfterSelf(
                new XElement("HighScore",
                        new XElement("Name", currentHighScore.Name),
                        new XElement("MyScore", currentHighScore.Score)));

                // Crea la lista giusta e salvo
                var orderedHighScores = xDocument.Descendants("HighScore").OrderByDescending(e => (int.Parse(e.Element("MyScore").Value)));
                root.ReplaceAll(orderedHighScores);
                xDocument.Save("HighScores.xml");
            }
            else
            {
                var settings = new XmlWriterSettings();
                settings.Indent = true;
                settings.NewLineOnAttributes = true;
                using (var writer = XmlWriter.Create("HighScores.xml", settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("HighScores");
                    writer.WriteStartElement("HighScore");

                    // Crea un nuovo nodo
                    writer.WriteElementString("Name", currentHighScore.Name);
                    writer.WriteElementString("MyScore", currentHighScore.Score.ToString());
                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                }
            }
        }

        #endregion Public Methods
    }
}
