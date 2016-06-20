using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public class InputManager
    {
        #region Public Fields

        public float DeltaTime;
        public Stopwatch GameTime;
        public List<Sprite> InGameSprites = new List<Sprite>();
        public Keys[] KeyHeld;
        public Keys[] KeyPressed;
        public Point MousePoint;

        #endregion Public Fields

        #region Public Methods

        public void update(Point mp, Keys[] kp, Keys[] kh, Stopwatch gt, float dt)
        {
            MousePoint = mp;
            KeyPressed = kp;
            KeyHeld = kh;
            GameTime = gt;
            DeltaTime = dt / 1000f;
        }

        #endregion Public Methods
    }
}
