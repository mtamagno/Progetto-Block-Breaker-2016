using System;

namespace BlockBreaker
{
    internal class CheckLife
    {
        #region Public Methods

        /// <summary>
        ///     Funzione per il Check dell'hit del bottom
        /// </summary>
        /// <param name="thisForm"></param>
        /// <param name="lifes"></param>
        /// <returns></returns>
        public int Check(Game thisForm, int lifes)
        {
            if (thisForm == null) throw new ArgumentNullException(nameof(thisForm));
            if (thisForm.Background.BottomCollide == 1)
            {
                thisForm.Ball.CanFall = false;
                thisForm.Ball.Y = thisForm.Racchetta.Y - thisForm.Ball.Height;
                thisForm.Ball.FollowPointer = true;
                thisForm.Ball.VelocityTot = 0;
                thisForm.Ball.Velocity.X = 0;
                thisForm.Ball.Velocity.Y = 0;
                lifes--;
                thisForm.Background.BottomCollide = 0;
                for (var i = lifes; i < 3; i++)
                {
                    if (lifes > 0)
                        thisForm.Vita[i].ToRender = false;
                }
            }
            return lifes;
        }

        #endregion Public Methods
    }
}
