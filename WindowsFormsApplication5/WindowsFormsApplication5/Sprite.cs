using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    class Sprite
    {
        public Bitmap Texture; 
        public float X, Y;
        public int Width, Height;
        public PointF velocity;
        public int Accel_y = 2;  
        public SpriteType Type;
        public enum SpriteType {player , ball , block, view};
        public bool canFall;
        public bool canCollide;
        public bool followPointer;
        public bool torender;
        public int remaining_bounces;
        public int velocity_tot_raggiunto;
        public float velocity_tot;
        static Random random = new Random();
        //Metodo sprite per la creazione di ogni sprite
        public Sprite(Bitmap texture,float x, float y, int width, int height, SpriteType thisType)
        {
            //Disegno il bitmap
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                //per adesso è "thisType == spritetype.ball" ma una volta cambiati gli sprite sarà "thisType != spritetype.background"
                if (thisType == SpriteType.ball)
                {
                    Color backColor = texture.GetPixel(0, 0);
                    texture.MakeTransparent(backColor);
                }
                g.DrawImage(texture,0,0,width,height);
            }

            //Setto tipo di sprite con relativi attributi e gli assegno le coordinate x e y
            Texture = b;
            X = x;
            Y = y;

            Width = width;
            Height = height;

            Type = thisType;

            switch (thisType)
            {
                case SpriteType.ball:
                    canFall = true;
                    canCollide = true;
                    followPointer = false;
                    torender = true;
                    break;

                case SpriteType.block:
                    remaining_bounces = random.Next(0, 4);
                    canFall = false;
                    canCollide = true;
                    if(remaining_bounces > 0)
                    torender = true;
                    followPointer = false;
                    break;

                case SpriteType.player:
                    canFall = false;
                    canCollide = true;
                    torender = true;
                    followPointer = true;
                    break;

                case SpriteType.view:
                    canFall = false;
                    torender = true;
                    canCollide = true;
                    followPointer = false;
                    break;
            }
        }

        //Funzione redraw necessaria ogni qual volta si effettua il resize dei vari sprite
        public void redraw(Sprite sprite, int new_Width, int new_Heigth,Bitmap risorsa, float nuova_X, float nuova_Y)
        {
            sprite.Width = new_Width;
            sprite.Height = new_Heigth;
            Console.WriteLine(sprite.Width);
            Bitmap b = new Bitmap(sprite.Width, sprite.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawImage(risorsa, 0, 0, sprite.Width, sprite.Height);
            }
            sprite.X = nuova_X;
            sprite.Y = nuova_Y;
            //Se il tipo di sprite è player, stiamo ridisegnando la racchetta, che mettiamo ad un altezza standard: 9/10 dell'altezza del form
            if (sprite.Type == Sprite.SpriteType.player)
                    sprite.Y = (Form1.ActiveForm.Height - sprite.Height) * 9 / 10;
            sprite.Texture = b;
        }

        //Funzione che restituisce l'angolo con cui la pallina deve essere fatta rimbalzare, a seconda del punto di impatto sulla racchetta
        public double angolo(float posizione_attuale, float posizione_massima)
        {
            double calcolo = 0;
            calcolo = (posizione_attuale / posizione_massima) * 90;
            calcolo = calcolo * Math.PI / 180;
            return calcolo;
        }

        //Funzione che aggiorna la posizione dei vari sprite utilizzando la loro velocità e posizione precedente
        public void Update(InputManager iManager)
        {
            //Calcolo la velocità totale della pallina che non deve superare i 3000
            velocity_tot = (float)Math.Sqrt((double)((velocity.X * velocity.X) + (velocity.Y * velocity.Y)));

            //Se la velocità totale non è ancora a 3000, setto la spia a 0 così da continuare a farla aumentare
            if (velocity_tot < 3000 )
                velocity_tot_raggiunto = 0;
            
            //Controllo che non ci siano impatti, in caso inverto prima x o y a seconda di cosa succede, poi aggiorno le posizioni
            if (canCollide == true)
                Collider(iManager);

            //Se il valore canFall è vero e quindi si tratta della pallina, in caso la velocità massima non sia arrivata a 3000
            //incremento la velocità y, altrimenti no
            if (canFall == true)
            {
                //Blocco l'incremento di velocità totale oltre i 3000
                if (velocity_tot >= 3000)
                {
                    velocity_tot = 3000;
                    velocity_tot_raggiunto = 1;
                }
                //Altrimenti aumento la velocità di y (o decremento se questa è negativa)
                else
                {
                    if(velocity_tot_raggiunto == 0)
                    {
                        if (velocity.Y >= 0)
                        {
                            velocity.Y += Accel_y;
                        }
                        else
                        {
                            velocity.Y -= Accel_y;
                        }
                    }
                }
                this.X += velocity.X * 1 / 500;
                this.Y += velocity.Y * 1 / 500;
            }

            //Se il gioco si trova in primo piano e il valore followPointer è vero, lo sprite (per noi solo la racchetta)
            //dovrà seguire il mouse
            if (followPointer == true)
            {
                    if(Form1.ActiveForm != null)
                        this.X = Form1.MousePosition.X - Form1.ActiveForm.Location.X - this.Width / 2 - this.Width / 16;
            }
        }

        //Il collider fa un check di eventuali impatti tra sprites
        private void Collider(InputManager iManager)
        {
            foreach (Sprite s in iManager.inGameSprites)
            {
                if (this.isCollidingWith(s))
                {
                    switch (this.Type)
                    {
                        case SpriteType.ball:
                            break;

                        case SpriteType.block:
                                switch (s.Type)
                                {
                                    case SpriteType.ball:
                                    if (s.isTouchingTop(this) || s.isTouchingBottom(this))
                                    {
                                        s.velocity.Y *= -1;
                                        this.remaining_bounces--;
                                        if (this.remaining_bounces <= 0)
                                        {
                                            this.torender = false;
                                            this.canCollide = false;
                                        }
                                    }
                                    else
                                    if (s.isTouchingLeft(this) || s.isTouchingRight(this))
                                    {
                                        s.velocity.X *= -1;
                                        this.remaining_bounces--;
                                        if (this.remaining_bounces <= 0)
                                        {
                                            this.torender = false;
                                            this.canCollide = false;
                                        }
                                    }
                                    break;
                                }
                            break;

                        case SpriteType.player:
                            switch (s.Type)
                            {
                                case SpriteType.ball:
                                    //La pallina impatta con la racchetta
                                    if (s.isTouchingBottom(this))
                                    {
                                        //La pallina impatta con la metà sinistra
                                        if ((s.X + s.Width / 2) <= (this.X + this.Width/2))
                                        {
                                            double coseno;
                                            coseno = Math.Abs(Math.Cos(angolo(s.X + s.Width / 2 - this.X, this.Width / 2)));
                                            s.velocity.X = - s.velocity_tot * (float)coseno;
                                            s.velocity.Y = -(float)Math.Sqrt(Math.Abs((double)((s.velocity_tot * s.velocity_tot) - (s.velocity.X * s.velocity.X))));
                                            s.Y = this.Y - s.Height;
                                        }
                                            else
                                            //Altrimenti con la metà destra
                                            {
                                            double seno;
                                            seno = Math.Abs(Math.Sin(angolo(s.X + s.Width / 2 - this.X - this.Width / 2, this.Width / 2)));
                                                s.velocity.X = s.velocity_tot * (float)seno;
                                                s.velocity.Y = -(float)Math.Sqrt((double)(Math.Abs((s.velocity_tot * s.velocity_tot) - (s.velocity.X * s.velocity.X))));
                                                s.Y = this.Y - s.Height;
                                            }
                                        }
                                    
                            break;

                                case SpriteType.block:
                                    break;

                                case SpriteType.player:
                                    break;

                                case SpriteType.view:
                                    break;
                            }
                            break;
                    }
                }

                //Qui analizzo il caso in cui la pallina impatti con il limite del client (e quindi dello sprite sfondo)
                //o che abbia superato il limite senza impattare con un lato perchè ha raggiunto una velocità troppo alta e si è mossa
                //di molti pixel in una volta
                switch (this.Type)
                {
                    case SpriteType.view:
                        switch (s.Type)
                        {
                            case SpriteType.ball:
                                //La X della pallina è oltre il limite destro o sinistro
                                if ((s.X + (float)s.Width) >= (float)this.Width)
                                {
                                    s.velocity.X *= -1;
                                    s.X = (float)this.Width - s.Width;
                                }else
                                if (s.X < 0)
                                {
                                    s.velocity.X *= -1;
                                    s.X = 0;
                                }

                                //La Y della pallina è oltre il limite superiore o inferiore
                                if ((s.Y + (float)s.Height) >= (float)this.Height)
                                {
                                    s.velocity.Y *= -1;
                                    s.Y = (float)this.Height - s.Height;
                                }else
                                if (s.Y < 0)
                                {
                                    s.velocity.Y *= -1;
                                    s.Y = 0;
                                }
                                break;
                        }
                        break;
                }
            }
        }
                
        //Funzioni che restituiscono la porzione dello sprite richiesta

        public Rectangle toRec
        {
            get { return new Rectangle((int)X, (int)Y,Width,Height); }
        }

        public Rectangle Top
        {
            get { return new Rectangle((int)X, (int)Y, Width, 20); }
        }

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
            get { return new Rectangle((int)X + this.Width, (int)Y , 20, Height); }
        }

        //Funzione Draw che rimanda alla funzione DrawImageUnscaled all'interno di SpriteBatch
        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this);
        }

      
    }

    //Classe SpriteHelper con le varie funzioni utili per sapere quando due sprite impattano e in particolare quali sezioni di questi due
    //si stanno intersecando
    static class SpriteHelper
    {
        public static bool isCollidingWith(this Sprite s1, Sprite s2)
        {
            if (s1.toRec.IntersectsWith(s2.toRec))
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

        public static bool isTouchingBottom(this Sprite s1, Sprite s2)
        {
            if (s1.Bottom.IntersectsWith(s2.Top))
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
    }



}
