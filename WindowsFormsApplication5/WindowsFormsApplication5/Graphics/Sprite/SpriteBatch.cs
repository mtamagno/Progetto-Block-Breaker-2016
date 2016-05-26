﻿using System.Drawing;

namespace WindowsFormsApplication5
{
    public class SpriteBatch
    {
        #region Public Fields

        public BufferedGraphics bfgfx;

        //Variabile per i buffered graphics
        public BufferedGraphicsContext cntxt = BufferedGraphicsManager.Current;

        public Graphics Gfx;

        #endregion Public Fields

        #region Public Constructors

        public SpriteBatch(Size clientSize, Graphics gfx)
        {
            cntxt.MaximumBuffer = new Size(clientSize.Width + 1, clientSize.Height + 1);
            bfgfx = cntxt.Allocate(gfx, new Rectangle(Point.Empty, clientSize));
            Gfx = gfx;
        }

        #endregion Public Constructors

        #region Public Methods

        public void Begin()
        {
            bfgfx.Graphics.Clear(Color.Transparent);
        }

        public void Draw(Sprite s)
        {
            try
            {
                bfgfx.Graphics.DrawImageUnscaled(s.Texture, s.toRec);
            }
            catch
            {
                // Errore gestito causato dal movimento della finestra che causa un errore nelle coordinate durante il ridisegno
            }
        }

        public void drawImage(Bitmap b, Rectangle rec)
        {
            bfgfx.Graphics.DrawImageUnscaled(b, rec);
        }

        public void End()
        {
            bfgfx.Render(Gfx);
        }

        #endregion Public Methods
    }
}