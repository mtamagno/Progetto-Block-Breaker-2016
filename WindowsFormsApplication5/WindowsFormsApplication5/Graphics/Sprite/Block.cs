using System;
using System.Drawing;

namespace BlockBreaker
{
    public class Block : Sprite
    {
        #region Fields

        public int BlockLife;

        // Variabili per vita e texture
        public int InitialLife;

        public Bitmap texture;

        // Crea il random che mi servirà a calcolare vite in modo casuale
        private static readonly Random Random = new Random();

        #endregion Fields

        #region Constructors

        public Block(float x, float y, int width, int height)
        {
            //Alla creazione genero un nuomero casuale di vita
            BlockLife = Random.Next(0, 5);
            InitialLife = BlockLife;

            // Imposta le proprietà dello sprite
            CanFall = false;
            CanCollide = true;

            // Imposta il render a true in caso abbia più di 0 vite
            if (BlockLife >= 1 && BlockLife <= 4)
                ToRender = true;
            FollowPointer = false;

            //se non bisogna renderizzarlo non deve poter collidere
            if (ToRender != true)
                CanCollide = false;

            this.TextureSwitcher();

            //renderizzo il blocco
            this.CreateSprite(texture, x, y, width, height);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Funzione che permette di cambiare la texture del blocco a seconda delle vite rimanenti
        /// </summary>
        public void TextureSwitcher()
        {
            //assegno texture diverse, a seconda della vita
            switch (BlockLife)
            {
                case 0:
                    this.texture = Properties.Resources.Block_1;
                    break;

                case 1:
                    this.texture = Properties.Resources.Block_1;
                    break;

                case 2:
                    this.texture = Properties.Resources.Block_2;
                    break;

                case 3:
                    this.texture = Properties.Resources.Block_3;
                    break;

                case 4:
                    this.texture = Properties.Resources.Block_4;
                    break;
            }
        }

        #endregion Methods
    }
}