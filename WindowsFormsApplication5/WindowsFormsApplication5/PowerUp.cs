using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace WindowsFormsApplication5
{
    class PowerUp : Sprite
    {
        #region Public Fields

        public Bitmap texture;

        #endregion Public Fields


        #region Public Constructors

        //       private static Random random = new Random();
        public PowerUp(float x, float y, int width, int height, Logic logic) : base(x, y, width, height)
        {
            texture = Properties.Resources.New_Piskel;
            canFall = true;
            canCollide = false;
            torender = true;
            followPointer = false;

            this.graphics(texture, x, y, width, height);
            logic.iManager.inGameSprites.Add(this);
        }
        #endregion Public Constructors
    }
}
