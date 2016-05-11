using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication5
{
    class View : Sprite
    {
        public View(Bitmap texture, float x, float y, int width, int height) : base(texture, x, y, width, height)
        {
            canFall = false;
            torender = true;
            canCollide = true;
            followPointer = false;

        }

        }
    }

