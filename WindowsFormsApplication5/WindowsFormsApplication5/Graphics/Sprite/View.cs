using System.Drawing;

namespace BlockBreaker
{
    public class Playground : Sprite
    {
        #region Fields

        public Bitmap texture;

        #endregion Fields

        #region Constructors

        public Playground(float x, float y, int width, int height, Logic logic)
        {
            //Imposta la texture e i vaolri di canfall toRender cancollide e followpointer
            this.texture = Properties.Resources.Schermo_800_600_GBA;
            this.canFall = false;
            this.toRender = true;
            this.canCollide = true;
            this.followPointer = false;

            this.CreateSprite(texture, x, y, width, height);
            logic.iManager.inGameSprites.Add(this);
        }

        #endregion Constructors
    }
}