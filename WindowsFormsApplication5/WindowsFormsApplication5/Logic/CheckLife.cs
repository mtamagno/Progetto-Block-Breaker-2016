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
            if (thisForm.MyPlayground.BottomCollide == 1)
            {
                thisForm.MyBall.CanFall = false;
                thisForm.MyBall.Y = thisForm.MyRacket.Y - thisForm.MyBall.Height;
                thisForm.MyBall.FollowPointer = true;
                thisForm.MyBall.VelocityTot = 0;
                thisForm.MyBall.Velocity.X = 0;
                thisForm.MyBall.Velocity.Y = 0;
                lifes--;
                thisForm.MyPlayground.BottomCollide = 0;
                for (var i = lifes; i < 3; i++)
                {
                    if (lifes > 0)
                        thisForm.MyLife[i].ToRender = false;
                }
            }
            return lifes;
        }

        #endregion Public Methods
    }
}
