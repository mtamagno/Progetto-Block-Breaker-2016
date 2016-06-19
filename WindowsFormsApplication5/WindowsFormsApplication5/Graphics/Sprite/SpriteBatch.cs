using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public class SpriteBatch
    {
        #region Constructors

        public SpriteBatch(Size clientSize, Graphics gfx)
        {
            if (gfx == null) throw new ArgumentNullException(nameof(gfx));
            Cntxt.MaximumBuffer = new Size(clientSize.Width + 1, clientSize.Height + 1);
            Bfgfx = Cntxt.Allocate(gfx, new Rectangle(Point.Empty, clientSize));
            Gfx = gfx;
        }

        #endregion Constructors

        #region Fields

        public BufferedGraphics Bfgfx;
        //Variabile per i buffered CreateSprite
        public BufferedGraphicsContext Cntxt = BufferedGraphicsManager.Current;

        public Graphics Gfx;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Funzione che pulisce bufferedgraphics
        /// </summary>
        public void Clear()
        {
            Bfgfx.Graphics.Clear(Color.Transparent);
        }

        /// <summary>
        ///     Funzione che gestisce l'animazione della racchetta e disegna gli sprites
        /// </summary>
        /// <param name="s"></param>
        public void Draw(Sprite s)
        {
            try
            {
                if (s.GetType().Name == "Paddle")
                {
                    var mypaddle = (Paddle) s;
                    if (mypaddle.Hurt)
                        s.CreateSprite(s.Texture, s.X, s.Y, s.Width, s.Height);
                }
                if (s.GetType().Name == "Ball")
                {
                    var myBall = (Ball) s;
                    if (float.IsNaN(s.X))
                        s.X = myBall.PreviousX;
                    if (float.IsNaN(s.Y))
                        s.Y = myBall.PreviousY;
                    if (float.IsNaN(myBall.VelocityTot))
                        myBall.VelocityTot = myBall.PreviousVelocityTot;
                    if (float.IsNaN(myBall.Velocity.X))
                        myBall.Velocity.X = myBall.PreviousVelocity.X;
                    if (float.IsNaN(myBall.Velocity.Y))
                        myBall.Velocity.Y = myBall.PreviousVelocity.Y;
                }

                Bfgfx.Graphics.DrawImageUnscaled(s.Texture, s.ToRec);
            }
            catch
            {
                // Errore gestito causato dal movimento della finestra che causa un errore nelle coordinate durante il ri Disegna
            }
        }

        /// <summary>
        ///     Funzione per la terminazione di bufferedGraphics
        /// </summary>
        public void End()
        {
            try
            {
                if (Form.ActiveForm != null && Gfx != null && !float.IsNaN(Gfx.ClipBounds.Location.X))
                    Bfgfx.Render(Gfx);
            }
            catch (ArgumentException)
            {
                // Errore che può essere generato dalla chiusura tramite il tasto x del form container mentre si sta cercando di disegnare
                // basta non fare nulla e attendere, dato che il form verrà chiuso dopo il dispose dei vari elementi ancora valorizzati.
            }
        }

        #endregion Methods
    }
}