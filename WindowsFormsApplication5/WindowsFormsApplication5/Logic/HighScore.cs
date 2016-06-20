using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace BlockBreaker
{
    public class HighScore
    {
        #region Properties

        public string Name { get; set; }

        public int Score { get; set; }

        #endregion Properties
    }

    public class HighScoreSaver
    {
        #region Methods

        /// <summary>
        /// Funzione che si occupa di aggiungere gli highScores e di riordinarli ogni volta in un file "HighScores.xml" che viene creato se non esiste
        /// </summary>
        /// <param name="CurrentHighScore"></param>
        public void ModifyOrCreateXML(HighScore CurrentHighScore)
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
                var orderedHighScores = xDocument.Descendants("HighScore").OrderByDescending(e => (int.Parse(e.Element("Score").Value)));
                root.ReplaceAll(orderedHighScores);
                xDocument.Save("HighScores.xml");
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

        #endregion Methods
    }
}