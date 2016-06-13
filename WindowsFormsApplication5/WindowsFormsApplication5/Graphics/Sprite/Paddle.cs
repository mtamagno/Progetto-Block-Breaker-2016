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

        /// <summary>
        /// Funzione che restituisce l'angolo con cui la pallina deve essere fatta rimbalzare, a seconda del punto di impatto sulla racchetta
        /// </summary>
        /// <param name="posizione_attuale"></param>
        /// <param name="posizione_massima"></param>
        /// <returns></returns>
        public double angolo(float posizione_attuale, float posizione_massima)
        {
            double calcolo = 0;
            calcolo = (posizione_attuale / posizione_massima) * 75;
            calcolo = calcolo * Math.PI / 180;
            return calcolo;
        }

        /// <summary>
        /// Funzione Update che richiama il collider e si occupa di spostare le coordinate della racchetta di volta in volta
        /// </summary>
        /// <param name="iManager"></param>
        /// <param name="thisform"></param>
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

        /// <summary>
        /// Funzione che rende la racchetta "animata" cambiando la texture per un breve periodo di tempo ad ogni hit
        /// </summary>
        public void hurt()
        {
            Texture = Properties.Resources.hurt;
            hurted = true;                
            Thread.Sleep(600);
            this.normal();
        }

        /// <summary>
        /// Funzione che rende la racchetta "nomale" facendo tornare la texture uguale a quella dello stato prima dell'impatto
        /// </summary>
        public void normal()
        {
            Texture = Properties.Resources.New_Piskel;
            Thread.Sleep(50);
            hurted = false;
        }
        #endregion Methods
    }
}