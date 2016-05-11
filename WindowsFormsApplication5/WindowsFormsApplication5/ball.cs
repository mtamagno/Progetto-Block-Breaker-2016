using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication5
{
    class ball : Sprite
    {
        public PointF velocity;
        public int Accel_y = 2;
        public float velocity_tot;
        public int velocity_tot_raggiunto;

        public ball(Bitmap texture, float x, float y, int width, int height) : base(texture, x, y, width, height)
        {
            canFall = true;
            canCollide = true;
            followPointer = false;
            torender = true;

        }




        public void Update(InputManager iManager)
        {
            Collider(iManager);
            //Calcolo la velocità totale della pallina che non deve superare i 3000
            velocity_tot = (float)Math.Sqrt((double)((velocity.X * velocity.X) + (velocity.Y * velocity.Y)));

            //Se la velocità totale non è ancora a 3000, setto la spia a 0 così da continuare a farla aumentare
            if (velocity_tot < 3000)
                velocity_tot_raggiunto = 0;

            //Controllo che non ci siano impatti, in caso inverto prima x o y a seconda di cosa succede, poi aggiorno le posizioni
            /*if (canCollide == true)
                this.Collider(iManager);*/

            //Se il valore canFall è vero e quindi si tratta della pallina, in caso la velocità massima non sia arrivata a 3000
            //incremento la velocità y, altrimenti no
            if (canFall == true)
            {
                //Blocco l'incremento di velocità totale oltre i 3000
                if (this.velocity_tot >= 3000)
                {
                    this.velocity_tot = 3000;
                    this.velocity_tot_raggiunto = 1;
                }
                //Altrimenti aumento la velocità di y (o decremento se questa è negativa)
                else
                {
                    if (this.velocity_tot_raggiunto == 0)
                    {
                        if (this.velocity.Y >= 0)
                        {
                            this.velocity.Y += Accel_y;
                        }
                        else
                        {
                            this.velocity.Y -= Accel_y;
                        }
                    }
                }
                this.X += this.velocity.X * 1 / 500;
                this.Y += this.velocity.Y * 1 / 500;
            }

            if (followPointer == true)
            {
                this.X = Form2.MousePosition.X+50;
            }

        }

        public void Collider(InputManager iManager)
        {
            foreach (Sprite s in iManager.inGameSprites) { 
                if (s.GetType().ToString().ToLower() == "windowsformsapplication5.block")
                {
                    Block myBlock = (Block)s;
                  
                    if (this.isCollidingWith(myBlock) && myBlock.canCollide == true)
                    {


                        if (this.isTouchingTop(myBlock) || this.isTouchingBottom(myBlock))
                        {
                            this.velocity.Y *= -1;
                            myBlock.remaining_bounces--;
                            if (myBlock.remaining_bounces <= 0)
                            {
                                myBlock.torender = false;
                                myBlock.canCollide = false;
                            }

                        }
                        else
                        if (myBlock.isTouchingLeft(this) || myBlock.isTouchingRight(this))
                        {
                            this.velocity.X *= -1;
                            myBlock.remaining_bounces--;
                            if (myBlock.remaining_bounces <= 0)
                            {
                                myBlock.torender = false;
                                myBlock.canCollide = false;
                            }
                        }
                       

                    }
                }

                if (s.GetType().ToString().ToLower() == "windowsformsapplication5.paddle")
                {
                    paddle mypaddle = (paddle)s;
                    if (mypaddle.isCollidingWith(this))
                    {
                        //La pallina impatta con la racchetta
                        if (this.isTouchingBottom(mypaddle))
                        {
                            //La pallina impatta con la metà sinistra
                            if ((this.X + this.Width / 2) <= (mypaddle.X + mypaddle.Width / 2))
                            {
                                double coseno;
                                coseno = Math.Abs(Math.Cos(mypaddle.angolo(this.X + this.Width / 2 - mypaddle.X, mypaddle.Width / 2)));
                                this.velocity.X = -this.velocity_tot * (float)coseno;
                                this.velocity.Y = -(float)Math.Sqrt(Math.Abs((double)((this.velocity_tot * this.velocity_tot) - (this.velocity.X * this.velocity.X))));
                                this.Y = mypaddle.Y - this.Height;
                            }
                            else
                            //Altrimenti con la metà destra
                            {
                                double seno;
                                seno = Math.Abs(Math.Sin(mypaddle.angolo(this.X + this.Width / 2 - mypaddle.X - mypaddle.Width / 2, mypaddle.Width / 2)));
                                this.velocity.X = this.velocity_tot * (float)seno;
                                this.velocity.Y = -(float)Math.Sqrt((double)(Math.Abs((this.velocity_tot * this.velocity_tot) - (this.velocity.X * this.velocity.X))));
                                this.Y = mypaddle.Y - this.Height;
                            }
                        }
                        // s.canCollide = false;


                    }
                }

                if (s.GetType().ToString().ToLower() == "windowsformsapplication5.view")
                {
                    View myview = (View)s;
                    if (myview.isCollidingWith(this))
                    {
                        //La X della pallina è oltre il limite destro o sinistro
                        if ((this.X + (float)this.Width) >= (float)myview.Width)
                        {
                            this.velocity.X *= -1;
                            this.X = (float)myview.Width - this.Width;
                        }
                        else
                        if (this.X < 0)
                        {
                            this.velocity.X *= -1;
                            this.X = 0;
                        }

                        //La Y della pallina è oltre il limite superiore o inferiore
                        if ((this.Y + (float)this.Height) >= (float)myview.Height)
                        {
                            myview.bottom_collide = 1;
                        }
                        else
                        if (this.Y < 0)
                        {
                            this.velocity.Y *= -1;
                            this.Y = 0;
                        }

                    }
                }
            }
        }

    }
}
