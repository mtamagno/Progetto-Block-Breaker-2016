using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public class Paddle : Sprite
    {
        #region Public Fields

        public Bitmap texture;

        #endregion Public Fields


        #region Public Constructors

        //       private static Random random = new Random();
        public Paddle(float x, float y, int width, int height, Logic logic) : base(x, y, width, height)
        {
            texture = Properties.Resources.New_Piskel;
            canFall = false;
            canCollide = true;
            torender = true;
            followPointer = true;

            this.graphics(texture, x, y, width, height);
            Paddle racchetta = this;
            logic.iManager.inGameSprites.Add(racchetta);
        }

        #endregion Public Constructors

        #region Public Methods

        //Funzione che restituisce l'angolo con cui la pallina deve essere fatta rimbalzare, a seconda del punto di impatto sulla racchetta
        public double angolo(float posizione_attuale, float posizione_massima)
        {
            double calcolo = 0;
            calcolo = (posizione_attuale / posizione_massima) * 90;
            calcolo = calcolo * Math.PI / 180;
            return calcolo;
        }

        public void Update(InputManager iManager, Form thisform)
        {
            try
            {
                if (followPointer)
                    if ((Cursor.Position.X - thisform.Location.X) >= 0 && Cursor.Position.X - thisform.Location.X < thisform.Width)
                        this.X = Cursor.Position.X - thisform.Location.X - this.Width / 2 - 10;
            }
            catch
            {
            }
        }

        #endregion Public Methods
    }
}