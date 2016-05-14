using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;
using System.Data;

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
        public void SaveToXml(string fileName)
        {
            XmlDocument Hiscore = new XmlDocument();
            Hiscore.Load("Hiscore.xml");
            using (XmlWriter writer = XmlWriter.Create("fileName"))
            {
                /*XmlSerializer ser = new XmlSerializer(typeof(HighScoreCollection));
                ser.Serialize(writer, this);*/
                writer.WriteStartDocument();
                writer.WriteStartElement("Highscore");
                writer.WriteElementString("nome", "Marco");
                writer.Flush();
                writer.Close();
            }
        }
    }
}
