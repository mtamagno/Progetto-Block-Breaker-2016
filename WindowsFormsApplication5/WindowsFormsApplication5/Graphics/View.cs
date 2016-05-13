using System.Drawing;

namespace WindowsFormsApplication5
{
    public class View : Sprite
    {
        #region Public Fields

        public Bitmap texture;

        #endregion Public Fields

        #region Public Constructors

        public View(float x, float y, int width, int height, Logic logic) : base(x, y, width, height)
        {
            texture = Properties.Resources.Background;
            canFall = false;
            torender = true;
            canCollide = true;
            followPointer = false;

            this.graphics(texture, x, y, width, height);
            View background = this;
            logic.iManager.inGameSprites.Add(background);
        }

        #endregion Public Constructors
    }
}