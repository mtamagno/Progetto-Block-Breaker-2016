using System;
using System.Drawing;

namespace BlockBreaker
{
    public class Playground : Sprite
    {
        #region Fields

  

        #endregion Fields

        #region Constructors

        public Playground(float x, float y, int width, int height, Logic logic)
        {
            if (logic == null) throw new ArgumentNullException(nameof(logic));
            if (x < 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0) throw new ArgumentOutOfRangeException(nameof(y));
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

            //Imposta la texture e i vaolri di canfall toRender cancollide e followpointer
            var texture = Properties.Resources.Schermo_800_600_GBA;
            this.CanFall = false;
            this.ToRender = true;
            this.CanCollide = true;
            this.FollowPointer = false;

            this.CreateSprite(texture, x, y, width, height);
            logic.IManager.inGameSprites.Add(this);
        }

        #endregion Constructors
    }
}