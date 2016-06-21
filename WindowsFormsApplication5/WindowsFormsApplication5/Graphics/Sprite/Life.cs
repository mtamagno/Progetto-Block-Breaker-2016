using BlockBreaker.Properties;
using System;

namespace BlockBreaker
{
    public class Life : Sprite
    {
        #region Public Constructors

        public Life(float x, float y, int width, int height)
        {
            if (x <= 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y <= 0) throw new ArgumentOutOfRangeException(nameof(y));
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

            // Imposta le variabili standard della vita alla creazione
            var texture = Resources.Life;
            CanFall = false;
            ToRender = true;
            CanCollide = false;
            FollowPointer = false;

            // Disegna la vita
            CreateSprite(texture, x, y, width, height);
        }

        #endregion Public Constructors
    }
}
