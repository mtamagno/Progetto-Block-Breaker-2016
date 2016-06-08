﻿using System.Drawing;

namespace WindowsFormsApplication5
{
    public class Life : Sprite
    {
        #region Fields

        private Bitmap texture;

        #endregion Fields

        #region Constructors

        public Life(float x, float y, int width, int height)
        {
            // Imposta le variabili standard della vita alla creazione
            texture = Properties.Resources.vita;
            canFall = false;
            torender = true;
            canCollide = false;
            followPointer = false;

            // Disegna la vita
            this.graphics(texture, x, y, width, height);
        }

        #endregion Constructors
    }
}