using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication5
{
    class life : Sprite
    {
        public int remaining_bounces;
        //       private static Random random = new Random();
        public life(Bitmap texture, float x, float y, int width, int height) : base(texture, x, y, width, height)
        {
            canFall = false;
            torender = true;
            canCollide = false;
            followPointer = false;
        }
    }
}
