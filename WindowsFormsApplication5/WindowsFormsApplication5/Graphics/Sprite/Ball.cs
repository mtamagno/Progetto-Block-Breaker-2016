using BlockBreaker.Properties;
//using Microsoft.Xna.Framework;
//using Microsoft.Xna.Framework.Audio;
using System;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

namespace BlockBreaker
{
    public class Ball : Sprite
    {
        #region Public Fields

        //variabili per accelerazione y, texture, velocità totale e attuale, spia che notifica la raggiunta della velocità massima
        public int AccelY = 50;
        public PointF PreviousVelocity;
        public float PreviousVelocityTot;
        public float PreviousX;
        public float PreviousY;
        public bool ReachedVelocityTotLimit;
        public PointF Velocity;
        public float VelocityTot;
        public float VelocityTotLimit = 3000;

        #endregion Public Fields

        #region Private Fields

        //       private readonly SoundEffect _sound;
        private bool _blockHit;
        private Thread _hurt;

        #endregion Private Fields

        #region Public Constructors

        public Ball(float x, float y, int width, int height, Logic logic)
        {
            if (logic == null) throw new ArgumentNullException(nameof(logic));
            if (x <= 0) throw new ArgumentOutOfRangeException(nameof(x));
            if (y <= 0) throw new ArgumentOutOfRangeException(nameof(y));
            if (width <= 0) throw new ArgumentOutOfRangeException(nameof(width));
            if (height <= 0) throw new ArgumentOutOfRangeException(nameof(height));

            // Imposta le proprietà della pallina
            PreviousVelocityTot = 0;
            PreviousVelocity.X = 0;
            PreviousVelocity.Y = 0;
            PreviousX = 0;
            PreviousY = 0;
            var texture = Resources.Ball;
            CanFall = false;
            CanCollide = true;
            FollowPointer = true;
            ToRender = true;
            //       var stream = TitleContainer.OpenStream(@"SoundEffect/Music.wav");
            //         _sound = SoundEffect.FromStream(stream);

            //rendo invisibile lo sfondo dello sprite della pallina
            var backColor = texture.GetPixel(0, 0);
            texture.MakeTransparent(backColor);

            // Disegna la pallina
            CreateSprite(texture, x, y, width, height);

            // aggiungo la pallina all'inputmanager che tiene conto di tutti gli sprite presenti nel gioco
            logic.IManager.InGameSprites.Add(this);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     Risetta la velocità totale massima a seconda della grandezza del client in modo che non rallenti o acceleri la
        ///     pallina a seconda del size
        /// </summary>
        /// <param name="lunghezzaClientIniziale"></param>
        /// <param name="altezzaClientIniziale"></param>
        /// <param name="lunghezzaClient"></param>
        /// <param name="altezzaClient"></param>
        public virtual void TotalVelocityReset(int lunghezzaClientIniziale, int altezzaClientIniziale,
            int lunghezzaClient, int altezzaClient)
        {
            Velocity.Y = Velocity.Y * altezzaClient / altezzaClientIniziale;
            Velocity.X = Velocity.X * lunghezzaClient / lunghezzaClientIniziale;

            //Calcolo la velocità totale della pallina che non deve superare i velocityTotLimit
            VelocityTot = (float) Math.Sqrt(Velocity.X*Velocity.X + Velocity.Y*Velocity.Y);
            if (ReachedVelocityTotLimit == true)
                VelocityTotLimit = VelocityTot;
        }

        /// <summary>
        ///     Funzione che dopo aver chiamato il collider, si occupa dello spostamento vero e proprio di pallina e racchetta
        /// </summary>
        public void Update(InputManager iManager, Form thisform)
        {
            if (thisform == null) return;
            Collider(iManager);

            //Calcolo la velocità totale della pallina che non deve superare i velocityTotLimit
            VelocityTot = (float)Math.Sqrt(Velocity.X * Velocity.X + Velocity.Y * Velocity.Y);

            //Se la velocità totale non è ancora a velocityTotLimit, Imposta la spia a 0 così da continuare a farla aumentare
            if (VelocityTot < VelocityTotLimit)
                ReachedVelocityTotLimit = false;

            //Se il valore canFall è vero e quindi si tratta della pallina,
            //in caso la velocità massima non sia arrivata a velocityTotLimit
            //incremento la velocità y, altrimenti no
            if (CanFall)
            {
                //Blocco l'incremento di velocità totale oltre i velocityTotLimit
                if (VelocityTot >= VelocityTotLimit)
                {
                    VelocityTot = VelocityTotLimit;
                    ReachedVelocityTotLimit = true;
                }

                //Altrimenti aumento la velocità di y (o decremento se questa è negativa)
                else
                {
                    if (ReachedVelocityTotLimit == false)
                    {
                        if (Velocity.Y >= 0)
                        {
                            Velocity.Y += AccelY;
                        }
                        else
                        {
                            Velocity.Y -= AccelY;
                        }
                    }
                }
                X += Velocity.X * 1 / 500;
                Y += Velocity.Y * 1 / 500;
            }

            //se deve seguire il mouse faccio in modo che lo faccia
            if (FollowPointer)
            {
                if (Cursor.Position.X - thisform.Location.X >= thisform.Width / 11 &&
                    Cursor.Position.X - thisform.Location.X < thisform.Width - thisform.Width / 11)
                    X = Cursor.Position.X - thisform.Location.X - Width / 2 - Width / 3 * 2 - Width / 10 * 9;
            }
        }

        #endregion Public Methods

        #region Private Methods

        //Gestisco i casi in cui la pallina collide contro i blocchi
        private void BlockCollision(Sprite s)
        {
            var myBlock = (Block)s;
            _blockHit = false;
            if (this.IsCollidingWith(myBlock) && myBlock.CanCollide)
            {
                // Se un blocco viene toccato dalla pallina gli tolgo una vita e cambio la texture
                if (this.IsTouchingTop(myBlock) || this.IsTouchingBottom(myBlock))
                {
                    if (X + (float)Width / 2 > myBlock.X && X + (float)Width / 2 < myBlock.X + myBlock.Width)
                    {
                        // Setta block_hit a true
                        _blockHit = true;

                        //Inverte la Velocità Y della pallina
                        Velocity.Y *= -1;

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
                    if (Y + (float)Height / 2 > myBlock.Y && Y + (float)Height / 2 < myBlock.Y + myBlock.Height)
                    {
                        //Inverte la Velocità X della pallina
                        Velocity.X *= -1;

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
                                myBlock.CreateSprite(myBlock.texture, myBlock.X, myBlock.Y, myBlock.Width,
                                    myBlock.Height);
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Metodo collider che calcola le azioni da svolgere in caso di impatto
        /// </summary>
        private void Collider(InputManager iManager)
        {
            // Per ogni sprite presente nella lista contenuta dell'imanager
            foreach (var s in iManager.InGameSprites)
            {
                if (s.GetType().Name == "Block" && s.CanCollide == true)
                {
                    BlockCollision(s);
                }
                if (s.GetType().Name == "Racket")
                {
                    RacketCollision(s);
                }
                if (s.GetType().Name == "Playground")
                {
                    ViewCollision(s);
                }
            }
        }

        /// <summary>
        ///     Metodo playsound che riproduce il suono
        /// </summary>
        private void PlaySound()
        {
            //     FrameworkDispatcher.Update();
            //    _sound.Play();
        }

        //Gestisco i casi in cui la pallina collide contro la racchetta
        private void RacketCollision(Sprite s)
        {
            Racket myRacket = (Racket) s;
            if (myRacket.IsCollidingWith(this))
            {
                _hurt = new Thread(myRacket.OnHurt);
                _hurt.Start();

                //La pallina impatta con la racchetta
                if (this.IsTouchingBottom(myRacket))
                {
                    //La pallina impatta con la metà sinistra
                    if (X + Width / 2 <= myRacket.X + myRacket.Width / 2)
                    {
                        double coseno;
                        coseno = Math.Abs(Math.Cos(myRacket.Angolo(X + Width/2 - myRacket.X, myRacket.Width/2)));
                        Velocity.X = -VelocityTot*(float) coseno;
                        Velocity.Y =
                            -(float) Math.Sqrt(Math.Abs((double) (VelocityTot*VelocityTot - Velocity.X*Velocity.X)));
                        Y = myRacket.Y - Height;
                    }
                    else

                    //Altrimenti con la metà destra
                    {
                        double seno;
                        seno =
                            Math.Abs(
                                Math.Sin(myRacket.Angolo(X + Width/2 - myRacket.X - myRacket.Width/2, myRacket.Width/2)));
                        Velocity.X = VelocityTot*(float) seno;
                        Velocity.Y = -(float) Math.Sqrt(Math.Abs(VelocityTot*VelocityTot - Velocity.X*Velocity.X));
                        Y = myRacket.Y - Height;
                    }
                }
            }
        }

        /// <summary>
        ///     Gestisco i casi in cui la pallina collide contro i bordi dello schermo
        /// </summary>
        /// <param name="s"></param>
        private void ViewCollision(Sprite s)
        {
            var myview = (Playground)s;

            // La X della pallina è oltre il limite destro o sinistro
            if (X + Width >= myview.Width + myview.X)
            {
                Velocity.X *= -1;
                X = (float)myview.Width - Width + myview.X;
            }
            else if (X <= myview.X)
            {
                Velocity.X *= -1;
                X = myview.X;
            }

            // Fa rimanere la pallina all'interno dello schermo e scalo una vita se la y della pallina arriva all'altezza di view.height
            // La Y della pallina è oltre il limite superiore o inferiore
            if (Y + Height >= myview.Height + myview.X)
            {
                myview.BottomCollide = 1;
            }
            else if (Y <= myview.Y)
            {
                Velocity.Y *= -1;
                Y = myview.Y;
            }
        }

        #endregion Private Methods
    }
}
