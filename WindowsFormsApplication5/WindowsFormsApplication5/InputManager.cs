using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    internal class InputManager
    {
        #region Public Fields

        public float deltaTime;
        public Stopwatch gameTime;
        public List<Sprite> inGameSprites = new List<Sprite>();
        public Keys[] keyHeld;
        public Keys[] keyPressed;
        public Point mousePoint;

        #endregion Public Fields



        #region Public Methods

        public void update(Point mp, Keys[] kp, Keys[] kh, Stopwatch gt, float dt)
        {
            mousePoint = mp;
            keyPressed = kp;
            keyHeld = kh;
            gameTime = gt;
            deltaTime = dt / 1000f;
        }

        #endregion Public Methods
    }
}