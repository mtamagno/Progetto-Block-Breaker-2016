using System;
using System.Drawing;
using System.Windows.Forms;

namespace WindowsFormsApplication5
{
    public class Ball : Sprite
    {
        #region Public Fields

        public int Accel_y = 100;
        public Bitmap texture;
        public PointF velocity;
        public float velocity_tot;
        public int velocity_tot_raggiunto;

        #endregion Public Fields

        #region Public Constructors

        public Ball(float x, float y, int width, int height, Logic logic) : base(x, y, width, height)
        {
            texture = Properties.Resources.ball;
            canFall = false;
            canCollide = true;
            followPointer = true;
            torender = true;

            //per adesso è "thisType == spritetype.ball" ma una volta cambiati gli sprite sarà "thisType != spritetype.background"
            if (this.GetType().ToString().ToLower() == "windowsformsapplication5.ball")
            {
                Color backColor = texture.GetPixel(0, 0);
                texture.MakeTransparent(backColor);
            }
            this.graphics(texture, x, y, width, height);

            Ball ball = this;
            logic.iManager.inGameSprites.Add(ball);
        }

        #endregion Public Constructors

        #region Public Methods

        public void Collider(InputManager iManager)
        {
            foreach (Sprite s in iManager.inGameSprites)
            {
                if (s.GetType().Name == "Block")
                {
                    Block myBlock = (Block)s;

                    if (this.isCollidingWith(myBlock) && myBlock.canCollide == true)
                    {
                        if (this.isTouchingTop(myBlock) || this.isTouchingBottom(myBlock))
                        {
                            if (this.X + this.Width / 2 > myBlock.X && this.X + this.Width / 2 < myBlock.X + myBlock.Width) { 
                            this.velocity.Y *= -1;
                            myBlock.remaining_bounces--;

                                switch (myBlock.remaining_bounces)
                                {
                                    case 1:
                                        myBlock.texture = Properties.Resources.Block_1;
                                        break;
                                    case 2:
                                        myBlock.texture = Properties.Resources.Block_2;
                                        break;
                                    case 3:
                                        myBlock.texture = Properties.Resources.Block_3;
                                        break;
                                }

                                myBlock.graphics(myBlock.texture, myBlock.X, myBlock.Y, myBlock.Width, myBlock.Height);

                                if (myBlock.remaining_bounces <= 0)
                            {
                                myBlock.torender = false;
                                myBlock.canCollide = false;
                            }
                        }
                        }
                        if (this.isTouchingLeft(myBlock) || this.isTouchingRight(myBlock))
                        {
                            if (this.Y + this.Height / 2 > myBlock.Y && this.Y + this.Height / 2 < myBlock.Y + myBlock.Height)
                            {
                                this.velocity.X *= -1;
                                myBlock.remaining_bounces--;

                                switch (myBlock.remaining_bounces)
                                {
                                    case 1:
                                        myBlock.texture = Properties.Resources.Block_1;
                                        break;
                                    case 2:
                                        myBlock.texture = Properties.Resources.Block_2;
                                        break;
                                    case 3:
                                        myBlock.texture = Properties.Resources.Block_3;
                                        break;
                                }

                                myBlock.graphics(myBlock.texture, myBlock.X, myBlock.Y, myBlock.Width, myBlock.Height);
                                
                                if (myBlock.remaining_bounces <= 0)
                                {
                                    myBlock.torender = false;
                                    myBlock.canCollide = false;
                                }
                            }
                        }
                    }
                }

                if (s.GetType().Name == "Paddle")
                {
                    Paddle mypaddle = (Paddle)s;
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

                if (s.GetType().Name == "View")
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

        public void Update(InputManager iManager, Form thisform)
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
                this.X = Cursor.Position.X - thisform.Location.X - this.Width / 2 - 15;
            }
        }

        #endregion Public Methods
    }
}