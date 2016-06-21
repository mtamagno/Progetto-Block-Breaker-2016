using BlockBreaker.Properties;
using System;

namespace BlockBreaker
{
    public class Skin : Sprite
    {
        #region Public Constructors

        public Skin(float x, float y, int width, int height, Logic logic)
        {
            if (logic == null) throw new ArgumentNullException(nameof(logic));
            if (x < 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y < 0) throw new ArgumentOutOfRangeException(nameof(y));
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
            var texture = Resources.Skin;
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
