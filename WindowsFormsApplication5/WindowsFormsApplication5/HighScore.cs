using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

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
            XmlDocument Hiscore = new XmlDocument();
            try
            {
                Hiscore.Load("Hiscore.xml");
                var numero_punteggi = Hiscore.DocumentElement.ChildNodes.Count;
                // Crea un nuovo elemento
                XmlElement elem = Hiscore.CreateElement("HighScore-" + (numero_punteggi++));
                elem.InnerText = CurrentHighScore.Name;
                elem.Value = CurrentHighScore.Score.ToString();
                //Aggiunge il nodo al documento
                Hiscore.DocumentElement.AppendChild(elem);
                Hiscore.Save("Hiscore.xml");
                Console.Out.Write(Hiscore);
            }
            catch
            {
                MemoryStream strm = new MemoryStream();
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.Indent = true;
                using (XmlWriter writer = XmlWriter.Create(strm, settings))
                {
                    writer.WriteStartDocument();

                    // Crea un nuovo nodo
                    writer.WriteElementString(CurrentHighScore.Name, CurrentHighScore.Score.ToString());
                    writer.Flush();
                    writer.Close();
                    writer.Dispose();
                    Console.Out.Write(strm);
                }
            }
        }
    }
}