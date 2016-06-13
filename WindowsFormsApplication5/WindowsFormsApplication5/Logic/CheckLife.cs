namespace WindowsFormsApplication5
{
    internal class CheckLife
    {
        #region Constructors

        public CheckLife()
        {
        }

        #endregion Constructors

        #region Methods

        public int check(Game ThisForm, int lifes)
        {
            if (ThisForm.background.bottomCollide == 1)
            {
                ThisForm.ball.canFall = false;
                ThisForm.ball.Y = ThisForm.racchetta.Y - ThisForm.ball.Height ;
                ThisForm.ball.followPointer = true;
                ThisForm.ball.velocityTot = 0;
                ThisForm.ball.velocity.X = 0;
                ThisForm.ball.velocity.Y = 0;
                lifes--;
                ThisForm.background.bottomCollide = 0;
                for (int i = lifes; i < 3; i++)
                {
                    if (lifes > 0)
                        ThisForm.vita[i].toRender = false;
                }
            }
            return (lifes);
        }

        #endregion Methods
    }
}