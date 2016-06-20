using BlockBreaker.Properties;
using System;

namespace BlockBreaker
{
    public class Playground : Sprite
    {
        #region Public Constructors

        public Playground(float x, float y, int width, int height, Logic logic)
        {
            if (logic == null) throw new ArgumentNullException(nameof(logic));
            if (x < 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0) throw new ArgumentOutOfRangeException(nameof(y));
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

            //Imposta la texture e i vaolri di canfall toRender cancollide e followpointer
            var texture = Resources.Schermo_800_600_GBA;
            CanFall = false;
            ToRender = true;
            CanCollide = true;
            FollowPointer = false;
            CreateSprite(texture, x, y, width, height);
            logic.IManager.InGameSprites.Add(this);
        }

        #endregion Public Constructors
    }
}
