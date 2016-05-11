using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication5
{
    class Block : Sprite
    {
        public int remaining_bounces;
              private static Random random = new Random();
        public Block(Bitmap texture, float x, float y, int width, int height) : base(texture, x, y, width, height)
        {
            //Random random = new Random();
            remaining_bounces = random.Next(0, 4);
            canFall = false;
            canCollide = true;
            if (remaining_bounces > 0)
                torender = true;
            followPointer = false;
            if (torender != true)
                canCollide = false;

        }

 
    }
}

