using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public class InputManager
    {
        #region Public Fields

        public List<Sprite> InGameSprites = new List<Sprite>();
        public Keys[] KeyHeld;
        public Keys[] KeyPressed;

        #endregion Public Fields

        #region Public Methods

        public void update(Keys[] kp, Keys[] kh)
        {
            KeyPressed = kp;
            KeyHeld = kh;
        }

        #endregion Public Methods
    }
}
