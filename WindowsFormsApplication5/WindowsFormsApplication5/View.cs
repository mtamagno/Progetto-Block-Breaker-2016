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
        public Bitmap texture;

        public View( float x, float y, int width, int height) : base(x, y, width, height)
        {
            texture = Properties.Resources.Background;
            canFall = false;
            torender = true;
            canCollide = true;
            followPointer = false;

            this.graphics(texture, x, y, width, height);
        }

        }
    }

