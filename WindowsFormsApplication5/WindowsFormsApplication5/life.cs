using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication5
{
    public class life : Sprite
    {
        public int remaining_bounces;
        Bitmap texture;

        //       private static Random random = new Random();
        public life(float x, float y, int width, int height) : base(x, y, width, height)
        {
            texture = Properties.Resources.vita;
            canFall = false;
            torender = true;
            canCollide = false;
            followPointer = false;

            this.graphics(texture, x, y, width, height);
        }
    }
}
