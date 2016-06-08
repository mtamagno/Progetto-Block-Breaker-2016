using System.Media;

namespace WindowsFormsApplication5
{
    internal class Music
    {
        #region Fields

        private SoundPlayer backgroundMusic;

        #endregion Fields

        #region Methods

        public void Dispose_Music()
        {
            backgroundMusic.Dispose();
        }

        public void Game()
        {
            backgroundMusic = new SoundPlayer(Properties.Resources.Game_Music);
            backgroundMusic.PlayLooping();
        }

        public void GameOver()
        {
            backgroundMusic = new SoundPlayer(Properties.Resources.GameOver_Music);
            backgroundMusic.PlayLooping();
        }

        public void Menu()
        {
            backgroundMusic = new SoundPlayer(Properties.Resources.Menu_Music);
            backgroundMusic.PlayLooping();
        }

        #endregion Methods
    }
}