using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Diagnostics;
namespace WindowsFormsApplication5
{
    class InputManager
    {
        public Point mousePoint;
        public Keys[] keyPressed;
        public Keys[] keyHeld;
        public Stopwatch gameTime;
        public float deltaTime;
        public List<Sprite> inGameSprites = new List<Sprite>();

        public void update(Point mp, Keys[] kp, Keys[] kh, Stopwatch gt, float dt)
        {
            mousePoint = mp;
            keyPressed = kp;
            keyHeld = kh;
            gameTime = gt;
            deltaTime = dt/1000;
        }
    }
}
