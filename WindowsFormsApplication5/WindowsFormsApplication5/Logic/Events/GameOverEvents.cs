using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlockBreaker
{
    class GameOverEvents
    {
        private GameOver GameOverForm;

        public GameOverEvents(GameOver gameOver)
        {
            GameOverForm = gameOver;
        }

        public void TextBox_Click(object sender, EventArgs e)
        {
            GameOverForm.TextBox.Clear();
        }

    }
}
