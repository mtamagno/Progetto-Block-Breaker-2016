using System;
using System.Drawing;

namespace WindowsFormsApplication5
{
    public class Block : Sprite
    {
        #region Public Fields

        //variabili per vita e texture
        public int initial_life;

        public int block_life;

        public Bitmap texture;

        #endregion Public Fields

        #region Private Fields

        //creo il random che mi servirà a calcolare vite in modo casuale
        private static Random random = new Random();

        #endregion Private Fields

        #region Public Constructors

        public Block(float x, float y, int width, int height) : base(x, y, width, height)
        {
            //Alla creazione genero un nuomero casuale di vita
            block_life = random.Next(0, 5);
            initial_life = block_life;

            //imposto le proprietà dello sprite
            canFall = false;
            canCollide = true;

            //imposto il render a true in caso abbia più di 0 vite
            if (block_life >= 1 && block_life <= 4)
                torender = true;
            followPointer = false;

            //se non bisogna renderizzarlo non deve poter collidere
            if (torender != true)
                canCollide = false;

            //assegno texture diverse, a seconda della vita
            switch (block_life)
            {
                case 0:
                    texture = Properties.Resources.Block_1;
                    break;

                case 1:
                    texture = Properties.Resources.Block_1;
                    break;

                case 2:
                    texture = Properties.Resources.Block_2;
                    break;

                case 3:
                    texture = Properties.Resources.Block_3;
                    break;

                case 4:
                    texture = Properties.Resources.Block_4;
                    break;
            }

            //renderizzo il blocco
            this.graphics(texture, x, y, width, height);
        }

        #endregion Public Constructors
    }
}