using System.Drawing;

namespace BlockBreaker
{
    public class Skin : Sprite
    {

        public Bitmap texture;

        public Skin(float x, float y, int width, int height, Logic logic)
        {
            texture = Properties.Resources.Skin;
            canFall = false;
            toRender = true;
            canCollide = true;
            followPointer = false;

            this.graphics(texture, x, y, width, height);
            logic.iManager.inGameSprites.Add(this);
        }
    }
}
