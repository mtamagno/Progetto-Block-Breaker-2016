using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace BlockBreaker
{
    public class Ball : Sprite
    {
        #region Fields

        //variabili per accelerazione y, texture, velocità totale e attuale, spia che notifica la raggiunta della velocità massima
        public int AccelY = 50;

        public Stream Stream;
        public PointF Velocity;
        public PointF PreviousVelocity;
        public float PreviousX;
        public float PreviousY;
        public float PreviousVelocityTot;
        public float VelocityTot;
        public float VelocityTotLimit = 3000;
        public int VelocityTotRaggiunto;
        private bool _blockHit = false;
        private readonly SoundEffect _music;
        private Thread _hurt;
        #endregion Fields

        #region Constructors

        public Ball(float x, float y, int width, int height, Logic logic)
        {
            if (logic == null) throw new ArgumentNullException(nameof(logic));
            if (x <= 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y <= 0) throw new ArgumentOutOfRangeException(nameof(y));
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));
            // Imposta le proprietà della pallina
            this.PreviousVelocityTot = 0;
            this.PreviousVelocity.X = 0;
            this.PreviousVelocity.Y = 0;
            this.PreviousX = 0;
            this.PreviousY = 0;
            var texture = Properties.Resources.Ball;
            CanFall = false;
            CanCollide = true;
            FollowPointer = true;
            ToRender = true;


            Stream = TitleContainer.OpenStream(@"SoundEffect/Music.wav");
            _music = SoundEffect.FromStream(Stream);

            //rendo invisibile lo sfondo dello sprite della pallina
            System.Drawing.Color backColor = texture.GetPixel(0, 0);
            texture.MakeTransparent(backColor);

            // Disegna la pallina
            this.CreateSprite(texture, x, y, width, height);

            // aggiungo la pallina all'inputmanager che tiene conto di tutti gli sprite presenti nel gioco
            logic.IManager.inGameSprites.Add(this);
        }

        #endregion Constructors

        #region Methods
        /// <summary>
        /// Risetta la velocità totale massima a seconda della grandezza del client in modo che non rallenti o acceleri la pallina a seconda del size
        /// </summary>
        /// <param name="lunghezzaClientIniziale"></param>
        /// <param name="altezzaClientIniziale"></param>
        /// <param name="lunghezzaClient"></param>
        /// <param name="altezzaClient"></param>
        /// 

        public virtual void TotalVelocityReset(int lunghezzaClientIniziale, int altezzaClientIniziale, int lunghezzaClient, int altezzaClient)
        {
            this.Velocity.Y = this.Velocity.Y * altezzaClient / altezzaClientIniziale;
            this.Velocity.X = this.Velocity.X * lunghezzaClient / lunghezzaClientIniziale;

            //Calcolo la velocità totale della pallina che non deve superare i velocityTotLimit
            this.VelocityTot = (float)Math.Sqrt((double)((this.Velocity.X * this.Velocity.X) + (this.Velocity.Y * this.Velocity.Y)));
            if (this.VelocityTotRaggiunto == 1)
                this.VelocityTotLimit = this.VelocityTot;
        }

        /// <summary>
        /// Funzione che dopo aver chiamato il collider, si occupa dello spostamento vero e proprio di pallina e racchetta
        /// </summary>
        public void Update(InputManager iManager, Form thisform)
        {
            if (thisform == null) throw new ArgumentNullException(nameof(thisform));
            Collider(iManager);

            //Calcolo la velocità totale della pallina che non deve superare i velocityTotLimit
            VelocityTot = (float)Math.Sqrt((double)((Velocity.X * Velocity.X) + (Velocity.Y * Velocity.Y)));

            //Se la velocità totale non è ancora a velocityTotLimit, Imposta la spia a 0 così da continuare a farla aumentare
            if (VelocityTot < VelocityTotLimit)
                VelocityTotRaggiunto = 0;

            //Se il valore canFall è vero e quindi si tratta della pallina,
            //in caso la velocità massima non sia arrivata a velocityTotLimit
            //incremento la velocità y, altrimenti no
            if (CanFall == true)
            {
                //Blocco l'incremento di velocità totale oltre i velocityTotLimit
                if (this.VelocityTot >= VelocityTotLimit)
                {
                    this.VelocityTot = VelocityTotLimit;
                    this.VelocityTotRaggiunto = 1;
                }

                //Altrimenti aumento la velocità di y (o decremento se questa è negativa)
                else
                {
                    if (this.VelocityTotRaggiunto == 0)
                    {
                        if (this.Velocity.Y >= 0)
                        {
                            this.Velocity.Y += AccelY;
                        }
                        else
                        {
                            this.Velocity.Y -= AccelY;
                        }
                    }
                }

                this.X += this.Velocity.X * 1 / 500;
                this.Y += this.Velocity.Y * 1 / 500;
            }

            //se deve seguire il mouse faccio in modo che lo faccia
            if (FollowPointer == true)
            {
                if ((Cursor.Position.X - thisform.Location.X) >= thisform.Width / 11 && Cursor.Position.X - thisform.Location.X < thisform.Width - thisform.Width / 11)
                    this.X = Cursor.Position.X - thisform.Location.X - this.Width / 2 - this.Width/3*2 - this.Width / 10 * 9;
            }

        }

        //Gestisco i casi in cui la pallina collide contro i blocchi
        private void BlockCollision(Sprite s)
        {
            Block myBlock = (Block)s;
            _blockHit = false;

            if (this.IsCollidingWith(myBlock) && myBlock.CanCollide == true)
            {
                // Se un blocco viene toccato dalla pallina gli tolgo una vita e cambio la texture
                if (this.IsTouchingTop(myBlock) || this.IsTouchingBottom(myBlock))
                {
                    if (this.X + (float)this.Width / 2 > myBlock.X && this.X + (float)this.Width / 2 < myBlock.X + myBlock.Width)
                    {
                        // Setta block_hit a true
                        _blockHit = true;

                        //Inverte la Velocità Y della pallina
                        this.Velocity.Y *= -1;

                        //Scala la vita del blocco di uno, riproduce il suono ed esegue la funzione textureSwitcher
                        myBlock.BlockLife--;
                        PlaySound();
                        myBlock.TextureSwitcher();

                        if (myBlock.BlockLife <= 0)
                        {
                            myBlock.ToRender = false;
                            myBlock.CanCollide = false;
                        }
                        else

                        // Disegna il blocco
                        {
                            myBlock.CreateSprite(myBlock.texture, myBlock.X, myBlock.Y, myBlock.Width, myBlock.Height);
                        }
                    }
                }
                if (this.IsTouchingLeft(myBlock) || this.IsTouchingRight(myBlock))
                {
                    if (this.Y + (float)this.Height / 2 > myBlock.Y && this.Y + (float)this.Height / 2 < myBlock.Y + myBlock.Height)
                    {
                        //Inverte la Velocità X della pallina
                        this.Velocity.X *= -1;

                        //Scala la vita del blocco di uno, riproduce il suono ed esegue la
                        //funzione textureSwitcher se block hit non è già a 1
                        if (!_blockHit)
                        {
                            myBlock.BlockLife--;
                            PlaySound();
                            myBlock.TextureSwitcher();

                            if (myBlock.BlockLife <= 0)
                            {
                                myBlock.ToRender = false;
                                myBlock.CanCollide = false;
                            }
                            else

                            // Disegna il blocco
                            {
                                myBlock.CreateSprite(myBlock.texture, myBlock.X, myBlock.Y, myBlock.Width, myBlock.Height);
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

                if (s.GetType().Name == "Playground")
                {
                    ViewCollision(s);
                }
            }
        }

        //Gestisco i casi in cui la pallina collide contro la racchetta
        private void PaddleCollision(Sprite s)
        {
            Paddle mypaddle = (Paddle)s;
            if (mypaddle.IsCollidingWith(this))
            {
                _hurt = new Thread(mypaddle.On_Hurt);
                _hurt.Start();

                //La pallina impatta con la racchetta
                if (this.IsTouchingBottom(mypaddle))
                {
                    //La pallina impatta con la metà sinistra
                    if ((this.X + this.Width / 2) <= (mypaddle.X + mypaddle.Width / 2))
                    {
                        double coseno;
                        coseno = Math.Abs(Math.Cos(mypaddle.Angolo(this.X + this.Width / 2 - mypaddle.X, mypaddle.Width / 2)));
                        this.Velocity.X = -this.VelocityTot * (float)coseno;
                        this.Velocity.Y = -(float)Math.Sqrt(Math.Abs((double)((this.VelocityTot * this.VelocityTot) - (this.Velocity.X * this.Velocity.X))));
                        this.Y = mypaddle.Y - this.Height;
                    }
                    else

                    //Altrimenti con la metà destra
                    {
                        double seno;
                        seno = Math.Abs(Math.Sin(mypaddle.Angolo(this.X + this.Width / 2 - mypaddle.X - mypaddle.Width / 2, mypaddle.Width / 2)));
                        this.Velocity.X = this.VelocityTot * (float)seno;
                        this.Velocity.Y = -(float)Math.Sqrt((double)(Math.Abs((this.VelocityTot * this.VelocityTot) - (this.Velocity.X * this.Velocity.X))));
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
            _music.Play();
        }

        /// <summary>
        /// Gestisco i casi in cui la pallina collide contro i bordi dello schermo
        /// </summary>
        /// <param name="s"></param>
        private void ViewCollision(Sprite s)
        {
            Playground myview = (Playground)s;

            // La X della pallina è oltre il limite destro o sinistro
            if ((this.X + (float)this.Width) >= (float)myview.Width + myview.X)
            {
                this.Velocity.X *= -1;
                this.X = (float)myview.Width - this.Width + myview.X;
            }
            else
            if (this.X <= myview.X)
            {
                this.Velocity.X *= -1;
                this.X = myview.X;
            }

            // Fa rimanere la pallina all'interno dello schermo e scalo una vita se la y della pallina arriva all'altezza di view.height
            // La Y della pallina è oltre il limite superiore o inferiore
            if ((this.Y + (float)this.Height) >= (float)myview.Height + myview.X)
            {
                myview.BottomCollide = 1;
            }
            else
            if (this.Y <= myview.Y)
            {
                this.Velocity.Y *= -1;
                this.Y = myview.Y;
            }
        }

        #endregion Methods
    }
}