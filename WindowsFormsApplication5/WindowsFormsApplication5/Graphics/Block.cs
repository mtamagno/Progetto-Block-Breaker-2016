using System;
using System.Drawing;

namespace WindowsFormsApplication5
{
    public class Block : Sprite
    {
        #region Public Fields

        public int block_life;
        public int remaining_bounces;
        public Bitmap texture;

        #endregion Public Fields

        #region Private Fields

        private static Random random = new Random();

        #endregion Private Fields

        #region Public Constructors

        public Block(float x, float y, int width, int height) : base(x, y, width, height)
        {
            //Random random = new Random();
            remaining_bounces = random.Next(0, 4);
            block_life = remaining_bounces;
            canFall = false;
            canCollide = true;
            if (remaining_bounces > 0)
                torender = true;
            followPointer = false;
            if (torender != true)
                canCollide = false;

            switch(block_life){
                case 0:
                    texture = Properties.Resources.Block;
                    break;
                case 1:
                    texture = Properties.Resources.Block;
                    break;
                case 2:
                    texture = Properties.Resources.Block;
                    break;
                case 3:
                    texture = Properties.Resources.Block;
                    break;
                case 4:
                    texture = Properties.Resources.Block;
                    break;
            }

            this.graphics(texture, x, y, width, height);
        }

        #endregion Public Constructors
    }
}