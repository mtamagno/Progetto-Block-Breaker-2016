using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public class SpriteBatch
    {
        #region Fields

        public BufferedGraphics bfgfx;
        //Variabile per i buffered CreateSprite
        public BufferedGraphicsContext cntxt = BufferedGraphicsManager.Current;

        public Graphics Gfx;

        #endregion Fields

        #region Constructors

        public SpriteBatch(Size clientSize, Graphics gfx)
        {
            cntxt.MaximumBuffer = new Size(clientSize.Width + 1, clientSize.Height + 1);
            bfgfx = cntxt.Allocate(gfx, new Rectangle(Point.Empty, clientSize));
            Gfx = gfx;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Funzione che pulisce bufferedgraphics
        /// </summary>
        public void Clear()
        {
            bfgfx.Graphics.Clear(Color.Transparent);
        }
        /// <summary>
        /// Funzione che gestisce l'animazione della racchetta e disegna gli sprites
        /// </summary>
        /// <param name="s"></param>
        public void Draw(Sprite s)
        {
                try
                {
                    if (s.GetType().Name == "Paddle")
                    {
                        Paddle mypaddle = (Paddle)s;
                        if (mypaddle.hurted == true)
                            s.CreateSprite(s.Texture, s.X, s.Y, s.Width, s.Height);
                    }
                    if (s.GetType().Name == "Ball")
                     {
                    Ball myBall = (Ball)s;
                    if (float.IsNaN(s.X))
                        s.X = myBall.previousX;
                    if (float.IsNaN(s.Y))
                        s.Y = myBall.previousY;
                    if (float.IsNaN(myBall.velocity.X))
                        myBall.velocity.X = myBall.previousVelocity.X;
                    if (float.IsNaN(myBall.velocity.Y))
                        myBall.velocity.Y = myBall.previousVelocity.Y;
                    if (float.IsNaN(myBall.velocityTot))
                        myBall.velocityTot = myBall.previousVelocityTot;
                }
               
                bfgfx.Graphics.DrawImageUnscaled(s.Texture, s.toRec);
                }
                catch
                {
                    // Errore gestito causato dal movimento della finestra che causa un errore nelle coordinate durante il ri Disegna
                }
            
        }

        /// <summary>
        /// Funzione che permette il disegno in bufferedGraphics di bitmap
        /// </summary>
        /// <param name="b"></param>
        /// <param name="rec"></param>
        public void drawImage(Bitmap b, Rectangle rec)
        {
            bfgfx.Graphics.DrawImageUnscaled(b, rec);
        }

        /// <summary>
        /// Funzione per la terminazione di bufferedGraphics
        /// </summary>
        public void End()
        {
            try
            {
                if(Form.ActiveForm != null && Gfx != null && (!float.IsNaN(Gfx.ClipBounds.Location.X)))
                    bfgfx.Render(Gfx);
            }
            catch (System.ArgumentException)
            {
                // Errore che può essere generato dalla chiusura tramite il tasto x del form container mentre si sta cercando di disegnare
                // basta non fare nulla e il form verrà chiuso
            }
        }

        #endregion Methods
    }
}