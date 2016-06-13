using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication5
{
    public class Paddle : Sprite
    {
        #region Fields

        public Bitmap texture;
        public bool hurted;


        #endregion Fields

        #region Constructors

        //       private static Random random = new Random();
        public Paddle(float x, float y, int width, int height, Logic logic)
        {
            texture = Properties.Resources.New_Piskel;
            canFall = false;
            canCollide = true;
            toRender = true;
            followPointer = true;
            hurted = false;

            this.graphics(texture, x, y, width, height);
            logic.iManager.inGameSprites.Add(this);
        }

        #endregion Constructors

        #region Methods

        //Funzione che restituisce l'angolo con cui la pallina deve essere fatta rimbalzare, a seconda del punto di impatto sulla racchetta
        public double angolo(float posizione_attuale, float posizione_massima)
        {
            double calcolo = 0;
            calcolo = (posizione_attuale / posizione_massima) * 75;
            calcolo = calcolo * Math.PI / 180;
            return calcolo;
        }

        public void Update(InputManager iManager, Form thisform)
        {
            try
            {
                if (followPointer)
                {
                    if ((Cursor.Position.X - thisform.Location.X) >= 0 && Cursor.Position.X - thisform.Location.X < thisform.Width)                    
                        this.X = Cursor.Position.X - thisform.Location.X - this.Width / 2 - 10;                                 
                }
            }
            catch
            {
                // Errore gestito nel caso in cui followpointer non sia ancora stato impostato a false,
                // ma l'utente abbia chiuso il form container o lo abbia minimizzato
            }
        }

        public void hurt()
        {
            Texture = Properties.Resources.hurt;
            hurted = true;                
            Thread.Sleep(600);
            this.normal();
        }

        public void normal()
        {
            Texture = Properties.Resources.New_Piskel;
            Thread.Sleep(50);
            hurted = false;
        }
        #endregion Methods
    }
}