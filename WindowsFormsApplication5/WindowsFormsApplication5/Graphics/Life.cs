using System.Drawing;

namespace WindowsFormsApplication5
{
    public class Life : Sprite
    {
        #region Public Fields

        public int remaining_bounces;

        #endregion Public Fields

        #region Private Fields

        private Bitmap texture;

        #endregion Private Fields

        #region Public Constructors

        //       private static Random random = new Random();
        public Life(float x, float y, int width, int height) : base(x, y, width, height)
        {
            texture = Properties.Resources.vita;
            canFall = false;
            torender = true;
            canCollide = false;
            followPointer = false;

            this.graphics(texture, x, y, width, height);
        }

        #endregion Public Constructors
    }
}