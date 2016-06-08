using System.Drawing;

namespace WindowsFormsApplication5
{
    public class View : Sprite
    {
        #region Fields

        public Bitmap texture;

        #endregion Fields

        #region Constructors

        public View(float x, float y, int width, int height, Logic logic)
        {
            texture = Properties.Resources.Background;
            canFall = false;
            torender = true;
            canCollide = true;
            followPointer = false;

            this.graphics(texture, x, y, width, height);
            logic.iManager.inGameSprites.Add(this);
        }

        #endregion Constructors
    }
}