using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

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
        public float velocityTot;
        public float velocityTotLimit = 3000;
        public int velocityTotRaggiunto;
        private SoundEffect Music;

        #endregion Fields

        #region Constructors

        public Ball(float x, float y, int width, int height, Logic logic) : base(x, y, width, height)
        {
            // Imposta le proprietà della pallina
            texture = Properties.Resources.ball;
            canFall = false;
            canCollide = true;
            followPointer = true;
            torender = true;

            stream = TitleContainer.OpenStream("Music.wav");
            Music = SoundEffect.FromStream(stream);

            //rendo invisibile lo sfondo dello sprite della pallina
            if (this.GetType().ToString().ToLower() == "windowsformsapplication5.ball")
            {
                System.Drawing.Color backColor = texture.GetPixel(0, 0);
                texture.MakeTransparent(backColor);
            }

            // Disegna la pallina
            this.graphics(texture, x, y, width, height);

            // aggiungo la pallina all'inputmanager che tiene conto di tutti gli sprite presenti nel gioco
            logic.iManager.inGameSprites.Add(this);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Metodo collider che calcola le azioni da svolgere in caso di impatto
        /// </summary>
        public void Collider(InputManager iManager)
        {
            //per ogni sprite presente nella lista contenuta dell'imanager
            foreach (Sprite s in iManager.inGameSprites)
            {
                if (s.GetType().Name == "Block")
                {
                    Block myBlock = (Block)s;

                    if (this.isCollidingWith(myBlock) && myBlock.canCollide == true)
                    {
                        //se un blocco viene toccato dalla pallina gli tolgo una vita e cambio la texture
                        if (this.isTouchingTop(myBlock) || this.isTouchingBottom(myBlock))
                        {
                            if (this.X + this.Width / 2 > myBlock.X && this.X + this.Width / 2 < myBlock.X + myBlock.Width)
                            {
                                this.velocity.Y *= -1;
                                myBlock.blockLife--;
                                PlaySound();
                                myBlock.textureSwitcher();

                                if (myBlock.blockLife <= 0)
                                {
                                    myBlock.torender = false;
                                    myBlock.canCollide = false;
                                }
                                else

                                    //ri Disegna il blocco
                                    myBlock.graphics(myBlock.texture, myBlock.X, myBlock.Y, myBlock.Width, myBlock.Height);
                            }
                        }
                        else
                        if (this.isTouchingLeft(myBlock) || this.isTouchingRight(myBlock))
                        {
                            if (this.Y + this.Height / 2 > myBlock.Y && this.Y + this.Height / 2 < myBlock.Y + myBlock.Height)
                            {
                                this.velocity.X *= -1;
                                myBlock.blockLife--;
                                PlaySound();
                                switch (myBlock.blockLife)
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

                                if (myBlock.blockLife <= 0)
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
                                //double coseno; DA PROVARE COSì
                                //coseno = Math.Abs(Math.Cos(mypaddle.angolo(this.X + this.Width / 2 - mypaddle.X, mypaddle.Width / 2)));
                                //this.velocity.X = -(float)Math.Min(Math.Abs(this.velocityTot * (float)coseno), Math.Abs(8 / 10 * velocityTot));
                                //this.velocity.Y = -(float)Math.Sqrt(Math.Abs((double)((this.velocityTot * this.velocityTot) - (this.velocity.X * this.velocity.X))));
                                //this.Y = mypaddle.Y - this.Height;
                                double coseno;
                                coseno = Math.Abs(Math.Cos(mypaddle.angolo(this.X + this.Width / 2 - mypaddle.X, mypaddle.Width / 2)));
                                this.velocity.X = z-this.velocityTot * (float)coseno;
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

                //faccio rimanere la pallina all'interno dello schermo e scalo una vita se la y della pallina arriva all'altezza di view.heigth
                if (s.GetType().Name == "View")
                {
                    View myview = (View)s;

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
                        myview.bottomCollide = 1;
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

        /// <summary>
        /// Metodo playsound che riproduce il suono
        /// </summary>
        public void PlaySound()
        {
            FrameworkDispatcher.Update();
            Music.Play();
        }

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
                if ((Cursor.Position.X - thisform.Location.X - this.Width * 2) >= 0 && Cursor.Position.X - thisform.Location.X < thisform.Width)
                    this.X = Cursor.Position.X - thisform.Location.X - this.Width / 2 - 15;
            }
        }

        #endregion Methods
    }
}