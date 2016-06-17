using System.Media;

namespace BlockBreaker
{
    internal class Music
    {
        #region Fields

        public SoundPlayer BackgroundMusic;

        #endregion Fields

        #region Methods

        /// <summary>
        /// Funzione per la liberazione delle risorse occupate dalla musica
        /// </summary>
        public void Dispose_Music()
        {
            this.BackgroundMusic.Stop();
            this.BackgroundMusic.Dispose();
        }

        /// <summary>
        /// Funzione per la riproduzione della musica del gioco
        /// </summary>
        public void Game()
        {
            this.BackgroundMusic = new SoundPlayer(Properties.Resources.Game_Music);
            this.BackgroundMusic.PlayLooping();
        }

        /// <summary>
        /// Funzione per la riproduzione della musica della schermata del gameover
        /// </summary>
        public void GameOver()
        {
            this.BackgroundMusic = new SoundPlayer(Properties.Resources.GameOver_Music);
            this.BackgroundMusic.PlayLooping();
        }

        /// <summary>
        /// Funzione per la riproduzione della musica della schermata del menù
        /// </summary>
        public void Menu()
        {
            this.BackgroundMusic = new SoundPlayer(Properties.Resources.Menu_Music);
            this.BackgroundMusic.PlayLooping();
        }

        #endregion Methods
    }
}