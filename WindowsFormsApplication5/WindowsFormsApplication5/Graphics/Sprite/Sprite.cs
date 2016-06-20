using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public class Sprite
    {
        #region Fields

        public int BottomCollide = 0;
        public bool CanCollide;
        public bool CanFall;
        public bool FollowPointer;
        public Bitmap Texture;
        public bool ToRender;
        public int Width, Height;
        public float X, Y;

        #endregion Fields

        #region Properties

        /// <summary>
        ///     Proprietà che ritornano le coordinate delle parti degi sprite
        /// </summary>
        public Rectangle Bottom => new Rectangle((int) X, (int) Y + Height, Width, 20);

        public Rectangle Left => new Rectangle((int) X, (int) Y, 20, Height);

        public Rectangle Right => new Rectangle((int) X + Width, (int) Y, 20, Height);

        public Rectangle Top => new Rectangle((int) X, (int) Y, Width, 20);

        public Rectangle ToRec => new Rectangle((int) X, (int) Y, Width, Height);

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Funzione Draw che rimanda alla funzione DrawImageUnscaled all'interno di SpriteBatch
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this);
        }

        /// <summary>
        ///     Funzione necessaria alla creazione da bitmap degli sprite, e dei loro disegni
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void CreateSprite(Bitmap texture, float x, float y, int width, int height)
        {
            // Disegna il bitmap
            var b = new Bitmap(width, height);
            var g = Graphics.FromImage(b);
            g.DrawImage(texture, 0, 0, width, height);

            // Imposta tipo di sprite con relativi attributi e gli assegno le coordinate x e y
            Texture = b;
            X = x;
            Y = y;
            Width = width;
            Height = height;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        //Funzione Redraw necessaria ogni qual volta si effettua il resize dei vari sprite
        public void Redraw(Sprite sprite, int newWidth, int newheight, Bitmap risorsa, float nuova_X, float nuova_Y)
        {
            if (newWidth > 0 && newheight > 0)
            {
            sprite.Width = newWidth;
            sprite.Height = newheight;
                var b = new Bitmap(sprite.Width, sprite.Height);
                using (var g = Graphics.FromImage(b))
            {
                // Imposta a transparent lo sfondo dello sprite della pallina
                if (sprite.GetType().Name == "Ball")
                {
                        var backColor = risorsa.GetPixel(0, 0);
                    risorsa.MakeTransparent(backColor);
                }
                    // Ridisegna lo sprite
                    g.DrawImage(risorsa, 0, 0, sprite.Width, sprite.Height);
                }
            sprite.X = nuova_X;
            sprite.Y = nuova_Y;

                // Se il tipo di sprite è player, stiamo ridisegnando la racchetta, che mettiamo ad un altezza standard: 9/10 dell'altezza del form
                if (sprite.GetType().Name == "Paddle" && Form.ActiveForm != null)
                    sprite.Y = Math.Abs(Form.ActiveForm.ClientRectangle.Height - sprite.Height)*9/10;

            // Imposta la texture dello sprite
            sprite.Texture = b;
        }

        #endregion Methods
        }

        //Il collider fa un check di eventuali impatti tra sprites
    }

    /// <summary>
    ///     Classe SpriteHelper con le varie funzioni utili per sapere quando due sprite impattano
    ///     e in particolare quali sezioni di questi due si stanno intersecando
    /// </summary>
    internal static class SpriteHelper
    {
        #region Methods

        public static bool IsCollidingWith(this Sprite s1, Sprite s2)
        {
            return s1.ToRec.IntersectsWith(s2.ToRec);
        }

        public static bool IsTouchingBottom(this Sprite s1, Sprite s2)
        {
            return s1.Bottom.IntersectsWith(s2.Top);
        }

        public static bool IsTouchingLeft(this Sprite s1, Sprite s2)
        {
            return s1.Right.IntersectsWith(s2.Left);
        }

        public static bool IsTouchingRight(this Sprite s1, Sprite s2)
        {
            return s1.Left.IntersectsWith(s2.Right);
        }

        public static bool IsTouchingTop(this Sprite s1, Sprite s2)
        {
            return s1.Top.IntersectsWith(s2.Bottom);
        }

        #endregion Methods
    }
}