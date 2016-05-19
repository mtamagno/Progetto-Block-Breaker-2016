using System;
using System.Drawing;

namespace WindowsFormsApplication5
{
    public class Sprite
    {
        #region Public Fields

        public int bottom_collide = 0;
        public bool canCollide;
        public bool canFall;
        public bool followPointer;
        public Bitmap Texture;
        public bool torender;
        public int Width, Height;
        public float X, Y;

        #endregion Public Fields

        #region Public Constructors

        //Metodo sprite per la creazione di ogni sprite
        public Sprite(float x, float y, int width, int height)
        {
        }

        public void graphics(Bitmap texture, float x, float y, int width, int height)
        {
            //Disegno il bitmap
            Bitmap b = new Bitmap(width, height);
            Graphics g = Graphics.FromImage(b);
            g.DrawImage(texture, 0, 0, width, height);

            //imposto tipo di sprite con relativi attributi e gli assegno le coordinate x e y
            Texture = b;
            X = x;
            Y = y;

            Width = width;
            Height = height;

            return;
        }

        #endregion Public Constructors

        #region Public Enums

        // public enum SpriteType { player, ball, block, view, life };

        #endregion Public Enums

        #region Public Properties

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

        #endregion Public Properties

        #region Public Methods

        //Funzione Draw che rimanda alla funzione DrawImageUnscaled all'interno di SpriteBatch
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this);
        }

        //Funzione redraw necessaria ogni qual volta si effettua il resize dei vari sprite
        public void redraw(Sprite sprite, int new_Width, int new_Heigth, Bitmap risorsa, float nuova_X, float nuova_Y)
        {
            sprite.Width = new_Width;
            sprite.Height = new_Heigth;
            Bitmap b = new Bitmap(sprite.Width, sprite.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                //imposto a transparent lo sfondo dello sprite della pallina
                if (sprite.GetType().Name == "Ball")
                {
                    Color backColor = risorsa.GetPixel(0, 0);
                    risorsa.MakeTransparent(backColor);
                }
                g.DrawImage(risorsa, 0, 0, sprite.Width, sprite.Height);
            }
            sprite.X = nuova_X;
            sprite.Y = nuova_Y;

            //Se il tipo di sprite è player, stiamo ridisegnando la racchetta, che mettiamo ad un altezza standard: 9/10 dell'altezza del form
            if (sprite.GetType().Name == "Paddle")
                sprite.Y = (Math.Abs(Container.ActiveForm.Height - sprite.Height)) * 9 / 10;

            //imposto la texture dello sprite
            sprite.Texture = b;
            return;
        }

        #endregion Public Methods

        #region Private Methods

        //Il collider fa un check di eventuali impatti tra sprites

        #endregion Private Methods
    }

    /// <summary>
    /// Classe SpriteHelper con le varie funzioni utili per sapere quando due sprite impattano
    /// e in particolare quali sezioni di questi due si stanno intersecando
    /// </summary>
    internal static class SpriteHelper
    {
        #region Public Methods

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

        #endregion Public Methods
    }
}