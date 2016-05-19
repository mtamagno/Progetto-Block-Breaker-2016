using System;
using System.Collections.Generic;
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
                Hiscore.LoadXml("Hiscore.xml");

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
                using (XmlWriter writer = XmlWriter.Create("Hiscore.xml"))
                {
                    writer.WriteStartDocument(true);

                    // Crea un nuovo nodo
                    XmlElement elem = Hiscore.CreateElement("HighScore-1");
                    XmlElement elem2 = Hiscore.CreateElement("Score");
                    elem.InnerText = CurrentHighScore.Name;

                    elem2.InnerText = CurrentHighScore.Score.ToString();
                    //Aggiunge il nodo al documento
                    Hiscore.AppendChild(elem);
                    XmlNode root = Hiscore.DocumentElement;
                    root.AppendChild(elem2);
                    Console.Out.Write(Hiscore);
                }
            }
        }
    }
}