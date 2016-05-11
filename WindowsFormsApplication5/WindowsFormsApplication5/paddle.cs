using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication5
{
    class paddle : Sprite
    {
        //       private static Random random = new Random();
        public paddle(Bitmap texture, float x, float y, int width, int height) : base(texture, x, y, width, height)
        {
            canFall = false;
            canCollide = true;
            torender = true;
            followPointer = true;
        }

        public void Update(InputManager iManager)
        {
            this.X = Form2.MousePosition.X;
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
