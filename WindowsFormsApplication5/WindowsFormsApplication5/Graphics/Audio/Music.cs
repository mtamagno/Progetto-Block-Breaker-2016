using System.Media;

namespace BlockBreaker
{
    internal class Music
    {
        #region Fields

        private SoundPlayer backgroundMusic;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Funzione per la liberazione delle risorse occupate dalla musica
        /// </summary>
        public void Dispose_Music()
        {
            this.backgroundMusic.Stop();
            this.backgroundMusic.Dispose();
        }

        /// <summary>
        /// Funzione per la riproduzione della musica del gioco
        /// </summary>
        public void Game()
        {
            this.backgroundMusic = new SoundPlayer(Properties.Resources.Game_Music);
            this.backgroundMusic.PlayLooping();
        }

        /// <summary>
        /// Funzione per la riproduzione della musica della schermata del gameover
        /// </summary>
        public void GameOver()
        {
            this.backgroundMusic = new SoundPlayer(Properties.Resources.GameOver_Music);
            this.backgroundMusic.PlayLooping();
        }

        /// <summary>
        /// Funzione per la riproduzione della musica della schermata del menù
        /// </summary>
        public void Menu()
        {
            this.backgroundMusic = new SoundPlayer(Properties.Resources.Menu_Music);
            this.backgroundMusic.PlayLooping();
        }

        #endregion Methods
    }
}