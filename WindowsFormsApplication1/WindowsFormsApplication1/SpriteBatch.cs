using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication1
{
    class SpriteBatch
    {
        private Graphics Gfx;
        private BufferedGraphics bfgfx;
        private BufferedGraphicsContext cntxt = BufferedGraphicsManager.Current;

        public SpriteBatch(Size clientsize, Graphics gfx)
        {
            cntxt.MaximumBuffer = new Size(clientsize.Width + 1, clientsize.Height + 1);
            bfgfx = cntxt.Allocate(gfx, new Rectangle(Point.Empty, clientsize));
            Gfx = gfx;
        }

        public void begin()
        {
            bfgfx.Graphics.Clear(Color.Black);
        }

        public void Draw(Sprite s)
        {
            bfgfx.Graphics.DrawImageUnscaled(s.Texture, (int)s.X, (int)s.Y, s.Width, s.Height);
        }

        public void drawImage(Bitmap b, Rectangle rec)
        {
            bfgfx.Graphics.DrawImageUnscaled(b, rec);    
        }

        public void End()
        {
            bfgfx.Render(Gfx);
        }
    }
}
