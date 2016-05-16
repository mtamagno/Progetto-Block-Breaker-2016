using System.Drawing;

namespace WindowsFormsApplication5
{
    public class Life : Sprite
    {
        #region Private Fields

        private Bitmap texture;

        #endregion Private Fields

        #region Public Constructors

        public Life(float x, float y, int width, int height) : base(x, y, width, height)
        {
            //setto le variabili standard della vita alla creazione
            texture = Properties.Resources.vita;
            canFall = false;
            torender = true;
            canCollide = false;
            followPointer = false;

            //disegno la vita
            this.graphics(texture, x, y, width, height);
        }

        #endregion Public Constructors
    }
}