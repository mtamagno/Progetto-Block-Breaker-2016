using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace WindowsFormsApplication5
{
    public class Ball : Sprite
    {
        #region Fields

        //variabili per accelerazione y, texture, velocità totale e attuale, spia che notifica la raggiunta della velocità massima
        public int Accel_y = 50;

        public Stream stream;
        public Bitmap texture;
        public PointF velocity;
        public PointF previousVelocity;
        public float previousX;
        public float previousY;
        public float previousVelocityTot;
        public float velocityTot;
        public float velocityTotLimit = 3000;
        public int velocityTotRaggiunto;
        private bool block_hit = false;
        private SoundEffect Music;
        private Thread hurt;
        #endregion Fields

        #region Constructors

        public Ball(float x, float y, int width, int height, Logic logic)
        {
            // Imposta le proprietà della pallina
            this.previousVelocityTot = 0;
            this.previousVelocity.X = 0;
            this.previousVelocity.Y = 0;
            this.previousX = 0;
            this.previousY = 0;
            texture = Properties.Resources.Ball;
            canFall = false;
            canCollide = true;
            followPointer = true;
            torender = true;


            stream = TitleContainer.OpenStream(@"Music.wav");
            Music = SoundEffect.FromStream(stream);

            //rendo invisibile lo sfondo dello sprite della pallina
            System.Drawing.Color backColor = texture.GetPixel(0, 0);
            texture.MakeTransparent(backColor);

            // Disegna la pallina
            this.graphics(texture, x, y, width, height);

            // aggiungo la pallina all'inputmanager che tiene conto di tutti gli sprite presenti nel gioco
            logic.iManager.inGameSprites.Add(this);
        }

        #endregion Constructors

        #region Methods

        public void totalVelocityReset(int lunghezza_client_iniziale, int altezza_client_iniziale, int lunghezza_client, int altezza_client)
        {
            this.velocity.Y = this.velocity.Y * altezza_client / altezza_client_iniziale;
            this.velocity.X = this.velocity.X * lunghezza_client / lunghezza_client_iniziale;

            //Calcolo la velocità totale della pallina che non deve superare i velocityTotLimit
            this.velocityTot = (float)Math.Sqrt((double)((this.velocity.X * this.velocity.X) + (this.velocity.Y * this.velocity.Y)));
            if (this.velocityTotRaggiunto == 1)
                this.velocityTotLimit = this.velocityTot;
        }

        /// <summary>
        /// Funzione che dopo aver chiamato il collider, si occupa dello spostamento vero e proprio di pallina e racchetta
        /// </summary>
        public void Update(InputManager iManager, Form thisform)
        {
            Collider(iManager);

            //Calcolo la velocità totale della pallina che non deve superare i velocityTotLimit
            velocityTot = (float)Math.Sqrt((double)((velocity.X * velocity.X) + (velocity.Y * velocity.Y)));

            //Se la velocità totale non è ancora a velocityTotLimit, Imposta la spia a 0 così da continuare a farla aumentare
            if (velocityTot < velocityTotLimit)
                velocityTotRaggiunto = 0;

            //Se il valore canFall è vero e quindi si tratta della pallina,
            //in caso la velocità massima non sia arrivata a velocityTotLimit
            //incremento la velocità y, altrimenti no
            if (canFall == true)
            {
                //Blocco l'incremento di velocità totale oltre i velocityTotLimit
                if (this.velocityTot >= velocityTotLimit)
                {
                    this.velocityTot = velocityTotLimit;
                    this.velocityTotRaggiunto = 1;
                }

                //Altrimenti aumento la velocità di y (o decremento se questa è negativa)
                else
                {
                    if (this.velocityTotRaggiunto == 0)
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

            //se deve seguire il mouse faccio in modo che lo faccia
            if (followPointer == true)
            {
                if(thisform != null)
                if ((Cursor.Position.X - thisform.Location.X - this.Width * 2) >= 0 && Cursor.Position.X - thisform.Location.X < thisform.Width)
                    this.X = Cursor.Position.X - thisform.Location.X - this.Width / 2 - 15;
            }

        }

        //Gestisco i casi in cui la pallina collide contro i blocchi
        private void BlockCollision(Sprite s)
        {
            Block myBlock = (Block)s;
            block_hit = false;

            if (this.isCollidingWith(myBlock) && myBlock.canCollide == true)
            {
                // Se un blocco viene toccato dalla pallina gli tolgo una vita e cambio la texture
                if (this.isTouchingTop(myBlock) || this.isTouchingBottom(myBlock))
                {
                    if (this.X + this.Width / 2 > myBlock.X && this.X + this.Width / 2 < myBlock.X + myBlock.Width)
                    {
                        // Setta block_hit a true
                        block_hit = true;

                        //Inverte la Velocità Y della pallina
                        this.velocity.Y *= -1;

                        //Scala la vita del blocco di uno, riproduce il suono ed esegue la funzione textureSwitcher
                        myBlock.blockLife--;
                        PlaySound();
                        myBlock.textureSwitcher();

                        if (myBlock.blockLife <= 0)
                        {
                            myBlock.torender = false;
                            myBlock.canCollide = false;
                        }
                        else

                        // Disegna il blocco
                        {
                            myBlock.graphics(myBlock.texture, myBlock.X, myBlock.Y, myBlock.Width, myBlock.Height);
                        }
                    }
                }
                if (this.isTouchingLeft(myBlock) || this.isTouchingRight(myBlock))
                {
                    if (this.Y + this.Height / 2 > myBlock.Y && this.Y + this.Height / 2 < myBlock.Y + myBlock.Height)
                    {
                        //Inverte la Velocità X della pallina
                        this.velocity.X *= -1;

                        //Scala la vita del blocco di uno, riproduce il suono ed esegue la
                        //funzione textureSwitcher se block hit non è già a 1
                        if (!block_hit)
                        {
                            myBlock.blockLife--;
                            PlaySound();
                            myBlock.textureSwitcher();

                            if (myBlock.blockLife <= 0)
                            {
                                myBlock.torender = false;
                                myBlock.canCollide = false;
                            }
                            else

                            // Disegna il blocco
                            {
                                myBlock.graphics(myBlock.texture, myBlock.X, myBlock.Y, myBlock.Width, myBlock.Height);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Metodo collider che calcola le azioni da svolgere in caso di impatto
        /// </summary>
        private void Collider(InputManager iManager)
        {
            // Per ogni sprite presente nella lista contenuta dell'imanager
            foreach (Sprite s in iManager.inGameSprites)
            {
                if (s.GetType().Name == "Block")
                {
                    BlockCollision(s);
                }

                if (s.GetType().Name == "Paddle")
                {
                    PaddleCollision(s);
                }

                if (s.GetType().Name == "View")
                {
                    ViewCollision(s);
                }
            }
        }

        //Gestisco i casi in cui la pallina collide contro la racchetta
        private void PaddleCollision(Sprite s)
        {
            Paddle mypaddle = (Paddle)s;
            if (mypaddle.isCollidingWith(this))
            {
                hurt = new Thread(mypaddle.hurt);
                hurt.Start();

                //La pallina impatta con la racchetta
                if (this.isTouchingBottom(mypaddle))
                {
                    //La pallina impatta con la metà sinistra
                    if ((this.X + this.Width / 2) <= (mypaddle.X + mypaddle.Width / 2))
                    {
                        double coseno;
                        coseno = Math.Abs(Math.Cos(mypaddle.angolo(this.X + this.Width / 2 - mypaddle.X, mypaddle.Width / 2)));
                        this.velocity.X = -this.velocityTot * (float)coseno;
                        this.velocity.Y = -(float)Math.Sqrt(Math.Abs((double)((this.velocityTot * this.velocityTot) - (this.velocity.X * this.velocity.X))));
                        this.Y = mypaddle.Y - this.Height;
                    }
                    else

                    //Altrimenti con la metà destra
                    {
                        double seno;
                        seno = Math.Abs(Math.Sin(mypaddle.angolo(this.X + this.Width / 2 - mypaddle.X - mypaddle.Width / 2, mypaddle.Width / 2)));
                        this.velocity.X = this.velocityTot * (float)seno;
                        this.velocity.Y = -(float)Math.Sqrt((double)(Math.Abs((this.velocityTot * this.velocityTot) - (this.velocity.X * this.velocity.X))));
                        this.Y = mypaddle.Y - this.Height;
                    }
                }              
            }

        }

        /// <summary>
        /// Metodo playsound che riproduce il suono
        /// </summary>
        private void PlaySound()
        {
            FrameworkDispatcher.Update();
            Music.Play();
        }

        //Gestisco i casi in cui la pallina collide contro i bordi dello schermo
        //faccio rimanere la pallina all'interno dello schermo e scalo una vita se la y della pallina arriva all'altezza di view.heigth
        private void ViewCollision(Sprite s)
        {
            View myview = (View)s;

            //La X della pallina è oltre il limite destro o sinistro
            if ((this.X + (float)this.Width) >= (float)myview.Width + myview.X)
            {
                this.velocity.X *= -1;
                this.X = (float)myview.Width - this.Width + myview.X;
            }
            else
            if (this.X <= myview.X)
            {
                this.velocity.X *= -1;
                this.X = myview.X;
            }

            //La Y della pallina è oltre il limite superiore o inferiore
            if ((this.Y + (float)this.Height) >= (float)myview.Height + myview.X)
            {
                myview.bottomCollide = 1;
            }
            else
            if (this.Y <= myview.Y)
            {
                this.velocity.Y *= -1;
                this.Y = myview.Y;
            }
           
        }

        #endregion Methods
    }
}