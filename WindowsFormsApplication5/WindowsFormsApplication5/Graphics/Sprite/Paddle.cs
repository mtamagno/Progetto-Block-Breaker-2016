using System;
using System.Windows.Forms;
using System.Threading;

namespace BlockBreaker
{
    public class Paddle : Sprite
    {
        #region Fields

        public bool Hurt;

        #endregion Fields

        #region Constructors

        public Paddle(float x, float y, int width, int height, Logic logic)
        {
            if (logic == null) throw new ArgumentNullException(nameof(logic));
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
            var texture = Properties.Resources.New_Piskel;
            CanFall = false;
            CanCollide = true;
            ToRender = true;
            FollowPointer = true;
            Hurt = false;

            this.CreateSprite(texture, x, y, width, height);
            logic.IManager.inGameSprites.Add(this);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Funzione che restituisce l'Angolo con cui la pallina deve essere fatta rimbalzare, a seconda del punto di impatto sulla racchetta
        /// </summary>
        /// <param name="posizione_attuale"></param>
        /// <param name="posizione_massima"></param>
        /// <returns></returns>
        public double Angolo(float posizione_attuale, float posizione_massima)
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
                if (FollowPointer)
                {
                    if(thisform != null)
                    if ((Cursor.Position.X - thisform.Location.X) >= thisform.Width/11 && Cursor.Position.X - thisform.Location.X < thisform.Width - thisform.Width / 11)                    
                        this.X = Cursor.Position.X - thisform.Location.X - this.Width / 2 - this.Width/13;                                 
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
        public void OnHurt()
        {
            Texture = Properties.Resources.hurt;
            Hurt = true;                
            Thread.Sleep(600);
            this.Normalize();
        }

        /// <summary>
        /// Funzione che rende la racchetta "nomale" facendo tornare la texture uguale a quella dello stato prima dell'impatto
        /// </summary>
        public void Normalize()
        {
            Texture = Properties.Resources.New_Piskel;
            Thread.Sleep(50);
            Hurt = false;
        }
        #endregion Methods
    }
}