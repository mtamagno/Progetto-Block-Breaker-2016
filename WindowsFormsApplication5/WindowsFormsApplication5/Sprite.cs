﻿using System;
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
        public int Accel_x = 2;  //setto la velocita' nell asse delle x
        public int Accel_y = 2;  //setto la velocita' nell asse delle y
        public SpriteType Type;
        public enum SpriteType { player , ball , blocks, view};
        public bool canFall;
        public bool canCollide;
        public bool followPointer;

        
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

                    break;

                case SpriteType.blocks:
                    canFall = false;
                    canCollide = true;
                    followPointer = false;
                    break;

                case SpriteType.player:
                    canFall = false;
                    canCollide = true;
                    followPointer = true;
                    break;

                case SpriteType.view:
                    canFall = false;
                    canCollide = true;
                    followPointer = false;
                    break;

            }

            
        }

        public void redraw(Sprite sprite, int new_Width, int new_Heigth,Bitmap risorsa, float nuova_X, float nuova_Y)
        {
            sprite.Width = new_Width;
            sprite.Height = new_Heigth;

            Bitmap b = new Bitmap(sprite.Width, sprite.Height);
            using (Graphics g = Graphics.FromImage(b))
            {
                g.DrawImage(risorsa,0, 0, sprite.Width, sprite.Height);
            }


            sprite.Texture = b;
        }

        public void Update(InputManager iManager)
        {
            if (canFall)
            {
                if (velocity.X >= 0)
                {
                    velocity.X += Accel_x;
                    this.X += velocity.X * 1/500;
                }
                else
                {
                    velocity.X -= Accel_x;
                    this.X += velocity.X * 1/500;
                }
                if (velocity.Y >= 0)
                {
                   
                    velocity.Y += Accel_y;
                    this.Y += velocity.Y * 1/500;

                }
                else
                {
                    velocity.Y -= Accel_y;
                    this.Y += velocity.Y * 1/500;
                }


            }

            if(followPointer)
            {
                if(Form1.MousePosition.X > Form1.ActiveForm.Location.X && Form1.MousePosition.X < Form1.ActiveForm.Location.X + Form1.ActiveForm.Width)
                this.X = Form1.MousePosition.X - Form1.ActiveForm.Location.X - this.Width / 2 - this.Width/16;
            }
            if(canCollide == true)
            Collider(iManager);
        }

        private void Collider(InputManager iManager)
        {
            foreach(Sprite s in iManager.inGameSprites)
            {           
                if (this.isCollidingWith(s))
                {
                    switch (this.Type)
                    {
                        case SpriteType.ball:
                            break;

                        case SpriteType.blocks:
                            break;

                        case SpriteType.player:
                            switch (s.Type)
                            {
                                case SpriteType.ball:
                                    //questo l'ho lasciato commentato anche se penso sia inutile perchè nel momento in cui tocca il top sta anche collidendo
                                    /*if (s.isCollidingWith(this)){
                                    s.Y = this.Y - s.Height;
                                        s.velocity.Y = -s.velocity.Y;
                                    }*/
                                    if (s.isTouchingBottom(this))
                                    {
                                        //s.Y = this.Y - s.Height;
                                        if (s.X < this.X + this.Width / 2)
                                        {
                                            if (s.velocity.X < 0)
                                                s.velocity.X *= ((this.X + this.Width) / (s.X + this.Width));
                                            else
                                                s.velocity.X *= (-(this.X + this.Width) / (s.X + this.Width));
                                        }
                                        else {
                                            if (s.velocity.X > 0)
                                                s.velocity.X *= ((this.X + this.Width) / (s.X + this.Width));
                                            else
                                                s.velocity.X *= (-(this.X + this.Width) / (s.X + this.Width));
                                        }
                                        s.velocity.Y *= -1;
                                    }
                                    break;

                                case SpriteType.blocks:
                                    break;

                                case SpriteType.player:
                                    break;

                                case SpriteType.view:
                                    break;
                            }
                            break;

                        case SpriteType.view:
                            switch (s.Type)
                            {
                                case SpriteType.ball:

                                    if (s.isTouchingTop(this) || s.isTouchingBottom(this))
                                    {
                                        s.velocity.Y = -s.velocity.Y;
                                    }
                                    if (s.isTouchingRight(this) || s.isTouchingLeft(this))
                                    {
                                        s.velocity.X = -s.velocity.X;
                                    }

                                    break;
                            }
                            break;

                    }
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
