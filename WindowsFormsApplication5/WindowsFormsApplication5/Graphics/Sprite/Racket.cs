using BlockBreaker.Properties;
using System;
using System.Threading;
using System.Windows.Forms;

namespace BlockBreaker
{
    public class Racket : Sprite
    {
        #region Public Fields

        public bool Hurt;

        #endregion Public Fields

        #region Public Constructors

        public Racket(float x, float y, int width, int height, Logic logic)
        {
            if (logic == null) throw new ArgumentNullException(nameof(logic));
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
            var texture = Resources.New_Piskel;
            CanFall = false;
            CanCollide = true;
            ToRender = true;
            FollowPointer = true;
            Hurt = false;
            CreateSprite(texture, x, y, width, height);
            logic.MyIManager.InGameSprites.Add(this);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Funzione che restituisce l'Angolo con cui la pallina deve essere fatta rimbalzare, a seconda del punto di impatto
        /// sulla racchetta
        /// </summary>
        /// <param name="posizioneAttuale"></param>
        /// <param name="posizioneMassima"></param>
        /// <returns></returns>
        public double Angolo(float posizioneAttuale, float posizioneMassima)
        {
            double calcolo = 0;
            calcolo = posizioneAttuale / posizioneMassima * 75;
            calcolo = calcolo * Math.PI / 180;
            return calcolo;
        }

        /// <summary>
        /// Funzione che rende la racchetta "nomale" facendo tornare la texture uguale a quella dello stato prima dell'impatto
        /// </summary>
        public void Normalize()
        {
            Texture = Resources.New_Piskel;
            Thread.Sleep(50);
            Hurt = false;
        }

        /// <summary>
        /// Funzione che rende la racchetta "animata" cambiando la texture per un breve periodo di tempo ad ogni hit
        /// </summary>
        public void OnHurt()
        {
            Texture = Resources.hurt;
            Hurt = true;
            Thread.Sleep(600);
            Normalize();
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
                    if (thisform != null)
                        if (Cursor.Position.X - thisform.Location.X >= thisform.Width / 11 &&
                            Cursor.Position.X - thisform.Location.X < thisform.Width - thisform.Width / 11)
                            X = Cursor.Position.X - thisform.Location.X - Width / 2 - Width / 13;
                }
            }
            catch
            {
                // Errore gestito nel caso in cui followpointer non sia ancora stato impostato a false,
                // ma l'utente abbia chiuso il form container o lo abbia minimizzato
            }
        }

        #endregion Public Methods
    }
}
