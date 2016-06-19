using System.Media;
using BlockBreaker.Properties;

namespace BlockBreaker
{
    internal class Music
    {
        #region Fields

        public SoundPlayer BackgroundMusic;

        #endregion Fields

        #region Methods

        /// <summary>
        ///     Funzione per la liberazione delle risorse occupate dalla musica
        /// </summary>
        public void Dispose_Music()
        {
            BackgroundMusic.Stop();
            BackgroundMusic.Dispose();
        }

        /// <summary>
        ///     Funzione per la riproduzione della musica del gioco
        /// </summary>
        public void Game()
        {
            BackgroundMusic = new SoundPlayer(Resources.Game_Music);
            BackgroundMusic.PlayLooping();
        }

        /// <summary>
        ///     Funzione per la riproduzione della musica della schermata del gameover
        /// </summary>
        public void GameOver()
        {
            BackgroundMusic = new SoundPlayer(Resources.GameOver_Music);
            BackgroundMusic.PlayLooping();
        }

        /// <summary>
        ///     Funzione per la riproduzione della musica della schermata del menù
        /// </summary>
        public void Menu()
        {
            BackgroundMusic = new SoundPlayer(Resources.Menu_Music);
            BackgroundMusic.PlayLooping();
        }

        #endregion Methods
    }
}