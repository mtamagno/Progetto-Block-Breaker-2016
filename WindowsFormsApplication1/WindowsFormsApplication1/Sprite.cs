using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication1
{
    class Sprite
    {
        public Bitmap Texture;
        public float X, Y;
        public int Width, Height;

        public Sprite(Bitmap texture, float x, float y, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawImage(texture, 0, 0, width, height);
            }
            Texture = b;
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }
    
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this);
        }
    }
}
