using System.Collections.Generic;
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

        public void Update(Keys[] kp, Keys[] kh)
        {
            KeyPressed = kp;
            KeyHeld = kh;
        }

        #endregion Public Methods
    }
}
