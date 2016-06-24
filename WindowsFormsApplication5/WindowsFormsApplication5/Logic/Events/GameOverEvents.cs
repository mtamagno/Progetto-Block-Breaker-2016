using System;

namespace BlockBreaker
{
    internal class GameOverEvents
    {
        #region Private Fields

        private GameOver GameOverForm;

        #endregion Private Fields

        #region Public Constructors

        public GameOverEvents(GameOver gameOver)
        {
            GameOverForm = gameOver;
        }

        #endregion Public Constructors

        #region Public Methods

        public void TextBox_Click(object sender, EventArgs e)
        {
            GameOverForm.TextBox.Clear();
        }

        #endregion Public Methods
    }
}
