using System;
using System.Drawing;

namespace WindowsFormsApplication5
{
    public class Sprite
    {
        #region Fields

        public int bottomCollide = 0;
        public bool canCollide;
        public bool canFall;
        public bool followPointer;
        public Bitmap Texture;
        public bool toRender;
        public int Width, Height;
        public float X, Y;

        #endregion Fields

        #region Properties
        /// <summary>
        /// Proprietà che ritornano le coordinate delle parti degi sprite
        /// </summary>

        public Rectangle Bottom
        {
            get { return new Rectangle((int)X, (int)Y + this.Height, Width, 20); }
        }

        public Rectangle Left
        {
            get { return new Rectangle((int)X, (int)Y, 20, Height); }
        }

        public Rectangle Right
        {
            get { return new Rectangle((int)X + this.Width, (int)Y, 20, Height); }
        }

        public Rectangle Top
        {
            get { return new Rectangle((int)X, (int)Y, Width, 20); }
        }

        public Rectangle toRec
        {
            get { return new Rectangle((int)X, (int)Y, Width, Height); }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Funzione Draw che rimanda alla funzione DrawImageUnscaled all'interno di SpriteBatch
        /// </summary>
        /// <param name="sb"></param>
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this);
        }

        /// <summary>
        /// Funzione necessaria alla creazione da bitmap degli sprite, e dei loro disegni
        /// </summary>
        /// <param name="texture"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        public void graphics(Bitmap texture, float x, float y, int width, int height)
        {
            // Disegna il bitmap
            Bitmap b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(b);
            g.DrawImage(texture, 0, 0, width, height);

            // Imposta tipo di sprite con relativi attributi e gli assegno le coordinate x e y
            Texture = b;
            X = x;
            Y = y;
            Width = width;
            Height = height;

            GC.Collect();
            GC.WaitForPendingFinalizers();
            return;
        }

        //Funzione redraw necessaria ogni qual volta si effettua il resize dei vari sprite
        public void redraw(Sprite sprite, int newWidth, int newHeigth, Bitmap risorsa, float nuova_X, float nuova_Y)
        {
            if (newWidth > 0 && newHeigth > 0)
            {
            sprite.Width = newWidth;
            sprite.Height = newHeigth;
            Bitmap b = new Bitmap(sprite.Width, sprite.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                // Imposta a transparent lo sfondo dello sprite della pallina
                if (sprite.GetType().Name == "Ball")
                {
                    Color backColor = risorsa.GetPixel(0, 0);
                    risorsa.MakeTransparent(backColor);
                }
                try
                {
                    g.DrawImage(risorsa, 0, 0, sprite.Width, sprite.Height);
                }
                catch
                {
                    g.DrawImage(risorsa, 0, 0, sprite.Width, sprite.Height);

                    // Errore gestito causato dal movimento della finestra che causa un errore nelle coordinate durante il ri Disegna
                }
            }
            sprite.X = nuova_X;
            sprite.Y = nuova_Y;

            try
            {
                // Se il tipo di sprite è player, stiamo ridisegnando la racchetta, che mettiamo ad un altezza standard: 9/10 dell'altezza del form
                    if (sprite.GetType().Name == "Paddle" && Container.ActiveForm != null)
                    sprite.Y = (Math.Abs(Container.ActiveForm.ClientRectangle.Height - sprite.Height)) * 9 / 10;
            }
            catch
            {
                // Errore gestito nel caso in cui si stia ridimensionando il form e venga variata di conseguenza l'altezza della racchetta
            }

            // Imposta la texture dello sprite
            sprite.Texture = b;
            return;
        }

        #endregion Methods
        }
        //Il collider fa un check di eventuali impatti tra sprites
    }

    /// <summary>
    /// Classe SpriteHelper con le varie funzioni utili per sapere quando due sprite impattano
    /// e in particolare quali sezioni di questi due si stanno intersecando
    /// </summary>
    internal static class SpriteHelper
    {
        #region Methods

        public static bool isCollidingWith(this Sprite s1, Sprite s2)
        {
            if (s1.toRec.IntersectsWith(s2.toRec))
                return true;
            else
                return false;
        }

        public static bool isOnStage(this Sprite s1, Rectangle clientRec)
        {
            if (s1.toRec.IntersectsWith(clientRec))
                return true;
            else
                return false;
        }

        public static bool isTouchingBottom(this Sprite s1, Sprite s2)
        {
            if (s1.Bottom.IntersectsWith(s2.Top))
                return true;
            else
                return false;
        }

        public static bool isTouchingLeft(this Sprite s1, Sprite s2)
        {
            if (s1.Right.IntersectsWith(s2.Left))
                return true;
            else
                return false;
        }

        public static bool isTouchingRight(this Sprite s1, Sprite s2)
        {
            if (s1.Left.IntersectsWith(s2.Right))
                return true;
            else
                return false;
        }

        public static bool isTouchingTop(this Sprite s1, Sprite s2)
        {
            if (s1.Top.IntersectsWith(s2.Bottom))
                return true;
            else
                return false;
        }

        #endregion Methods
    }
}