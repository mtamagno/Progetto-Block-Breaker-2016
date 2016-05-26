using System.Media;

namespace WindowsFormsApplication5
{
    internal class Music
    {
        #region Fields

        private SoundPlayer backgroundMusic;

        #endregion Fields

        #region Constructors

        public Music()
        {
        }

        #endregion Constructors

        #region Methods

        public void Dispose_Music()
        {
            backgroundMusic.Dispose();
        }

        public void Game()
        {
            backgroundMusic = new SoundPlayer(Properties.Resources.Background_Music);
            backgroundMusic.PlayLooping();
        }

        public void GameOver()
        {
            backgroundMusic = new SoundPlayer(Properties.Resources.Background_GameOver);
            backgroundMusic.PlayLooping();
        }

        public void Menu()
        {
            backgroundMusic = new SoundPlayer(Properties.Resources.Background_Menu);
            backgroundMusic.PlayLooping();
        }

        #endregion Methods
    }
}