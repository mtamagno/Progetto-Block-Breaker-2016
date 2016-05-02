using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication5
{
    
    class SpriteBatch
    {
        public Graphics Gfx;
        public BufferedGraphics bfgfx; //Variabile per i buffered graphics
        public BufferedGraphicsContext cntxt = BufferedGraphicsManager.Current;
        

        public SpriteBatch(Size clientSize , Graphics gfx)
        {
            cntxt.MaximumBuffer = new Size(clientSize.Width + 1, clientSize.Height + 1);
            bfgfx = cntxt.Allocate(gfx, new Rectangle(Point.Empty, clientSize));
            Gfx = gfx;
        }

        public void Begin()
        {
            bfgfx.Graphics.Clear(Color.Transparent);
        }

        public void Draw(Sprite s)
        {
            bfgfx.Graphics.DrawImageUnscaled(s.Texture, s.toRec);
        }

        public void drawImage(Bitmap b,Rectangle rec)
        {
            bfgfx.Graphics.DrawImageUnscaled(b,rec);
        }

        public void End()
        {
            bfgfx.Render(Gfx);
        }


    }
}
