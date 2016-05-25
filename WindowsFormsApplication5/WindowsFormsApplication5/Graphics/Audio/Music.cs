using System.Media;

namespace WindowsFormsApplication5
{
    class Music
    {

        private SoundPlayer backgroundMusic;

        public Music()
        {

        }

        public void GameOver()
        {
            backgroundMusic = new SoundPlayer(Properties.Resources.Background_GameOver);
            backgroundMusic.PlayLooping();
        }

        public void Game()
        {
            backgroundMusic = new SoundPlayer(Properties.Resources.Background_Music);
            backgroundMusic.PlayLooping();
        }

        public void Menu()
        {
            backgroundMusic = new SoundPlayer(Properties.Resources.Background_Menu);
            backgroundMusic.PlayLooping();
        }

        public void Dispose_Music()
        {
            backgroundMusic.Dispose();

        }
    }
}
