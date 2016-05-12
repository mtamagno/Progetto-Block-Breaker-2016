using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;

namespace WindowsFormsApplication5
{
   public class paddle : Sprite
    {
        public Bitmap texture;
        //       private static Random random = new Random();
        public paddle( float x, float y, int width, int height) : base(x, y, width, height)
        {
            texture = Properties.Resources.New_Piskel;
            canFall = false;
            canCollide = true;
            torender = true;
            followPointer = true;

            this.graphics(texture, x, y, width, height);
        }

        public void Update(InputManager iManager, Form thisform)
        {
            try {
                //activeForm.Cursor.HotSpot.X;
                if ((Cursor.Position.X - thisform.Location.X) >= 0 && Cursor.Position.X - thisform.Location.X < thisform.Width)
                    this.X = Cursor.Position.X - thisform.Location.X -this.Width / 2 - 10;
            }
            catch 
            {

            }

        }


        //Funzione che restituisce l'angolo con cui la pallina deve essere fatta rimbalzare, a seconda del punto di impatto sulla racchetta
        public double angolo(float posizione_attuale, float posizione_massima)
        {
            double calcolo = 0;
            calcolo = (posizione_attuale / posizione_massima) * 90;
            calcolo = calcolo * Math.PI / 180;
            return calcolo;
        }

    }
}
