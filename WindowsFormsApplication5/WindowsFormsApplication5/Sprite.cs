using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication5
{
    class Sprite
    {

        public Bitmap Texture;
        public float X, Y;
        public int Width, Height;

       public Sprite(Bitmap texture,float x, float y, int width, int height)
        {
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawImage(texture,0,0,width,height);
            }

            Texture = b;
            X = x;
            Y = y;

            Width = width;
            Height = height;
        }

        public Rectangle toRec
        {
            get { return new Rectangle((int)X, (int)Y,Width,Height); }
        }

        public Rectangle Top
        {
            get { return new Rectangle((int)X, (int)Y, Width, Height/4); }
        }

        public Rectangle Bottom
        {
            get { return new Rectangle((int)X, (int)Y + this.Height/2 + this.Height/4, Width, Height); }
        }

        public Rectangle Left
        {
            get { return new Rectangle((int)X, (int)Y, Width/4, Height); }
        }

        public Rectangle Right
        {
            get { return new Rectangle((int)X + Width/2 + Width/4, (int)Y + this.Height / 2 + this.Height / 4, Width, Height); }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this);
        }

      
    }

    static class SpriteHelper
    {

    }
}
