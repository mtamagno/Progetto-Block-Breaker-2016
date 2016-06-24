using BlockBreaker.Properties;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace BlockBreaker
{
    public partial class Menu : Form
    {
        #region Public Fields

        private MenuButton _help;
        private MenuButton _highscores;
        private Instructions _instructions;
        private readonly Panel _menuPanel = new Panel();
        private Size _s;
        public MenuButton Start;

        #endregion Public Fields

        #region Private Fields

        private Bitmap _backgroundimage;
        private HighScoresPanel _highScoresPanel;
        private PictureBox _logo;
        private bool _showHighScore;

        #endregion Private Fields

        #region Public Constructors

        public Menu()
        {
            _instructions = new Instructions(0, 0, ClientSize.Width, ClientSize.Height);
            InitializeComponent();
            _showHighScore = false;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        /// Funzione che permette di pulire dalla memoria le immagini caricate nel menù e che segnala se quando ciò avviene
        /// erano aperte o meno le istruzioni,
        /// questo è necessario per capire poi se riaprirle dopo un resize
        /// </summary>
        /// <returns></returns>
        public bool Cleaner()
        {
            //Crea la variabile che segnala se erano aperte le istruzioni
            var makeInstructionsVisible = false;

            //Libera la memoria dalle immagini
            BackgroundImage.Dispose();
            _backgroundimage.Dispose();

            if (_instructions.Visible)
            {
                _instructions.Visible = false;
                makeInstructionsVisible = true;
            }

            _instructions.Dispose();
            GC.Collect();
            GC.WaitForFullGCComplete();
            return makeInstructionsVisible;
        }

        /// <summary>
        /// Gestore eventi per la pressione di tasti
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void Help_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Escape)
            {
                _menuPanel.Visible = true;
                Start.Visible = true;
                _help.Visible = true;
                _instructions.Visible = false;
                _highScoresPanel.Visible = false;
                _logo.Visible = true;
                _showHighScore = false;
            }
        }

        /// <summary>
        /// Funzione che si occupa del resize di questo form e dei sui componenti
        /// </summary>
        /// <param name="l">new Width</param>
        /// <param name="h">new height</param>
        public void OnResize(int l, int h)
        {
            if (ClientSize.Width > 0 && ClientSize.Height > 0)
            {
                // Libera la memoria e capisce se si era sulle istruzioni o meno
                var makeInstructionsVisible = Cleaner();

                // Ricrea il BackGround
                BackgroundImage = new Bitmap(Resources.BackGround_Image, ClientSize);
                BackgroundImageLayout = ImageLayout.Stretch;

                //Ricrea il Panel
                CreatePanel();


                //Ricrea il logo
                CreateLogo(ClientSize.Width, ClientSize.Height);

                //Ricrea le istruzioni
                _instructions = new Instructions(0, 0, ClientSize.Width, ClientSize.Height);
                _highScoresPanel.Dispose();
                _highScoresPanel = new HighScoresPanel(0, 0, ClientSize.Width, ClientSize.Height);
                Controls.Add(_instructions);
                Controls.Add(_highScoresPanel);
                if (_showHighScore)
                {
                    _highScoresPanel.Visible = true;
                    _menuPanel.Visible = false;
                    _logo.Visible = false;
                }
                if (makeInstructionsVisible)
                {
                    _menuPanel.Visible = false;
                    _logo.Visible = false;
                    _instructions.Visible = true;
                }
                _help.KeyPress += Help_KeyPress;
                Start.Text = "Play";
            }
        }


        #endregion Public Methods

        #region Private Methods

        /// <summary>
        /// Carica il menu
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnLoad(object sender, EventArgs e)
        {
            Starter();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        /// <summary>
        /// Aggiunge i tasti al panel, poi il panel e le istruzioni ai controlli del form
        /// </summary>
        private void ControlsAdder()
        {
            // Aggiunge i bottoni al panel e poi il panel ai controlli
            Controls.Add(_menuPanel);
            _menuPanel.Controls.Add(Start);
            _menuPanel.Controls.Add(_help);
            _menuPanel.Controls.Add(_highscores);
            Controls.Add(_instructions);
            Controls.Add(_highScoresPanel);
        }

        /// <summary>
        /// Crea il logo con le giuste dimensioni
        /// </summary>
        /// <param name="Left"></param>
        /// <param name="Top"></param>
        /// <param name="Width"></param>
        /// <param name="height"></param>
        private void CreateLogo(int Width, int height)
        {
            if (_logo != null)
            {
                _logo.Dispose();
            }
            _logo = new PictureBox();
            _logo.Width = _menuPanel.Width * 4;
            _logo.Height = _menuPanel.Top;
            _logo.Image = new Bitmap(Resources.logo1, _logo.Size);
            _logo.BackColor = Color.Transparent;
            _logo.Top = 0;
            _logo.Left = Width / 2 - _logo.Width / 2;
            Controls.Add(_logo);
        }

        /// <summary>
        /// Dimensiona il panel a seconda della grandezza della grandezza del
        /// client e lo riempie con i pulsanti Start e help
        /// </summary>
        private void CreatePanel()
        {
            _s = new Size(ClientSize.Width / 6, ClientSize.Height / 10);

            // Imposta le dimensioni e la posizione del pannello panel che conterrà Start e help
            _menuPanel.Size = new Size(ClientRectangle.Width / 5, ClientRectangle.Height / 2);
            _menuPanel.Top = ClientRectangle.Height / 2 - _menuPanel.Size.Height / 15 * 2;
            _menuPanel.Left = ClientRectangle.Width / 2 - _menuPanel.Size.Width / 2;
            _menuPanel.BackColor = Color.FromArgb(150, Color.Black);
            _menuPanel.BorderStyle = BorderStyle.Fixed3D;

            // Imposta le dimensioni e la posizione di Start
            if (Start != null)
                Start.Dispose();
            Start = new MenuButton(_s);
            Start.Top = _menuPanel.Height / 4 - Start.Height / 5 * 2;
            Start.Left = _menuPanel.Width / 2 - Start.Width / 2;

            // Imposta le dimensioni e la posizione di help
            _help?.Dispose();
            _help = new MenuButton(_s);
            _help.Top = _menuPanel.Height / 3 + _help.Height / 5 * 2;
            _help.Left = _menuPanel.Width / 2 - _help.Width / 2;
            _help.Text = "Help";

            // Imposta le dimensioni e la posizione di Highscore
            _highscores?.Dispose();
            _highscores = new MenuButton(_s);
            _highscores.Top = _menuPanel.Height / 4 + _highscores.Height * 2;
            _highscores.Left = _menuPanel.Width / 2 - _highscores.Width / 2;
            _highscores.Text = "Highscores";

            // Eventhandler
            _help.Click += ShowInstructions;
            _highscores.Click += ShowHighScore;
            _menuPanel.Controls.Add(Start);
            _menuPanel.Controls.Add(_help);
            _menuPanel.Controls.Add(_highscores);
        }

        /// <summary>
        /// Funzione che permette di mostrare Gli highScores migliori
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowHighScore(object sender, EventArgs e)
        {
            _menuPanel.Visible = false;
            _logo.Visible = false;
            _highScoresPanel.Visible = true;
            Focus();
            KeyPress += Help_KeyPress;
            _showHighScore = true;
        }

        /// <summary>
        /// Evento che permette di nascondere le istruzioni nascondendo il resto
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ShowInstructions(object sender, EventArgs e)
        {
            _menuPanel.Visible = false;
            _logo.Visible = false;
            _instructions.Visible = true;
            Focus();
            KeyPress += Help_KeyPress;
        }

        /// <summary>
        /// Direttive che vanno eseguite in ogni caso
        /// </summary>
        private void Starter()
        {
            _s = new Size(ClientSize.Width / 5, ClientSize.Height / 10);

            // MyPlayground
            _backgroundimage = new Bitmap(Resources.BackGround_Image, Size);
            BackgroundImage = _backgroundimage;

            // Crea e riempie il panel centrale
            CreatePanel();

            // Instructions
            _instructions = new Instructions(0, 0, ClientSize.Width, ClientSize.Height);
            _highScoresPanel = new HighScoresPanel(0, 0, ClientSize.Width, ClientSize.Height);

            //_logo
            CreateLogo(ClientSize.Width, ClientSize.Height);

            // Aspetta il Garbage collector
            GC.Collect();
            GC.WaitForPendingFinalizers();

            //Effettua un OnResize che annulla un particolare bug nelle librerie di Drawings.dll
            //Che si può indurre eliminando questo primo OnResize e cliccando su help, poi effettuando il (primo) resize dalla schermata delle istruzioni
            OnResize(ClientSize.Width, ClientSize.Height);

            // Aggiunge i tasti al panel, poi il panel e le istruzioni ai controlli del form
            ControlsAdder();
        }

        #endregion Private Methods
    }
}
   