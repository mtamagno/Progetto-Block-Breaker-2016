
using BlockBreaker.Properties;
using System;
using System.Drawing;

namespace BlockBreaker
{
    public class Block : Sprite
    {
        #region Public Fields

        public int BlockLife;

        // Variabili per vita e texture
        public int InitialLife;

        public Bitmap texture;

        #endregion Public Fields

        #region Private Fields

        // Crea il random che mi servirà a calcolare vite in modo casuale
        private static readonly Random Random = new Random();

        #endregion Private Fields

        #region Public Constructors

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
            TextureSwitcher();

            //renderizzo il blocco
            CreateSprite(texture, x, y, width, height);
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Funzione che permette di cambiare la texture del blocco a seconda delle vite rimanenti
        /// </summary>
        public void TextureSwitcher()
        {
            //assegno texture diverse, a seconda della vita
            switch (BlockLife)
            {
                case 0:
                    texture = Resources.Block_1;
                    break;

                case 1:
                    texture = Resources.Block_1;
                    break;

                case 2:
                    texture = Resources.Block_2;
                    break;

                case 3:
                    texture = Resources.Block_3;
                    break;

                case 4:
                    texture = Resources.Block_4;
                    break;
            }
        }

        #endregion Public Methods
    }
}
