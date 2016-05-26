﻿using System;
using System.Drawing;

namespace WindowsFormsApplication5
{
    public class Block : Sprite
    {
        #region Public Fields

        //variabili per vita e texture
        public int initialLife;
        public int blockLife;
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
            blockLife = random.Next(0, 5);
            initialLife = blockLife;

            //imposto le proprietà dello sprite
            canFall = false;
            canCollide = true;

            //imposto il render a true in caso abbia più di 0 vite
            if (blockLife >= 1 && blockLife <= 4)
                torender = true;
            followPointer = false;

            //se non bisogna renderizzarlo non deve poter collidere
            if (torender != true)
                canCollide = false;

            this.textureSwitcher();
            //renderizzo il blocco
            this.graphics(texture, x, y, width, height);
        }

        #endregion Public Constructors
        
        public void textureSwitcher()
        {
            //assegno texture diverse, a seconda della vita
            switch (blockLife)
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
        }
    }
}