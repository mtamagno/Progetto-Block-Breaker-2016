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
        public int Accel_y = 2;  //setto l'accelerazione nell' asse delle y
        public SpriteType Type;
        public enum SpriteType { player , ball , block, view};
        public bool canFall;
        public bool canCollide;
        public bool followPointer;
        public bool torender;
        public int remaining_bounces;
        public int velocity_tot_raggiunto;
        public float velocity_tot;
        static Random random = new Random();

        public Sprite(Bitmap texture,float x, float y, int width, int height, SpriteType thisType)
        {
            Bitmap b = new Bitmap(width, height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawImage(texture,0,0,width,height);
            }

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
                 
            if (sprite.Type == Sprite.SpriteType.view) ;
            else
                sprite.Y = nuova_Y;
            //redraw del player e della pallina alla y giusta
            if (sprite.Type == Sprite.SpriteType.player)
                sprite.Y = (Form1.ActiveForm.Height - sprite.Height) * 9 / 10;
            sprite.Texture = b;
        }

        public double angolo(float posizione_attuale, float posizione_massima)
        {
            double calcolo = 0;
            calcolo = (posizione_attuale / posizione_massima) * 90;
            calcolo = calcolo * Math.PI / 180;
            return calcolo;
        }

        public void Update(InputManager iManager)
        {
            //calcolo velotà totale pallina
            velocity_tot = (float)Math.Sqrt((double)((velocity.X * velocity.X) + (velocity.Y * velocity.Y)));
            if (velocity_tot < 3000)
                velocity_tot_raggiunto = 0;

            if (canCollide == true)
                Collider(iManager);
            if (canFall)
            {
                //blocco l'incremento o decremento di velocità oltre i +-5000
                if (velocity_tot >= 3000 || velocity_tot <= -3000)
                {
                    velocity_tot = 3000;
                    velocity_tot_raggiunto = 1;
                }
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
            if (followPointer)
            {
                if (Form1.ActiveForm != null)
                    if(Form1.MousePosition.X > Form1.ActiveForm.Location.X && Form1.MousePosition.X < Form1.ActiveForm.Location.X + Form1.ActiveForm.Width)
                        this.X = Form1.MousePosition.X - Form1.ActiveForm.Location.X - this.Width / 2 - this.Width / 16;
            }
        }
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
                                    //la pallina collide con la racchetta
                                    if (s.isTouchingBottom(this))
                                    {
                                        //la pallina tocca entro la prima metà
                                        if ((s.X + s.Width / 2) <= (this.X + this.Width/2))
                                        {
                                            double coseno;
                                            coseno = Math.Abs(Math.Cos(angolo(s.X + s.Width / 2 - this.X, this.Width / 2)));
                                            s.velocity.X = - s.velocity_tot * (float)coseno;
                                            s.velocity.Y = -(float)Math.Sqrt(Math.Abs((double)((s.velocity_tot * s.velocity_tot) - (s.velocity.X * s.velocity.X))));
                                            s.Y = this.Y - s.Height;
                                        }
                                            else
                                            //Dopo metà 
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

                switch (this.Type)
                {
                    case SpriteType.view:
                        switch (s.Type)
                        {
                            case SpriteType.ball:
                                //Ball.X oltre il limite  + s.velocity.X
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

                                //Ball.Y oltre il limite + s.velocity.Y
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
                

        public Rectangle toRec
        {
            get { return new Rectangle((int)X, (int)Y,Width,Height); }
        }

        public Rectangle Top
        {
            get { return new Rectangle((int)X, (int)Y, Width, 10); }
        }

        public Rectangle Bottom
        {
            get { return new Rectangle((int)X, (int)Y + this.Height, Width, 10); }
        }

        public Rectangle Left
        {
            get { return new Rectangle((int)X, (int)Y, 10, Height); }
        }

        public Rectangle Right
        {
            get { return new Rectangle((int)X + this.Width, (int)Y , 10, Height); }
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(this);
        }

      
    }

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
