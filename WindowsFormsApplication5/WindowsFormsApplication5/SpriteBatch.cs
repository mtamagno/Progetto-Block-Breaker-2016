using System.Drawing;

namespace WindowsFormsApplication5
{
    internal class SpriteBatch
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
            /*Console.WriteLine(s.toRec);
            Console.WriteLine(s.Texture);
            Console.WriteLine(s.Type);*/
            bfgfx.Graphics.DrawImageUnscaled(s.Texture, s.toRec);
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